using System;

namespace WinParse.BusinessLogic.Realizations
{
    public static class ConverterHelper
    {
        internal static T EnumParse<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}