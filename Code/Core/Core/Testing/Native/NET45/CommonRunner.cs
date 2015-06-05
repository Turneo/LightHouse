using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using LightHouse.Testing.xUnit;
using LightHouse.Core.Collections;

namespace LightHouse.Core.Testing
{
    public class TestRunner
    {
        public TestRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void CreatePropertyChangingEventArgs()
        {
            TestConfiguration.Common.CreatePropertyChangingEventArgs();
        }        

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void TestAllContractObjects()
        {
            LightHouse.Base.Testing.Operator.RunCommonTests();
        }        
    }
}
