namespace WinParse.DataParser.Models
{
    public class Teams_Football_Hokey : Teams
    {
        public Teams_Football_Hokey() : base()
        {
        }

        public Teams_Football_Hokey(string _eventid, string nameTeam1, string nameTeam2, string date,
            string win1, string x, string win2,
            string x_win1, string x_win2, string win1_win2,
            string fora1, string fora2, string less, string more)
            : base(_eventid, nameTeam1, nameTeam2, date, win1, win2, fora1, fora2, less, more)
        {
            this.x = x;
            this.x_win1 = x_win1;
            this.x_win2 = x_win2;
            this.win1_win2 = win1_win2;
        }
    }
}