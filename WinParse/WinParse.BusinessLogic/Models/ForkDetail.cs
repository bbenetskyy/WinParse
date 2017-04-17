using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteAccess.Model;

namespace FormulasCollection.Models
{
    public class ForkDetail
    {
        public double TypeCoef { get; set; }
        public string LineId { get; set; }
        public int Period { get; set; }
        public SideType SideType { get; set; }
        public TeamType TeamType { get; set; }
        public BetType BetType { get; set; }
    }
}
