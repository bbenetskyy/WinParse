using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MarathonBetLibrary.Tools
{
    public class HTMLTools
    {
        private string PATH;
        public HTMLTools(string path)
        {
            this.PATH = path;
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
            if (string.IsNullOrEmpty(PATH)) return null;
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;
                html = wc.DownloadString(PATH);
            }
            return html;
        }
    }
}
