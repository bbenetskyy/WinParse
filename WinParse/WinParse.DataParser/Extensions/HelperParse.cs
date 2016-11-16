using FormulasCollection.Models;
using System.Text;

namespace DataParser.Extensions
{
    public static class HelperParse
    {
        public static string Substrings(this string line, string start, string end = "</")
        {
            string replaceStartElement = "@@";
            string replaceEndElement = "##";
            line = line.Replace(start, replaceStartElement).Replace(end, replaceEndElement);
            int indexStart = line.IndexOf(replaceStartElement) + replaceStartElement.Length;
            int indexEnd = line.IndexOf(replaceEndElement);
            return line.Substring(indexStart, indexEnd - indexStart);
        }

        public static string GetAttribut(this string line, int i = -1, bool date = false)
        {
            bool isStartTag = true;
            bool isFinish = false;
            string result = "";
            foreach (var l in line)
            {
                if (l == '<')
                    isStartTag = false;
                if (isStartTag)
                    result += l;
                if (l == '>')
                    isStartTag = true;
                if (string.IsNullOrEmpty(result.Trim()) && !isStartTag)
                    result = "";
                if (!string.IsNullOrEmpty(result.Trim()) && !isStartTag)
                {
                    isFinish = true;
                    result += (date && !result.Contains("/2016")) ? "/2016 " : "";

                }
                if (isFinish && !date)
                    return result;
            }
            return !date ? "" : result;
        }
        public static string GetAttribut2(this string line)
        {
            string res = string.Empty;
            bool start = false;
            foreach (var l in line)
            {
                if ('\"' == l)
                    start = !start;
                if (start)
                {
                    if (l != '\"')
                        res += l;
                }
            }

            return res.Replace(" ","");
        }
        public static string getValueWithoutStartTags(this string line)
        {
            string result = string.Empty;
            foreach (var l in line)
            {
                if (l.Equals('<'))
                    break;
                result += l;
            }
            return result;
        }

        public static bool _Contains(this string line, params string[] elements)
        {
            foreach (var e in elements)
                if (!line.Contains(e)) return false;
            return true;
        }

        public static string GetEventID(this string line)
        {
            string eventid = null;
            int start = line.IndexOf(MarathonTags.EventID) + MarathonTags.EventID.Length + 2;
            line = line.Substring(start);
            eventid = line.Substring(0, line.IndexOf("\""));
            return eventid;
        }
        public static string TagsContent(this string line, string nameTag)
        {
            string findTag = "";
            int index = 0;
            bool selectContent = false;
            string result = "";
            foreach (var l in line)
            {
                if (findTag.Length > nameTag.Length)
                    return null;
                if (findTag == nameTag)
                {
                    if (selectContent && l != '\"')
                        result += l;
                    if (l == '\"')
                        selectContent = !selectContent;
                    if (!selectContent && !string.IsNullOrEmpty(result))
                        return result;
                }
                else
                {
                    if (l == nameTag[index])
                    {
                        findTag += l;
                        index++;
                    }
                }
            }
            return result;


        }

        public static string TagsContent2(this string line, string nameTag)
        {
            string findTag = "";
            int index = 0;
            bool selectContent = false;
            string result = "";
            foreach (var l in line)
            {
                if (findTag.Length > nameTag.Length)
                    return null;
                if (findTag == nameTag)
                {
                    if (selectContent && l != '\'')
                        result += l;
                    if (l == '\'')
                        selectContent = !selectContent;
                    if (!selectContent && !string.IsNullOrEmpty(result))
                        return result;
                }
                else
                {
                    if (l == nameTag[index])
                    {
                        findTag += l;
                        index++;
                    }
                }
            }
            return result;


        }

        public static bool CheckFullData(this DataMarathonForAutoPlays obj)
        {
            if (obj == null) return false;
            return obj != null &&
                   obj.cid != null &&
                   obj.epr != null &&
                   obj.ewc != null &&
                   obj.ewf != null &&
                   obj.mn != null &&
                   obj.prices != null &&
                   obj.prt != null &&
                   obj.selection_key != null &&
                   obj.sn != null;
        }
        public static string EventToString(this ResultForForks ev)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(ev.SportType))
            {
                sb.Append("   SportType:  " + ev.SportType);
            }
            else sb.Append("   SportType:  " + "NULL");

            if (!string.IsNullOrEmpty(ev.Event))
            {
                sb.Append("   Event:  " + ev.Event);
            }
            else sb.Append("   Event:  " + "NULL");

            if (!string.IsNullOrEmpty(ev.Type))
            {
                sb.Append("   Type:  " + ev.Type);
            }
            else sb.Append("   Type:  " + "NULL");

            if (!string.IsNullOrEmpty(ev.Coef))
            {
                sb.Append("   Coef:  " + ev.Coef);
            }
            else sb.Append("   Coef:  " + "NULL");


            if (!string.IsNullOrEmpty(ev.MatchDateTime))
            {
                sb.Append("   MatchDateTime:  " + ev.MatchDateTime);
            }
            else sb.Append("   MatchDateTime:  " + "NULL");





            if (!string.IsNullOrEmpty(ev.EventId))
            {
                sb.Append("   EventId:  " + ev.EventId);
            }
            else sb.Append("   EventId:  " + "NULL");


            if (!string.IsNullOrEmpty(ev.League))
            {
                sb.Append("   League:  " + ev.League);
            }
            else sb.Append("   League:  " + "NULL");

            if (!string.IsNullOrEmpty(ev.Bookmaker))
            {
                sb.Append("   Bookmaker:  " + ev.Bookmaker);
            }
            else sb.Append("   Bookmaker:  " + "NULL");

            return sb.ToString();
        }
        public static bool isFullData(this ResultForForks team)
        {
            if (team == null) return false;
            else
            {
                return !string.IsNullOrEmpty(team.Event)
                      && !string.IsNullOrEmpty(team.Type)
                      && !string.IsNullOrEmpty(team.Coef)
                      && !string.IsNullOrEmpty(team.EventId)
                      && !string.IsNullOrEmpty(team.League)
                      && !string.IsNullOrEmpty(team.Bookmaker)
                      && !string.IsNullOrEmpty(team.SportType)
                      && !string.IsNullOrEmpty(team.MatchDateTime)
                      && team.marathonAutoPlay.CheckFullData();
            }
        }
        public static bool Validate(this ResultForForks team)
        {
            if (team == null) return false;
            else
            {
                return !string.IsNullOrEmpty(team.EventId)
                     && !string.IsNullOrEmpty(team.MatchDateTime)
                     && team.marathonAutoPlay.CheckFullData();
                     //&& team.AllCoef != null;
            }
        }

    }
}