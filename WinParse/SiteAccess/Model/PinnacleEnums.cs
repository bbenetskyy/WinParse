namespace SiteAccess.Enums
{
    // Bets for pinnacle

    public enum OddsFormat
    {
        AMERICAN,
        DECIMAL,
        HONGKONG,
        INDONESIAN,
        MALAY
    }

    public enum WinRiskType
    {
        WIN,
        RISK
    }

    public enum BetType
    {
        MONEYLINE,
        TEAM_TOTAL_POINTS,
        SPREAD,
        TOTAL_POINTS,
        SPECIAL
    }

    public enum TeamType
    {
        DRAW,
        TEAM1,
        TEAM2
    }

    public enum SideType
    {
        OVER,
        UNDER
    }
}