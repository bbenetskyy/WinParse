namespace SiteAccess.Helpers
{
    public static class Extensions
    {
        public static string GetString(this bool val)
        {
            if (val)
                return "TRUE";
            return "FALSE";
        }
    }
}