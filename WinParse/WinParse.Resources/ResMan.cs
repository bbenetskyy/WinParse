using System;
using System.Collections;
using System.Resources;
using ToolsPortable;

namespace WinParse.Resources
{
    public static class ResMan
    {
        private static ResourceManager _instance;
        public static string NoDataSource = "__NO_DATA__";
        public static event EventHandler ResourceChaned;

        public static void SetResource(ResourceManager mgr)
        {
            _instance = mgr;
            ResourceChaned?.Invoke(mgr, new EventArgs());
        }

        public static string GetString(string code)
        {
            if (code == null)
                return NoDataSource;
            var res = _instance?.GetString(code);
            return res.IsNotBlank() ? res : NoDataSource;
        }

        public static ResourceManager GetResourceByName(string resourceName)
        {
            switch (resourceName)
            {
                case "en-GB":
                    return en_GB.ResourceManager;
                case "ru-RU":
                    return ru_RU.ResourceManager;
                case "uk-UA":
                    return uk_UA.ResourceManager;
                default:
                    return null;
            }
        }
    }
}
