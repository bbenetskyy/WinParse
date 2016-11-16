namespace DataParser.Models
{
    public class Teams
    {
        protected string nameTeam1;
        protected string nameTeam2;
        protected string date;

        public string eventId;
        public string win1;
        public string win2;
        public string fora1;
        public string fora2;
        public string less;
        public string more;

        public string x;
        public string x_win1;
        public string x_win2;
        public string win1_win2;

        public Teams()
        {
        }

        public Teams(string _eventid, string nameTeam1, string nameTeam2, string date,
            string win1, string win2,
            string fora1, string fora2, string less, string more)
        {
            this.eventId = _eventid;
            this.nameTeam1 = nameTeam1;
            this.nameTeam2 = nameTeam2;
            this.date = date;

            this.win1 = win1;
            this.win2 = win2;
            this.fora1 = fora1;
            this.fora2 = fora2;
            this.less = less;
            this.more = more;
        }

        public string NameTeame1 { get { return this.nameTeam1; } set { this.nameTeam1 = value; } }
        public string NameTeame2 { get { return this.nameTeam2; } set { this.nameTeam2 = value; } }
        public string Date { get { return this.date; } set { this.date = value; } }
    }
}