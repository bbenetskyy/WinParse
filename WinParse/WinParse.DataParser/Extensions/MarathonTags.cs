using System.Collections.Generic;

namespace DataParser.Extensions
{
    public static class MarathonTags
    {
        public static readonly string NameTeam = "member-name nowrap";// <div class="today-member-name nowrap " data-ellipsis="{}">;
        public static readonly string Date = "<td class=\"date\">";
        public static readonly string EventId = "data-event-treeId";
        public static readonly string Coff = "data-selection-price=\"";
        public static readonly string TypeCoff = "<span class=\"hint\">";
        public static readonly string Fora = "data-market-type=\"HANDICAP\"";
        public static readonly string Total = "data-market-type=\"TOTAL\"";
        public static readonly string Liga = "<span class=\"nowrap\">";


        public static readonly string ForaFull = "coeff-handicap";

        public static readonly string IsLive = "data-live";
        public static readonly string LigaContainerId = "data-category-treeId";






        public static readonly string NewTypeCoeff = "coupone-labels";
        public static readonly string NewTagTypeCoeff = "</tr>";
        public static readonly string NewFora = "\"HANDICAP\"";
        public static readonly string NewTotal = "\"TOTAL\"";
        public static readonly string NewclassToTypeCoef = "\"hint\"";



        public static readonly string MainTagForEvent = "tbody";
        public static readonly string NewEventId = "data-event-treeId";

        public static readonly string NewTeamName = "member-name nowrap";

        public static readonly string NewDate = "class=\"date \"";

        public static readonly string NewIssLive = "isLive";

        public static readonly string NewAutoplay = "data-sel=";
        public static string NewSelectionKey = "data-selection-key";

        public static readonly string NewValueCoef = "data-selection-price";






        //Fora
        public static string ForaStartTab = "block-market-table-wrapper";
        public static string ForaNameTab = "name-field";
        public static string ForaTypeCoef = "coeff-handicap"; //+1
        public static string ForaValueCoef = "data-selection-price";

        public static List<string> WithoutName = new List<string>()
        {
            "период",
            "Период",
            "четвер",
            "Четвер",
            "половин",
            "Половин"
        };
        public static List<string> WithoutForeNum = new List<string>()
        {
            "0",
            "-1"
        };

    }
}