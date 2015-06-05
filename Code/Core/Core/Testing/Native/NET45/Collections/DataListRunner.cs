using LightHouse.Core.Testing;
using LightHouse.Testing.xUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Collections.Runner
{
    public class DataListRunner
    {
        public DataListRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetDynamicTypeOfDataList()
        {
            TestConfiguration.DataList.SetDynamicTypeOfDataList();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetIncludeInDataList()
        {
            TestConfiguration.DataList.SetIncludeInDataList();
        }
    }
}
