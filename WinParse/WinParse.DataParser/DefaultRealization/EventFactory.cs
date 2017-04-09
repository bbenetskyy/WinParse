using System;
using DataParser.Models;
using SiteAccess.Enums;
using System.Collections.Generic;
using FormulasCollection.Helpers;

namespace DataParser.DefaultRealization
{
    internal class EventFactory
    {
        private string lineId;
        private int matchPeriod;
        private long? leagueId;
        private string matchDateTime;

        public EventFactory(string lineId,
                            int matchPeriod,
                            long? leagueId,
                            string matchDateTime)
        {
            this.lineId = lineId;
            this.matchPeriod = matchPeriod;
            this.leagueId = leagueId;
            this.matchDateTime = matchDateTime;
        }

        internal IEnumerable<EventWithTotalDictionary> CreateEventsWithTotal(string totalType,
                                                               string totalValue,
                                                               TeamType team,
                                                               SideType sideType,
                                                               BetType betType)
        {
            switch (matchPeriod)
            {
                case 0:
                    return new[]{ new EventWithTotalDictionary
                    {
                        LineId = lineId,
                        TotalType = totalType,
                        TotalValue = totalValue,
                        MatchDateTime = matchDateTime,
                        LeagueId = leagueId,
                        MatchPeriod = matchPeriod,
                        TeamType = team,
                        SideType = sideType,
                        BetType = betType
                    } };

                case 1:
                case 2:
                    return new[]{
                    new EventWithTotalDictionary
                    {
                        LineId = lineId,
                        TotalType = totalType.ExtendType("H",matchPeriod),
                        TotalValue = totalValue,
                        MatchDateTime = matchDateTime,
                        LeagueId = leagueId,
                        MatchPeriod = matchPeriod,
                        TeamType = team,
                        SideType = sideType,
                        BetType = betType
                    } ,
                    new EventWithTotalDictionary
                    {
                        LineId = lineId,
                        TotalType = totalType.ExtendType("P",matchPeriod),
                        TotalValue = totalValue,
                        MatchDateTime = matchDateTime,
                        LeagueId = leagueId,
                        MatchPeriod = matchPeriod,
                        TeamType = team,
                        SideType = sideType,
                        BetType = betType
                    },
                    new EventWithTotalDictionary
                    {
                        LineId = lineId,
                        TotalType = totalType.ExtendType("Q",matchPeriod),
                        TotalValue = totalValue,
                        MatchDateTime = matchDateTime,
                        LeagueId = leagueId,
                        MatchPeriod = matchPeriod,
                        TeamType = team,
                        SideType = sideType,
                        BetType = betType
                    } ,
                    new EventWithTotalDictionary
                    {
                        LineId = lineId,
                        TotalType = totalType.ExtendType("G",matchPeriod),
                        TotalValue = totalValue,
                        MatchDateTime = matchDateTime,
                        LeagueId = leagueId,
                        MatchPeriod = matchPeriod,
                        TeamType = team,
                        SideType = sideType,
                        BetType = betType
                    },
                    new EventWithTotalDictionary
                    {
                        LineId = lineId,
                        TotalType = totalType.ExtendType("S",matchPeriod),
                        TotalValue = totalValue,
                        MatchDateTime = matchDateTime,
                        LeagueId = leagueId,
                        MatchPeriod = matchPeriod,
                        TeamType = team,
                        SideType = sideType,
                        BetType = betType
                    },
                    new EventWithTotalDictionary
                    {
                        LineId = lineId,
                        TotalType = totalType.ExtendType("PR",matchPeriod),
                        TotalValue = totalValue,
                        MatchDateTime = matchDateTime,
                        LeagueId = leagueId,
                        MatchPeriod = matchPeriod,
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