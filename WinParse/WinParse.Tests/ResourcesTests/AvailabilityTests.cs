using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinParse.Resources;

namespace WinParse.Tests.ResourcesTests
{
    [TestClass]
    public class AvailabilityTests
    {
        [TestMethod]
        public void ResourceNameAvailabilityTest_EN()
        {
            TestAvailability("en-GB");
        }


        [TestMethod]
        public void ResourceNameAvailabilityTest_RU()
        {
            TestAvailability("ru-RU");
        }

        [TestMethod]
        public void ResourceNameAvailabilityTest_UA()
        {
            TestAvailability("uk-UA");
        }

        private void TestAvailability(string localization)
        {
            var instance = ResMan.GetResourceByName(localization);
            foreach (var property in typeof(ResKeys)
                .GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
               var label = (string)property.GetValue(null);
                Assert.IsNotNull(instance.GetString(label),label);
            }
        }

    }
}
