using LightHouse.Testing.xUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Testing
{
    public class DataEnumeratorRunner
    {
        public DataEnumeratorRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void ExecuteMoveNext()
        {
            TestConfiguration.DataEnumerator.ExecuteMoveNext();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void ExecuteReset()
        {
            TestConfiguration.DataEnumerator.ExecuteReset();
        }
    }
}
