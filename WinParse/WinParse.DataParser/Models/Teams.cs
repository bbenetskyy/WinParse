namespace DataParser.Models
{
    public class Teams
    {
        protected string NameTeam1;
        protected string NameTeam2;
        protected string date;

        public string EventId;
        public string Win1;
        public string Win2;
        public string Fora1;
        public string Fora2;
        public string Less;
        public string More;

        public string X;
        public string XWin1;
        public string XWin2;
        public string Win1Win2;

        public Teams()
        {
        }

        public Teams(string eventid, string nameTeam1, string nameTeam2, string date,
            string win1, string win2,
            string fora1, string fora2, string less, string more)
        {
            EventId = eventid;
            this.NameTeam1 = nameTeam1;
            this.NameTeam2 = nameTeam2;
            this.date = date;

            this.Win1 = win1;
            this.Win2 = win2;
            this.Fora1 = fora1;
            this.Fora2 = fora2;
            this.Less = less;
            this.More = more;
        }

        public string NameTeame1 { get { return NameTeam1; } set { NameTeam1 = value; } }
        public string NameTeame2 { get { return NameTeam2; } set { NameTeam2 = value; } }
        public string Date { get { return date; } set { date = value; } }
    }
}