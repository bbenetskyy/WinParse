using System.Collections.Generic;

namespace DataParser.Extensions
{
    public static class MarathonTags
    {
        public static readonly string NameTeam = "member-name nowrap";// <div class="today-member-name nowrap " data-ellipsis="{}">;
        public static readonly string Date = "<td class=\"date\">";
        public static readonly string EventID = "data-event-treeId";
        public static readonly string Coff = "data-selection-price=\"";
        public static readonly string TypeCoff = "<span class=\"hint\">";
        public static readonly string Fora = "data-market-type=\"HANDICAP\"";
        public static readonly string Total = "data-market-type=\"TOTAL\"";
        public static readonly string Liga = "<span class=\"nowrap\">";


        public static readonly string ForaFull = "coeff-handicap";

        public static readonly string IsLive = "data-live";
        public static readonly string Liga_ContainerID = "data-category-treeId";






        public static readonly string newTypeCoeff = "coupone-labels";
        public static readonly string newTag_TypeCoeff = "</tr>";
        public static readonly string newFora = "\"HANDICAP\"";
        public static readonly string newTotal = "\"TOTAL\"";
        public static readonly string newclassToTypeCoef = "\"hint\"";



        public static readonly string mainTagForEvent = "tbody";
        public static readonly string newEventID = "data-event-treeId";

        public static readonly string newTeamName = "member-name nowrap";

        public static readonly string newDate = "class=\"date \"";

        public static readonly string newIssLive = "isLive";

        public static readonly string newAutoplay = "data-sel=";
        public static string newSelection_key = "data-selection-key";

        public static readonly string newValueCoef = "data-selection-price";






        //Fora
        public static string fora_startTab = "block-market-table-wrapper";
        public static string fora_nameTab = "name-field";
        public static string fora_typeCoef = "coeff-handicap"; //+1
        public static string fora_valueCoef = "data-selection-price";

        public static List<string> WITHOUT_NAME = new List<string>()
        {
            "период",
            "Период",
            "четвер",
            "Четвер",
            "половин",
            "Половин"
        };
        public static List<string> WITHOUT_FORE_NUM = new List<string>()
        {
            "0",
            "-1"
        };

    }
}