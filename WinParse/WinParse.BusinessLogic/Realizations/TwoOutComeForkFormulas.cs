using DataParser.Enums;
using FormulasCollection.Enums;
using FormulasCollection.Helpers;
using FormulasCollection.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ToolsPortable;

namespace FormulasCollection.Realizations
{
    public class TwoOutComeForkFormulas
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly TwoOutComeCalculatorFormulas _calculatorFormulas;
        private readonly Dictionary<string, string> _pinKeyCache = new Dictionary<string, string>();

        public TwoOutComeForkFormulas()
        {
            _calculatorFormulas = new TwoOutComeCalculatorFormulas();
        }

        public bool CheckIsFork(double? coef1, double? coef2, ResultForForks marEvent, ResultForForksDictionary pinEvent)
        {
            if (coef1 == null || coef2 == null) return false;

            return GetProfit(coef1.Value, coef2.Value) > 0;
        }

        private double GetProfit(double coef1,
            double coef2)
        {
            var defRate = 100d;
            var rates = _calculatorFormulas.GetRecommendedRates(defRate, coef1, coef2);

            var rate1 = Convert.ToDouble(rates.Item1);
            var rate2 = Convert.ToDouble(rates.Item2);
            var allRate = _calculatorFormulas.CalculateSummaryRate(rate1, rate2);
            var income1 = Convert.ToDouble(_calculatorFormulas.CalculateRate(allRate, allRate - rate2, coef1));
            var income2 = Convert.ToDouble(_calculatorFormulas.CalculateRate(allRate, allRate - rate1, coef2));
            var income3 = Convert.ToDouble(_calculatorFormulas.CalculateClearRate(rate2, income1));
            var income4 = Convert.ToDouble(_calculatorFormulas.CalculateClearRate(rate1, income2));
            //todo delete this shit and refactored to one command
            return Math.Round(Convert.ToDouble(_calculatorFormulas.CalculateSummaryIncome(income3,
                income4)) - defRate,
                2);
        }

        private bool CheckIsMustToBeRevert(string eventMarathon, string eventPinacle) =>
           Extentions.GetStringSimilarityInPercent(eventMarathon.Split('#')[0],
                eventPinacle.Split('#')[0], true) < 75
            && Extentions.GetStringSimilarityInPercent(eventMarathon.Split('#')[1],
                eventPinacle.Split('#')[1], true) < 75;

        public List<Fork> GetAllForksDictionary(Dictionary<string, ResultForForksDictionary> pinnacle,
            List<ResultForForks> marathon)
        {
            var start = DateTime.Now;
            var resList = new List<Fork>();
            foreach (var eventItem in marathon)
            {
                if (eventItem.Event == null) continue;
                string pinKey = null;
                if (_pinKeyCache.ContainsKey(eventItem.Event))
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
                            .Key;

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
                    var pinEventKey = IsAnyForkAll(eventItem, pinnacle[pinKey], eventItem.SportType.EnumParse<SportType>());
                    if (pinEventKey == null) continue;
                    if (pinEventKey.IsNotBlank())
                    {
                        //fork variable is created for debug, please don't refactor it into resList.Add function
                        var fork = new Fork
                        {
                            League = eventItem.League,
                            MarathonEventId = eventItem.EventId,
                            PinnacleEventId = pinnacleEvent.EventId,
                            Event = eventItem.Event,
                            TypeFirst = eventItem.Type,
                            CoefFirst = eventItem.Coef,
                            TypeSecond = pinEventKey.ToString(CultureInfo.InvariantCulture),
                            CoefSecond = pinnacleEvent.TypeCoefDictionary[pinEventKey].ToString(CultureInfo.InvariantCulture),
                            Sport = eventItem.SportType,
                            MatchDateTime = pinnacleEvent.MatchDateTime.ToString(CultureInfo.CurrentCulture),
                            BookmakerSecond = pinKey,
                            BookmakerFirst = eventItem.Event_RU,
                            Type = ForkType.Current,
                            LineId = pinnacleEvent.TypeLineIdDictionary[pinEventKey],
                            Profit = GetProfit(Convert.ToDouble(eventItem.Coef),
                                Convert.ToDouble(pinnacleEvent.TypeCoefDictionary[pinEventKey])),
                            sn = eventItem.marathonAutoPlay.sn,
                            mn = eventItem.marathonAutoPlay.mn,
                            ewc = eventItem.marathonAutoPlay.ewc,
                            cid = eventItem.marathonAutoPlay.cid,
                            prt = eventItem.marathonAutoPlay.prt,
                            ewf = eventItem.marathonAutoPlay.ewf,
                            epr = eventItem.marathonAutoPlay.epr,
                            prices = eventItem.marathonAutoPlay.prices,
                            selection_key = eventItem.marathonAutoPlay.selection_key
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
            var fd = marathon.FirstOrDefault();
            if (fd != null)
                _logger.Fatal($"{fd.SportType} {DateTime.Now - start}");
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

        private void RevertValues(Dictionary<string, ResultForForksDictionary> pinnacle, string pinKey)
        {
            var pin = pinnacle[pinKey];
            var tmpDic = new Dictionary<string, double>();
            foreach (var typeCoef in pin.TypeCoefDictionary)
            {
                if (typeCoef.Key == "1")
                    tmpDic.Add("2", typeCoef.Value);
                else if (typeCoef.Key == "2")
                    tmpDic.Add("1", typeCoef.Value);
                else if (typeCoef.Key.StartsWith("F1"))
                    tmpDic.Add("F2" + typeCoef.Key.Remove(0, 2), typeCoef.Value);
                else if (typeCoef.Key.StartsWith("F2"))
                    tmpDic.Add("F1" + typeCoef.Key.Remove(0, 2), typeCoef.Value);
                else
                    tmpDic.Add(typeCoef.Key, typeCoef.Value);
            }
            pin.TypeCoefDictionary = tmpDic;
        }

        private string IsAnyForkAll(ResultForForks marEvent, ResultForForksDictionary pinEvent, SportType st)
        {
            try
            {
                marEvent.Type = marEvent.Type.Trim();
                var resType = SportsConverterTypes.TypeParseAll(marEvent.Type,
                    st);
                if (!pinEvent.TypeCoefDictionary.ContainsKey(resType))
                    return null;
                var isFork = CheckIsFork(marEvent.Coef.ConvertToDoubleOrNull(),
                    pinEvent.TypeCoefDictionary[resType],
                    marEvent,
                    pinEvent);
                return isFork
                    ? resType
                    : null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
                return null;
            }
        }
    }
}