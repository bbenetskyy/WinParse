using WinParse.MarathonBetLibrary.Model;

namespace FormulasCollection.Models
{
    public class ResultForForks
    {
        public string Event { get; set; }

        public string EventRu { get; set; }

        public string Type { get; set; }

        public string Coef { get; set; }

        public string EventId { get; set; }

        public string League { get; set; }

        public string Bookmaker { get; set; }
        public string FuulId { get { return "event_" + EventId; } }

        public DataMarathonForAutoPlays MarathonAutoPlay { get; set; }
        public MarathonEvent ParentEvent { get; set; }

        public ResultForForks() { }

        public ResultForForks(string eventId, string nameTeam1, string nameTeam2, string date, string nameCoff, string coef, string type, string bookmaker, string league, DataMarathonForAutoPlays obj)
        {
            Event = nameTeam1.Trim() + " # " + nameTeam2.Trim();
            MatchDateTime = date;
            Type = nameCoff;
            Coef = coef;
            SportType = type;
            Bookmaker = bookmaker;
            EventId = eventId;
            League = league;

            MarathonAutoPlay = obj;
        }

        public string SportType { get; set; }
        public string MatchDateTime { get; set; }
    }
}