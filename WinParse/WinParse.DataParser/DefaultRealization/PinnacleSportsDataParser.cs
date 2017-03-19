using NLog;
using SiteAccess.Enums;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net;
using System.Text;
using ToolsPortable;
using WinParse.BusinessLogic.Helpers;
using WinParse.BusinessLogic.Models;
using WinParse.BusinessLogic.Realizations;
using WinParse.DataParser.Enums;
using WinParse.DataParser.Models;

namespace WinParse.DataParser.DefaultRealization
{
    public class PinnacleSportsDataParser
    {
        private SportType _sportType = SportType.NoType;
        private readonly ConverterFormulas _converter;

        // ReSharper disable once InconsistentNaming
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public PinnacleSportsDataParser()
        {
            _converter = new ConverterFormulas();
        }

        private Dictionary<long, List<EventWithTotalDictionary>> ParseEventWithTotalsDictionaty(HttpWebResponse totalResp)
        {
            var resList = new Dictionary<long, List<EventWithTotalDictionary>>();
            try
            {
                var sportEvents = (JsonObject)JsonValue.Load(totalResp.GetResponseStream());

                if (sportEvents == null
                    || !sportEvents.ContainsKey("leagues")
                    || sportEvents["leagues"] == null) return resList;

                foreach (var league in sportEvents["leagues"])
                {
                    var leagueId = league.Value["id"].ConvertToLongOrNull();
                    if (league.Value == null || !league.Value.ContainsKey("events")) continue;
                    foreach (var sportEvent in league.Value["events"])
                    {
                        if (sportEvent.Value == null || !sportEvent.Value.ContainsKey("id") || sportEvent.Value["id"] == null) continue;

                        var id = Convert.ToInt64(sportEvent.Value["id"].ToString());

                        if (!resList.ContainsKey(id)) resList.Add(id, new List<EventWithTotalDictionary>());

                        if (!sportEvent.Value.ContainsKey("periods")) continue;

                        foreach (var period in sportEvent.Value["periods"])
                        {
                            if (!period.Value.ContainsKey("cutoff") || period.Value["cutoff"] == null) continue;
                            if (!period.Value.ContainsKey("lineId") || period.Value["lineId"] == null) continue;
                            var matchPeriod = Convert.ToInt32(period.Value["number"].ToString());
                            var matchDateTime = period.Value["cutoff"].ToString();
                            var lineId = period.Value["lineId"].ToString();

                            if (period.Value.ContainsKey("moneyline") && period.Value["moneyline"] != null)
                            {
                                var moneyLine = period.Value["moneyline"];
                                if (moneyLine.ContainsKey("home") && moneyLine["home"] != null)
                                    resList[id].Add(new EventWithTotalDictionary
                                    {
                                        LineId = lineId,
                                        TotalType = "1",
                                        TotalValue = moneyLine["home"].ToString(),
                                        MatchDateTime = matchDateTime,
                                        LeagueId = leagueId,
                                        MatchPeriod = matchPeriod,
                                        TeamType = TeamType.TEAM1,
                                        SideType = SideType.OVER,
                                        BetType = BetType.MONEYLINE
                                    });
                                if (moneyLine.ContainsKey("away") && moneyLine["away"] != null)
                                    resList[id].Add(new EventWithTotalDictionary
                                    {
                                        LineId = lineId,
                                        TotalType = "2",
                                        TotalValue = moneyLine["away"].ToString(),
                                        MatchDateTime = matchDateTime,
                                        LeagueId = leagueId,
                                        MatchPeriod = matchPeriod,
                                        TeamType = TeamType.TEAM2,
                                        SideType = SideType.OVER,
                                        BetType = BetType.MONEYLINE
                                    });
                                if (moneyLine.ContainsKey("draw") && moneyLine["draw"] != null)
                                    resList[id].Add(new EventWithTotalDictionary
                                    {
                                        LineId = lineId,
                                        TotalType = "X",
                                        TotalValue = moneyLine["draw"].ToString(),
                                        MatchDateTime = matchDateTime,
                                        LeagueId = leagueId,
                                        MatchPeriod = matchPeriod,
                                        TeamType = TeamType.DRAW,
                                        SideType = SideType.OVER,
                                        BetType = BetType.MONEYLINE
                                    });
                            }
                            if (period.Value.ContainsKey("spreads") && period.Value["spreads"] != null && matchPeriod == 0)
                                foreach (var spread in period.Value["spreads"])
                                {
                                    if (!spread.Value.ContainsKey("hdp") || spread.Value["hdp"] == null) continue;

                                    if (spread.Value.ContainsKey("home") && spread.Value["home"] != null)
                                        resList[id].Add(new EventWithTotalDictionary
                                        {
                                            LineId = lineId,
                                            TotalType = $"F1({spread.Value["hdp"].ToString().LocalizeToMarathon()})".MinimalizeValue(),
                                            TotalValue = spread.Value["home"].ToString(),
                                            MatchDateTime = matchDateTime,
                                            LeagueId = leagueId,
                                            MatchPeriod = matchPeriod,
                                            TeamType = TeamType.TEAM1,
                                            SideType = SideType.OVER,
                                            BetType = BetType.SPREAD
                                        });

                                    if (spread.Value.ContainsKey("away") && spread.Value["away"] != null)
                                        resList[id].Add(new EventWithTotalDictionary
                                        {
                                            LineId = lineId,
                                            TotalType = $"F2({spread.Value["hdp"].ToString().InvertValue()})".MinimalizeValue(),
                                            TotalValue = spread.Value["away"].ToString(),
                                            MatchDateTime = matchDateTime,
                                            LeagueId = leagueId,
                                            TeamType = TeamType.TEAM2,
                                            SideType = SideType.OVER,
                                            BetType = BetType.SPREAD
                                        });
                                }
                            if (period.Value.ContainsKey("totals") && period.Value["totals"] != null)
                                foreach (var total in period.Value["totals"])
                                {
                                    if (!total.Value.ContainsKey("points") || total.Value["points"] == null) continue;
                                    if (total.Value.ContainsKey("over") && total.Value["over"] != null)
                                    {
                                        var totalType = string.Empty;
                                        //matchPeriod can be only 0,1 and 2 according API
                                        switch (matchPeriod)
                                        {
                                            case 0:
                                                totalType = $"TO({total.Value["points"]})".MinimalizeValue();
                                                break;

                                            default:
                                                switch (_sportType)
                                                {
                                                    case SportType.Soccer:
                                                        totalType = $"{DictionatyTypeCoef.TT}O({total.Value["points"]})".MinimalizeValue();
                                                        break;

                                                    case SportType.Basketball:
                                                        totalType = $"{DictionatyTypeCoef.TPT}O({total.Value["points"]})".MinimalizeValue();
                                                        break;

                                                    case SportType.Hockey:
                                                        totalType = $"{DictionatyTypeCoef.TPR}O({total.Value["points"]})".MinimalizeValue();
                                                        break;

                                                    case SportType.Tennis:
                                                        totalType = $"{DictionatyTypeCoef.TS}O({total.Value["points"]})".MinimalizeValue();
                                                        break;

                                                    case SportType.Volleyball:
                                                        totalType = $"TO({total.Value["points"]})".MinimalizeValue();
                                                        break;
                                                }
                                                break;
                                        }
                                        resList[id].Add(new EventWithTotalDictionary
                                        {
                                            LineId = lineId,
                                            TotalType = totalType,
                                            TotalValue = total.Value["over"].ToString(),
                                            MatchDateTime = matchDateTime,
                                            LeagueId = leagueId,
                                            MatchPeriod = matchPeriod,
                                            TeamType = TeamType.DRAW,
                                            SideType = SideType.OVER,
                                            BetType = BetType.TOTAL_POINTS
                                        });
                                    }
                                    if (total.Value.ContainsKey("under") && total.Value["under"] != null)
                                    {
                                        var totalType = string.Empty;
                                        //matchPeriod can be only 0,1 and 2 according API
                                        switch (matchPeriod)
                                        {
                                            case 0:
                                                totalType = $"TU({total.Value["points"]})".MinimalizeValue();
                                                break;

                                            default:
                                                switch (_sportType)
                                                {
                                                    case SportType.Soccer:
                                                        totalType = $"{DictionatyTypeCoef.TT}U({total.Value["points"]})".MinimalizeValue();
                                                        break;

                                                    case SportType.Basketball:
                                                        totalType = $"{DictionatyTypeCoef.TPT}U({total.Value["points"]})".MinimalizeValue();
                                                        break;

                                                    case SportType.Hockey:
                                                        totalType = $"{DictionatyTypeCoef.TPR}U({total.Value["points"]})".MinimalizeValue();
                                                        break;

                                                    case SportType.Tennis:
                                                        totalType = $"{DictionatyTypeCoef.TS}U({total.Value["points"]})".MinimalizeValue();
                                                        break;

                                                    case SportType.Volleyball:
                                                        totalType = $"TU({total.Value["points"]})".MinimalizeValue();
                                                        break;
                                                }
                                                break;
                                        }

                                        resList[id].Add(new EventWithTotalDictionary
                                        {
                                            LineId = lineId,
                                            TotalType = totalType,
                                            TotalValue = total.Value["under"].ToString(),
                                            MatchDateTime = matchDateTime,
                                            LeagueId = leagueId,
                                            MatchPeriod = matchPeriod,
                                            TeamType = TeamType.DRAW,
                                            SideType = SideType.UNDER,
                                            BetType = BetType.TOTAL_POINTS
                                        });
                                    }
                                }
                            if (period.Value.ContainsKey("teamTotal") && period.Value["teamTotal"] != null)
                            {
                                var teamTotal = period.Value["teamTotal"];
                                if (teamTotal.ContainsKey("home") && teamTotal["home"] != null)
                                {
                                    var home = teamTotal["home"];
                                    if (!home.ContainsKey("points") || home["points"] == null) continue;
                                    if (home.ContainsKey("over") && home["over"] != null)
                                        resList[id].Add(new EventWithTotalDictionary
                                        {
                                            LineId = lineId,
                                            TotalType = $"TF1O({home["points"]})".MinimalizeValue(),
                                            TotalValue = home["over"].ToString(),
                                            MatchDateTime = matchDateTime,
                                            LeagueId = leagueId,
                                            MatchPeriod = matchPeriod,
                                            SideType = SideType.OVER,
                                            TeamType = TeamType.TEAM1,
                                            BetType = BetType.TEAM_TOTAL_POINTS
                                        });
                                    if (home.ContainsKey("under") && home["under"] != null)
                                        resList[id].Add(new EventWithTotalDictionary
                                        {
                                            LineId = lineId,
                                            TotalType = $"TF1U({home["points"]})".MinimalizeValue(),
                                            TotalValue = home["under"].ToString(),
                                            MatchDateTime = matchDateTime,
                                            LeagueId = leagueId,
                                            MatchPeriod = matchPeriod,
                                            SideType = SideType.UNDER,
                                            TeamType = TeamType.TEAM1,
                                            BetType = BetType.TEAM_TOTAL_POINTS
                                        });
                                }
                                if (teamTotal.ContainsKey("away") && teamTotal["away"] != null)
                                {
                                    var away = teamTotal["away"];
                                    if (!away.ContainsKey("points") || away["points"] == null) continue;
                                    if (away.ContainsKey("over") && away["over"] != null)
                                        resList[id].Add(new EventWithTotalDictionary
                                        {
                                            LineId = lineId,
                                            TotalType = $"TF2O({away["points"]})".MinimalizeValue(),
                                            TotalValue = away["over"].ToString(),
                                            MatchDateTime = matchDateTime,
                                            LeagueId = leagueId,
                                            MatchPeriod = matchPeriod,
                                            SideType = SideType.OVER,
                                            TeamType = TeamType.TEAM2,
                                            BetType = BetType.TEAM_TOTAL_POINTS
                                        });
                                    if (away.ContainsKey("under") && away["under"] != null)
                                        resList[id].Add(new EventWithTotalDictionary
                                        {
                                            LineId = lineId,
                                            TotalType = $"TF2U({away["points"]})".MinimalizeValue(),
                                            TotalValue = away["under"].ToString(),
                                            MatchDateTime = matchDateTime,
                                            LeagueId = leagueId,
                                            MatchPeriod = matchPeriod,
                                            SideType = SideType.UNDER,
                                            TeamType = TeamType.TEAM2,
                                            BetType = BetType.TEAM_TOTAL_POINTS
                                        });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
                throw;
            }
            return resList;
        }

        private Dictionary<long, string> ParseEventWithNamesDictionary(HttpWebResponse teamNamesResp)
        {
            var resList = new Dictionary<long, string>();
            try
            {
                var jsonValue = ((JsonObject)JsonValue.Load(teamNamesResp.GetResponseStream()))["league"];
                if (jsonValue == null) return resList;
                foreach (var league in jsonValue)
                {
                    var sportEvents = league.Value["events"];

                    if (sportEvents == null) continue;

                    foreach (var sportEvent in sportEvents)
                    {
                        var id = sportEvent.Value["id"].ConvertToLongOrNull();
                        if (id == null) continue;
                        System.Diagnostics.Debug.WriteLine(sportEvent.Value["status"]);
                        if (!resList.ContainsKey(id.Value))
                            resList.Add(id.Value, $"{sportEvent.Value["home"]} - {sportEvent.Value["away"]}".Replace("\"", ""));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
                throw;
            }
            return resList;
        }

        private Dictionary<long, string> ParseEventWithLeguesNamesDictionary(HttpWebResponse leaguesNamesResp)
        {
            if (leaguesNamesResp == null) return new Dictionary<long, string>();
            var resList = new Dictionary<long, string>();
            try
            {
                var jsonValue = ((JsonObject)JsonValue.Load(leaguesNamesResp.GetResponseStream()))["leagues"];
                if (jsonValue == null) return resList;
                foreach (var league in jsonValue)
                {
                    var id = league.Value["id"].ConvertToLongOrNull();
                    if (id == null) continue;
                    if (!resList.ContainsKey(id.Value))
                        resList.Add(id.Value, league.Value["name"].ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
                throw;
            }
            return resList;
        }

        private Dictionary<string, ResultForForksDictionary> GroupResponsesDictionary(HttpWebResponse totalResp, HttpWebResponse teamNamesResp, HttpWebResponse leaguesNamesResp)
        {
            var eventsWithNames = ParseEventWithNamesDictionary(teamNamesResp);
            var eventsWithTotal = ParseEventWithTotalsDictionaty(totalResp);
            var eventsWithLeaguesNames = ParseEventWithLeguesNamesDictionary(leaguesNamesResp);

            var resDic = new Dictionary<string, ResultForForksDictionary>();
            try
            {
                foreach (var eventWithName in eventsWithNames)
                {
                    if (!eventsWithTotal.ContainsKey(eventWithName.Key)) continue;

                    foreach (var eventWithTotal in eventsWithTotal[eventWithName.Key])
                    {
                        var key = $"{eventWithName.Value}";
                        if (!resDic.ContainsKey(key))
                        {
                            resDic.Add(key, new ResultForForksDictionary
                            {
                                TeamNames = eventWithName.Value,
                                EventId = eventWithName.Key.ToString(),
                                MatchDateTime = DateTime.Parse(eventWithTotal.MatchDateTime.Replace("\"", "")),
                                ForkDetailDictionary = new Dictionary<string, ForkDetail>(),
                            });

                            resDic[key].ForkDetailDictionary.Add(eventWithTotal.TotalType, new ForkDetail
                            {
                                TypeCoef = _converter.ConvertAmericanToDecimal(eventWithTotal.TotalValue.ConvertToDoubleOrNull()),
                                LineId = eventWithTotal.LineId,
                                Period = eventWithTotal.MatchPeriod,
                                SideType = eventWithTotal.SideType,
                                TeamType = eventWithTotal.TeamType,
                                BetType = eventWithTotal.BetType
                            });

                            if (eventWithTotal.LeagueId != null &&
                                eventsWithLeaguesNames.ContainsKey(eventWithTotal.LeagueId.Value))
                                resDic[key].LeagueName = eventsWithLeaguesNames[eventWithTotal.LeagueId.Value];
                        }
                        else
                        {
                            if (!resDic[key].ForkDetailDictionary.ContainsKey(eventWithTotal.TotalType))
                                resDic[key].ForkDetailDictionary.Add(eventWithTotal.TotalType, new ForkDetail
                                {
                                    TypeCoef = _converter.ConvertAmericanToDecimal(eventWithTotal.TotalValue.ConvertToDoubleOrNull()),
                                    LineId = eventWithTotal.LineId,
                                    Period = eventWithTotal.MatchPeriod,
                                    SideType = eventWithTotal.SideType,
                                    TeamType = eventWithTotal.TeamType,
                                    BetType = eventWithTotal.BetType
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
                throw;
            }
            return resDic;
        }

        public Dictionary<string, ResultForForksDictionary> GetAllPinacleEventsDictionary(SportType sportType, string userLogin, string userPass)
        {
            Dictionary<string, ResultForForksDictionary> resDic;
            _sportType = sportType;
            try
            {
                var totalResp = GetAllTotals(userLogin, userPass);
                var teamNamesResp = GetAllTeamNames(userLogin, userPass);
                HttpWebResponse leaguesNamesResp = null;
                try
                {
                    leaguesNamesResp = GetAllLeaguesNames(userLogin, userPass);
                }
                catch (Exception ex)
                {
                    //we can simple go ahead, it's just a leagues
                    _logger.Error(ex.Message);
                    _logger.Error(ex.StackTrace);
                }
                resDic = GroupResponsesDictionary(totalResp, teamNamesResp, leaguesNamesResp);
                var listForRemove = new List<string>();

                foreach (var key in resDic.Keys)
                {
                    listForRemove.Clear();

                    listForRemove.AddRange(
                        from typeCoefKey in resDic[key].ForkDetailDictionary.Keys
                        let coef = resDic[key].ForkDetailDictionary[typeCoefKey].TypeCoef
                        where Math.Abs(coef) < 0.01 || Math.Abs(coef - _converter.IncorrectAmericanOdds) < 0.01
                        select typeCoefKey);

                    foreach (var keyForRemove in listForRemove) resDic[key].ForkDetailDictionary.Remove(keyForRemove);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
                return new Dictionary<string, ResultForForksDictionary>();
            }
            return resDic;
        }

        /// <summary>
        /// This function get events with their team names 
        /// </summary>
        /// <returns></returns>
        protected virtual HttpWebResponse GetAllTeamNames(string userLogin, string userPass)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://api.pinnaclesports.com/v1/fixtures?sportid=" + (int)_sportType);
            var credentials = $"{userLogin}:{userPass}"; //for test "VB794327", "artem89@"
            var bytes = Encoding.UTF8.GetBytes(credentials);
            var base64 = Convert.ToBase64String(bytes);
            var authorization = string.Concat("Basic ", base64);
            request.Headers.Add("Authorization", authorization);
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0)";
            request.Method = "GET";
            request.Accept = "application/json";
            request.ContentType = "application/json; charset=utf-8";
            return (HttpWebResponse)request.GetResponse();
        }

        /// <summary>
        /// This function get events with their totals 
        /// </summary>
        /// <returns></returns>
        protected virtual HttpWebResponse GetAllTotals(string userLogin, string userPass)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://api.pinnaclesports.com/v1/odds?sportid=" + (int)_sportType);
            var credentials = $"{userLogin}:{userPass}"; //for test "VB794327", "artem89@"
            var bytes = Encoding.UTF8.GetBytes(credentials);
            var base64 = Convert.ToBase64String(bytes);
            var authorization = string.Concat("Basic ", base64);
            request.Headers.Add("Authorization", authorization);
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0)";
            request.Method = "GET";
            request.Accept = "application/json";
            request.ContentType = "application/json; charset=utf-8";
            return (HttpWebResponse)request.GetResponse();
        }

        /// <summary>
        /// This function get events with their totals 
        /// </summary>
        /// <returns></returns>
        protected virtual HttpWebResponse GetAllLeaguesNames(string userLogin, string userPass)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://api.pinnacle.com/v2/leagues?sportid=" + (int)_sportType);
            var credentials = $"{userLogin}:{userPass}"; //for test "VB794327", "artem89@"
            var bytes = Encoding.UTF8.GetBytes(credentials);
            var base64 = Convert.ToBase64String(bytes);
            var authorization = string.Concat("Basic ", base64);
            request.Headers.Add("Authorization", authorization);
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0)";
            request.Method = "GET";
            request.Accept = "application/json";
            request.ContentType = "application/json; charset=utf-8";
            return (HttpWebResponse)request.GetResponse();
        }
    }
}