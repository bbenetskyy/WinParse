using System;
using System.Collections.Generic;

namespace FormulasCollection.Realizations
{
    public class ConverterFormulas
    {
        public double IncorrectAmericanOdds => -127;

        public List<string> ConvertToAsian(double? asian1Cof, double? asian2Cof) => (asian1Cof == null || asian2Cof == null)
            ? new List<string>(new[] { "No any Rates" })
            : new List<string>(new[] { $"{asian1Cof.Value},{(asian1Cof.Value + asian2Cof.Value) / 2},{asian2Cof.Value}" });

        public double ConvertAmericanToDecimal(double? american) => american == null
                ? IncorrectAmericanOdds
                : (american.Value > 0
                    ? PositiveConvertationFormula(american.Value)
                    : NegativeConvertationFormula(american.Value));

        public double PositiveConvertationFormula(double american)
        {
            var r = american <= 0
                  ? IncorrectAmericanOdds
                  : (american / 100) + 1;
            return r;
        }

        public double NegativeConvertationFormula(double american)
        {
            var r = american >= 0 ? IncorrectAmericanOdds
              : (100 / Math.Abs(american)) + 1;
            return r;
        }
    }
}