using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WinParse.MarathonBetLibrary.Model;
using WinParse.MarathonBetLibrary.Setup;

namespace WinParse.MarathonBetLibrary.Tools
{
    public class Helper
    {
        public static string CheckPositionForNameTeam(string sn, NameEvent eventNameRu)
        {
            string result = "NULL";
            if (!sn.Contains(eventNameRu.NameTeam1.ToLower()) && !sn.Contains(eventNameRu.NameTeam2.ToLower()))
            {
                string res1 = CheckExistTeamName(sn, eventNameRu.NameTeam1);
                if (res1 != "NULL") eventNameRu.NameTeam1 = res1;
                else
                {
                    string res2 = CheckExistTeamName(sn, eventNameRu.NameTeam2);
                    if (res2 != "NULL") eventNameRu.NameTeam2 = res2;
                }
            }
            if (sn.Contains("&#39;"))
            {
                sn = sn.Replace("&#39;", "'");
            }
            if (sn.Contains(eventNameRu.NameTeam1.ToLower()))
                result = eventNameRu.PositionTeam(eventNameRu.NameTeam1);
            else if (sn.Contains(eventNameRu.NameTeam2.ToLower()))
                result = eventNameRu.PositionTeam(eventNameRu.NameTeam2);
            if (result.Equals("NULL"))
            {
            }
            return result;
        }

        public static string CheckTotal(string sn)
        {
            string result = "NULL";
            if (sn.Contains(CoefTypes.Under))
                result = "U";
            else if (sn.Contains(CoefTypes.Over))
                result = "O";
            else if (sn.Contains(CoefTypes.Odd))
            {
                result = CoefTypes.OddPart;
            }
            else if (sn.Contains(CoefTypes.Even))
            {
                result = CoefTypes.EvenPart;
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

        public static string ContainsTeamRuAndEn(string nameEventRu, string nameEventEn)
        {
            if (nameEventEn == null) return nameEventRu;
            string newNameRu = string.Empty;

            string[] splitRu = nameEventRu.Split(' ');
            string[] splitEn = nameEventEn.Split(' ');
            string result = string.Empty;
            newNameRu = nameEventRu.Replace(" - ", " # ");
            return newNameRu;
        }

        public static string CheckExistTeamName(string sn, string nameTeam)
        {
            nameTeam = nameTeam.ToLower();
            string newSn = string.Empty;
            string newNameTeam = string.Empty;
            int index = 0;
            for (int i = 0; i < sn.Length; i++)
            {
                if (index == nameTeam.Length)
                {
                    if (!string.IsNullOrEmpty(newSn) && !string.IsNullOrEmpty(newNameTeam) && newSn.Equals(newNameTeam))
                        return newNameTeam;
                    else
                        return "NULL";
                }
                if (sn[i] == nameTeam[index])
                {
                    newSn += sn[i].ToString();
                    newNameTeam += nameTeam[index].ToString();
                }
                else
                {
                    newNameTeam = string.Empty;
                    newSn = string.Empty;
                    i = -1;
                }

                index++;
            }
            return "NULL";
        }

        public static bool IsPartGame(string mn)
        {
            bool result = false;
            result = mn.Contains(CoefTypes.Half)
                || mn.Contains(CoefTypes.Half2)
                || mn.Contains(CoefTypes.Quarter)
                || mn.Contains(CoefTypes.Period)
                || mn.Contains(CoefTypes.Game)
                || mn.Contains(CoefTypes.Set)
                || mn.Contains(CoefTypes.Party);
            return result;
        }

        public static string CheckPartGame(string mn)
        {
            string result = "ERROR";
            if (mn.Contains(CoefTypes.Half) || mn.Contains(CoefTypes.Half2))
                result = CoefTypes.HalfPart;
            else if (mn.Contains(CoefTypes.Quarter))
                result = CoefTypes.QuarterPart;
            else if (mn.Contains(CoefTypes.Period))
                result = CoefTypes.PeriodPart;
            else if (mn.Contains(CoefTypes.Game))
                result = CoefTypes.GamePart;
            else if (mn.Contains(CoefTypes.Set))
                result = CoefTypes.SetPart;
            else if (mn.Contains(CoefTypes.Party))
                result = CoefTypes.PartyPart;
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

        public static bool CheckCountEventsInPage(string html)
        {
            var eventsId = new List<string>();
            foreach (Match match in Regex.Matches(html, Tags.EventIdFull))
            {
                string id = match.Groups[1].Value;
                if (!eventsId.Any(x => x.Equals(id)))
                    eventsId.Add(id);
            }
            return eventsId.Count < 2;
        }
    }
}