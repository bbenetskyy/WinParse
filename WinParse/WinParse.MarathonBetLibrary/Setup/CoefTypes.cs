namespace WinParse.MarathonBetLibrary.Setup
{
    public class CoefTypes
    {
        public static string ResultsDraw = "результативная ничья";
        public static string ResultsDrawPart = "RD";

        public static string Wins = "победа";
        public static string Draw = "ничья";

        public static string WinsWithFora = "победа с учетом форы";
        public static string WinsWithAsiatFora = "победа с учетом азиатской форы";
        public static string WinsWithForaVolleyball = "победа в матче с учетом форы по очкам";

        public static string Total = "тотал голов";
        public static string TotalAsiat = "азиатский тотал голов";
        public static string TotalVolleyball = "тотал матча по очкам";
        public static string Under = "меньше";
        public static string Over = "больше";

        public static string Odd = "нечет";
        public static string OddPart = "TOOD";
        public static string Even = "чет";
        public static string EvenPart = "TEVEN";

        public static string Half = "тайм";
        public static string Half2 = "половина";
        public static string Quarter = "четверть";
        public static string Period = "период";
        public static string Game = "гейм";
        public static string Party = "парти";
        public static string Set = "сет";
        public static string PartyPart = "PR";
        public static string SetPart = "S";
        public static string GamePart = "G";
        public static string PeriodPart = "P";
        public static string QuarterPart = "Q";
        public static string HalfPart = "H";
        public static string Team = "T";

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
