namespace DataParser.Models
{
    public class Surebet
    {
        public string Booker { get; set; }
        public string Time { get; set; }
        public string Event { get; set; }
        public string Coeff { get; set; }
        public string Value { get; set; }

        public Surebet() { }
        public Surebet(string booker, string time, string _event, string coeff, string value)
        {
            Booker = booker;
            Time = time;
            Event = _event;
            Coeff = coeff;
            Value = value;
        }
    }
}