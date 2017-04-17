using WinParse.MarathonBetLibrary.Enums;

namespace WinParse.MarathonBetLibrary.Setup
{
    public class LinksMarathonBet
    {
        public readonly string MainLink = "https://www.marathonbet.com/";
        public string Language { get; private set; }

        private readonly string _ru = "su";
        private readonly string _en = "en";

        private readonly string _soccer = "Football/";
        private readonly string _basketball = "Basketball/";
        private readonly string _hockey = "Ice+Hockey/";
        private readonly string _tenis = "Tennis/";
        private readonly string _volleyball = "Volleyball/";

        private readonly string _betting = "/betting/";
        private readonly string _events ="/events.htm?id=";

        public LinksMarathonBet(bool loadEnglishName)
        {
            Language = loadEnglishName ? _en : _ru;
        }

        public string Soccer { get { return MainLink + Language + _betting + _soccer; } private set { } }
        public string Basketball { get { return MainLink + Language + _betting + _basketball; } private set { } }
        public string Hockey { get { return MainLink + Language + _betting + _hockey; } private set { } }
        public string Tenis { get { return MainLink + Language + _betting + _tenis; } private set { } }
        public string Volleyball { get { return MainLink + Language + _betting + _volleyball; } private set { } }

        public string GetSportLink(SportType sportType)
        {
            switch (sportType)
            {
                case SportType.Soccer: return Soccer;
                case SportType.Basketball: return Basketball;
                case SportType.Hockey: return Hockey;
                case SportType.Tennis: return Tenis;
                case SportType.Volleyball: return Volleyball;
                default: return string.Empty;
            }
        }

        public string LoadSelectedLinkForEvent(string eventId)
        {
            return MainLink + Language + _events + eventId;
        }
    }
}
