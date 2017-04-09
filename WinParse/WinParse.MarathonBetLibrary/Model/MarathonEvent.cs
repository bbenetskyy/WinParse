using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarathonBetLibrary.Model
{
    public class MarathonEvent
    {
        public MarathonEvent()
        {
            this.EventNameEN = new NameEvent();
            this.EventNameRU = new NameEvent();
            this.Coefs = new List<MarathonCoef>();
        }
        public string EventId { get; set; }
        public string LeguaId { get; set; }
        public NameEvent EventNameEN { get; set; }
        public NameEvent EventNameRU { get; set; }
        public string Queue { get; set; }
        public string LeguaName { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public string SportType { get; set; }
        public bool isLive { get; set; }
        public List<MarathonCoef> Coefs { get; set; }
    }
    public class NameEvent
    {
        public NameEvent() { }
        public NameEvent(string name1, string name2)
        {
            this.NameTeam1 = name1;
            this.NameTeam2 = name2;
        }
        public string NameTeam1 { get; set; }
        public string NameTeam2 { get; set; }
        public string FullName { get { return NameTeam1 + "#" + NameTeam2; } }
        public string PositionTeam(string nameTeam)
        {
            if (NameTeam1.ToLower().Equals(nameTeam.ToLower())) return "1";
            if (NameTeam2.ToLower().Equals(nameTeam.ToLower())) return "2";
            return "NULL";
        }
    }
    public class MarathonCoef
    {
        public MarathonCoef()
        {
            AutoPlay = new DataMarathonForAutoPlays();
        }
        public string Recid { get; set; }
        public string NameCoef { get; set; }
        public double ValueCoef { get; set; }
        public bool isAsiat { get; set; }
        public string Description { get; set; }
        public string Error { get; set; }
        public DataMarathonForAutoPlays AutoPlay { get; set; }
    }
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
}
