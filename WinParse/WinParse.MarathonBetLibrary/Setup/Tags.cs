namespace WinParse.MarathonBetLibrary.Setup
{
    public class Tags
    {
        private static readonly string ContentRegix = "(.*?)";
        private static readonly string ContentRegixDoubleQuotes = "=\"" + ContentRegix + "\""; // ="...." 
        private static readonly string ContentRegixDoubleQuotes2 = "\"" + ContentRegix + "\""; //  "...."
        private static readonly string ContentRegixOneQuotes = "='" + ContentRegix + "'";      //  '....'

        private static readonly string EventId = "data-event-treeId";
        private static readonly string nameEvent = "data-event-name";
        private static readonly string data_sel = "data-sel";
        private static readonly string selection_key = "data-selection-key";
        private static readonly string category_id = "data-category-treeid";
        private static readonly string Live = "data-live";

        #region[AUTO PLAY]
        public static string Sn { get { return ContextRegexAutoPlay("sn"); } }
        public static string Mn { get { return ContextRegexAutoPlay("mn"); } }
        public static string Ewc { get { return ContextRegexAutoPlay("ewc"); } }
        public static string Cid { get { return ContextRegexAutoPlay("cid"); } }
        public static string Prt { get { return ContextRegexAutoPlay("prt"); } }
        public static string Ewf { get { return ContextRegexAutoPlay("ewf"); } }
        public static string Epr { get { return ContextRegexAutoPlay("epr"); } }
        public static string Prices { get { return ContextRegexAutoPlay("prices"); } }

        public static string ContextRegexAutoPlay(string value)
        {
            if (value.Equals("cid")) return "\"" + value + "\":" + ContentRegix + ",";
            else if (value.Equals("prices")) { return "\"" + value + "\":{" + ContentRegix + "}"; }
            return "\"" + value + "\":" + ContentRegixDoubleQuotes2;
        }
        #endregion

        public static readonly string EventIdFull = EventId + ContentRegixDoubleQuotes;
        public static readonly string NameEvent = nameEvent + ContentRegixDoubleQuotes;
        public static readonly string DataSel = data_sel + ContentRegixOneQuotes;
        public static readonly string SelectionKey = selection_key + ContentRegixDoubleQuotes;
        public static readonly string CategoryId = "(" + category_id + ContentRegixDoubleQuotes + ")";
        public static readonly string IsLive = Live + ContentRegixDoubleQuotes;
        public static string GetNumTeamRegex { get { return "member-number\">" + ContentRegix + "<"; } }
        public static string DateTime { get { return "([0-9]{2}.*)((янв|фев|мар|апр|мая|июн|июл|авг|сен|окт|ноя|дек).*)([0-9]{2}.*)(:)([0-9]{2}.*)"; } }
        public static string Time { get { return "([0-9]{2}.*)(:)([0-9]{2}.*)"; } }
        public static string Date { get { return "class=\"date \""; } }


    }
}
