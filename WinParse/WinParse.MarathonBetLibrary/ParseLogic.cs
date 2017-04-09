using HtmlAgilityPack;
using MarathonBetLibrary.Enums;
using MarathonBetLibrary.Model;
using MarathonBetLibrary.Setup;
using MarathonBetLibrary.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarathonBetLibrary
{
    public class ParseLogic
    {
        private List<MarathonEvent> Events;
        private SportType sportType;
        private bool isEnglish;
        public ParseLogic(SportType sportType)
        {
            Events = new List<MarathonEvent>();
            this.sportType = sportType;
        }
        public List<string> LoadID()
        {
            var _eventsID = new List<string>();
            string HTML = LoadData(true);

            foreach (Match match in Regex.Matches(HTML, Tags.EventID + " " + Tags.NameEvent))
            {
                string id = match.Groups[1].Value;
                _eventsID.Add(id+"#"+match.Groups[2].Value);
               // MarathonEvent eventName = GetMarathonEvent(id, match.Groups[2].Value);
            }
            return _eventsID;
        }
        public MarathonEvent GetMarathonEvent(string eventId, string eventTeamsEN)
        {
            MarathonEvent eventBet = null;
            string HTML = LoadData(false, eventId);
            if (!Helper.CheckCountEventsInPage(HTML))
                return null;
            var data_sel = Regex.Matches(HTML, Tags.DataSel);
            var selection_key = Regex.Matches(HTML, Tags.SelectionKey);

            if (data_sel.Count == selection_key.Count)
            {
                eventBet = new MarathonEvent();
                eventBet.Queue = ParseTools.QueueTeams(HTML);
                eventBet.EventNameEN = ParseTools.CreateEventName(eventTeamsEN, eventBet.Queue);
                if (eventBet.EventNameEN == null) return null;
                string nameEventRU = ParseTools.RegexByTags(HTML, Tags.NameEvent, 1);
                if (nameEventRU.Where(x => x.Equals('-')).Count() > 1)
                {
                    nameEventRU = Helper.ContainsTeamRuAndEn(nameEventRU, eventTeamsEN);
                }
                eventBet.EventNameRU = ParseTools.CreateEventName(nameEventRU, eventBet.Queue);
                eventBet.EventId = eventId;
                eventBet.LeguaId = Regex.Match(HTML, Tags.CategoryId).Groups[1].Value;
                eventBet.Date = ParseTools.ConvertStringToDateTime(Helper.GetDate(HTML));
               // eventBet.Date = ParseTools.ConvertStringToDateTime(Regex.Match(HTML, Tags.DateTime).Value);
                eventBet.isLive = Convert.ToBoolean(ParseTools.RegexByTags(HTML, Tags.IsLive));
                eventBet.SportType = sportType.ToString();

                for (int i = 0; i < data_sel.Count; i++)
                {
                    DataMarathonForAutoPlays autoPlay = ParseTools.ParseAutoPlay(data_sel[i].Groups[1].Value, selection_key[i].Groups[1].Value);
                    bool isAsiat = autoPlay.mn.ToLower().Contains("азиат");
                    eventBet.Coefs.Add(new MarathonCoef()
                    {
                        Recid = eventId,
                        AutoPlay = autoPlay,
                        isAsiat = isAsiat,
                        NameCoef = ParseTools.TypeCoef(eventBet.EventNameRU, autoPlay.sn, autoPlay.mn, isAsiat),
                        ValueCoef = Double.Parse(autoPlay.epr),
                        Description = autoPlay.mn + $"[{autoPlay.sn}]"
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
                linksMarathonBet.GetSportLink(sportType) :
                linksMarathonBet.LoadSelectedLinkForEvent(eventId);

            string HTML = (new HTMLTools(link)).LoadHtmlString();
            return HTML;
        }
    }
}
