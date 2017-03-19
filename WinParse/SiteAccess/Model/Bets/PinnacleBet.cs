using SiteAccess.Enums;
using System;

namespace SiteAccess.Model.Bets
{
    public class PinnacleBet : ICloneable
    {
        /// <summary>
        /// АПИ требует для того, чтобы 2 одинаковых запроса случайно не попали на сервер как 2 разных. Guid генерируется
        /// у себя, главное, чтобы следущий такой же отправился не раньше, чем через 30 мин, иначе сгенерирует ошибку.
        /// </summary>
        public string Guid;

        /// <summary>
        /// Принять лучшие коеффициенты 
        /// </summary>
        public bool AcceptBetterLine;

        public string CustomerReference; // not reqiured
        public OddsFormat OddsFormat;

        /// <summary>
        /// Ставка. 
        /// </summary>
        public decimal Stake;

        public WinRiskType WinRiskRate;
        public int SportId;
        public long Eventid;
        public int PeriodNumber;
        public BetType BetType;
        public TeamType TeamType; // for bet type == SPREAD, MONEYLINE and TEAM_TOTAL_POINTS
        public SideType Side; // for bet type == TOTAL_POINTS and TEAM_TOTAL_POINTS
        public long LineId;
        public long? AlternativeLineId; // not required
        public bool? Pitcher1MustStart; //baseballonly (sportid = 3)
        public bool? Pitcher2MustStart; //baseballonly

        public object Clone()
        {
            return new PinnacleBet
            {
                Guid = Guid,
                AcceptBetterLine = AcceptBetterLine,
                CustomerReference = CustomerReference,
                OddsFormat = OddsFormat,
                Stake = Stake,
                WinRiskRate = WinRiskRate,
                SportId = SportId,
                Eventid = Eventid,
                PeriodNumber = PeriodNumber,
                TeamType = TeamType,
                Side = Side,
                LineId = LineId,
                AlternativeLineId = AlternativeLineId,
                Pitcher1MustStart = Pitcher1MustStart,
                Pitcher2MustStart = Pitcher2MustStart
            };
        }
    }
}