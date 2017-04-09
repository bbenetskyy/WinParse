using FormulasCollection.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTest.FormulasCollectionTests.HelpersTests
{
    [TestClass]
    public class SportConverterTypesTests
    {
        [TestMethod]
        public void MinimalizeValueTest_TestWithZero()
        {
            var inValue = "F1(+1.0)";
            var outValue = "F1(+1)";
            Assert.AreEqual(inValue.MinimalizeValue(), outValue);

            inValue = "F1(-1.50)";
            outValue = "F1(-1.5)";
            Assert.AreEqual(inValue.MinimalizeValue(), outValue);

            inValue = "F1(0)";
            outValue = "F1(0)";
            Assert.AreEqual(inValue.MinimalizeValue(), outValue);

            inValue = "F2(0.25)";
            outValue = "F2(0.25)";
            Assert.AreEqual(inValue.MinimalizeValue(), outValue);

            inValue = "F2(-3.0)";
            outValue = "F2(-3)";
            Assert.AreEqual(inValue.MinimalizeValue(), outValue);
        }

        [TestMethod]
        public void MinimalizeValueTest_TestWithoutZero()
        {
            var inValue = "F1(+3.5)";
            var outValue = "F1(+3.5)";
            Assert.AreEqual(inValue.MinimalizeValue(), outValue);

            inValue = "F1(+3.75)";
            outValue = "F1(+3.75)";
            Assert.AreEqual(inValue.MinimalizeValue(), outValue);

            inValue = "F2(-1.75)";
            outValue = "F2(-1.75)";
            Assert.AreEqual(inValue.MinimalizeValue(), outValue);

            inValue = string.Empty;
            outValue = string.Empty;
            Assert.AreEqual(inValue.MinimalizeValue(), outValue);

            inValue = null;
            outValue = null;
            Assert.AreEqual(inValue.MinimalizeValue(), outValue);
        }

        [TestMethod]
        public void InvertValue_TestWithNumbers()
        {
            var inValue = "-2.0";
            var outValue = "2.0";
            Assert.AreEqual(inValue.InvertValue(), outValue);

            inValue = "2.0";
            outValue = "-2.0";
            Assert.AreEqual(inValue.InvertValue(), outValue);

            inValue = "0";
            outValue = "0";
            Assert.AreEqual(inValue.InvertValue(), outValue);

            inValue = "0.0";
            outValue = "0.0";
            Assert.AreEqual(inValue.InvertValue(), outValue);
        }

        [TestMethod]
        public void LocalizeToMarathon_TestWithNumbers()
        {
            var inValue = "-2";
            var outValue = "-2";
            Assert.AreEqual(inValue.LocalizeToMarathon(), outValue);

            inValue = "2";
            outValue = "+2";
            Assert.AreEqual(inValue.LocalizeToMarathon(), outValue);
        }

        [TestMethod]
        public void ExtendType_TestWithSimple()
        {
            var extention = "H";
            var period = 1;
            var inValue = "1";
            var outValue = "1H1";
            Assert.AreEqual(inValue.ExtendType(extention, period), outValue);

            extention = "P";
            period = 3;
            inValue = "2";
            outValue = "2P3";
            Assert.AreEqual(inValue.ExtendType(extention, period), outValue);

            extention = "HQE";
            period = 2;
            inValue = "X";
            outValue = "XHQE2";
            Assert.AreEqual(inValue.ExtendType(extention, period), outValue);

            extention = null;
            period = 3;
            inValue = "2";
            outValue = "2";
            Assert.AreEqual(inValue.ExtendType(extention, period), outValue);

            extention = "H";
            period = 0;
            inValue = "2";
            outValue = "2";
            Assert.AreEqual(inValue.ExtendType(extention, period), outValue);
        }

        [TestMethod]
        public void ExtendType_TestWithCombined()
        {
            var extention = "H";
            var period = 1;
            var inValue = "F2(+4)";
            var outValue = "F2H1(+4)";
            Assert.AreEqual(inValue.ExtendType(extention, period), outValue);

            extention = "P";
            period = 3;
            inValue = "TO(+7.5)";
            outValue = "TOP3(+7.5)";
            Assert.AreEqual(inValue.ExtendType(extention, period), outValue);

            extention = "HQE";
            period = 2;
            inValue = "F2(+1)";
            outValue = "F2HQE2(+1)";
            Assert.AreEqual(inValue.ExtendType(extention, period), outValue);

            extention = null;
            period = 3;
            inValue = "TU(-5.25)";
            outValue = "TU(-5.25)";
            Assert.AreEqual(inValue.ExtendType(extention, period), outValue);

            extention = "HQE";
            period = -3;
            inValue = "TU(-5.25)";
            outValue = "TU(-5.25)";
            Assert.AreEqual(inValue.ExtendType(extention, period), outValue);
        }
    }
}