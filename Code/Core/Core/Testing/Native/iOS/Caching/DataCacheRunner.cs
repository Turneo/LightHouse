using LightHouse.Core.Testing;
using LightHouse.Testing.xUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Caching.Testing
{
    public class DataCacheRunner
    {
        public DataCacheRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void AddAndGetPerson()
        {
            TestConfiguration.DataCache.AddAndGetPerson();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void AddAndGetPersonWithRegion()
        {
            TestConfiguration.DataCache.AddAndGetPersonWithRegion();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void CreateAndClearRegion()
        {
            TestConfiguration.DataCache.CreateAndClearRegion();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetObjectFromDefaultRegion()
        {
            TestConfiguration.DataCache.GetObjectFromDefaultRegion();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetObjectFromRegion()
        {
            TestConfiguration.DataCache.GetObjectFromRegion();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetObjectsFromRegion()
        {
            TestConfiguration.DataCache.GetObjectsFromRegion();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void CreateDuplicateRegion()
        {
            TestConfiguration.DataCache.CreateDuplicateRegion();
        }
    }
}
