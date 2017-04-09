using System;

namespace FormulasCollection.Enums
{
    [Flags]
    public enum ForkType
    {
        /// <summary>
        /// Forks available in SearchPage only
        /// </summary>
        Current,
        /// <summary>
        /// Forks available in AccountingPage only
        /// </summary>
        Saved
    }
}
