using LightHouse.Core.Testing;
using LightHouse.Testing.xUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Elite.Locating.Testing
{
    public class LocatorRunner
    {
        public LocatorRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public new void GetType()
        {
            TestConfiguration.Locator.GetType();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetTypePerformance()
        {
            TestConfiguration.Locator.GetTypePerformance();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetBaseTypesOfDynamicObject()
        {
            TestConfiguration.Locator.GetBaseTypesOfDynamicObject();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetBaseTypesOfNonDynamicObject()
        {
            TestConfiguration.Locator.GetBaseTypesOfNonDynamicObject();
        }
    }
}
