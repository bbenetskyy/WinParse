using FormulasCollection.Enums;
using System;
using System.Collections.Generic;
using SiteAccess.Model;

namespace FormulasCollection.Models
{
    /// <summary>
    /// Class contains fork details
    /// </summary>
    public class Fork
    {
        /// <summary>
        /// Id stored in DB, have auto increment so not required to fill
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Sport Type
        /// </summary>
        public string Sport { get; set; }

        /// <summary>
        /// Default place for Team Names, time of match and other details
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// Details for first type of Fork
        /// </summary>
        public string TypeFirst { get; set; }

        /// <summary>
        /// Value for first coef of Fork
        /// </summary>
        public string CoefFirst { get; set; }

        /// <summary>
        /// Details for second type of Fork
        /// </summary>
        public string TypeSecond { get; set; }

        /// <summary>
        /// Value for second coef of Fork
        /// </summary>
        public string CoefSecond { get; set; }

        /// <summary>
        /// Game time
        /// </summary>
        public DateTime? MatchDateTime { get; set; }

        /// <summary>
        /// First Bookmaker Name
        /// </summary>
        public string BookmakerFirst { get; set; }

        /// <summary>
        /// Second Bookmaker Name
        /// </summary>
        public string BookmakerSecond { get; set; }

        /// <summary>
        /// Profit from Fork
        /// </summary>
        public double Profit { get; set; }

        /// <summary>
        /// Type of Fork
        /// </summary>
        public ForkType Type { get; set; }

        /// <summary>
        /// Event Id for Pinnacle search from Marathon
        /// </summary>
        public string MarathonEventId { get; set; }

        /// <summary>
        /// Event Id for Pinnacle search from Pinnacle
        /// </summary>
        public string PinnacleEventId { get; set; }

        /// <summary>
        /// League of game
        /// </summary>
        public string League { get; set; }

        /// <summary>
        /// Id of event for placing bet in Pinnacle
        /// </summary>
        public string LineId { get; set; }

        public string Sn { get; set; }

        public string Mn { get; set; }

        public string Ewc { get; set; }

        public string Cid { get; set; }

        public string Prt { get; set; }

        public string Ewf { get; set; }

        public string Epr { get; set; }

        public List<string> Prices { get; set; }

        public string SelectionKey { get; set; }

        public string MarRate { get; set; }

        public string PinRate { get; set; }

        public string MarSuccess { get; set; }

        public string PinSuccess { get; set; }

        public SideType? SideType { get; set; }

        public TeamType? TeamType { get; set; }
            
        public int Period { get; set; }

        public BetType BetType { get; set; }
    }
}