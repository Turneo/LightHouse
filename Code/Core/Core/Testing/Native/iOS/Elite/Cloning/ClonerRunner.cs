using LightHouse.Core.Testing;
using LightHouse.Testing.xUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Elite.Cloning.Testing
{
    public class ClonerRunner
    {
        public ClonerRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void ClonePersonAsDataObject()
        {
            TestConfiguration.Cloner.ClonePersonAsDataObject();
        }
    }
}
