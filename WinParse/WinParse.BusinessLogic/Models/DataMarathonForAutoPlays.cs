using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulasCollection.Models
{
    /*{"sn":"Burgos (+9.5)",
   "mn":"Победа с учетом форы",
   "ewc":"1/1 1",
   "cid":10182298350,
   "prt":"CP",
   "ewf":"1.0",
   "epr":"1.83",
   "prices"
       :{"0":"83/100",
         "1":"1.83",
         "2":"-121",
         "3":"0.83",
         "4":"0.83",
         "5":"-1.21"}}*/
    public class DataMarathonForAutoPlays
    {
        public string sn { get; set; }
        public string mn { get; set; }
        public string ewc { get; set; }
        public string cid { get; set; }
        public string prt { get; set; }
        public string ewf { get; set; }
        public string epr { get; set; }
        public List<string> prices { get; set; }
        public string selection_key { get; set; }

        public DataMarathonForAutoPlays()
        {
            this.prices = new List<string>();
        }
    }

    public class Tags_DataMarathonForAutoPlays
    {
        public static string data_sel = "data-sel=";
        public static string data_selection_key = "data-selection-key";
    }
}
