namespace DataParser.Models
{
    public class EventWithTotal
    {
        public long? Id { get; set; }

        public string TotalType { get; set; }

        public string TotalValue { get; set; }

        public string MatchDateTime { get; set; }
    }
}