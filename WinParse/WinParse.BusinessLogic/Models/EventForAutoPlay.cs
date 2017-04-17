using WinParse.MarathonBetLibrary.Model;

namespace FormulasCollection.Models
{
    public class EventForAutoPlay
    {
        public string EventId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public DataMarathonForAutoPlays MarathonAutoPlay { get; set; }

        public EventForAutoPlay()
        {
        }

        public EventForAutoPlay(string eventId, string type, string value, DataMarathonForAutoPlays autoplay)
        {
            EventId = eventId;
            Type = type;
            Value = value;
            MarathonAutoPlay = autoplay;
        }
    }
}