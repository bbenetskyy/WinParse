namespace DataParser.Models
{
    public class TeamsTenisVolleyballBasketball : Teams
    {
        public TeamsTenisVolleyballBasketball() : base()
        {
        }

        public TeamsTenisVolleyballBasketball(string eventid, string nameTeam1, string nameTeam2, string date,
            string win1, string win2,
            string fora1, string fora2, string less, string more)
            : base(eventid, nameTeam1, nameTeam2, date, win1, win2, fora1, fora2, less, more)
        {
            X = null;
            XWin1 = null;
            XWin2 = null;
            Win1Win2 = null;
        }
    }
}