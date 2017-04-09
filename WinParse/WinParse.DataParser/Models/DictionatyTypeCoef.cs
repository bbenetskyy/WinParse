using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models
{
    public static class DictionatyTypeCoef
    {
        #region[FOOTBALL]
        #region[Totals]
        public readonly static string TT = "H"; //Тайм  TTO(5) and TTU(5)
        public readonly static string TF = "TF"; // Total для 1 -2 команди нп. TF1O(5) and TF2U(5)
        #endregion
        #region[Fora]
        //public readonly static string
        #endregion
        #endregion

        #region[BASKETBALL]
        #region[Totals]
        public readonly static string TPT = "TPT";// Половини TPTU(5) and TPTO(5)
        #endregion
        #region[Fora]
        //public readonly static string
        #endregion
        #endregion

        #region[Hokej]
        #region[Totals]
        public readonly static string TPR = "TPR"; // Період TPRO(5) and TPRU(5)
        #endregion
        #region[Fora]
        //public readonly static string
        #endregion
        #endregion

        #region[VOLEYBALL]
        #region[Totals]
        //public readonly static string
        #endregion
        #region[Fora]
        //public readonly static string
        #endregion
        #endregion

        #region[TENNIS]
        #region[Totals]
        public readonly static string TS = "TS";// сети TSO(5) and TSU(5)
        public readonly static string TG = "TG";// гейми TGO(5) and TGO(5)
        #endregion
        #region[Fora]
        //public readonly static string
        #endregion
        #endregion
    }
}
