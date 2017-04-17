using System;
using System.Collections;
using System.Resources;
using ToolsPortable;

namespace WinParse.Resources
{
    public class ResMan : SortedList
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
    }
}
