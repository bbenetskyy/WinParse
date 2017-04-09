using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarathonBetLibrary.Model;

namespace FormulasCollection.Models
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

        public MarathonEvent() { }

        public MarathonEvent(string eventID, string date, string sportType, string bookmaker, string league, DataMarathonForAutoPlays obj)
        {
            this.MatchDateTime = date;
            SportType = sportType;
            Bookmaker = bookmaker;
            this.EventId = eventID;
            this.League = league;
        }

    }
    public class EventForAutoPlay
    {
        public string EventID { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public DataMarathonForAutoPlays marathonAutoPlay { get; set; }
        public EventForAutoPlay() { }
        public EventForAutoPlay(string eventID, string type, string value, DataMarathonForAutoPlays autoplay)
        {
            this.EventID = eventID;
            this.Type = type;
            this.Value = value;
            this.marathonAutoPlay = autoplay;
        }
    }
}
