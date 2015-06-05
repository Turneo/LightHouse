using LightHouse.Core.Testing;
using LightHouse.Testing.xUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Caching.Testing
{
    public class DataRegionRunner
    {
        public DataRegionRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void ClearRegion()
        {
            TestConfiguration.DataRegion.ClearRegion();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void AddObjectToRegion()
        {
            TestConfiguration.DataRegion.AddObjectToRegion();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetObject()
        {
            TestConfiguration.DataRegion.GetObject();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetObjects()
        {
            TestConfiguration.DataRegion.GetObjects();
        }
    }
}
