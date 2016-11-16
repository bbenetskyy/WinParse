using Common.Modules.AntiCaptha;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SiteAccess.Helpers
{
    public class LocalCaptcha : IAntiCaptcha
    {
        public string GetAnswer( string data )
        {
            return GetAnswer(Encoding.Default.GetBytes(data));
        }

        public string GetAnswer( byte[] data )
        {
            return string.Empty;
        }
    }
}
