using FormulasCollection.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ToolsPortable;

namespace FormulasCollection.Realizations
{
    public class TwoOutComeCalculatorFormulas
    {
        public TwoOutComeCalculatorFormulas()
        {
            Formatter = "-";
        }

        public string CalculateRate(double? rateMain, double? rateCurrent, double? kof)
        {
            var rate = rateCurrent * kof - rateMain;
            if (rate != null)
                return Math.Round(rate.Value, 2).
                    ToString(CultureInfo.CurrentCulture);
            return null;
        }

        public Tuple<string, string> GetRecommendedRates(double? rate, double? kof1, double? kof2)
        {
            if ((rate == null) || (kof1 == null) || (kof2 == null)) return null;

            var rate1 = Math.Round(rate.Value / (kof1.Value + kof2.Value) * kof2.Value, 2);
            var rate2 = Math.Round(rate.Value / (kof1.Value + kof2.Value) * kof1.Value, 2);
            return new Tuple<string, string>(
                rate1.ToString(CultureInfo.CurrentCulture),
                rate2.ToString(CultureInfo.CurrentCulture));
        }

        public decimal CalculateRatePart(double? currentRate, double? kof1, double? kof2)
        {
            decimal res = 0;
            bool? over = null;
            // ReSharper disable once PossibleInvalidOperationException
            var startRate = (decimal)currentRate;
            var sumRate = currentRate * 2;
            do
            {
                var recommendedRates = GetRecommendedRates(sumRate, kof1, kof2);
                if (recommendedRates?.Item1.ConvertToDecimalOrNull() == null
                 || recommendedRates.Item2.ConvertToDecimalOrNull() == null)
                    break;
                // ReSharper disable once PossibleInvalidOperationException
                var resRate = recommendedRates.Item1.ConvertToDecimalOrNull().Value;
                // ReSharper disable once PossibleInvalidOperationException
                res = recommendedRates.Item2.ConvertToDecimalOrNull().Value;
                if (resRate == startRate) break;
                if (over == null)
                {
                    over = resRate > startRate;
                }
                if (over.Value)
                {
                    if (resRate < startRate)//we can't find value
                        return 0;
                    sumRate -= 0.01;
                }
                else
                {
                    if (resRate > startRate)//we can't find value
                        return 0;
                    sumRate += 0.01;
                }
            } while (true);
            return res;
        }

        public double? CalculateSummaryRate(params double?[] rates) => rates?.Sum();

        public string CalculateAverageProfit(params double?[] profit) => (profit?.Sum() / 2).ToString();

        public string CalculateSummaryIncome(params double?[] incomes) => incomes?.Sum().ToString();

        public string Formatter { get; set; }

        public string CalculateClearRate(double? v1, double? v2)
        {
            return (v1 + v2).ToString();
        }

        public List<Fork> FilteredForks(List<Fork> forks, Filter filterPage)
        {
            if (filterPage.AfterTime != null)
                forks.RemoveAll(f => f.MatchDateTime < filterPage.AfterTime.Value);
            if (filterPage.BeforeTime != null)
                forks.RemoveAll(f => f.MatchDateTime > filterPage.BeforeTime.Value);
            if (filterPage.MinPercent != null)
                forks.RemoveAll(f => Convert.ToDecimal(f.Profit) <= filterPage.MinPercent.Value);
            if (filterPage.MaxPercent != null)
                forks.RemoveAll(f => Convert.ToDecimal(f.Profit) >= filterPage.MaxPercent.Value);
            return forks;
        }

        public double GetProfit(double coef1,
            double coef2)
        {
            var defRate = 100d;
            var rates = GetRecommendedRates(defRate, coef1, coef2);

            var rate1 = Convert.ToDouble(rates.Item1);
            var rate2 = Convert.ToDouble(rates.Item2);
            var allRate = CalculateSummaryRate(rate1, rate2);
            var income1 = Convert.ToDouble(CalculateRate(allRate, allRate - rate2, coef1));
            var income2 = Convert.ToDouble(CalculateRate(allRate, allRate - rate1, coef2));
            //todo delete this shit and refactored to one command
            return income1 < income2
                ? income1
                : income2;
        }
    }
}