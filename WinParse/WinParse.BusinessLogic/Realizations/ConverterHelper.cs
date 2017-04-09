using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulasCollection.Realizations
{
    public static class ConverterHelper
    {
        [DebuggerStepThrough]
        internal static T EnumParse<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}