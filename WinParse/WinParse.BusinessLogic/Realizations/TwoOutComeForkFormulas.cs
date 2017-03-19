//#define TestCoef
//#define TestNames
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using ToolsPortable;
using WinParse.BusinessLogic.Enums;
using WinParse.BusinessLogic.Helpers;
using WinParse.BusinessLogic.Models;
using WinParse.DataParser.Enums;

namespace WinParse.BusinessLogic.Realizations
{
    public class TwoOutComeForkFormulas
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly TwoOutComeCalculatorFormulas _calculatorFormulas;
        private readonly Dictionary<string, string> _pinKeyCache = new Dictionary<string, string>();

        public static int CalculateSimilarity(string first, string second, DateTime dateFirst, DateTime dateSecond)
        {
            try
            {
                if (dateSecond.Year != dateFirst.Year || dateSecond.DayOfYear != dateFirst.DayOfYear)
                    return 0;
                Regex regex = new Regex("U|O^[A-Za-z]?\\d+");
                if (regex.IsMatch(first) && !regex.IsMatch(second)
                || !regex.IsMatch(first) && regex.IsMatch(second))
                    return 0;
                var source = first.ToLower();
                var target = second.ToLower();
                if ((source.Length == 0) || (target.Length == 0)) return 0;
                if (source == target) return 200;
                if (source.Contains(target) || target.Contains(source)) return 200;

                var sourceSplit = source.Split(new[] { " - ", " v " }, StringSplitOptions.None);
                var targetSplit = target.Split(new[] { " - ", " v ", " @ " }, StringSplitOptions.None);

                var stepsToSameOne = ComputeLevenshteinDistance(sourceSplit[0], targetSplit[0]);
                var stepsToSameTwo = ComputeLevenshteinDistance(sourceSplit[1], targetSplit[1]);
                var one = stepsToSameOne / Math.Max(sourceSplit[0].Length, targetSplit[0].Length) * 100;
                var two = stepsToSameTwo / Math.Max(sourceSplit[1].Length, targetSplit[1].Length) * 100;

                var stepsToSameOneR = ComputeLevenshteinDistance(sourceSplit[1], targetSplit[0]);
                var stepsToSameTwoR = ComputeLevenshteinDistance(sourceSplit[0], targetSplit[1]);
                var oneR = stepsToSameOneR / Math.Max(sourceSplit[1].Length, targetSplit[0].Length) * 100;
                var twoR = stepsToSameTwoR / Math.Max(sourceSplit[0].Length, targetSplit[1].Length) * 100;
                return Math.Max(one + two, oneR + twoR);
            }
            catch
            {
                return 0;
            }
        }

        private static int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;
            var i = 0;
            var targetSplit = target.Split(' ');
            var sourceSplit = source.Split(' ');
            foreach (var first in sourceSplit)
            {
                foreach (var second in targetSplit)
                {
                    if (first.Contains(second))
                    {
                        i = i + second.Length;
                    }
                    else if (second.Contains(first))
                    {
                        i = i + first.Length;
                    }
                }
            }
            return i;
        }

        public TwoOutComeForkFormulas()
        {
            _calculatorFormulas = new TwoOutComeCalculatorFormulas();
        }

        public bool CheckIsFork(double? coef1, double? coef2, ResultForForks marEvent, ResultForForksDictionary pinEvent)
        {
            if (marEvent.Event_RU.Contains("Манчестер Юнайтед") && marEvent.Event_RU.Contains("Халл Сити"))
            {
                int i = 0;
            }
            if (coef1 == null || coef2 == null) return false;

            return _calculatorFormulas.GetProfit(coef1.Value, coef2.Value) > 0;
        }

        public List<Fork> GetAllForksDictionary(Dictionary<string, ResultForForksDictionary> pinnacle,
            List<ResultForForks> marathon)
        {
            var resList = new List<Fork>();
#if TestCoef
            var alltypes = new List<string>();
            foreach (var @event in marathon)
            {
                if (!alltypes.Contains(@event.Type))
                    alltypes.Add(@event.Type);
            }
#endif
#if TestNames
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"D:\T2.txt"))
            {
#endif
            foreach (var eventItem in marathon)
            {
                if (eventItem.Event == null) continue;
                string pinKey = null;
                if (_pinKeyCache.ContainsKey(eventItem.Event) && pinnacle.ContainsKey(_pinKeyCache[eventItem.Event]))
                {
                    pinKey = _pinKeyCache[eventItem.Event];
                }
                else
                {
                    try
                    {
                        if (eventItem.MatchDateTime.Length <= 5) //for all times like "00:00"
                            eventItem.MatchDateTime = DateTime.Now.ToString(CultureInfo.CurrentCulture);

                        pinKey = pinnacle.FirstOrDefault(pinEvent =>
                            Extentions.GetStringSimilarityForSportTeams(
                                eventItem.Event,
                                pinEvent.Key,
                                true,
                                ConvertToDateTimeFromMarathon(eventItem.MatchDateTime),
                                pinEvent.Value.MatchDateTime)
                            >= 85)
                            .Key
                                 ?? pinnacle.FirstOrDefault(pinEvent =>
                                     CalculateSimilarity(
                                         eventItem.Event,
                                         pinEvent.Key,
                                         ConvertToDateTimeFromMarathon(eventItem.MatchDateTime),
                                         pinEvent.Value.MatchDateTime)
                                     >= 100)
                                     .Key;
#if TestNames
                            if (pinKey != null)
                            {
                                file.WriteLine($"{eventItem.Event} => {pinKey}");
                            }
#endif
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(eventItem.MatchDateTime);
                        _logger.Error(ex.Message);
                        _logger.Error(ex.StackTrace);
                    }
                }
                if (pinKey == null)
                    continue;
                if (!_pinKeyCache.ContainsKey(eventItem.Event))
                    _pinKeyCache.Add(eventItem.Event, pinKey);

                var pinnacleEvent = pinnacle[pinKey];

                try
                {
                    var pinEventKeys = IsAnyForkAll(eventItem, pinnacle[pinKey],
                        eventItem.SportType.EnumParse<SportType>());
                    if (pinEventKeys.Count == 0) continue;
                    foreach (var pinEventKey in pinEventKeys)
                    {
                        //fork variable is created for debug, please don't refactor it into resList.Add function
                        var fork = new Fork
                        {
                            League = pinnacleEvent.LeagueName,
                            MarathonEventId = eventItem.EventId,
                            PinnacleEventId = pinnacleEvent.EventId,
                            Event = eventItem.Event,
                            TypeFirst = eventItem.Type,
                            CoefFirst = eventItem.Coef,
                            TypeSecond = pinEventKey.ToString(CultureInfo.InvariantCulture),
                            CoefSecond = pinnacleEvent.ForkDetailDictionary[pinEventKey].TypeCoef.ToString(CultureInfo.InvariantCulture),
                            Sport = eventItem.SportType,
                            MatchDateTime = pinnacleEvent.MatchDateTime,
                            BookmakerSecond = pinKey,
                            BookmakerFirst = eventItem.Event_RU,
                            Type = ForkType.Current,
                            LineId = pinnacleEvent.ForkDetailDictionary[pinEventKey].LineId,
                            Profit = _calculatorFormulas.GetProfit(Convert.ToDouble(eventItem.Coef),
                                                                   Convert.ToDouble(pinnacleEvent.ForkDetailDictionary[pinEventKey].TypeCoef)),
                            sn = eventItem.marathonAutoPlay.sn,
                            mn = eventItem.marathonAutoPlay.mn,
                            ewc = eventItem.marathonAutoPlay.ewc,
                            cid = eventItem.marathonAutoPlay.cid,
                            prt = eventItem.marathonAutoPlay.prt,
                            ewf = eventItem.marathonAutoPlay.ewf,
                            epr = eventItem.marathonAutoPlay.epr,
                            prices = eventItem.marathonAutoPlay.prices,
                            selection_key = eventItem.marathonAutoPlay.selection_key,
                            Period = pinnacleEvent.ForkDetailDictionary[pinEventKey].Period,
                            SideType = pinnacleEvent.ForkDetailDictionary[pinEventKey].SideType,
                            TeamType = pinnacleEvent.ForkDetailDictionary[pinEventKey].TeamType,
                            BetType = pinnacleEvent.ForkDetailDictionary[pinEventKey].BetType
                        };
                        resList.Add(fork);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    _logger.Error(ex.StackTrace);
                }
            }
#if TestNames
            }
#endif
            return resList;
        }

        private DateTime ConvertToDateTimeFromMarathon(string matchDateTime)
        {
            DateTime tmpDateTime;
            if (DateTime.TryParse(matchDateTime,
                out tmpDateTime))
                return tmpDateTime;
            var year = 2000;
            //its for "20апр201714:00" types of time
            if (matchDateTime.Length == "20апр201714:00".Length)
            {
                year = Convert.ToInt32(matchDateTime.Substring(5, 4));
                matchDateTime = matchDateTime.Remove(5, 4);
            }
            var day = matchDateTime.Substring(0, 2);
            string month;
            switch (matchDateTime.Substring(2, 3))
            {
                case "янв":
                    month = "01";
                    break;

                case "фев":
                    month = "02";
                    break;

                case "мар":
                    month = "03";
                    break;

                case "апр":
                    month = "04";
                    break;

                case "май":
                    month = "05";
                    break;

                case "июн":
                    month = "06";
                    break;

                case "июл":
                    month = "07";
                    break;

                case "авг":
                    month = "08";
                    break;

                case "сен":
                    month = "09";
                    break;

                case "окт":
                    month = "10";
                    break;

                case "ноя":
                    month = "11";
                    break;

                case "дек":
                    month = "12";
                    break;

                default:
                    month = matchDateTime.Substring(2, 3);
                    break;
            }
            var time = matchDateTime.Substring(5);
            var fullTime = year != 2000
                           ? $"{day}/{month}/{year - 2000} {time}"
                           : $"{day}/{month}/{DateTime.Now.Year - 2000} {time}";
            var timeFormat = "dd/MM/yy HH:mm";

            return DateTime.ParseExact(fullTime,
                timeFormat,
                CultureInfo.CurrentCulture);
        }

        private List<string> IsAnyForkAll(ResultForForks marEvent, ResultForForksDictionary pinEvent, SportType st)
        {
            var resList = new List<string>();
            try
            {
                marEvent.Type = marEvent.Type.Trim();
                var resTypes = SportsConverterTypes.TypeParseAll(marEvent.Type, st);
                if (resTypes == null) return resList;
                foreach (var resType in resTypes)
                {
                    if (!pinEvent.ForkDetailDictionary.ContainsKey(resType))
                        continue;
                    var isFork = CheckIsFork(marEvent.Coef.ConvertToDoubleOrNull(),
                        pinEvent.ForkDetailDictionary[resType].TypeCoef,
                        marEvent,
                        pinEvent);
                    if (isFork)
                        resList.Add(resType);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
            }
            return resList;
        }
    }
}