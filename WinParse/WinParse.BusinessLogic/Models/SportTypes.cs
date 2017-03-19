using System.Collections.Generic;

namespace WinParse.BusinessLogic.Models
{
    public static class SportTypes
    {
        private static Dictionary<string, string> CoefsSoccer;
        private static Dictionary<string, string> CoefsTennis;
        private static Dictionary<string, string> CoefsBasketBall;
        private static Dictionary<string, string> CoefsHockey;
        private static Dictionary<string, string> CoefsVolleyBall;

        public static Dictionary<string, string> TypeCoefsSoccer => CoefsSoccer;
        public static Dictionary<string, string> TypeCoefsTennis => CoefsTennis;
        public static Dictionary<string, string> TypeCoefsBasketBall => CoefsBasketBall;
        public static Dictionary<string, string> TypeCoefsHockey => CoefsHockey;
        public static Dictionary<string, string> TypeCoefsVolleyBall => CoefsVolleyBall;

        static SportTypes()
        {
            initCoefsAll();
        }

        private static void initCoefsAll()
        {
            CoefsSoccer = new Dictionary<string, string>
            {
                { "12", "X"},
                { "1X", "2"},
                { "X2", "1"},
                { "1", "X2"},
                { "2", "1X"},
                { "X", "12"},
                { "TO","TU"},
                {"TU","TO" },
                { "TTO", "TTU" },
                { "TTU", "TTO" },
                {"F1","F2"},
                {"F2","F1"},
                { "TF1U", "TF1O"},
                { "TF2U", "TF2O"},
                { "TF1O", "TF1U"},
                { "TF2O", "TF2U"}
            };
            CoefsTennis = new Dictionary<string, string>
            {
                { "1","2"},
                { "2","1"},
                { "TO","TU"},
                {"TU","TO" },
                {"F1","F2"},
                {"F2","F1"},
                {"TSO","TSU" },
                {"TSU","TSO" },
                {"TGO","TGU" },
                {"TGU","TGO" },
                { "TF1U", "TF1O"},
                { "TF2U", "TF2O"},
                { "TF1O", "TF1U"},
                { "TF2O", "TF2U"}
            };
            CoefsBasketBall = new Dictionary<string, string>
            {
                {"1","2"},
                {"2","1"},
                {"TO","TU"},
                {"TU","TO" },
                {"F1","F2"},
                {"F2","F1"},
                { "TPTO","TPTU" },
                { "TPTU","TPTO" },
                { "TF1U", "TF1O"},
                { "TF2U", "TF2O"},
                { "TF1O", "TF1U"},
                { "TF2O", "TF2U"}
            };
            CoefsHockey = new Dictionary<string, string>
            {
                { "12", "X"},
                { "1X", "2"},
                { "X2", "1"},
                { "1", "X2"},
                { "2", "1X"},
                { "X", "12"},
                { "TO","TU"},
                {"TU","TO" },
                {"F1","F2"},
                {"F2","F1"},
                { "TPRO","TPRU" },
                { "TPRU","TPRO" },
                { "TF1U", "TF1O"},
                { "TF2U", "TF2O"},
                { "TF1O", "TF1U"},
                { "TF2O", "TF2U"}
            };
            CoefsVolleyBall = new Dictionary<string, string>
            {
                { "1","2"},
                { "2","1"},
                {"TO","TU"},
                {"TU","TO"},
                {"F1","F2"},
                {"F2","F1"},
                { "TF1U", "TF1O"},
                { "TF2U", "TF2O"},
                { "TF1O", "TF1U"},
                { "TF2O", "TF2U"}
            };
        }
    }
}