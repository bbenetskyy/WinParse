namespace DataParser.Models
{
    public class TeamsFootballHokey : Teams
    {
        public TeamsFootballHokey() : base()
        {
        }

        public TeamsFootballHokey(string eventid, string nameTeam1, string nameTeam2, string date,
            string win1, string x, string win2,
            string xWin1, string xWin2, string win1Win2,
            string fora1, string fora2, string less, string more)
            : base(eventid, nameTeam1, nameTeam2, date, win1, win2, fora1, fora2, less, more)
        {
            this.X = x;
            this.XWin1 = xWin1;
            this.XWin2 = xWin2;
            this.Win1Win2 = win1Win2;
        }
    }
}