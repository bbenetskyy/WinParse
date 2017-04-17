using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace WinParse.MarathonBetLibrary.Tools
{
    public class HtmlTools
    {
        private string _path;
        public HtmlTools(string path)
        {
            _path = path;
        }
        public HtmlDocument LoadHtmlDocument()
        {
            string html = LoadHtmlString();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            return document;
        }
        public string LoadHtmlString()
        {
            string html = string.Empty;
            if (string.IsNullOrEmpty(_path)) return null;
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                html = wc.DownloadString(_path);
            }
            return html;
        }
    }
}
