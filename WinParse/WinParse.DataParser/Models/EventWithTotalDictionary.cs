using SiteAccess.Enums;

namespace WinParse.DataParser.Models
{
    public class EventWithTotalDictionary
    {
        public string TotalType { get; set; }

        public string TotalValue { get; set; }

        public string MatchDateTime { get; set; }

        public string LineId { get; set; }

        public long? LeagueId { get; set; }

        public int MatchPeriod { get; set; }

        public SideType SideType { get; set; }

        public TeamType TeamType { get; set; }

        public BetType BetType { get; set; }
    }
}