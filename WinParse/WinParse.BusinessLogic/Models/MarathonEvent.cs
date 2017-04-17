using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinParse.MarathonBetLibrary.Model;

namespace FormulasCollection.Models
{
    public class MarathonEvent
    {
        public string EventId { get; set; }

        public string Event { get; set; }

        public string EventRu { get; set; }

        public string MatchDateTime { get; set; }

        public string League { get; set; }

        public string Bookmaker { get; set; }

        public string SportType { get; set; }

        public List<EventForAutoPlay> Coef { get; set; }

        public MarathonEvent() { }

        public MarathonEvent(string eventId, string date, string sportType, string bookmaker, string league, DataMarathonForAutoPlays obj)
        {
            MatchDateTime = date;
            SportType = sportType;
            Bookmaker = bookmaker;
            EventId = eventId;
            League = league;
        }

    }
}
