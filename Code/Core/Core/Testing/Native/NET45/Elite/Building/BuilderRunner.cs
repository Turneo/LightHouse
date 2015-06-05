using LightHouse.Core.Testing;
using LightHouse.Testing.xUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Elite.Building.Testing
{
    public class BuilderRunner
    {
        public BuilderRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void BuildsAPerson()
        {
            TestConfiguration.Builder.BuildsAPerson();
        }
    }
}
