using LightHouse.Testing.xUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Testing
{
    public class ContractObjectRunner
    {
        public ContractObjectRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void Create10000Person()
        {
            TestConfiguration.ContractObject.Create10000Person();
        }
    }
}
