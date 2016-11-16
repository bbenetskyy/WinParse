namespace DataParser.Models
{
    public class TwoBooker
    {
        public Surebet Pinnacle { get; set; }
        public Surebet Marathon { get; set; }
        public TwoBooker() { }
        public TwoBooker(Surebet pinnacle, Surebet marathon)
        {
            this.Pinnacle = pinnacle;
            this.Marathon = marathon;
        }
    }
}