using LightHouse.Core.Testing;
using LightHouse.Testing.xUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Collections.Runner
{
    public class ContractListRunner
    {
        public ContractListRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetDynamicTypeOfContractList()
        {
            TestConfiguration.ContractList.SetDynamicTypeOfContractList();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetIncludeInContractList()
        {
            TestConfiguration.ContractList.SetIncludeInContractList();
        }
    }
}
