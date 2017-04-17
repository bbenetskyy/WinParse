using DataSaver.Models;
using DataSaver.RavenDB;
using FormulasCollection.Enums;
using FormulasCollection.Models;
using NLog;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;
using Raven.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SiteAccess.Model;

namespace DataSaver
{
    public class LocalSaver
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static bool IsAlive
        {
            get
            {
                try
                {
                    return Store?.OpenSession() != null;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
                return false;
            }
        }

        internal static DocumentStore Store;

        private IDocumentSession _session;

        public const int PageSize = 128;


        public LocalSaver(string url, string dbName)
        {
            try
            {
                Store = new DocumentStore
                {
                    Url = url,//"http://localhost:8765"
                    DefaultDatabase = dbName,//"Parser"
                    Conventions = { ShouldCacheRequest = should => false },
                };
                Store.Initialize();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Store = null;
            }
        }

        public void ClearDatabase()
        {
            var indexDefinitions = Store.DatabaseCommands.GetIndexes(0, 100);
            foreach (var indexDefinition in indexDefinitions)
            {
                Store.DatabaseCommands.DeleteByIndex(indexDefinition.Name, new IndexQuery());
            }
        }

        internal IDocumentSession Session
        {
            get
            {
                if (_session == null)
                {
                    _session = Store.OpenSession();
                    _session.Advanced.MaxNumberOfRequestsPerSession = int.MaxValue;
                }

                // ReSharper disable once InvertIf
                if (_session.Advanced.NumberOfRequests >=
                    _session.Advanced.MaxNumberOfRequestsPerSession)
                {
                    _session.Dispose();
                    _session = Store.OpenSession();
                }
                return _session;
            }
        }

        public void InsertForks(List<Fork> forkList)
        {
            if (forkList == null)
                return;

            foreach (var fork in forkList)
            {
                try
                {
                    Session.Store(MapForkToForkRow(fork));
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    Logger.Error(ex.StackTrace);
                }
            }
            Session.SaveChanges();
        }

        public User AuthenticateUser(string login, string password)
        {
            return Session
                .Query<User>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .FirstOrDefault(u => u.Login == login && u.Password == password);
        }

        public void ClearAndInsertForks(List<Fork> forkList, SportType sportType)
        {
            if (forkList == null || forkList.Count == 0)
                return;

            var rowsForDelete = MoveForks(forkList, sportType);
            ClearForks(rowsForDelete);
            InsertForks(forkList);
        }

        public List<ForkRow> MoveForks(List<Fork> forkList, SportType sportType)
        {
            if (forkList == null || forkList.Count == 0)
                return new List<ForkRow>();

            var savedForks = GetAllForkRows()
                ?.ToList();

            if (savedForks == null || savedForks.Count == 0)
                return new List<ForkRow>();

            var rowsForDelete = savedForks.Where(fBase => fBase.Sport == sportType.ToString())
                .ToList();
            foreach (var fBase in rowsForDelete.Where(f => f.Type == ForkType.Saved))
            {
                var fork = forkList.FirstOrDefault(fNew => IsSameFork(fNew, fBase));
                if (fork == null)
                    continue;
                forkList.Remove(fork);
            }
            rowsForDelete.RemoveAll(f => f.Type == ForkType.Saved);
            return rowsForDelete;
        }

        public bool IsSameFork(Fork fNew, ForkRow fBase)
        {
            // ReSharper disable once JoinDeclarationAndInitializer
            bool bRes;

            bRes = fNew.Event == fBase.Event;
            bRes = bRes && fNew.MatchDateTime == fBase.MatchDateTime;
            bRes = bRes && fNew.TypeFirst == fBase.TypeFirst;
            bRes = bRes && fNew.TypeSecond == fBase.TypeSecond;

            return bRes;
        }

        public void ClearForks(List<ForkRow> rowsForDelete)
        {
            if (rowsForDelete == null || rowsForDelete.Count <= 0)
                return;
            rowsForDelete.ForEach(f =>
                Store.DatabaseCommands.Delete(f.Id,
                    null));
            rowsForDelete.Clear();
        }

        public IEnumerable<ForkRow> GetAllForkRows()
        {
            var resList = new List<ForkRow>();
            using (Store.DatabaseCommands.DisableAllCaching())
            {
                resList.AddRange(Session.Query<ForkRow>()
                                        .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                                        .GetAllRecords(PageSize));
            }
            return resList;
        }

        public async Task<List<Fork>> GetForksAsync(Filter searchCriteria, ForkType forkType)
        {
            return await Task.Run(() => GetForks(searchCriteria, forkType));
        }

        public List<Fork> GetForks(Filter searchCriteria, ForkType forkType)
        {
            return GetAllForkRows().Where(f => f.Type == forkType &&
                                               ((searchCriteria.Basketball && f.Sport == SportType.Basketball.ToString()) ||
                                                (searchCriteria.Football && f.Sport == SportType.Soccer.ToString()) ||
                                                (searchCriteria.Hockey && f.Sport == SportType.Hockey.ToString()) ||
                                                (searchCriteria.Volleyball && f.Sport == SportType.Volleyball.ToString()) ||
                                                (searchCriteria.Tennis && f.Sport == SportType.Tennis.ToString())))
                .Select(MapForkRowToFork).ToList();
        }

        public void UpdateFork(ForkRow forkRow)
        {
            var forkDocument = Session.Load<ForkRow>(forkRow.Id);
            forkDocument.Type = forkRow.Type;
            Session.SaveChanges();
        }

        public void DeleteFork(ForkRow forkRow)
        {
            var forkDocument = Session.Load<ForkRow>(forkRow.Id);
            try
            {
                Session.Delete(forkDocument);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                Logger.Error(ex.StackTrace);
            }
            Session.SaveChanges();
        }

        public void UpdateFork(Fork fork)
        {
            UpdateFork(MapForkToForkRow(fork));
        }

        public void DeleteFork(Fork fork)
        {
            DeleteFork(MapForkToForkRow(fork));
        }

        protected ForkRow MapJsonDocumentToForkRow(JsonDocument json)
        {
            var prices = new List<string>();
            json.DataAsJson.Value<RavenJArray>("prices")?
                .ToList()
                .ForEach(price => prices.Add(price.Value<string>()));
            var result = new ForkRow
            {
                Id = json.Key,
                Event = json.DataAsJson.Value<string>("Event"),
                Sport = json.DataAsJson.Value<string>("Sport"),
                TypeFirst = json.DataAsJson.Value<string>("TypeFirst"),
                CoefFirst = json.DataAsJson.Value<string>("CoefFirst"),
                TypeSecond = json.DataAsJson.Value<string>("TypeSecond"),
                CoefSecond = json.DataAsJson.Value<string>("CoefSecond"),
                MatchDateTime = json.DataAsJson.Value<DateTime>("MatchDateTime"),
                BookmakerFirst = json.DataAsJson.Value<string>("BookmakerFirst"),
                BookmakerSecond = json.DataAsJson.Value<string>("BookmakerSecond"),
                MarathonEventId = json.DataAsJson.Value<string>("MarathonEventId"),
                PinnacleEventId = json.DataAsJson.Value<string>("PinnacleEventId"),
                League = json.DataAsJson.Value<string>("League"),
                Type = (ForkType)Enum.Parse(typeof(ForkType), json.DataAsJson.Value<string>("Type")),
                LineId = json.DataAsJson.Value<string>("LineId"),
                Sn = json.DataAsJson.Value<string>("sn"),
                Mn = json.DataAsJson.Value<string>("mn"),
                Ewc = json.DataAsJson.Value<string>("ewc"),
                Cid = json.DataAsJson.Value<string>("cid"),
                Prt = json.DataAsJson.Value<string>("prt"),
                Ewf = json.DataAsJson.Value<string>("ewf"),
                Epr = json.DataAsJson.Value<string>("epr"),
                Prices = prices,
                SelectionKey = json.DataAsJson.Value<string>("selection_key"),
                MarRate = json.DataAsJson.Value<string>("MarRate"),
                MarSuccess = json.DataAsJson.Value<string>("MarSuccess"),
                PinRate = json.DataAsJson.Value<string>("PinRate"),
                PinSuccess = json.DataAsJson.Value<string>("PinSuccess"),
                Period = json.DataAsJson.Value<int>("Period"),
                SideType = json.DataAsJson.Value<SideType>("SideType"),
                TeamType = json.DataAsJson.Value<TeamType>("TeamType"),
                BetType = json.DataAsJson.Value<BetType>("BetType"),
                Profit = json.DataAsJson.Value<double>("Profit")
            };
            return result;
        }
        protected UserData MapJsonDocumentToUser(JsonDocument json)
        {
            var result = new UserData
            {
                Id = json.Key,
                LoginPinnacle = json.DataAsJson.Value<string>("LoginPinnacle"),
                PasswordPinnacle = json.DataAsJson.Value<string>("PasswordPinnacle"),
                LoginMarathon = json.DataAsJson.Value<string>("LoginMarathon"),
                PasswordMarathon = json.DataAsJson.Value<string>("PasswordMarathon"),
                AntiGateCode = json.DataAsJson.Value<string>("AntiGateCode")
            };
            return result;
        }

        protected Filter MapJsonDocumentToFilter(JsonDocument json)
        {
            var result = new Filter
            {
                Id = json.Key,
                MinPercent = json.DataAsJson.Value<decimal?>("MinPercent"),
                MaxPercent = json.DataAsJson.Value<decimal?>("MaxPercent"),
                MinRate = json.DataAsJson.Value<decimal?>("MinRate"),
                MaxRate = json.DataAsJson.Value<decimal?>("MaxRate"),
                Basketball = json.DataAsJson.Value<bool>("Basketball"),
                Football = json.DataAsJson.Value<bool>("Football"),
                Hockey = json.DataAsJson.Value<bool>("Hockey"),
                LicenseKey = json.DataAsJson.Value<string>("LicenseKey"),
                Tennis = json.DataAsJson.Value<bool>("Tennis"),
                Volleyball = json.DataAsJson.Value<bool>("Volleyball"),
                AutoUpdateTime = json.DataAsJson.Value<int?>("AutoUpdateTime"),
                AutoDelete = json.DataAsJson.Value<bool>("AutoDelete"),
                AutoDeleteTime = json.DataAsJson.Value<int?>("AutoDeleteTime"),
                PinnaclePlace = json.DataAsJson.Value<bool>("PinnaclePlace")
            };
            return result;
        }

        protected ForkRow MapForkToForkRow(Fork fork)
        {
            var result = new ForkRow
            {
                Id = fork.Id,
                BookmakerFirst = fork.BookmakerFirst,
                BookmakerSecond = fork.BookmakerSecond,
                CoefFirst = fork.CoefFirst,
                CoefSecond = fork.CoefSecond,
                Event = fork.Event,
                MatchDateTime = fork.MatchDateTime,
                Sport = fork.Sport,
                TypeFirst = fork.TypeFirst,
                TypeSecond = fork.TypeSecond,
                Type = fork.Type,
                MarathonEventId = fork.MarathonEventId,
                PinnacleEventId = fork.PinnacleEventId,
                League = fork.League,
                LineId = fork.LineId,

                Sn = fork.Sn,
                Mn = fork.Mn,
                Ewc = fork.Ewc,
                Cid = fork.Cid,
                Prt = fork.Prt,
                Ewf = fork.Ewf,
                Epr = fork.Epr,
                Prices = fork.Prices,
                SelectionKey = fork.SelectionKey,


                MarRate = fork.MarRate,
                PinRate = fork.PinRate,
                MarSuccess = fork.MarSuccess,
                PinSuccess = fork.PinSuccess,

                Period = fork.Period,
                SideType = fork.SideType,
                TeamType = fork.TeamType,
                BetType = fork.BetType,
                Profit = fork.Profit
            };
            return result;
        }


        protected Fork MapForkRowToFork(ForkRow forkRow)
        {
            var result = new Fork
            {
                Id = forkRow.Id,
                BookmakerFirst = forkRow.BookmakerFirst,
                BookmakerSecond = forkRow.BookmakerSecond,
                CoefFirst = forkRow.CoefFirst,
                CoefSecond = forkRow.CoefSecond,
                Event = forkRow.Event,
                MatchDateTime = forkRow.MatchDateTime,
                Sport = forkRow.Sport,
                TypeFirst = forkRow.TypeFirst,
                TypeSecond = forkRow.TypeSecond,
                Type = forkRow.Type,
                MarathonEventId = forkRow.MarathonEventId,
                PinnacleEventId = forkRow.PinnacleEventId,
                League = forkRow.League,
                LineId = forkRow.LineId,

                Sn = forkRow.Sn,
                Mn = forkRow.Mn,
                Ewc = forkRow.Ewc,
                Cid = forkRow.Cid,
                Prt = forkRow.Prt,
                Ewf = forkRow.Ewf,
                Epr = forkRow.Epr,
                Prices = forkRow.Prices,
                SelectionKey = forkRow.SelectionKey,

                MarRate = forkRow.MarRate,
                PinRate = forkRow.PinRate,
                MarSuccess = forkRow.MarSuccess,
                PinSuccess = forkRow.PinSuccess,

                Period = forkRow.Period,
                SideType = forkRow.SideType,
                TeamType = forkRow.TeamType,
                BetType = forkRow.BetType,
                Profit = forkRow.Profit
            };
            return result;
        }

        public void UpdateUser(UserData user)
        {
            var userDocument = Session.Load<UserData>(user.Id);

            userDocument.LoginPinnacle = user.LoginPinnacle;
            userDocument.PasswordPinnacle = user.PasswordPinnacle;
            userDocument.LoginMarathon = user.LoginMarathon;
            userDocument.PasswordMarathon = user.PasswordMarathon;
            userDocument.AntiGateCode = user.AntiGateCode;

            Session.SaveChanges();
        }

        public void UpdateFilter(Filter filter)
        {
            var userDocument = Session.Load<Filter>(filter.Id);

            userDocument.Basketball = filter.Basketball;
            userDocument.MinPercent = filter.MinPercent;
            userDocument.MaxPercent = filter.MaxPercent;
            userDocument.MinRate = filter.MinRate;
            userDocument.MaxRate = filter.MaxRate;
            userDocument.Football = filter.Football;
            userDocument.Hockey = filter.Hockey;
            userDocument.LicenseKey = filter.LicenseKey;
            userDocument.Tennis = filter.Tennis;
            userDocument.Volleyball = filter.Volleyball;
            userDocument.AutoUpdateTime = filter.AutoUpdateTime;
            userDocument.AutoDelete = filter.AutoDelete;
            userDocument.AutoUpdateTime = filter.AutoUpdateTime;
            userDocument.AfterTime = filter.AfterTime;
            userDocument.BeforeTime = filter.BeforeTime;
            userDocument.PinnaclePlace = filter.PinnaclePlace;

            Session.SaveChanges();
        }

        /// <summary>
        /// Add new User into Raven Db
        /// </summary>
        /// <param name="loginPinnacle">User pinnacle login</param>
        /// <param name="passwordPinnacle">User pinnacle password</param>
        /// <param name="loginMarathon">User marathon login</param>
        /// <param name="passwordMarathon">User marathon password</param>
        /// <param name="antiGateCode">anticaptcha user private Id</param>
        /// <returns></returns>
        public bool AddUserToDb(string loginPinnacle,
            string passwordPinnacle,
            string loginMarathon,
            string passwordMarathon,
            string antiGateCode)
        {
            var user = new UserData
            {
                LoginPinnacle = loginPinnacle,
                PasswordPinnacle = passwordPinnacle,
                LoginMarathon = loginMarathon,
                PasswordMarathon = passwordMarathon,
                AntiGateCode = antiGateCode
            };

            try
            {
                Session.Store(user);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                Logger.Error(ex.StackTrace);
            }
            Session.SaveChanges();
            return true;
        }

        public bool AddUserToDb(UserData user)
        {
            try
            {
                Session.Store(user);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                Logger.Error(ex.StackTrace);
            }
            Session.SaveChanges();
            return true;
        }

        public bool AddFilterToDb(Filter filter)
        {
            try
            {
                Session.Store(filter);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                Logger.Error(ex.StackTrace);
            }
            Session.SaveChanges();
            return true;
        }


        public UserData FindUser()
        {
            var resList = new List<UserData>();
            using (Store.DatabaseCommands.DisableAllCaching())
            {
                resList.AddRange(Session.Query<UserData>()
                                        .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                                        .GetAllRecords(PageSize));
            }
            return resList.FirstOrDefault();
        }

        public Filter FindFilter()
        {
            var resList = new List<Filter>();
            using (Store.DatabaseCommands.DisableAllCaching())
            {
                resList.AddRange(Session.Query<Filter>()
                                        .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                                        .GetAllRecords(PageSize));
            }
            var first = resList.FirstOrDefault();
            if (first != null)
                return first;

            AddFilterToDb(new Filter());
            return FindFilter();
        }
    }
}
