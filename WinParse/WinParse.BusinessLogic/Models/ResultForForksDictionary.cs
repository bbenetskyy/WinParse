using System;
using System.Collections.Generic;

namespace FormulasCollection.Models
{
    public class ResultForForksDictionary
    {
        public string TeamNames { get; set; }

        public DateTime MatchDateTime { get; set; }

        public string EventId { get; set; }

        public string LeagueName { get; set; }

        public Dictionary<string, ForkDetail> ForkDetailDictionary { get; set; }
    }
}