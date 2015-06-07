using LightHouse.Core.Testing;
using LightHouse.Testing.xUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Elite.Merging.Testing
{
    public class MergerRunner
    {
        public MergerRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public new void MergePersonAsDataObject()
        {
            TestConfiguration.Merger.MergePersonAsDataObject();
        }
    }
}
