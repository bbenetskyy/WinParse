using DataParser.Enums;
using System;
using System.Collections.Generic;

namespace FormulasCollection.Models
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
                {"F1","F2"},
                {"F2","F1"}
            };
            CoefsTennis = new Dictionary<string, string>
            {
                { "1","2"},
                { "2","1"},
                { "TO","TU"},
                {"TU","TO" },
                {"F1","F2"},
                {"F2","F1"}
            };
            CoefsBasketBall = new Dictionary<string, string>
            {
                { "1","2"},
                { "2","1"},
                { "TO","TU"},
                {"TU","TO" },
                {"F1","F2"},
                {"F2","F1"}
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
                {"F2","F1"}
            };
            CoefsVolleyBall = new Dictionary<string, string>
            {
                { "1","2"},
                { "2","1"},
                {"TO","TU"},
                {"TU","TO"},
                {"F1","F2"},
                {"F2","F1"}
            };
        }

    }
}
