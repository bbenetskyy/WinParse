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
    public class Helper
    {
        public static string CheckPositionForNameTeam(string sn, NameEvent EventNameRU)
        {
            string result = "NULL";
            if (!sn.Contains(EventNameRU.NameTeam1.ToLower()) && !sn.Contains(EventNameRU.NameTeam2.ToLower()))
            {
                string res1 = CheckExistTeamName(sn, EventNameRU.NameTeam1);
                if (res1 != "NULL") EventNameRU.NameTeam1 = res1;
                else
                {
                    string res2 = CheckExistTeamName(sn, EventNameRU.NameTeam2);
                    if (res2 != "NULL") EventNameRU.NameTeam2 = res2;
                }
            }
            if (sn.Contains("&#39;"))
            {
                sn = sn.Replace("&#39;", "'");
            }
            if (sn.Contains(EventNameRU.NameTeam1.ToLower()))
                result = EventNameRU.PositionTeam(EventNameRU.NameTeam1);
            else if (sn.Contains(EventNameRU.NameTeam2.ToLower()))
                result = EventNameRU.PositionTeam(EventNameRU.NameTeam2);
            if (result.Equals("NULL"))
            {
            }
            return result;
        }

        public static string CheckTotal(string sn)
        {
            string result = "NULL";
            if (sn.Contains(CoefTypes.UNDER))
                result = "U";
            else if (sn.Contains(CoefTypes.OVER))
                result = "O";
            else if (sn.Contains(CoefTypes.ODD))
            {
                result = CoefTypes.ODD_PART;
            }
            else if (sn.Contains(CoefTypes.EVEN))
            {
                result = CoefTypes.EVEN_PART;
            }
            if (result.Equals("NULL"))
            {
            }
            return result;
        }

        public static string ConvertAsiatType(string sn)
        {
            try
            {
                string result = "ERROR_ASIAT";
                string[] split = string.Join("", ParseTools.RegexByTagsMany(sn, "(([-]|[+]|[0-9]|[.]|[0-9]*)(,*)([-]|[+]|[0-9]|[.]|[0-9]*))")).Replace(" ", "").Split(',');
                double num1 = Double.Parse(CheckCharacters(split[0]));
                double num2 = Double.Parse(CheckCharacters(split[1]));
                result = ((num1 + num2) / 2).ToString();
                return result;
            }
            catch { return "ERROR_ASIAT"; }
        }

        private static string CheckCharacters(string line)
        {
            string res = line;
            if (line.Where(x => x.Equals('-')).Count() > 0)
            {
                res = "-" + line.Replace("-", "");
            }
            if (line.Where(x => x.Equals('+')).Count() > 0)
            {
                res = "+" + line.Replace("+", "");
            }
            if (line.Contains("+") && line.Contains("-"))
            {
                res = line.Substring(1);
            }
            return res;
        }

        public static string TrueOrFalse(string sn)
        {
            return sn.Equals("да") ? "T" : "F";
        }

        public static string ContainsTeamRuAndEn(string nameEventRU, string nameEventEN)
        {
            if (nameEventEN == null) return nameEventRU;
            string newNameRU = string.Empty;

            string[] splitRU = nameEventRU.Split(' ');
            string[] splitEN = nameEventEN.Split(' ');
            string result = string.Empty;
            newNameRU = nameEventRU.Replace(" - ", " # ");
            return newNameRU;
        }

        public static string CheckExistTeamName(string sn, string nameTeam)
        {
            nameTeam = nameTeam.ToLower();
            string newSN = string.Empty;
            string newNameTeam = string.Empty;
            int index = 0;
            for (int i = 0; i < sn.Length; i++)
            {
                if (index == nameTeam.Length)
                {
                    if (!string.IsNullOrEmpty(newSN) && !string.IsNullOrEmpty(newNameTeam) && newSN.Equals(newNameTeam))
                        return newNameTeam;
                    else
                        return "NULL";
                }
                if (sn[i] == nameTeam[index])
                {
                    newSN += sn[i].ToString();
                    newNameTeam += nameTeam[index].ToString();
                }
                else
                {
                    newNameTeam = string.Empty;
                    newSN = string.Empty;
                    i = -1;
                }

                index++;
            }
            return "NULL";
        }

        public static bool IsPartGame(string mn)
        {
            bool result = false;
            result = mn.Contains(CoefTypes.HALF)
                || mn.Contains(CoefTypes.HALF2)
                || mn.Contains(CoefTypes.QUARTER)
                || mn.Contains(CoefTypes.PERIOD)
                || mn.Contains(CoefTypes.GAME)
                || mn.Contains(CoefTypes.SET)
                || mn.Contains(CoefTypes.PARTY);
            return result;
        }

        public static string CheckPartGame(string mn)
        {
            string result = "ERROR";
            if (mn.Contains(CoefTypes.HALF) || mn.Contains(CoefTypes.HALF2))
                result = CoefTypes.HALF_PART;
            else if (mn.Contains(CoefTypes.QUARTER))
                result = CoefTypes.QUARTER_PART;
            else if (mn.Contains(CoefTypes.PERIOD))
                result = CoefTypes.PERIOD_PART;
            else if (mn.Contains(CoefTypes.GAME))
                result = CoefTypes.GAME_PART;
            else if (mn.Contains(CoefTypes.SET))
                result = CoefTypes.SET_PART;
            else if (mn.Contains(CoefTypes.PARTY))
                result = CoefTypes.PARTY_PART;
            return result;
        }

        public static string GetDate(string html)
        {
            bool isDate = false;
            string result = string.Empty;
            string[] lines = html.Split('\n');
            foreach (string item in lines)
            {
                if (isDate)
                {
                    result = item.Trim();
                    break;
                    isDate = false;
                }
                if (item.Contains(Tags.Date))
                {
                    isDate = true;
                }
            }
            return result;
        }

        public static bool CheckCountEventsInPage(string HTML)
        {
            var _eventsID = new List<string>();
            foreach (Match match in Regex.Matches(HTML, Tags.EventID))
            {
                string id = match.Groups[1].Value;
                if (!_eventsID.Any(x => x.Equals(id)))
                    _eventsID.Add(id);
            }
            return _eventsID.Count < 2;
        }
    }
}