namespace WinParse.DataParser.Models
{
    public class EnglishNameTeams
    {
        public string eventid;
        public string name1;
        public string name2;
        public string league;
        public string eventRU;

        public EnglishNameTeams()
        {
        }

        public EnglishNameTeams(string _eventid, string _name1, string _name2)
        {
            this.eventid = _eventid;
            this.name1 = _name1;
            this.name2 = _name2;
        }
    }
}