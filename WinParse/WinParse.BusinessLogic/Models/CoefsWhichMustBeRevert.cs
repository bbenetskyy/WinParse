using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulasCollection.Models
{
    public static class CoefsWhichMustBeRevert
    {
        public static Dictionary<string, string> RevertCoefs { get; private set; }

        static CoefsWhichMustBeRevert()
        {
            FillCoefsToRevert();
        }

        private static void FillCoefsToRevert()
        {
            RevertCoefs = new Dictionary<string, string>
                {
                { "12", "X"},
                { "1X", "1"},
                { "X2", "2"},
                { "1", "1X"},
                { "2", "X2"},
                { "X", "12"},
                {"F1","F2"},
                {"F2","F1"}
            };
        }

        public static string TypeRevertParse(string typeEvent)
        {
            string typeEventTrim = typeEvent.Trim();
            if (typeEventTrim[0].Equals('F'))
            {
                string val = typeEvent.Split('(', ')')[1].ToString();
                if (val == null) return null;
                return typeEventTrim[1].Equals('1')
                    ? ("F1(" + (val[0].Equals('-') ? (val.Substring(1)) : ("-" + val.Substring(1))) + ")")
                    : ("F2(" + (val[0].Equals('-') ? (val.Substring(1)) : ("-" + val.Substring(1))) + ")");
            }

            return null;
        }
    }
}
