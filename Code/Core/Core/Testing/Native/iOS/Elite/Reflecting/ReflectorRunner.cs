using LightHouse.Core.Testing;
using LightHouse.Testing.xUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Elite.Reflecting.Testing
{
    public class ReflectorRunner
    {
        public ReflectorRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetProperties()
        {
            TestConfiguration.Reflector.GetProperties();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetFields()
        {
            TestConfiguration.Reflector.GetFields();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetProperty()
        {
            TestConfiguration.Reflector.GetProperty();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetPropertyValueByString()
        {
            TestConfiguration.Reflector.GetPropertyValueByString();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetPropertyValueByPropertyInfo()
        {
            TestConfiguration.Reflector.GetPropertyValueByPropertyInfo();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetPropertyValueByString()
        {
            TestConfiguration.Reflector.SetPropertyValueByString();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetPropertyValueByPropertyInfo()
        {
            TestConfiguration.Reflector.SetPropertyValueByPropertyInfo();
        }
    }
}
