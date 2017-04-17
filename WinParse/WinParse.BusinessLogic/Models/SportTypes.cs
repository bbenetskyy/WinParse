using System;
using System.Collections.Generic;

namespace FormulasCollection.Models
{
    public static class SportTypes
    {
        public static Dictionary<string, string> TypeCoefsAll { get; private set; }

        static SportTypes()
        {
            InitCoefsAll();
        }
        private static void InitCoefsAll()
        {
            TypeCoefsAll = new Dictionary<string, string>
            {
                {"F1","F2"},
                {"F2","F1"},
                {"1","X2"},
                {"X","12"},
                {"2","1X"},
                {"1X","2"},
                {"X2","1"},
                {"F1H1","F2H1"},
                {"F2H1","F1H1"},
                {"12","X"},
                {"F1H2","F2H2"},
                {"F2H2","F1H2"},
                {"F1Q1","F2Q1"},
                {"F2Q1","F1Q1"},
                {"F1Q2","F2Q2"},
                {"F2Q2","F1Q2"},
                {"F1Q3","F2Q3"},
                {"F2Q3","F1Q3"},
                {"F1Q4","F2Q4"},
                {"F2Q4","F1Q4"},
                {"1H1","X2H1"},
                {"XH1","12H1"},
                {"2H1","1XH1"},
                {"1XH1","2H1"},
                {"X2H1","1H1"},
                {"1H2","X2H2"},
                {"XH2","12H2"},
                {"2H2","1XH2"},
                {"1XH2","2H2"},
                {"X2H2","1H2"},
                {"1Q1","X2Q1"},
                {"XQ1","12Q1"},
                {"2Q1","1XQ1"},
                {"1XQ1","2Q1"},
                {"X2Q1","1Q1"},
                {"1Q2","X2Q2"},
                {"XQ2","12Q2"},
                {"2Q2","1XQ2"},
                {"1XQ2","2Q2"},
                {"X2Q2","1Q2"},
                {"1Q3","X2Q3"},
                {"XQ3","12Q3"},
                {"2Q3","1XQ3"},
                {"1XQ3","2Q3"},
                {"X2Q3","1Q3"},
                {"1Q4","X2Q4"},
                {"XQ4","12Q4"},
                {"2Q4","1XQ4"},
                {"1XQ4","2Q4"},
                {"X2Q4","1Q4"},
                {"12H1","XH1"},
                {"12H2","XH2"},
                {"12Q1","XQ1"},
                {"12Q2","XQ2"},
                {"12Q3","XQ3"},
                {"12Q4","XQ4"},
                {"TU","TO"},
                {"TO","TU"},
                {"F2P1","F1P1"},
                {"F1P1","F2P1"},
                {"F2P2","F1P2"},
                {"F1P2","F2P2"},
                {"F2P3","F1P3"},
                {"F1P3","F2P3"},
                {"TOOD","TEVEN"},
                {"TEVEN","TOOD"},
                {"TUT2","TOT2"},
                {"TOT2","TUT2"},
                {"TUT1","TOT1"},
                {"TOT1","TUT1"},
                {"TUP1","TOP1"},
                {"TOP1","TUP1"},
                {"TUP2","TOP2"},
                {"TOP2","TUP2"},
                {"TUP3","TOP3"},
                {"TOP3","TUP3"},
                {"2P1","1XP1"},
                {"XP1","12P1"},
                {"1P1","X2P1"},
                {"X2P1","1P1"},
                {"12P1","XP1"},
                {"1XP1","2P1"},
                {"2P2","1XP2"},
                {"XP2","12P2"},
                {"1P2","X2P2"},
                {"X2P2","1P2"},
                {"12P2","XP2"},
                {"1XP2","2P2"},
                {"2P3","1XP3"},
                {"XP3","12P3"},
                {"1P3","X2P3"},
                {"X2P3","1P3"},
                {"12P3","XP3"},
                {"1XP3","2P3"}
            };
        }

    }
}
