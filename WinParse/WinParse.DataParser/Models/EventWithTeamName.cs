namespace DataParser.Models
{
    public class EventWithTeamName
    {
        public long? Id { get; set; }

        /// <summary>
        /// First home, Second away
        /// </summary>
        public string TeamNames { get; set; }
    }
}