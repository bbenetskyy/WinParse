using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarathonBetLibrary.Tools
{
    public class DateFormatHelper
    {
        //янв|фев|мар|апр|мая|июн|июл|авг|сен|окт|ноя|дек
        public static Dictionary<string, string> DateFormat = new Dictionary<string, string>
        {
            {"янв","01"},
            {"фев","02"},
            {"мар","03"},
            {"апр","04"},
            {"мая","05"},
            {"июн","06"},
            {"июл","07"},
            {"авг","08"},
            {"сен","09"},
            {"окт","10"},
            {"ноя","11"},
            {"дек","12"}
        };
    }
}
