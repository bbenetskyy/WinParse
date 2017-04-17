using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WinParse.MarathonBetLibrary.Enums;
using WinParse.MarathonBetLibrary.Model;
using WinParse.MarathonBetLibrary.Setup;
using WinParse.MarathonBetLibrary.Tools;

namespace WinParse.MarathonBetLibrary
{
    public class ParseLogic
    {
        private List<MarathonEvent> _events;
        private SportType _sportType;
        private bool _isEnglish;
        public ParseLogic(SportType sportType)
        {
            _events = new List<MarathonEvent>();
            this._sportType = sportType;
        }
        public List<string> LoadId()
        {
            var eventsId = new List<string>();
            string html = LoadData(true);

            foreach (Match match in Regex.Matches(html, Tags.EventIdFull + " " + Tags.NameEvent))
            {
                string id = match.Groups[1].Value;
                eventsId.Add(id+"#"+match.Groups[2].Value);
               // MarathonEvent eventName = GetMarathonEvent(id, match.Groups[2].Value);
            }
            return eventsId;
        }
        public MarathonEvent GetMarathonEvent(string eventId, string eventTeamsEn)
        {
            MarathonEvent eventBet = null;
            string html = LoadData(false, eventId);
            if (!Helper.CheckCountEventsInPage(html))
                return null;
            var dataSel = Regex.Matches(html, Tags.DataSel);
            var selectionKey = Regex.Matches(html, Tags.SelectionKey);

            if (dataSel.Count == selectionKey.Count)
            {
                eventBet = new MarathonEvent();
                eventBet.Queue = ParseTools.QueueTeams(html);
                eventBet.EventNameEn = ParseTools.CreateEventName(eventTeamsEn, eventBet.Queue);
                if (eventBet.EventNameEn == null) return null;
                string nameEventRu = ParseTools.RegexByTags(html, Tags.NameEvent, 1);
                if (nameEventRu.Where(x => x.Equals('-')).Count() > 1)
                {
                    nameEventRu = Helper.ContainsTeamRuAndEn(nameEventRu, eventTeamsEn);
                }
                eventBet.EventNameRu = ParseTools.CreateEventName(nameEventRu, eventBet.Queue);
                eventBet.EventId = eventId;
                eventBet.LeguaId = Regex.Match(html, Tags.CategoryId).Groups[1].Value;
                eventBet.Date = ParseTools.ConvertStringToDateTime(Helper.GetDate(html));
               // eventBet.Date = ParseTools.ConvertStringToDateTime(Regex.Match(HTML, Tags.DateTime).Value);
                eventBet.IsLive = Convert.ToBoolean(ParseTools.RegexByTags(html, Tags.IsLive));
                eventBet.SportType = _sportType.ToString();

                for (int i = 0; i < dataSel.Count; i++)
                {
                    DataMarathonForAutoPlays autoPlay = ParseTools.ParseAutoPlay(dataSel[i].Groups[1].Value, selectionKey[i].Groups[1].Value);
                    bool isAsiat = autoPlay.Mn.ToLower().Contains("азиат");
                    eventBet.Coefs.Add(new MarathonCoef()
                    {
                        Recid = eventId,
                        AutoPlay = autoPlay,
                        IsAsiat = isAsiat,
                        NameCoef = ParseTools.TypeCoef(eventBet.EventNameRu, autoPlay.Sn, autoPlay.Mn, isAsiat),
                        ValueCoef = Double.Parse(autoPlay.Epr),
                        Description = autoPlay.Mn + $"[{autoPlay.Sn}]"
                    });
                }
                eventBet.Coefs = eventBet.Coefs.Where(y => !string.IsNullOrEmpty(y.NameCoef) && !y.NameCoef.Contains("ERROR") && !y.NameCoef.Contains("UNDEFINED")).Select(x => x).ToList<MarathonCoef>();
            }
            return eventBet;
        }
        private string LoadData(bool isEnglish, string eventId = null)
        {
            LinksMarathonBet linksMarathonBet = new LinksMarathonBet(isEnglish);

            string link = string.IsNullOrEmpty(eventId) ?
                linksMarathonBet.GetSportLink(_sportType) :
                linksMarathonBet.LoadSelectedLinkForEvent(eventId);

            string html = (new HtmlTools(link)).LoadHtmlString();
            return html;
        }
    }
}
