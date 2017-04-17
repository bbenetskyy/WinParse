using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WinParse.MarathonBetLibrary.Model;
using WinParse.MarathonBetLibrary.Setup;

namespace WinParse.MarathonBetLibrary.Tools
{
    public class ParseTools
    {
        public static DataMarathonForAutoPlays ParseAutoPlay(string line, string selectionKey) => new DataMarathonForAutoPlays()
        {
            Sn = RegexByTags(line, Tags.Sn),
            Mn = RegexByTags(line, Tags.Mn),
            Ewc = RegexByTags(line, Tags.Ewc),
            Cid = RegexByTags(line, Tags.Cid),
            Prt = RegexByTags(line, Tags.Prt),
            Ewf = RegexByTags(line, Tags.Ewf),
            Epr = RegexByTags(line, Tags.Epr),
            Prices = RegexByTags(line, Tags.Prices)
                                .Replace('\"', ' ')
                                .Split(new char[] { ',', ':' }, StringSplitOptions.RemoveEmptyEntries)
                                .Where((x, y) => y % 2 != 0)
                                .ToList(),
            SelectionKey = selectionKey
        };

        public static string RegexByTags(string line, string tag, int groupNum = 1)
        {
            return Regex.Match(line, tag).Groups[groupNum].Value;
        }

        public static List<string> RegexByTagsMany(string line, string tag, int groupNum = 1)
        {
            List<string> result = new List<string>();
            foreach (Match item in Regex.Matches(line, tag))
            {
                result.Add(item.Groups[groupNum].Value);
            }
            return result;
        }

        public static NameEvent CreateEventName(string eventName, string queueTeams)
        {
            NameEvent nameEvent = null;
            string midlePart = string.Empty;
            if (eventName.Contains("vs"))
            {
                eventName = eventName.Replace("vs", "#");
            }
            else if (eventName.Contains("@"))
            {
                eventName = eventName.Replace("@", "#");
            }
            else if (eventName.Contains("-") && !eventName.Contains("#") && !eventName.Contains("@"))
            {
                eventName = eventName.Replace("-", "#");
            }
            else if (!eventName.Contains("#"))
            {
                return null;
            }

            string[] nums = queueTeams.Split('-');
            if (eventName.Contains("#"))
            {
                string name1 = eventName.Split('#')[0].Trim();
                string name2 = eventName.Split('#')[1].Trim();
                if (nums[0] == "1" && nums[1] == "2")
                    nameEvent = new NameEvent(name1, name2);
                else nameEvent = new NameEvent(name2, name1);
            }
            return nameEvent;
        }

        public static string QueueTeams(string line, int groupNum = 1)
        {
            var nums = RegexByTagsMany(line, Tags.GetNumTeamRegex);
            if (nums.Count > 1)
                return nums[0][0] + "-" + nums[1][0];
            else return "";
        }

        public static DateTime ConvertStringToDateTime(string date)
        {
            //21 мар 16:30
            //20:30
            string year = string.Empty;
            string month = string.Empty;
            string day = string.Empty;
            string time = string.Empty;

            date = date.Trim();
            DateTime dateTime = DateTime.Now;
            if (!date.Contains(" "))
            {
                year = DateTime.Now.Year.ToString();
                month = DateTime.Now.Month.ToString();
                day = DateTime.Now.Day.ToString();
                time = date;
            }
            else
            {
                string[] partDate = date.Split(' ');
                year = DateTime.Now.Year.ToString();
                month = DateFormatHelper.DateFormat[partDate[1]];
                day = partDate[0];
                time = partDate[2];
            }
            try
            {
                dateTime = DateTime.Parse(day + "/" + month + "/" + year + " " + time);
            }
            catch
            {
                return DateTime.Parse(day + "/" + month + "/" + year);
            }

            return dateTime;
        }

        public static string TypeCoef(NameEvent eventNameRu, string sn, string mn, bool isAsiat)
        {
            sn = sn.ToLower();
            mn = mn.ToLower();
            string result = string.Empty;
            if (mn.Contains("результат"))
            {
                result = Results(eventNameRu, sn, mn);
            }
            else if (mn.Equals("победа в матче"))
            {
                result = ResultsForVolleyball(eventNameRu, sn, mn);
            }
            else if (mn.Contains("победа") && mn.Contains("учетом") && mn.Contains("форы"))
            {
                result = Fora(eventNameRu, sn, mn, isAsiat);
            }
            else if (mn.Contains("тотал голов") || mn.Contains("тотал матча по очкам"))
            {
                result = Total(eventNameRu, sn, mn, isAsiat);
            }
            else if (mn.Contains("счет матча"))
            {
            }
            else if (sn.Equals("да") || sn.Equals("нет"))
            {
            }
            else if (mn.Equals("тайм / матч"))
            {
            }
            else if (mn.Equals("количество голов"))
            {
            }
            else if (mn.Equals("команда забьет первой"))
            {
            }
            else if (mn.Equals("последний гол в матче"))
            {
            }
            else if (mn.Contains("что произойдет раньше"))
            {
            }
            else
            {
                result = "UNDEFINED";
            }
            return result;
        }

        private static string Results(NameEvent eventNameRu, string sn, string mn)
        {
            string result = "ERROR";
            if (sn.Contains(CoefTypes.Wins))
            {
                if (!sn.Contains(CoefTypes.Draw))
                {
                    result = Helper.CheckPositionForNameTeam(sn, eventNameRu);
                    if (sn.Contains(eventNameRu.NameTeam1.ToLower()) && sn.Contains(eventNameRu.NameTeam2.ToLower()))
                        result = "12";
                }
                else
                {
                    string position = Helper.CheckPositionForNameTeam(sn, eventNameRu);
                    if ("1".Equals(position))
                        result = "1X";
                    else if ("2".Equals(position))
                        result = "X2";
                }
            }
            else if (sn.Contains(CoefTypes.Draw))
            {
                result = "X";
            }
            if (Helper.IsPartGame(mn))
            {
                string otherType = Helper.CheckPartGame(mn);
                result += otherType + RegexByTags(mn, "([0-9])");
            }
            if (mn.Equals(CoefTypes.ResultsDraw))
            {
                result = CoefTypes.ResultsDrawPart + "(" + Helper.TrueOrFalse(sn) + ")";
            }
            if ("ERROR".Equals(result))
            {
            }
            return result;
        }

        private static string ResultsForVolleyball(NameEvent eventNameRu, string sn, string mn)
        {
            string position = Helper.CheckPositionForNameTeam(sn, eventNameRu);
            string result = "ERROR";
            if (mn.Contains(CoefTypes.Wins))
            {
                result = position;
            }

            return result;
        }

        private static string Fora(NameEvent eventNameRu, string sn, string mn, bool isAsiat)
        {
            string result = "ERROR";
            string positionTeam = Helper.CheckPositionForNameTeam(sn, eventNameRu);
            string foraValue = sn.Split(' ').LastOrDefault();
            if (isAsiat)
            {
                foraValue = Helper.ConvertAsiatType(sn);
                foraValue = foraValue.Contains("-") ? $"(foraValue)" : $"(+{foraValue})";
            }
            if (mn.Equals(CoefTypes.WinsWithFora)
                || mn.Equals(CoefTypes.WinsWithAsiatFora)
                || mn.Equals(CoefTypes.WinsWithForaVolleyball))
            {
                result = CoefTypes.SimpleFora(positionTeam, foraValue);
            }
            else if (Helper.IsPartGame(mn))
            {
                string otherType = Helper.CheckPartGame(mn);
                string numHalf = RegexByTags(mn, "([0-9])");
                result = CoefTypes.OtherFora(positionTeam, foraValue, otherType + numHalf);
            }
            if ("ERROR".Equals(result))
            {
                //todo code here is missed, log it or remove
            }
            return result;
        }

        private static string Total(NameEvent eventNameRu, string sn, string mn, bool isAsiat)
        {
            string result = "ERROR";
            string underOrOver = Helper.CheckTotal(sn);
            string totalValue = sn.Split(' ').LastOrDefault();
            if (isAsiat)
            {
                totalValue = $"{Helper.ConvertAsiatType(sn)}";
            }
            if (mn.Equals(CoefTypes.Total)
                || mn.Equals(CoefTypes.TotalAsiat)
                || mn.Equals(CoefTypes.TotalVolleyball))
            {
                if (sn.Contains(CoefTypes.Odd))
                    result = CoefTypes.OddPart;
                else if (sn.Contains(CoefTypes.Even))
                    result = CoefTypes.EvenPart;
                else if (!string.IsNullOrEmpty(totalValue) && !string.IsNullOrEmpty(underOrOver))
                    result = CoefTypes.SimpleTotal(totalValue, underOrOver);
            }
            else if (Helper.IsPartGame(mn))
            {
                string otherType = Helper.CheckPartGame(mn);

                string numHalf = RegexByTags(mn, "([0-9])");
                if (!string.IsNullOrEmpty(numHalf))
                    result = CoefTypes.OtherTotal(totalValue, underOrOver, otherType + numHalf);
            }
            else if (mn.Contains(CoefTypes.Total) && (mn.Contains(eventNameRu.NameTeam1.ToLower()) || mn.Contains(eventNameRu.NameTeam2.ToLower())))
            {
                result = CoefTypes.OtherTotal(totalValue, underOrOver,
                    CoefTypes.Team + Helper.CheckPositionForNameTeam(mn, eventNameRu));
            }
            if ("ERROR".Equals(result))
            {
            }
            return result;
        }
    }
}