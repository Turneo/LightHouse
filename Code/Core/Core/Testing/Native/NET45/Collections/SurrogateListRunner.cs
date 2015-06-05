using LightHouse.Core.Testing;
using LightHouse.Testing.xUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Collections.Runner
{
    public class SurrogateListRunner
    {
        public SurrogateListRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetDynamicTypeOfSurrogateList()
        {
            TestConfiguration.SurrogateList.SetDynamicTypeOfSurrogateList();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetIncludeInSurrogateList()
        {
            TestConfiguration.SurrogateList.SetIncludeInSurrogateList();
        }
    }
}
