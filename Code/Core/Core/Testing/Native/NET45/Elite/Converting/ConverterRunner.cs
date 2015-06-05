using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core.Testing;
using LightHouse.Testing.xUnit;

namespace LightHouse.Core.Elite.Converting.Testing
{
    public class ConverterRunner
    {
        public ConverterRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void ConvertContractObjectToDynamicSurrogate()
        {
            TestConfiguration.Converter.ConvertContractObjectToDynamicSurrogate();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void ConvertContractObjectToSurrogateObject()
        {
            TestConfiguration.Converter.ConvertContractObjectToSurrogateObject();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void ConvertContractObjectToString()
        {
            TestConfiguration.Converter.ConvertContractObjectToString();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void ConvertDataObjectToDynamicSurrogate()
        {
            TestConfiguration.Converter.ConvertDataObjectToDynamicSurrogate();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void ConvertDataObjectToSurrogateObject()
        {
            TestConfiguration.Converter.ConvertDataObjectToSurrogateObject();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void ConvertDataObjectToString()
        {
            TestConfiguration.Converter.ConvertDataObjectToString();
        }
    }
}
