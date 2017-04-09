using MarathonBetLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarathonBetLibrary.Setup
{
    public class LinksMarathonBet
    {
        public readonly string MAIN_LINK = "https://www.marathonbet.com/";
        public string Language { get; private set; }

        private readonly string RU = "su";
        private readonly string EN = "en";

        private readonly string soccer = "Football/";
        private readonly string basketball = "Basketball/";
        private readonly string hockey = "Ice+Hockey/";
        private readonly string tenis = "Tennis/";
        private readonly string volleyball = "Volleyball/";

        private readonly string betting = "/betting/";
        private readonly string events ="/events.htm?id=";

        public LinksMarathonBet(bool LoadEnglishName)
        {
            Language = LoadEnglishName ? EN : RU;
        }

        public string SOCCER { get { return MAIN_LINK + Language + betting + soccer; } private set { } }
        public string BASKETBALL { get { return MAIN_LINK + Language + betting + basketball; } private set { } }
        public string HOCKEY { get { return MAIN_LINK + Language + betting + hockey; } private set { } }
        public string TENIS { get { return MAIN_LINK + Language + betting + tenis; } private set { } }
        public string VOLLEYBALL { get { return MAIN_LINK + Language + betting + volleyball; } private set { } }

        public string GetSportLink(SportType sportType)
        {
            switch (sportType)
            {
                case SportType.Soccer: return SOCCER;
                case SportType.Basketball: return BASKETBALL;
                case SportType.Hockey: return HOCKEY;
                case SportType.Tennis: return TENIS;
                case SportType.Volleyball: return VOLLEYBALL;
                default: return string.Empty;
            }
        }

        public string LoadSelectedLinkForEvent(string eventID)
        {
            return MAIN_LINK + Language + events + eventID;
        }
    }
}
