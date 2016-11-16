namespace DataParser.Models
{
    public class Teams_Tenis_Volleyball_Basketball : Teams
    {
        public Teams_Tenis_Volleyball_Basketball() : base()
        {
        }

        public Teams_Tenis_Volleyball_Basketball(string _eventid, string nameTeam1, string nameTeam2, string date,
            string win1, string win2,
            string fora1, string fora2, string less, string more)
            : base(_eventid, nameTeam1, nameTeam2, date, win1, win2, fora1, fora2, less, more)
        {
            this.x = null;
            this.x_win1 = null;
            this.x_win2 = null;
            this.win1_win2 = null;
        }
    }
}