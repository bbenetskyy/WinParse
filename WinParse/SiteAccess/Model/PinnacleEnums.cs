namespace SiteAccess.Model
{
    // Bets for pinnacle

    public enum OddsFormat
    {
        American,
        Decimal,
        Hongkong,
        Indonesian,
        Malay
    }

    public enum WinRiskType
    {
        Win,
        Risk
    }

    public enum BetType
    {
        Moneyline,
        TeamTotalPoints,
        Spread,
        TotalPoints,
        Special
    }

    public enum TeamType
    {
        Draw,
        Team1,
        Team2
    }

    public enum SideType
    {
        Over,
        Under
    }
}