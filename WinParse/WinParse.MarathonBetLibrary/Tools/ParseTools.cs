using MarathonBetLibrary.Model;
using MarathonBetLibrary.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarathonBetLibrary.Tools
{
    public class ParseTools
    {
        public static DataMarathonForAutoPlays ParseAutoPlay(string line, string selectionKey) => new DataMarathonForAutoPlays()
        {
            sn = RegexByTags(line, Tags.SN),
            mn = RegexByTags(line, Tags.MN),
            ewc = RegexByTags(line, Tags.EWC),
            cid = RegexByTags(line, Tags.CID),
            prt = RegexByTags(line, Tags.PRT),
            ewf = RegexByTags(line, Tags.EWF),
            epr = RegexByTags(line, Tags.EPR),
            prices = RegexByTags(line, Tags.PRICES)
                                .Replace('\"', ' ')
                                .Split(new char[] { ',', ':' }, StringSplitOptions.RemoveEmptyEntries)
                                .Where((x, y) => y % 2 != 0)
                                .ToList(),
            selection_key = selectionKey
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

        public static NameEvent CreateEventName(string eventName, string QueueTeams)
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

            string[] nums = QueueTeams.Split('-');
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

        public static string TypeCoef(NameEvent EventNameRU, string sn, string mn, bool isAsiat)
        {
            sn = sn.ToLower();
            mn = mn.ToLower();
            string result = string.Empty;
            if (mn.Contains("результат"))
            {
                result = Results(EventNameRU, sn, mn);
            }
            else if (mn.Equals("победа в матче"))
            {
                result = ResultsForVolleyball(EventNameRU, sn, mn);
            }
            else if (mn.Contains("победа") && mn.Contains("учетом") && mn.Contains("форы"))
            {
                result = Fora(EventNameRU, sn, mn, isAsiat);
            }
            else if (mn.Contains("тотал голов") || mn.Contains("тотал матча по очкам"))
            {
                result = Total(EventNameRU, sn, mn, isAsiat);
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

        private static string Results(NameEvent EventNameRU, string sn, string mn)
        {
            string result = "ERROR";
            if (sn.Contains(CoefTypes.WINS))
            {
                if (!sn.Contains(CoefTypes.DRAW))
                {
                    result = Helper.CheckPositionForNameTeam(sn, EventNameRU);
                    if (sn.Contains(EventNameRU.NameTeam1.ToLower()) && sn.Contains(EventNameRU.NameTeam2.ToLower()))
                        result = "12";
                }
                else
                {
                    string position = Helper.CheckPositionForNameTeam(sn, EventNameRU);
                    if ("1".Equals(position))
                        result = "1X";
                    else if ("2".Equals(position))
                        result = "X2";
                }
            }
            else if (sn.Contains(CoefTypes.DRAW))
            {
                result = "X";
            }
            if (Helper.IsPartGame(mn))
            {
                string otherType = Helper.CheckPartGame(mn);
                result += otherType + RegexByTags(mn, "([0-9])");
            }
            if (mn.Equals(CoefTypes.RESULTS_DRAW))
            {
                result = CoefTypes.RESULTS_DRAW_PART + "(" + Helper.TrueOrFalse(sn) + ")";
            }
            if ("ERROR".Equals(result))
            {
            }
            return result;
        }

        private static string ResultsForVolleyball(NameEvent EventNameRU, string sn, string mn)
        {
            string position = Helper.CheckPositionForNameTeam(sn, EventNameRU);
            string result = "ERROR";
            if (mn.Contains(CoefTypes.WINS))
            {
                result = position;
            }

            return result;
        }

        private static string Fora(NameEvent EventNameRU, string sn, string mn, bool isAsiat)
        {
            string result = "ERROR";
            string positionTeam = Helper.CheckPositionForNameTeam(sn, EventNameRU);
            string foraValue = sn.Split(' ').LastOrDefault();
            if (isAsiat)
            {
                foraValue = Helper.ConvertAsiatType(sn);
                foraValue = foraValue.Contains("-") ? $"(foraValue)" : $"(+{foraValue})";
            }
            if (mn.Equals(CoefTypes.WINS_WITH_FORA)
                || mn.Equals(CoefTypes.WINS_WITH_ASIAT_FORA)
                || mn.Equals(CoefTypes.WINS_WITH_FORA_VOLLEYBALL))
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

        private static string Total(NameEvent EventNameRU, string sn, string mn, bool isAsiat)
        {
            string result = "ERROR";
            string underOrOver = Helper.CheckTotal(sn);
            string totalValue = sn.Split(' ').LastOrDefault();
            if (isAsiat)
            {
                totalValue = $"{Helper.ConvertAsiatType(sn)}";
            }
            if (mn.Equals(CoefTypes.TOTAL)
                || mn.Equals(CoefTypes.TOTAL_ASIAT)
                || mn.Equals(CoefTypes.TOTAL_VOLLEYBALL))
            {
                if (sn.Contains(CoefTypes.ODD))
                    result = CoefTypes.ODD_PART;
                else if (sn.Contains(CoefTypes.EVEN))
                    result = CoefTypes.EVEN_PART;
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
            else if (mn.Contains(CoefTypes.TOTAL) && (mn.Contains(EventNameRU.NameTeam1.ToLower()) || mn.Contains(EventNameRU.NameTeam2.ToLower())))
            {
                result = CoefTypes.OtherTotal(totalValue, underOrOver,
                    CoefTypes.TEAM + Helper.CheckPositionForNameTeam(mn, EventNameRU));
            }
            if ("ERROR".Equals(result))
            {
            }
            return result;
        }
    }
}