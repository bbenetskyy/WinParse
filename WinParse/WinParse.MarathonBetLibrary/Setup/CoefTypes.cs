using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarathonBetLibrary.Setup
{
    public class CoefTypes
    {
        public static string RESULTS_DRAW = "результативная ничья";
        public static string RESULTS_DRAW_PART = "RD";

        public static string WINS = "победа";
        public static string DRAW = "ничья";

        public static string WINS_WITH_FORA = "победа с учетом форы";
        public static string WINS_WITH_ASIAT_FORA = "победа с учетом азиатской форы";
        public static string WINS_WITH_FORA_VOLLEYBALL = "победа в матче с учетом форы по очкам";

        public static string TOTAL = "тотал голов";
        public static string TOTAL_ASIAT = "азиатский тотал голов";
        public static string TOTAL_VOLLEYBALL = "тотал матча по очкам";
        public static string UNDER = "меньше";
        public static string OVER = "больше";

        public static string ODD = "нечет";
        public static string ODD_PART = "TOOD";
        public static string EVEN = "чет";
        public static string EVEN_PART = "TEVEN";

        public static string HALF = "тайм";
        public static string HALF2 = "половина";
        public static string QUARTER = "четверть";
        public static string PERIOD = "период";
        public static string GAME = "гейм";
        public static string PARTY = "парти";
        public static string SET = "сет";
        public static string PARTY_PART = "PR";
        public static string SET_PART = "S";
        public static string GAME_PART = "G";
        public static string PERIOD_PART = "P";
        public static string QUARTER_PART = "Q";
        public static string HALF_PART = "H";
        public static string TEAM = "T";

        public static string SimpleFora(string numTeam, string fora)
        {
            string result = "F" + numTeam + fora;
            if (result.Contains(" ")) result = result.Replace(" ", "");
            return result;
        }
        public static string OtherFora(string numTeam, string fora, string other = null)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(other))
                result = "F" + numTeam + fora;
            else
                result = "F" + numTeam + other + fora;
            if (result.Contains(" ")) result = result.Replace(" ", "");
            return result;
        }

        public static string SimpleTotal(string totalValue, string overOrUnder)
        {
            string result = $"T{overOrUnder}({totalValue})";
            if (result.Contains(" ")) result = result.Replace(" ", "");
            return result;
        }
        public static string OtherTotal(string totalValue, string overOrUnder, string other = null)
        {
            string result = $"T{overOrUnder}{other}({totalValue})";
            if (result.Contains(" ")) result = result.Replace(" ", "");
            return result;
        }
    }
}
