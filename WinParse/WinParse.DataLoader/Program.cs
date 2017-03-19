//#define PlaceBets
//#define TestSoccer
//#define TestPinnacleOnly

using WinParse.DataParser.Enums;
using DataSaver;
using WinParse.DataSaver.Models;
using WinParse.BusinessLogic.Enums;
using WinParse.BusinessLogic.Models;
using WinParse.BusinessLogic.Realizations;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using SiteAccess.Enums;
using ToolsPortable;
using WinParse.DataParser.DefaultRealization;

#if PlaceBets

using Common.Modules.AntiCaptha;
using SiteAccess.Access;
using SiteAccess.Model.Bets;

#endif

namespace DataLoader
{
    internal class Program
    {
        private static User _currentUser;

        private static PinnacleSportsDataParser _pinnacle;
        private static MarathonParser _marathon;
        private static TwoOutComeForkFormulas _forkFormulas;
        private static LocalSaver _localSaver;
        private static TwoOutComeCalculatorFormulas _calculatorFormulas;
        private const decimal MinPinnacleRate = 1;
#if PlaceBets
        private static MarathonAccess _marath;
        private static PinncaleAccess _pinn;
#endif
        private static double _defRate;
        private static Filter _filter;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private static void Main()
        {
            // Create a new object, representing the German culture.
            CultureInfo culture = new CultureInfo("en-US");

            // The following line provides localization for the application's user interface.
            Thread.CurrentThread.CurrentUICulture = culture;

            // The following line provides localization for data formats.
            Thread.CurrentThread.CurrentCulture = culture;

            // Set this culture as the default culture for all threads in this application.
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            _calculatorFormulas = new TwoOutComeCalculatorFormulas();
            _pinnacle = new PinnacleSportsDataParser();
            _marathon = new MarathonParser();
            _localSaver = new LocalSaver();
            _forkFormulas = new TwoOutComeForkFormulas();
            StartLoadDictionary();
        }

        private static void GetUserData()
        {
            _currentUser = _localSaver.FindUser();
            if (_currentUser != null) return;

            _currentUser = new User();
            Console.WriteLine("Please enter Pinnacle user login");//for test "VB794327", "artem89@"
            _currentUser.LoginPinnacle = Console.ReadLine();

            Console.WriteLine("Please enter Pinnacle user password");
            _currentUser.PasswordPinnacle = Console.ReadLine();

            Console.WriteLine("Please enter Marathon user login");//for test "2127864", "Artemgus88"
            _currentUser.LoginMarathon = Console.ReadLine();
            Console.WriteLine("Please enter Marathon user password");
            _currentUser.PasswordMarathon = Console.ReadLine();

            Console.WriteLine("Please enter Anti Gate Code");
            _currentUser.AntiGateCode = Console.ReadLine();

            _localSaver.AddUserToDb(_currentUser);
        }

        private static void StartLoadDictionary()
        {
            while (true)
            {
                GetUserData();
                Console.WriteLine("Start Loading with Dictionary");
                Console.WriteLine($"Pinnacle Login = '{_currentUser.LoginPinnacle}'");
                Console.WriteLine($"Pinnacle Password = '{_currentUser.PasswordPinnacle}'");
                Console.WriteLine($"Marathon Login = '{_currentUser.LoginMarathon}'");
                Console.WriteLine($"Marathon Password = '{_currentUser.PasswordMarathon}'");
                Console.WriteLine($"Anti Gate Code = '{_currentUser.AntiGateCode}'");

#if TestSoccer
                var sportsToLoading = new[] { SportType.Hockey };

#else
                //always loading all sports
                var sportsToLoading = new[] { SportType.Basketball, SportType.Hockey, SportType.Soccer, SportType.Tennis, SportType.Volleyball };
#endif

#if PlaceBets
                _marath = new MarathonAccess(new AntiGate(_currentUser.AntiGateCode));
                _marath.Login(_currentUser.LoginMarathon, _currentUser.PasswordMarathon);
                //https://www.pinnacle.com/ru/api/manual#pbet
                _pinn = new PinncaleAccess();
                _pinn.Login(_currentUser.LoginPinnacle, _currentUser.PasswordPinnacle);
#endif

                foreach (var sportType in sportsToLoading)
                {
                    var watch = new Stopwatch();
                    watch.Start();
                    _filter = _localSaver.FindFilter();
                    Console.WriteLine($"Min Percent {_filter.MinPercent}");
                    Console.WriteLine($"Max Percent {_filter.MaxPercent}");
                    Console.WriteLine($"Min Rate {_filter.MinRate}");
                    Console.WriteLine($"Max Rate {_filter.MaxRate}");
                    Console.WriteLine($"After Time {_filter.AfterTime}");
                    Console.WriteLine($"Before Time {_filter.BeforeTime}");
                    switch (sportType)
                    {
                        case SportType.Soccer:
                            if (!_filter.Football) continue;
                            break;

                        case SportType.Basketball:
                            if (!_filter.Basketball) continue;
                            break;

                        case SportType.Hockey:
                            if (!_filter.Hockey) continue;
                            break;

                        case SportType.Tennis:
                            if (!_filter.Tennis) continue;
                            break;

                        case SportType.Volleyball:
                            if (!_filter.Volleyball) continue;
                            break;
                    }
                    var pinSport = LoadPinacleDictionary(sportType);
                    var marSport = LoadMarathon(sportType);
                    var forks = GetForksDictionary(sportType, pinSport, marSport);

                    ClearForks(forks, sportType);
                    if (forks.Count > 0)
                    {
                        PlaceAllBets(forks, sportType);
                        SaveNewForks(forks, sportType);
                    }
                    watch.Stop();
                    _logger.Fatal($"StartLoadDictionary {sportType} {watch.Elapsed}");
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static void ClearForks(List<Fork> forks, SportType sportType)
        {
            Console.WriteLine($"Starting remove old forks. Sport type {sportType}");
            if (forks == null || forks.Count == 0)
                return;

            var watch = new Stopwatch();
            watch.Start();
            var rowsForDelete = _localSaver.MoveForks(forks, sportType);
            _localSaver.ClearForks(rowsForDelete);
            watch.Stop();
            _logger.Fatal($"ClearForks {sportType} {watch.Elapsed}");
            Console.WriteLine($"{forks.Count} forks left for place and save. Sport type {sportType}");
        }

        private static void PlaceAllBets(List<Fork> forks, SportType sportType)
        {
            Console.WriteLine($"Starting place bets for new {forks.Count} forks. Sport type {sportType}");
            var watch = new Stopwatch();
            watch.Start();

            var tmpRate = _filter.MaxRate ?? _filter.MinRate;
            if (tmpRate == null) return;

            foreach (var fork in _calculatorFormulas.FilteredForks(forks.Select(f => f).ToList(), _filter).OrderBy(f => Convert.ToDateTime(f.MatchDateTime)))
            {
                bool resM = false;
#if PlaceBets
                var rate = tmpRate.Value;
                var recomendedPinnacle = _calculatorFormulas.CalculateRatePart((double)rate,
                                                                               fork.CoefFirst.ConvertToDoubleOrNull(),
                                                                               fork.CoefSecond.ConvertToDoubleOrNull());
#if !TestPinnacleOnly
                if (recomendedPinnacle >= MinPinnacleRate)
                {
                    do
                    {
                        var betM = new MarathonBet
                        {
                            Id = fork.selection_key,
                            Name = fork.BookmakerFirst,
                            // ReSharper disable once PossibleInvalidOperationException
                            Stake = (double)rate,
                            AddData =
                                $"{{\"sn\":\"{fork.sn}\"," + $"\"mn\":\"{fork.mn}\"," + $"\"ewc\":\"{fork.ewc}\"," +
                                $"\"cid\":{fork.cid}," + $"\"prt\":\"{fork.prt}\"," + $"\"ewf\":\"{fork.ewf}\"," +
                                $"\"epr\":\"{fork.epr}\"," + "\"prices\"" + $":{{\"0\":\"{fork.prices[0]}\"," +
                                $"\"1\":\"{fork.prices[1]}\"," + $"\"2\":\"{fork.prices[2]}\"," +
                                $"\"3\":\"{fork.prices[3]}\"," + $"\"4\":\"{fork.prices[4]}\"," +
                                $"\"5\":\"{fork.prices[5]}\"}}}}"
                        };
                        resM = _marath.MakeBet(betM);
                        Console.WriteLine($"Place result {resM} for {rate} into {fork.BookmakerFirst}");
                        if (!resM)
                        {
                            rate -= 0.1m;
                        }
                    } while (!resM && rate >= _filter.MinRate);
                }
                if (rate < _filter.MinRate) rate = _filter.MinRate ?? rate;
                fork.MarRate = rate.ToString(CultureInfo.CurrentCulture);
                fork.MarSuccess = resM.ToString(CultureInfo.CurrentCulture);
                if (resM)
                {
#endif
                var betP = new PinnacleBet
                {
                    AcceptBetterLine = true,
                    BetType = fork.BetType,
                    Eventid = Convert.ToInt64(fork.PinnacleEventId),
                    Guid = Guid.NewGuid().ToString(),
                    OddsFormat = OddsFormat.DECIMAL,
                    LineId = Convert.ToInt64(fork.LineId),
                    PeriodNumber = fork.Period,
                    WinRiskRate = WinRiskType.WIN,
                    Stake = recomendedPinnacle,
                    SportId = (int)(SportType)Enum.Parse(typeof(SportType), fork.Sport, false),
                    Side = fork.SideType ?? default(SideType),
                    TeamType = fork.TeamType ?? default(TeamType)
                };
                var resP = _pinn.MakeBet(betP);
                Console.WriteLine(resP.Success
                                 ? $"Place result {resP.Success} for {recomendedPinnacle} into {fork.BookmakerFirst}"
                                 : $"Place result {resP.Success} with code {resP.Status} and description {resP.Error} for {recomendedPinnacle} into {fork.BookmakerFirst}");
                fork.PinRate = recomendedPinnacle.ToString(CultureInfo.CurrentCulture);
                fork.PinSuccess = $"{resP.Success} {resP.Status} {resP.Error}";
#if !TestPinnacleOnly
            }
#endif
#endif
                var outF =
                    forks.First(
                        f =>
                            f.MarathonEventId == fork.MarathonEventId
                            && f.PinnacleEventId == fork.PinnacleEventId
                            && f.TypeFirst == fork.TypeFirst
                            && f.TypeSecond == fork.TypeSecond);
                if (outF.Type != ForkType.Saved)
                    outF.Type = ForkType.Saved;
            }

            watch.Stop();
            _logger.Fatal($"PlaceAllBets {sportType} {watch.Elapsed}");
            Console.WriteLine(
                $"End placing bet. Was placed {forks.Count(f => f.Type == ForkType.Saved)} forks. Sport type {sportType}");
        }

        private static List<Fork> GetForksDictionary(SportType sportType, Dictionary<string, ResultForForksDictionary> pinSport, List<ResultForForks> marSport)
        {
            Console.WriteLine($"Start Calculate Forks for {sportType} sport type");

            var watch = new Stopwatch();
            watch.Start();
            var resList = _forkFormulas.GetAllForksDictionary(pinSport, marSport);
            watch.Stop();
            _logger.Fatal($"GetForksDictionary {sportType} {watch.Elapsed}");
            Console.WriteLine("Calculate finished");
            Console.WriteLine($"Was founded {resList.Count} {sportType} Forks");

            return resList;
        }

        private static void SaveNewForks(List<Fork> forks, SportType sportType)
        {
            Console.WriteLine($"Start Saving Forks for {sportType} sport type");

            var watch = new Stopwatch();
            watch.Start();
            _localSaver.InsertForks(forks);
            watch.Stop();
            _logger.Fatal($"SaveNewForks {sportType} {watch.Elapsed}");

            Console.WriteLine("End Saving.");
        }

        private static List<ResultForForks> LoadMarathon(SportType sportType)
        {
            Console.WriteLine($"Start Loading {sportType} Events from Marathon");

            var watch = new Stopwatch();
            watch.Start();
            var resList = _marathon.InitiMultiThread(sportType);
            watch.Stop();
            _logger.Fatal($"LoadMarathon {sportType} {watch.Elapsed}");
            Console.WriteLine("Loading finished");
            Console.WriteLine($"Was founded {resList.Count} {sportType} Events from Marathon");

            return resList;
        }

        private static Dictionary<string, ResultForForksDictionary> LoadPinacleDictionary(SportType sportType)
        {
            Console.WriteLine($"Start Loading {sportType} Events from Pinnacle");

            var watch = new Stopwatch();
            watch.Start();
            var newList = _pinnacle.GetAllPinacleEventsDictionary(sportType, _currentUser.LoginPinnacle, _currentUser.PasswordPinnacle);
            watch.Stop();
            _logger.Fatal($"LoadPinacleDictionary {sportType} {watch.Elapsed}");

            Console.WriteLine("Loading finished");
            Console.WriteLine($"Was founded {newList.Count} {sportType} Events from Pinnacle");

            return newList;
        }
    }
}