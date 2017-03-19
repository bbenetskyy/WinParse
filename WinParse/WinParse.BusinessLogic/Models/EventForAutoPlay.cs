namespace WinParse.BusinessLogic.Models
{
    public class EventForAutoPlay
    {
        public string EventID { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public DataMarathonForAutoPlays marathonAutoPlay { get; set; }

        public EventForAutoPlay()
        {
        }

        public EventForAutoPlay(string eventID, string type, string value, DataMarathonForAutoPlays autoplay)
        {
            this.EventID = eventID;
            this.Type = type;
            this.Value = value;
            this.marathonAutoPlay = autoplay;
        }
    }
}