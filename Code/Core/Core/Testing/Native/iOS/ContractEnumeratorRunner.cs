using LightHouse.Testing.xUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Testing
{
    public class ContractEnumeratorRunner
    {
        public ContractEnumeratorRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void ExecuteMoveNext()
        {
            TestConfiguration.ContractEnumerator.ExecuteMoveNext();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void ExecuteReset()
        {
            TestConfiguration.ContractEnumerator.ExecuteReset();
        }
    }
}
