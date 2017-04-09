using FormulasCollection.Realizations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolsPortable;

namespace UnitTest.FormulasCollectionTests.CalculationTests
{
    [TestClass]
    public class RatesTests
    {
        [TestMethod]
        public void CalculateRatePartTest()
        {
            var obj = new TwoOutComeCalculatorFormulas();
            var marRate = 1.0;
            var kof1 = 4.25;
            var kof2 = 2.67;
            var pinRate = obj.CalculateRatePart(marRate, kof1, kof2);

            Assert.IsTrue(pinRate > 0);

            marRate = 1.0;
            kof1 = 2.33;
            kof2 = 2.34;
            pinRate = obj.CalculateRatePart(marRate, kof1, kof2);

            Assert.IsTrue(pinRate > 0);

            marRate = 1.0;
            kof1 = 1.56;
            kof2 = 2.81;
            pinRate = obj.CalculateRatePart(marRate, kof1, kof2);

            Assert.IsTrue(pinRate > 0);

            marRate = 1.0;
            kof1 = 1.6;
            kof2 = 2.73;
            pinRate = obj.CalculateRatePart(marRate, kof1, kof2);

            Assert.IsTrue(pinRate > 0);

        }

        [TestMethod]
        public void GetRecommendedRatesTest()
        {
            var obj = new TwoOutComeCalculatorFormulas();
            var rate = 2.0;
            var kof1 = 4.25;
            var kof2 = 2.67;
            var recommendedRates = obj.GetRecommendedRates(rate, kof1, kof2);

            Assert.IsNotNull(recommendedRates);
            Assert.IsNotNull(recommendedRates.Item1.ConvertToDecimalOrNull());
            Assert.IsNotNull(recommendedRates.Item2.ConvertToDecimalOrNull());

            rate = 2.0;
            kof1 = 2.33;
            kof2 = 2.34;
            recommendedRates = obj.GetRecommendedRates(rate, kof1, kof2);

            Assert.IsNotNull(recommendedRates);
            Assert.IsNotNull(recommendedRates.Item1.ConvertToDecimalOrNull());
            Assert.IsNotNull(recommendedRates.Item2.ConvertToDecimalOrNull());

            rate = 2.0;
            kof1 = 1.56;
            kof2 = 2.81;
            recommendedRates = obj.GetRecommendedRates(rate, kof1, kof2);

            Assert.IsNotNull(recommendedRates);
            Assert.IsNotNull(recommendedRates.Item1.ConvertToDecimalOrNull());
            Assert.IsNotNull(recommendedRates.Item2.ConvertToDecimalOrNull());

            rate = 2.0;
            kof1 = 1.6;
            kof2 = 2.73;
            recommendedRates = obj.GetRecommendedRates(rate, kof1, kof2);

            Assert.IsNotNull(recommendedRates);
            Assert.IsNotNull(recommendedRates.Item1.ConvertToDecimalOrNull());
            Assert.IsNotNull(recommendedRates.Item2.ConvertToDecimalOrNull());
        }
    }
}
