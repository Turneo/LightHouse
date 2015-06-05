using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core.Testing;
using LightHouse.Testing.xUnit;

namespace LightHouse.Core.Attributes.Testing
{
    public class PathRunner
    {
        public PathRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void CheckPathAttributeOnSurrogateObject()
        {
            TestConfiguration.Path.CheckPathAttributeOnSurrogateObject();
        }
    }
}
