using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteAccess.Helpers
{
    public static class Extensions
    {
        public static string GetString( this bool val )
        {
            if(val)
                return "TRUE";
            return "FALSE";
        }
    }
}
