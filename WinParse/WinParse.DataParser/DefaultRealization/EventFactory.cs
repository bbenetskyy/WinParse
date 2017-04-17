using System;
using DataParser.Models;
using System.Collections.Generic;
using FormulasCollection.Helpers;
using SiteAccess.Model;

namespace DataParser.DefaultRealization
{
    internal class EventFactory
    {
        private string _lineId;
        private int _matchPeriod;
        private long? _leagueId;
        private string _matchDateTime;

        public EventFactory(string lineId,
                            int matchPeriod,
                            long? leagueId,
                            string matchDateTime)
        {
            this._lineId = lineId;
            this._matchPeriod = matchPeriod;
            this._leagueId = leagueId;
            this._matchDateTime = matchDateTime;
        }

        internal IEnumerable<EventWithTotalDictionary> CreateEventsWithTotal(string totalType,
                                                               string totalValue,
                                                               TeamType team,
                                                               SideType sideType,
                                                               BetType betType)
        {
            switch (_matchPeriod)
            {
                case 0:
                    return new[]{ new EventWithTotalDictionary
                    {
                        LineId = _lineId,
                        TotalType = totalType,
                        TotalValue = totalValue,
                        MatchDateTime = _matchDateTime,
                        LeagueId = _leagueId,
                        MatchPeriod = _matchPeriod,
                        TeamType = team,
                        SideType = sideType,
                        BetType = betType
                    } };

                case 1:
                case 2:
                    return new[]{
                    new EventWithTotalDictionary
                    {
                        LineId = _lineId,
                        TotalType = totalType.ExtendType("H",_matchPeriod),
                        TotalValue = totalValue,
                        MatchDateTime = _matchDateTime,
                        LeagueId = _leagueId,
                        MatchPeriod = _matchPeriod,
                        TeamType = team,
                        SideType = sideType,
                        BetType = betType
                    } ,
                    new EventWithTotalDictionary
                    {
                        LineId = _lineId,
                        TotalType = totalType.ExtendType("P",_matchPeriod),
                        TotalValue = totalValue,
                        MatchDateTime = _matchDateTime,
                        LeagueId = _leagueId,
                        MatchPeriod = _matchPeriod,
                        TeamType = team,
                        SideType = sideType,
                        BetType = betType
                    },
                    new EventWithTotalDictionary
                    {
                        LineId = _lineId,
                        TotalType = totalType.ExtendType("Q",_matchPeriod),
                        TotalValue = totalValue,
                        MatchDateTime = _matchDateTime,
                        LeagueId = _leagueId,
                        MatchPeriod = _matchPeriod,
                        TeamType = team,
                        SideType = sideType,
                        BetType = betType
                    } ,
                    new EventWithTotalDictionary
                    {
                        LineId = _lineId,
                        TotalType = totalType.ExtendType("G",_matchPeriod),
                        TotalValue = totalValue,
                        MatchDateTime = _matchDateTime,
                        LeagueId = _leagueId,
                        MatchPeriod = _matchPeriod,
                        TeamType = team,
                        SideType = sideType,
                        BetType = betType
                    },
                    new EventWithTotalDictionary
                    {
                        LineId = _lineId,
                        TotalType = totalType.ExtendType("S",_matchPeriod),
                        TotalValue = totalValue,
                        MatchDateTime = _matchDateTime,
                        LeagueId = _leagueId,
                        MatchPeriod = _matchPeriod,
                        TeamType = team,
                        SideType = sideType,
                        BetType = betType
                    },
                    new EventWithTotalDictionary
                    {
                        LineId = _lineId,
                        TotalType = totalType.ExtendType("PR",_matchPeriod),
                        TotalValue = totalValue,
                        MatchDateTime = _matchDateTime,
                        LeagueId = _leagueId,
                        MatchPeriod = _matchPeriod,
                        TeamType = team,
                        SideType = sideType,
                        BetType = betType
                    }};

                default:
                    return new List<EventWithTotalDictionary>();
            }
        }
    }
}