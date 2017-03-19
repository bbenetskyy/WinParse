using System.Collections.Generic;

namespace WinParse.BusinessLogic.Models
{
    public class MarathonEvent
    {
        public string EventId { get; set; }

        public string Event { get; set; }

        public string Event_RU { get; set; }

        public string MatchDateTime { get; set; }

        public string League { get; set; }

        public string Bookmaker { get; set; }

        public string SportType { get; set; }

        public List<EventForAutoPlay> Coef { get; set; }

        public MarathonEvent()
        {
        }

        public MarathonEvent(string eventID, string date, string sportType, string bookmaker, string league, DataMarathonForAutoPlays obj)
        {
            this.MatchDateTime = date;
            SportType = sportType;
            Bookmaker = bookmaker;
            this.EventId = eventID;
            this.League = league;
        }
    }
}