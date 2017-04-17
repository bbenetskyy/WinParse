using System;
using System.Collections.Generic;

namespace WinParse.MarathonBetLibrary.Model
{
    public class MarathonEvent
    {
        public MarathonEvent()
        {
            EventNameEn = new NameEvent();
            EventNameRu = new NameEvent();
            Coefs = new List<MarathonCoef>();
        }
        public string EventId { get; set; }
        public string LeguaId { get; set; }
        public NameEvent EventNameEn { get; set; }
        public NameEvent EventNameRu { get; set; }
        public string Queue { get; set; }
        public string LeguaName { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public string SportType { get; set; }
        public bool IsLive { get; set; }
        public List<MarathonCoef> Coefs { get; set; }
    }
    public class NameEvent
    {
        public NameEvent() { }
        public NameEvent(string name1, string name2)
        {
            NameTeam1 = name1;
            NameTeam2 = name2;
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
        public bool IsAsiat { get; set; }
        public string Description { get; set; }
        public string Error { get; set; }
        public DataMarathonForAutoPlays AutoPlay { get; set; }
    }
    public class DataMarathonForAutoPlays
    {
        public string Sn { get; set; }
        public string Mn { get; set; }
        public string Ewc { get; set; }
        public string Cid { get; set; }
        public string Prt { get; set; }
        public string Ewf { get; set; }
        public string Epr { get; set; }
        public List<string> Prices { get; set; }
        public string SelectionKey { get; set; }
        public DataMarathonForAutoPlays()
        {
            Prices = new List<string>();
        }
    }
}
