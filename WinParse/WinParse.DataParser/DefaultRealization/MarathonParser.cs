
using DataParser.Enums;
using DataParser.Extensions;
using DataParser.Models;
using FormulasCollection.Models;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.DefaultRealization
{
    public class MarathonParser
    {
        #region [Public field]
        public List<ResultForForks> result;
        public List<ResultForForks> newResult;
        #endregion

        #region [Private field]
        private Dictionary<string, EnglishNameTeams> englishNameTeams_Dictionary;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private PhantomJSDriver _driver;
        OpenQA.Selenium.Firefox.FirefoxDriver firefox;
        private string isClick_IdEvent = ".";
        private string RefreshPage = "";



        private List<string> errorReadEvent = null;
        #endregion

        #region[Static field]
        public static List<ResultForForks> winik = new List<ResultForForks>();
        #endregion

        #region [Constructors]
        public MarathonParser()
        {
            #region Selenium
            /*
                        var prof = new FirefoxProfile();
                        prof.SetPreference("browser.startup.homepage_override.mstone",
                            "ignore");
                        prof.SetPreference("startup.homepage_welcome_url.additional",
                            "about:blank");
                        prof.EnableNativeEvents = false;

                         firefox = new OpenQA.Selenium.Firefox.FirefoxDriver(prof);
                         firefox.Manage().Window.Maximize();
                         firefox.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromMinutes(35));
                         firefox.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromMinutes(35));
                         firefox.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromMinutes(35));*/

            /*
             var driverService = PhantomJSDriverService.CreateDefaultService(); 
             _driver = new PhantomJSDriver();
             _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromMinutes(35));
             _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromMinutes(35));
             _driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromMinutes(35));*/
            #endregion

            result = new List<ResultForForks>();
            newResult = new List<ResultForForks>();
            errorReadEvent = new List<string>();
            // Initi(sportType);
        }

        public MarathonParser(SportType sportType)
        {

        }
        #endregion

        #region [Initi]

        public List<ResultForForks> InitiMultiThread(SportType sportType)
        {
            var result2 = new List<ResultForForks>();

            try
            {
                //var ids = LoadID(SportType.Basketball);
                result2.AddRange(GetAllEvents(sportType));
            }
            catch (Exception e)
            {
                var ee = e.StackTrace;
                var eee = e.Message;
            }
            return result2;
        }

        private IEnumerable<ResultForForks> GetAllEvents(SportType sportType)
        {
            var result = new List<ResultForForks>();
            var englishTeams = GetEnglishNameTEams(sportType);
            var russianTeams = GetEnglishNameTEams(sportType, true);

            var ids = englishTeams.Keys.ToList();

            var tasks = new Task<List<ResultForForks>>[ids.Count];
            var idCounter = 0;

            for (var index = 0; index < ids.Count; index++)
            {
                // ReSharper disable once AccessToModifiedClosure
                tasks[index] = Task.Factory.StartNew(() => LoadEvent(ids[idCounter++], sportType, englishTeams, russianTeams));
            }
            Task.WaitAll(tasks);
            foreach (var task in tasks)
            {
                if (task.Result != null)
                    result.AddRange(task.Result);
            }


            return result;
        }

        #endregion

        #region [Load Event for ID]
        public List<ResultForForks> LoadEvent(string eventID, SportType sportType, Dictionary<string, EnglishNameTeams> englishNameTeams_Dictionary, Dictionary<string, EnglishNameTeams> russianNameTeams_Dictionary)
        {
            ResultForForks teamToAdd = null;

            DataMarathonForAutoPlays obj = new DataMarathonForAutoPlays();
            List<ResultForForks> resEvent = null;
            teamToAdd = new ResultForForks();
            teamToAdd.EventId = eventID;
            teamToAdd.League = "NON";
            teamToAdd.SportType = sportType.ToString();
            teamToAdd.Bookmaker = Site.MarathonBet.ToString();
            if (englishNameTeams_Dictionary.ContainsKey(teamToAdd.EventId))
            {
                teamToAdd.Event = englishNameTeams_Dictionary[teamToAdd.EventId].name1 + " - " + englishNameTeams_Dictionary[teamToAdd.EventId].name2;
                // teamToAdd.Event_RU = russianNameTeams_Dictionary[teamToAdd.EventId].name1 + " - " + russianNameTeams_Dictionary[teamToAdd.EventId].name2;
                teamToAdd.Event_RU = russianNameTeams_Dictionary[teamToAdd.EventId].eventRU;
            }
            else
            {
                _logger.Error("Event not found!!!    EventID :  " + teamToAdd.EventId);
            }
            try
            {
                resEvent = FullParse(eventID, teamToAdd, sportType);
            }
            catch (Exception e)
            {
                _logger.Error("GetNameTeamsAndDateAsync(SportType sportType)\n\n\n if(isEventID) :" + eventID + "\n\n\n\n" + e.Message + "\n\n\n\n\n" + e.StackTrace);
            }
            return resEvent;
        }
        public Dictionary<string,string> LoadID(SportType sportType)
        {
            Dictionary<string, string> ListIDforSelectedSport = new Dictionary<string, string>();
            //string
            string url = string.Empty;
            string namefile = string.Empty;
            //string selectedEvent = string.Empty;
            string eventid = string.Empty;
            string _teamName = string.Empty;
            //string totalOrFora = string.Empty;

            //bool
            bool isEventID = false;
            bool isTeamName = false;


            UrlAndNameFile(sportType, out url, out namefile);
            string gotHtml = Html(url);
            string[] lines = gotHtml.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(">Итоги<") || lines[i].Contains(">Итоги.<"))
                    break;
                if (lines[i]._Contains(MarathonTags.mainTagForEvent, MarathonTags.newEventID))
                {
                    isEventID = true;
                }
                if (lines[i].Contains(MarathonTags.newTeamName))
                {
                    isTeamName = true;
                }
                if (isEventID)
                {
                    string eventID = lines[i].GetEventID();
                    try
                    {
                        //ListIDforSelectedSport.Add(eventID);
                    }
                    catch (Exception e)
                    {
                        _logger.Error("GetNameTeamsAndDateAsync(SportType sportType)\n\n\n if(isEventID) :" + eventID + "\n\n\n i = " + i + "\n\n\n\n" + e.Message + "\n\n\n\n\n" + e.StackTrace);
                    }
                    isEventID = false;
                    eventid = eventID;

                }
                if (isTeamName)
                {

                    if (string.IsNullOrEmpty(_teamName))
                    {
                        var teamName = lines[i].GetAttribut();
                        _teamName = teamName.Trim(' ');
                    }
                    else
                    {
                        i++;
                        var teamName = lines[i].GetAttribut();
                        _teamName += " - " + teamName.Trim(' ');
                        isTeamName = false;
                    }
                }
                if(!isTeamName && !string.IsNullOrEmpty(_teamName) && !isEventID && !string.IsNullOrEmpty(eventid))
                {
                    ListIDforSelectedSport[eventid] = _teamName;
                }
            }
            return ListIDforSelectedSport;
        }
        #endregion

        #region [Parse]

        private List<ResultForForks> GetNameTeamsAndDateAsync(SportType sportType)
        {
            result.Clear();

            ResultForForks teamToAdd = null;

            //List<string>
            List<string> list_coeffType = new List<string>();


            //string
            string url = string.Empty;
            string namefile = string.Empty;
            //string selectedEvent = string.Empty;
            string eventid = string.Empty;
            //string totalOrFora = string.Empty;

            //bool
            bool isEventID = false;

            DataMarathonForAutoPlays obj = new DataMarathonForAutoPlays();
            List<ResultForForks> resEvent = null;


            UrlAndNameFile(sportType, out url, out namefile);
            string gotHtml = Html(url);
            string[] lines = gotHtml.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(">Итоги<") || lines[i].Contains(">Итоги.<"))
                    break;
                if (lines[i]._Contains(MarathonTags.mainTagForEvent, MarathonTags.newEventID))
                {
                    isEventID = true;
                }
                if (isEventID)
                {
                    string eventID = lines[i].GetEventID();
                    teamToAdd = new ResultForForks();
                    teamToAdd.EventId = eventID;
                    teamToAdd.League = "NON";
                    teamToAdd.SportType = sportType.ToString();
                    teamToAdd.Bookmaker = Site.MarathonBet.ToString();
                    if (englishNameTeams_Dictionary.ContainsKey(teamToAdd.EventId))
                    {
                        teamToAdd.Event = englishNameTeams_Dictionary[teamToAdd.EventId].name1 + " # " + englishNameTeams_Dictionary[teamToAdd.EventId].name2;
                    }
                    else
                    {
                        _logger.Error("Event not found!!!    EventID :  " + teamToAdd.EventId);
                    }
                    try
                    {
                        resEvent = FullParse(eventID, teamToAdd, sportType);
                    }
                    catch (Exception e)
                    {
                        _logger.Error("GetNameTeamsAndDateAsync(SportType sportType)\n\n\n if(isEventID) :" + eventID + "\n\n\n i = " + i + "\n\n\n\n" + e.Message + "\n\n\n\n\n" + e.StackTrace);
                    }
                    isEventID = false;
                    eventid = eventID;

                }
                if (resEvent != null)
                {

                    ResultForForks newTseamToAdd = teamToAdd;
                    result.AddRange(resEvent);
                    //result.Add(newTseamToAdd);
                    teamToAdd = new ResultForForks();
                    resEvent = null;
                }
                else
                {
                    if (!string.IsNullOrEmpty(eventid))
                    {
                        errorReadEvent.Add(sportType.ToString() + " - " + eventid.ToString());
                    }

                }
            }
            try
            {
                //WriteToDocument(errorReadEvent, "error.txt");
            }
            catch
            {
                int y = 0;
            }
            return result;
        }
        private List<ResultForForks> GetNameTeamsAndDateAsync2(SportType sportType)
        {
            result.Clear();

            ResultForForks teamToAdd = null;

            //List<string>
            List<string> list_coeffType = new List<string>();

            //integer
            int indexEvent = 0;

            //string
            string url = string.Empty;
            string namefile = string.Empty;
            //string selectedEvent = string.Empty;
            string eventid = string.Empty;
            //string totalOrFora = string.Empty;

            //bool
            bool isTypeCoff = false;
            bool canParseDataToTeam = false;
            bool isEventID = false;
            //bool isTeamName = false;
            bool isDate = false;
            bool isDataToAutoPlay = false;
            bool isSelectionKey = false;
            bool isValueCoef = false;
            bool isFora = false;
            bool isTotal = false;

            //bool isChangeEvent = false;

            bool isLigue = false;

            DataMarathonForAutoPlays obj = new DataMarathonForAutoPlays();



            UrlAndNameFile(sportType, out url, out namefile);
            string gotHtml = Html(url);
            string[] lines = gotHtml.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    if (lines[i].Contains(">Итоги<") || lines[i].Contains(">Итоги.<"))
                        break;
                    if (lines[i].Contains(MarathonTags.newTypeCoeff))
                    {
                        isTypeCoff = true;
                        indexEvent = 0;
                        list_coeffType = new List<string>();
                    }
                    else if (isTypeCoff && lines[i].Contains(MarathonTags.newTag_TypeCoeff))
                    {
                        isTypeCoff = false;
                    }
                    if (lines[i]._Contains(MarathonTags.mainTagForEvent, MarathonTags.newEventID))
                    {
                        canParseDataToTeam = true;
                        isEventID = true;
                    }
                    /*else if (lines[i].Contains(MarathonTags.newTeamName))
                    {
                        isTeamName = true;
                    }*/
                    else if (lines[i].Contains(MarathonTags.newDate))
                    {
                        isDate = true;
                    }
                    else if (lines[i].Contains(MarathonTags.newAutoplay))
                    {
                        isDataToAutoPlay = true;
                    }
                    else if (lines[i].Contains(MarathonTags.newSelection_key))
                    {
                        isSelectionKey = true;
                    }
                    else if (lines[i].Contains(MarathonTags.newValueCoef))
                    {
                        isValueCoef = true;
                    }
                    else if (lines[i].Contains(MarathonTags.newFora))
                    {
                        isFora = true;
                    }
                    else if (lines[i].Contains(MarathonTags.newTotal))
                    {
                        isTotal = true;
                    }


                    //Parse
                    if (isTypeCoff)
                    {
                        if (lines[i].Contains(MarathonTags.newclassToTypeCoef))
                        {
                            i++;
                            string typeCoeff = string.Empty;
                            if (lines[i].Contains("<b>"))
                            {
                                string start = lines[i].getValueWithoutStartTags().Trim();
                                start = start.Replace(" ", "");
                                string newLine = !string.IsNullOrEmpty(start) ? lines[i].Replace(start, "") : lines[i];
                                start = start.Equals("Фора") ? "F" : start;
                                //start = start.Equals("Тотал") ? "T" : start;
                                typeCoeff = newLine.GetAttribut();
                                typeCoeff = start + typeCoeff;
                            }
                            else
                            {
                                typeCoeff = lines[i].Contains("Меньше") ? "TU" : (lines[i].Contains("Больше") ? "TO" : "NOForaNOTotal");

                            }
                            typeCoeff = typeCoeff.Replace(" ", "");
                            if (!string.IsNullOrEmpty(typeCoeff))
                            {
                                if (list_coeffType.Equals(typeCoeff))
                                {
                                    isTypeCoff = false;
                                }
                                else
                                {
                                    list_coeffType.Add(typeCoeff);
                                }
                            }
                        }
                    }


                    if (isEventID)
                    {
                        string eventID = lines[i].GetEventID();
                        if (!eventID.Equals(eventid))
                        {
                            teamToAdd = new ResultForForks();
                            indexEvent = 0;
                        }
                        teamToAdd.EventId = eventID;
                        //FullParse(eventID);
                        indexEvent = 0;
                        isTypeCoff = false;
                        isEventID = false;
                        eventid = eventID;

                    }

                    /*if (isTeamName)
                    {
                        
                        if (string.IsNullOrEmpty(teamToAdd.Event))
                        {
                            var teamName = lines[i].GetAttribut();
                            //teamToAdd.Event = teamName.Trim(' ');
                        }
                        else
                        {
                            i++;
                            var teamName = lines[i].GetAttribut();
                            //teamToAdd.Event += " - " + teamName.Trim(' ');
                            isTeamName = false;
                        }
                    }*/
                    if (isDate)
                    {
                        i++;
                        teamToAdd.MatchDateTime = lines[i].Replace(" ", "");
                        isDate = false;
                    }
                    if (isDataToAutoPlay || isSelectionKey)
                    {
                        if (lines[i].Contains(Tags_DataMarathonForAutoPlays.data_sel))
                        {
                            obj = ParseForAutoPlay(lines[i], Tags_DataMarathonForAutoPlays.data_sel);
                            isDataToAutoPlay = false;
                        }
                        else if (lines[i].Contains(Tags_DataMarathonForAutoPlays.data_selection_key))
                        {
                            obj = ParseForAutoPlay(lines[i], Tags_DataMarathonForAutoPlays.data_selection_key, obj);
                            teamToAdd.marathonAutoPlay = obj;
                            isSelectionKey = false;
                        }

                    }
                    if (isValueCoef)
                    {
                        var valueCoeff = lines[i].GetAttribut2();
                        //teamToAdd.Coef = valueCoeff;
                        isValueCoef = false;
                        /*if (!isFora && !isTotal && string.IsNullOrEmpty(teamToAdd.Type))
                        {
                            indexEvent = (indexEvent >= list_coeffType.Count) ? 0 : indexEvent;
                            var typecoeff = list_coeffType[indexEvent];
                           // teamToAdd.Type = typecoeff;
                            indexEvent++;
                        }*/
                        isValueCoef = false;
                    }
                    if (isFora || isTotal)
                    {
                        i++;
                        indexEvent = (indexEvent >= list_coeffType.Count) ? 0 : indexEvent;
                        string totalOrFora = lines[i].getValueWithoutStartTags();
                        var typecoeff = list_coeffType[indexEvent] + totalOrFora.Replace(" ", "");
                        //teamToAdd.Type = typecoeff;
                        indexEvent++;
                        isFora = false;
                        isTotal = false;
                    }

                    if (teamToAdd != null)
                    {
                        if (teamToAdd.isFullData())
                        {
                            teamToAdd.League = "NON";
                            teamToAdd.SportType = sportType.ToString();
                            teamToAdd.Bookmaker = Site.MarathonBet.ToString();
                            if (englishNameTeams_Dictionary.ContainsKey(teamToAdd.EventId))
                            {
                                teamToAdd.Event = englishNameTeams_Dictionary[teamToAdd.EventId].name1 + " # " + englishNameTeams_Dictionary[teamToAdd.EventId].name2;
                            }
                            else
                            {
                                _logger.Error("Event not found!!!    EventID :  " + teamToAdd.EventId);
                            }
                            var newteamToAdd = teamToAdd;
                            result.Add(new ResultForForks()
                            {
                                Bookmaker = newteamToAdd.Bookmaker,
                                //Type = newteamToAdd.Type,
                                //Coef = newteamToAdd.Coef,
                                Event = newteamToAdd.Event,
                                EventId = newteamToAdd.EventId,
                                League = newteamToAdd.League,
                                MatchDateTime = newteamToAdd.MatchDateTime,
                                marathonAutoPlay = newteamToAdd.marathonAutoPlay,
                                SportType = newteamToAdd.SportType
                            });

                            //teamToAdd.Coef = string.Empty;
                            // teamToAdd.Type = string.Empty;
                            teamToAdd.marathonAutoPlay = null;

                            isTypeCoff = false;
                            canParseDataToTeam = false;
                            isEventID = false;
                            //isTeamName = false;
                            isDate = false;
                            isDataToAutoPlay = false;
                            isSelectionKey = false;
                            isValueCoef = false;
                            isFora = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    /* if (!File.Exists("log.txt"))
                     {
                         File.Create("log.txt");
                     }
                     using (StreamWriter sw = new StreamWriter("log.txt", true))
                     {
                         sw.WriteLine("EXCEPTION: " + teamToAdd.EventToString());
                         sw.Close();
                     }*/
                    _logger.Error(e.StackTrace + "/n" + e.Message);
                }
            }
            return result;
        }

        private List<ResultForForks> FullParse(string event_id, ResultForForks teamToAdd, SportType sportType)
        {
            //https://www.marathonbet.com/su/events.htm?id=4563829

            string reURL = "https://www.marathonbet.com/su/events.htm?id=" + event_id;
            string html = Html(reURL);
            //WriteToDocument(html);
            string[] lines = html.Split('\n');
            bool isDataToAutoPlay = false;
            bool isSelectionKey = false;
            List<DataMarathonForAutoPlays> list = new List<DataMarathonForAutoPlays>();
            DataMarathonForAutoPlays obj = new DataMarathonForAutoPlays();

            bool isDate = false;
            bool isTeamName = false;
            Dictionary<string, string> nameTeams = new Dictionary<string, string>();

            MarathonEvent eventCoefList = new MarathonEvent();

            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    if (lines[i].Contains(MarathonTags.newTeamName))
                    {
                        isTeamName = true;
                    }
                    if (isTeamName)
                    {
                        string numberTeam = lines[i - 1].GetAttribut();
                        var teamName = lines[i].GetAttribut();

                        if (nameTeams.Count >= 2)
                        {

                            bool ifExistsID = result.Find(x => x.EventId.Equals(event_id)) != null;
                            bool ifExists = result.Find(x => x.Event.Equals(teamName)) != null;
                            var findTeams = ifExists ? result.Find(x => x.Event.Equals(teamName)).Event : string.Empty;

                            /*MessageBox.Show("Dublicate teams !!!\n\n\n" + "Teams in dictionaries : \n\n" + "Team1 : " + nameTeams["1"] + "    Team1 : " + nameTeams["2"]
                                + "\n\n Dublicate : " + teamName + (ifExistsID ? ("\nExistID : " + event_id) : "\n") +
                                findTeams);*/
                            break;
                        }

                        nameTeams.Add(numberTeam[0].ToString(), teamName);

                        isTeamName = false;
                    }

                    if (lines[i].Contains(MarathonTags.newDate))
                    {
                        isDate = true;
                    }
                    if (isDate)
                    {
                        i++;
                        teamToAdd.MatchDateTime = lines[i].Replace(" ", "");
                        isDate = false;
                    }

                    if (lines[i].Contains(MarathonTags.newAutoplay))
                    {
                        isDataToAutoPlay = true;
                    }
                    else if (lines[i].Contains(MarathonTags.newSelection_key))
                    {
                        isSelectionKey = true;
                    }

                    if (isDataToAutoPlay || isSelectionKey)
                    {
                        if (lines[i].Contains(Tags_DataMarathonForAutoPlays.data_sel))
                        {
                            obj = ParseForAutoPlay(lines[i], Tags_DataMarathonForAutoPlays.data_sel);
                            isDataToAutoPlay = false;
                        }
                        else if (lines[i].Contains(Tags_DataMarathonForAutoPlays.data_selection_key))
                        {
                            obj = ParseForAutoPlay(lines[i], Tags_DataMarathonForAutoPlays.data_selection_key, obj);
                            list.Add(obj);
                            teamToAdd.marathonAutoPlay = obj;
                            obj = new DataMarathonForAutoPlays();
                            isSelectionKey = false;
                        }

                    }
                }
                catch (Exception e)
                {
                    _logger.Error("Error in FullParse(string event_id)!!" + "\n\n\n");
                }
            }
            if (sportType.ToString().Equals(SportType.Volleyball.ToString()))
            {
                CreateEventVoleyball(list, ref eventCoefList, nameTeams, event_id);
            }
            else
            {
                CreateEvent(list, ref eventCoefList, nameTeams, event_id);
            }
            eventCoefList.EventId = teamToAdd.EventId;
            eventCoefList.Event = teamToAdd.Event;
            eventCoefList.Event_RU = teamToAdd.Event_RU;
            eventCoefList.League = teamToAdd.League;
            eventCoefList.MatchDateTime = teamToAdd.MatchDateTime;
            eventCoefList.Bookmaker = teamToAdd.Bookmaker;
            eventCoefList.SportType = teamToAdd.SportType;

            return ConvertWith_MarathonEvent_To_ListResultForForks(eventCoefList);
        }
        List<string> dictionary = new List<string>() {
            "Победитель матча",
            "Победа с учетом форы",
            "Тотал очков"
        };
        private void CreateEvent(List<DataMarathonForAutoPlays> list, ref MarathonEvent teamToAdd, Dictionary<string, string> nameTeams, string eventID)
        {
            Dictionary<string, string> mainCoef = new Dictionary<string, string>();
            List<EventForAutoPlay> Coef = new List<EventForAutoPlay>();

            for (int i = 0; i < list.Count; i++)
            {
                string type = string.Empty;
                string value = string.Empty;
                try
                {
                    if (list[i].CheckFullData())
                    {
                        switch (list[i].mn)
                        {
                            case "Победитель матча":
                                if (list[i].sn.Equals(nameTeams["1"]))
                                {
                                    if (!mainCoef.ContainsKey("1"))
                                    {
                                        mainCoef.Add("1", list[i].epr);
                                        type = "1";
                                        value = list[i].epr;
                                    }
                                }
                                else if (list[i].sn.Equals(nameTeams["2"]))
                                {
                                    if (!mainCoef.ContainsKey("2"))
                                    {
                                        mainCoef.Add("2", list[i].epr);
                                        type = "2";
                                        value = list[i].epr;
                                    }
                                }
                                //else_logger.Error(l.sn + " - " + l.epr);
                                break;
                            case "Результат":
                                string r = ChangeFormatResult(list[i].sn, nameTeams);
                                if (!mainCoef.ContainsKey(r))
                                {
                                    mainCoef.Add(r, list[i].epr);
                                    type = r;
                                    value = list[i].epr;
                                }
                                //else_logger.Error(l.sn + " - " + l.epr);
                                break;
                            case "Победа с учетом форы":
                                string f = ChangeFormatFora(list[i].sn, nameTeams["1"]);
                                if (!mainCoef.ContainsKey(f))
                                {
                                    mainCoef.Add(f, list[i].epr);
                                    type = f;
                                    value = list[i].epr;
                                }
                                //else_logger.Error(l.sn + " - " + l.epr);
                                break;

                            case "Тотал очков":
                                string t = ChangeFormatTotals(list[i].sn);
                                if (!mainCoef.ContainsKey(t))
                                {
                                    mainCoef.Add(t, list[i].epr);
                                    type = t;
                                    value = list[i].epr;
                                }
                                //else_logger.Error(l.sn + " - " + l.epr);
                                break;

                            //Tennis
                            case "Результат матча":
                                string rr = ChangeFormatResult(list[i].sn, nameTeams);
                                if (!mainCoef.ContainsKey(rr))
                                {
                                    mainCoef.Add(rr, list[i].epr);
                                    type = rr;
                                    value = list[i].epr;
                                }
                                break;
                            /*case "Победа с учетом форы по сетам":
                                string ff = ChangeFormatFora(list[i].sn, nameTeams["1"],eventID);
                                if (!mainCoef.ContainsKey(ff))
                                    mainCoef.Add(ff, list[i].epr);
                                //else_logger.Error(l.sn + " - " + l.epr);
                                break;
                            case "Победа с учетом форы по геймам":
                                string fff = ChangeFormatFora(list[i].sn, nameTeams["1"],eventID);
                                if (!mainCoef.ContainsKey(fff))
                                    mainCoef.Add(fff, list[i].epr);
                                //else_logger.Error(l.sn + " - " + l.epr);
                                break;*/
                            case "Тотал по сетам":
                                string tt = ChangeFormatTotals(list[i].sn);
                                if (!mainCoef.ContainsKey(tt))
                                {
                                    mainCoef.Add(tt, list[i].epr);
                                    type = tt;
                                    value = list[i].epr;
                                }
                                //else_logger.Error(l.sn + " - " + l.epr);
                                break;
                            case "Тотал по геймам":
                                string ttt = ChangeFormatTotals(list[i].sn);
                                if (!mainCoef.ContainsKey(ttt))
                                {
                                    mainCoef.Add(ttt, list[i].epr);
                                    type = ttt;
                                    value = list[i].epr;
                                }
                                //else_logger.Error(l.sn + " - " + l.epr);
                                break;

                        }
                        if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(value))
                        {
                            Coef.Add(new EventForAutoPlay() { EventID = eventID, Type = type, Value = value, marathonAutoPlay = list[i] });
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Error("Error in CreateEvent(List<DataMarathonForAutoPlays> list)\n\n\n\n"
                         + "Origin: " + "[" + i + "]" + list[i].sn + " - " + list[i].epr + "\n\n\n" +
                         mainCoef.Keys.ToString() + " \n\n\n" + mainCoef.Values.ToString());
                }
            }


            teamToAdd.Coef = Coef;
            //teamToAdd.AllCoef = mainCoef;
        }

        private void CreateEventVoleyball(List<DataMarathonForAutoPlays> list, ref MarathonEvent teamToAdd, Dictionary<string, string> nameTeams, string eventID)
        {
            Dictionary<string, string> mainCoef = new Dictionary<string, string>();

            List<EventForAutoPlay> Coef = new List<EventForAutoPlay>();

            for (int i = 0; i < list.Count; i++)
            {
                string type = string.Empty;
                string value = string.Empty;
                try
                {
                    if (list[i].CheckFullData())
                    {
                        switch (list[i].mn)
                        {
                            case "Победа в матче":
                                if (list[i].sn.Equals(nameTeams["1"]))
                                {
                                    if (!mainCoef.ContainsKey("1"))
                                    {
                                        mainCoef.Add("1", list[i].epr);
                                        type = "1";
                                        value = list[i].epr;
                                    }
                                }
                                else if (list[i].sn.Equals(nameTeams["2"]))
                                {
                                    if (!mainCoef.ContainsKey("2"))
                                    {
                                        mainCoef.Add("2", list[i].epr);
                                        type = "2";
                                        value = list[i].epr;
                                    }
                                }
                                //else_logger.Error(l.sn + " - " + l.epr);
                                break;
                            case "Результат":
                                string r = ChangeFormatResult(list[i].sn, nameTeams);
                                if (!mainCoef.ContainsKey(r))
                                {
                                    mainCoef.Add(r, list[i].epr);
                                    type = r;
                                    value = list[i].epr;
                                }
                                //else_logger.Error(l.sn + " - " + l.epr);
                                break;
                            case "Победа в матче с учетом форы по очкам":
                                string f = ChangeFormatFora(list[i].sn, nameTeams["1"]);
                                if (!mainCoef.ContainsKey(f))
                                {
                                    mainCoef.Add(f, list[i].epr);
                                    type = f;
                                    value = list[i].epr;
                                }
                                //else_logger.Error(l.sn + " - " + l.epr);
                                break;

                            case "Тотал матча по очкам":
                                string t = ChangeFormatTotals(list[i].sn);
                                if (!mainCoef.ContainsKey(t))
                                {
                                    mainCoef.Add(t, list[i].epr);
                                    type = t;
                                    value = list[i].epr;
                                }
                                //else_logger.Error(l.sn + " - " + l.epr);
                                break;
                            case "Тотал матча по партиям":
                                string tt = ChangeFormatTotals(list[i].sn);
                                if (!mainCoef.ContainsKey(tt))
                                {
                                    mainCoef.Add(tt, list[i].epr);
                                    type = tt;
                                    value = list[i].epr;
                                }
                                //else_logger.Error(l.sn + " - " + l.epr);
                                break;
                            case "Тотал 1-й партии по очкам":
                                string ttt = ChangeFormatTotals(list[i].sn);
                                if (!mainCoef.ContainsKey(ttt))
                                {
                                    mainCoef.Add(ttt, list[i].epr);
                                    type = ttt;
                                    value = list[i].epr;
                                }
                                //else_logger.Error(l.sn + " - " + l.epr);
                                break;
                        }
                    }
                    if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(value))
                    {
                        Coef.Add(new EventForAutoPlay() { EventID = eventID, Type = type, Value = value, marathonAutoPlay = list[i] });
                    }
                }
                catch (Exception e)
                {
                    _logger.Error("Error in CreateEvent(List<DataMarathonForAutoPlays> list)\n\n\n\n"
                         + "Origin: " + "[" + i + "]" + list[i].sn + " - " + list[i].epr + "\n\n\n" +
                         mainCoef.Keys.ToString() + " \n\n\n" + mainCoef.Values.ToString());
                }
            }


            //teamToAdd.AllCoef = mainCoef;
            teamToAdd.Coef = Coef;
        }
        private List<ResultForForks> ConvertWith_MarathonEvent_To_ListResultForForks(MarathonEvent eventCoefList)
        {
            List<ResultForForks> res = new List<ResultForForks>();
            for (int i = 0; i < eventCoefList.Coef.Count; i++)
            {
                try
                {
                    res.Add(new ResultForForks()
                    {
                        EventId = eventCoefList.EventId,
                        Event = eventCoefList.Event,
                        Event_RU = eventCoefList.Event_RU,
                        MatchDateTime = eventCoefList.MatchDateTime,
                        League = eventCoefList.League,
                        Bookmaker = eventCoefList.Bookmaker,
                        SportType = eventCoefList.SportType,
                        Type = eventCoefList.Coef[i].Type,
                        Coef = eventCoefList.Coef[i].Value,
                        marathonAutoPlay = eventCoefList.Coef[i].marathonAutoPlay
                    });
                }
                catch
                {
                    _logger.Error("ConvertWith_MarathonEvent_To_ListResultForForks(MarathonEvent eventCoefList)\n\n\n EventID : " + eventCoefList.EventId.ToString()
                        + " i : " + i.ToString());
                }
            }
            return res;
        }

        private string ChangeFormatFora(string fora, string nameteam, string eventID = "")
        {
            string resFora = string.Empty;
            for (int i = fora.Length - 1; i >= 0; i--)
            {
                if (fora[i] != ' ')
                    resFora = fora[i] + resFora;
                else break;
            }
            string type = string.Empty;
            if (string.IsNullOrEmpty(eventID))
            {
                type = fora.Contains(nameteam) ? "F1" : "F2";
            }
            else
            {
                string enNameTeam1 = englishNameTeams_Dictionary[eventID].name1;
                type = (nameteam.Contains(fora) || enNameTeam1.Contains(fora)) ? "F1" : "F2";
            }
            return type + resFora;
        }
        private string ChangeFormatTotals(string total)
        {
            string resFora = string.Empty;
            for (int i = total.Length - 1; i >= 0; i--)
            {
                if (total[i] != ' ')
                    resFora = total[i] + resFora;
                else break;
            }
            string type = (total.Contains("Меньше") || total.Contains("Under")) ? "TU" : "TO";
            return type + "(" + resFora + ")";
        }
        private string ChangeFormatResult(string result, Dictionary<string, string> nameTeam)
        {
            string res = string.Empty;
            if ((result.Contains("Ничья") || result.Contains("ничья")) && result.Contains(nameTeam["1"])) res = "1X";
            else if ((result.Contains("Ничья") || result.Contains("ничья")) && result.Contains(nameTeam["2"])) res = "X2";
            else if (result.Contains(nameTeam["1"]) && result.Contains(nameTeam["2"])) res = "12";
            else if (result.Contains(nameTeam["1"]))
                res = "1";
            else if (result.Contains(nameTeam["2"]))
                res = "2";

            //tennis
            else if ((result.Contains("ничья") || (result.Contains("Ничья")) && nameTeam["1"].Contains(result))) res = "1X";
            else if ((result.Contains("ничья") || (result.Contains("Ничья")) && nameTeam["2"].Contains(result))) res = "X2";
            else if (nameTeam["1"].Contains(result) && nameTeam["2"].Contains(result)) res = "12";
            else if (nameTeam["1"].Contains(result))
                res = "1";
            else if (nameTeam["2"].Contains(result))
                res = "2";

            else if (result.Contains("Ничья") || result.Contains("ничья")) res = "X";
            return res;
        }

        public Dictionary<string, EnglishNameTeams> GetEnglishNameTEams(SportType sportType, bool isRU = false)
        {
            var resultEnglishTeams = new Dictionary<string, EnglishNameTeams>();
            var url = "";
            var namefile = "";
            string name1 = null;
            string name2 = null;
            string _eventid = null;
            string ru = null;

            if (!isRU)
            {
                UrlAndNameFile(sportType, out url, out namefile, true);
            }
            else
            {
                UrlAndNameFile(sportType, out url, out namefile);
            }
            string html = Html(url, false);
            var lines = html.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(">Ante Post<") || lines[i].Contains(">Ante Post.<"))
                    break;
                if (lines[i]._Contains(MarathonTags.newEventID))
                {
                    _eventid = lines[i].GetEventID();

                    ru = lines[i].TagsContent("data-event-name=");
                }

                if (lines[i]._Contains(MarathonTags.newTeamName))
                {
                    if (name1 == null)
                        name1 = GetAttribut(lines[i]);// line.Substrings(Tags.NameTeam);
                    else name2 = GetAttribut(lines[i]);//line.Substrings(Tags.NameTeam);
                }
                if (!string.IsNullOrEmpty(name1) && !string.IsNullOrEmpty(name2) && !string.IsNullOrEmpty(_eventid))
                {
                    var teams = new EnglishNameTeams();
                    teams.name1 = name1;
                    teams.name2 = name2;
                    teams.eventid = _eventid;
                    teams.eventRU = ru;
                    resultEnglishTeams.Add(_eventid, teams);
                    _eventid = null;
                    name1 = null;
                    name2 = null;
                    ru = null;
                }
            }


            return resultEnglishTeams;
        }

        private DataMarathonForAutoPlays ParseForAutoPlay(string line, string tag, DataMarathonForAutoPlays obj = null)
        {

            obj = obj == null ? (obj = new DataMarathonForAutoPlays()) : obj;
            if (tag.Equals(Tags_DataMarathonForAutoPlays.data_sel))
            {
                line = line.TagsContent2(Tags_DataMarathonForAutoPlays.data_sel);
                var a = getString(line);

                var b = a.Split(':');

                var b1 = b[0].Replace("[", "").Replace("]", "").Split(',');
                var price = b[1].Replace("[", "").Replace("]", "").Split(',');

                var subj = line.Split(',');
                obj.sn = b1[0];
                obj.mn = b1[1];
                obj.ewc = b1[2];
                obj.cid = b1[3];
                obj.prt = b1[4];
                obj.ewf = b1[5];
                obj.epr = b1[6];

                for (int i = 0; i < price.Length; i++)
                {
                    obj.prices.Add(price[i].Replace("\"", ""));
                }
            }
            if (tag.Equals(Tags_DataMarathonForAutoPlays.data_selection_key))
            {
                obj.selection_key = line.TagsContent(Tags_DataMarathonForAutoPlays.data_selection_key);
            }
            return obj;
        }
        /*{"sn":"Burgos (+9.5)",
  "mn":"Победа с учетом форы",
  "ewc":"1/1 1",
  "cid":10182298350,
  "prt":"CP",
  "ewf":"1.0",
  "epr":"1.83",
  "prices"
      :{"0":"83/100",
        "1":"1.83",
        "2":"-121",
        "3":"0.83",
        "4":"0.83",
        "5":"-1.21"}}*/
        private string getString(string line)
        {
            line = line.Trim('{');
            string result = "";
            bool find_ = false; // :
            bool _find = false; // {
            string element = "";
            string price = "";
            foreach (var l in line)
            {

                if (l == ',')
                {

                    if (find_ && !_find)
                    {
                        find_ = false;
                        result += (!string.IsNullOrEmpty(result)) ? "," : "[";
                        result += element.Trim('\"');
                        element = "";
                    }
                    if (find_ && _find)
                    {
                        find_ = false;
                        price += (!string.IsNullOrEmpty(price)) ? "," : "[";
                        price += element.Trim('\"');
                        element = "";
                    }

                }
                if (l == '{' || l == '}')
                {
                    find_ = false;
                }
                if (find_)
                {

                    element += l;

                }
                if (l == ':')
                {
                    find_ = true;
                }
                if (l == '{')
                {
                    _find = true;
                }

            }
            result += "]:";
            result += (price + "," + element + "]");

            return result;
        }
        #endregion

        #region[Page and URL]
        private static string Html(string url, bool a = true)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            List<string> result = new List<string>();
            string HTML = null;
            StreamReader reader = null;
            try
            {
                Debug.Assert(response != null, "response != null");
                reader = new StreamReader(response.GetResponseStream());
                HTML = reader.ReadToEnd();
                reader.Close();
            }
            catch (FileLoadException ex)
            {
                _logger.Error(ex.Message); _logger.Error(ex.StackTrace);
                //reader.Close();
            }
            finally
            {
                reader?.Close();
            }
            //WriteToDocument(HTML);
            return HTML;
        }
        private void UrlAndNameFile(SportType sportType, out string url, out string namefile, bool isEnglish = false)
        {
            string language = isEnglish ? "en" : "su";
            string en_namefile = isEnglish ? "en" : "";
            url = "https://www.marathonbet.com/" + language + "/betting/Ice+Hockey";
            namefile = "Default.html";
            switch (sportType)
            {
                case SportType.Soccer:
                    namefile = "Soccer" + en_namefile + ".html";
                    url = "https://www.marathonbet.com/" + language + "/betting/Football/"; //"/betting/Football/England/Championship/Promotion+Play-Offs/Semi+Final/1st+Leg/";
                    break;

                case SportType.Basketball:
                    namefile = "Basketball" + en_namefile + ".html";
                    url = "https://www.marathonbet.com/" + language + "/betting/Basketball/";
                    break;

                case SportType.Hockey:
                    namefile = "Hokey" + en_namefile + ".html";
                    url = "https://www.marathonbet.com/" + language + "/betting/Ice+Hockey/";
                    break;

                case SportType.Tennis:
                    namefile = "Tenis" + en_namefile + ".html";
                    url = "https://www.marathonbet.com/" + language + "/betting/Tennis/";
                    break;

                case SportType.Volleyball:
                    namefile = "Volleyball" + en_namefile + ".html";
                    url = "https://www.marathonbet.com/" + language + "/betting/Volleyball/";
                    break;
            }
        }
        #endregion

        #region [Documents]
        public static void WriteToDocument(List<ResultForForks> teams, string namefile = "check.txt")
        {
            if (File.Exists(namefile))
                File.Delete(namefile);
            //File.Create(namefile);
            StringBuilder sb = new StringBuilder();
            //StreamWriter sw = new StreamWriter(namefile, true);

            foreach (var team in teams)
            {
                sb.AppendLine(team.EventToString());
            }
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();


            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                var a = sb.ToString().Split('\n');
                var b = sb.ToString();
                //sw.WriteLineAsync(b);
                // File.WriteAllLines(namefile, a);

                StreamWriter writer = null;
                writer = new StreamWriter(namefile);
                writer.WriteLine(b);
                writer.Close();


            }
        }

        //private static void WriteToDocument(string html, string nameFile = "html__.txt")
        //{
        //    using (StreamWriter sw = new StreamWriter(nameFile))
        //    {
        //        sw.WriteLine(html);
        //        sw.Close();
        //    }
        //}
        //private static void WriteToDocument(List<string> html, string nameFile = "html__.txt")
        //{
        //    using (StreamWriter sw = new StreamWriter(nameFile))
        //    {
        //        foreach (var i in html)
        //        {
        //            sw.WriteLine(i);
        //        }
        //        sw.Close();
        //    }
        //}
        #endregion

        #region [Help]
        //------------------HELP_PARSE------------------------------
        private static string GetAttribut(string line, bool date = false)
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
                if (String.IsNullOrEmpty(result.Trim()) && !isStartTag)
                    result = "";
                if (!String.IsNullOrEmpty(result.Trim()) && !isStartTag)
                {
                    isFinish = true;
                    result += (date && !result.Contains("/2016")) ? "/2016 " : "";

                }
                if (isFinish && !date)
                    return result;
            }
            return !date ? "" : result;
        }
        private static string GetLigue(string line)
        {
            bool isStartTag = true;
            string result = "";
            foreach (var l in line)
            {
                if (l == '<')
                    isStartTag = false;
                if (isStartTag)
                    result += l;
                if (l == '>')
                    isStartTag = true;
                if (String.IsNullOrEmpty(result.Trim()) && !isStartTag)
                    result = "";
            }
            return result;
        }
        #endregion

        #region [Selenium]
        private string PhantomDriver(string urlTypeSport, string id, string ligaID, bool today, SportType sporttype)
        {
            id = id.Contains("event_") ? id : "event_" + id;
            string liveTag = today ? "today-name" : "name";
            var res = "";

            try
            {
                if (!String.IsNullOrEmpty(this.isClick_IdEvent))
                {
                    if (this.isClick_IdEvent != id)
                    {
                        _driver.Navigate().GoToUrl(urlTypeSport);
                        var idTag = _driver.FindElement(By.Id(id));
                        var click = idTag.FindElement(By.ClassName(liveTag));
                        click.Click();

                        //var a = _driver.FindElement(By.Id(id)).FindElement(By.ClassName("blocks-area"));
                        var a = _driver.FindElement(By.Id(id));
                        res = a.Text;
                    }
                }
            }
            catch (Exception e)
            {
                /*MessageBox.Show(e.Message.ToString());
                _driver.Close();
                _driver = new PhantomJSDriver();*/
                //return PhantomDriver(urlTypeSport, id, ligaID, today, sporttype);
            }
            this.isClick_IdEvent = id;
            this.WriteToDocumentWithSelenium(res, sporttype, ligaID, id);
            return res;
        }
        private string PhantomFireFox(string urlTypeSport, string id, string ligaID, bool today, SportType sporttype)
        {
            id = id.Contains("event_") ? id : "event_" + id;
            string liveTag = today ? "today-name" : "name";
            var res = "";

            try
            {
                if (!String.IsNullOrEmpty(this.isClick_IdEvent))
                {
                    if (this.isClick_IdEvent != id)
                    {
                        firefox.Navigate().GoToUrl(urlTypeSport);
                        var idTag = firefox.FindElement(By.Id(id));
                        var click = idTag.FindElement(By.ClassName(liveTag));
                        click.Click();

                        //var a = firefox.FindElement(By.Id(id)).FindElement(By.ClassName("blocks-area"));
                        var a = firefox.FindElement(By.Id(id));
                        res = a.Text;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message.ToString());
                firefox.Close();
                firefox = new OpenQA.Selenium.Firefox.FirefoxDriver();
                return PhantomFireFox(urlTypeSport, id, ligaID, today, sporttype);
            }
            this.isClick_IdEvent = id;
            this.WriteToDocumentWithSelenium(res, sporttype, ligaID, id);
            return res;
        }
        private void WriteToDocumentWithSelenium(string data, SportType sporttype, string LigueId, string id)
        {
            string mainNameFolder = "Selenium Results";
            string nameFolderForTypeSport = sporttype.ToString();
            string nameFolderForLigueId = LigueId;
            string nameFile = id + ".txt";
            string path = "";
            if (!Directory.Exists(mainNameFolder))
            {
                Directory.CreateDirectory(mainNameFolder);
            }
            path += mainNameFolder + "\\";
            if (!Directory.Exists(path + nameFolderForTypeSport))
            {
                Directory.CreateDirectory(path + nameFolderForTypeSport);
            }
            path += nameFolderForTypeSport + "\\";
            if (!Directory.Exists(path + nameFolderForLigueId))
            {
                Directory.CreateDirectory(path + nameFolderForLigueId);
            }
            path += nameFolderForLigueId + "\\";
            /*if (!File.Exists(path + nameFile))
            {
                path += nameFile;
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine(data);
                    sw.Close();
                }
            }*/
        }
        #endregion

    }
}