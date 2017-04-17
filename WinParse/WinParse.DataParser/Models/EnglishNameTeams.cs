namespace DataParser.Models
{
    public class EnglishNameTeams
    {
        public string Eventid;
        public string Name1;
        public string Name2;
        public string League;
        public string EventRu;

        public EnglishNameTeams()
        {
        }

        public EnglishNameTeams(string _eventid, string _name1, string _name2)
        {
            Eventid = _eventid;
            Name1 = _name1;
            Name2 = _name2;
        }
    }
}