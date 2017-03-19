using Common.Modules.AntiCaptha;
using System.Text;

namespace SiteAccess.Helpers
{
    public class LocalCaptcha : IAntiCaptcha
    {
        public string GetAnswer(string data)
        {
            return GetAnswer(Encoding.Default.GetBytes(data));
        }

        public string GetAnswer(byte[] data)
        {
            return string.Empty;
        }
    }
}