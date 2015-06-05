using LightHouse.Testing.xUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Testing
{
    public class DataObjectRunner
    {
        public DataObjectRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void Create10000DynamicPerson()
        {
            TestConfiguration.DataObject.Create10000DynamicPerson();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetPropertyStandardNetTypes()
        {
            TestConfiguration.DataObject.SetPropertyStandardNetTypes();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetPropertyOfDataObject()
        {
            TestConfiguration.DataObject.SetPropertyOfDataObject();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetPropertyOfLocalString()
        {
            TestConfiguration.DataObject.SetPropertyOfLocalString();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetPropertyOfDataList()
        {
            TestConfiguration.DataObject.SetPropertyOfDataList();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetPropertyMethodStandardNetTypes()
        {
            TestConfiguration.DataObject.SetPropertyMethodStandardNetTypes();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetPropertyMethodOfLocalString()
        {
            TestConfiguration.DataObject.SetPropertyMethodOfLocalString();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetPropertyMethodOfDataObject()
        {
            TestConfiguration.DataObject.SetPropertyMethodOfDataObject();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetPropertyMethodOfDataList()
        {
            TestConfiguration.DataObject.SetPropertyMethodOfDataList();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetPropertyOfLocalString()
        {
            TestConfiguration.DataObject.GetPropertyOfLocalString();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetPropertyStandardNetTypes()
        {
            TestConfiguration.DataObject.GetPropertyStandardNetTypes();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetPropertyOfDataObject()
        {
            TestConfiguration.DataObject.GetPropertyOfDataObject();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetPropertyOfDataList()
        {
            TestConfiguration.DataObject.GetPropertyOfDataList();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetPropertyMethodOfLocalString()
        {
            TestConfiguration.DataObject.GetPropertyMethodOfLocalString();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetPropertyMethodStandardNetTypes()
        {
            TestConfiguration.DataObject.GetPropertyMethodStandardNetTypes();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetPropertyMethodOfDataObject()
        {
            TestConfiguration.DataObject.GetPropertyMethodOfDataObject();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetPropertyMethodOfDataList()
        {
            TestConfiguration.DataObject.GetPropertyMethodOfDataList();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetContractPropertyMethodStandardNetTypes()
        {
            TestConfiguration.DataObject.SetContractPropertyMethodStandardNetTypes();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetContractPropertyMethodOfLocalString()
        {
            TestConfiguration.DataObject.SetContractPropertyMethodOfLocalString();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetContractPropertyMethodOfDataObject()
        {
            TestConfiguration.DataObject.SetContractPropertyMethodOfDataObject();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void SetContractPropertyMethodOfDataList()
        {
            TestConfiguration.DataObject.SetContractPropertyMethodOfDataList();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetContractPropertyMethodOfLocalString()
        {
            TestConfiguration.DataObject.GetContractPropertyMethodOfLocalString();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetContractPropertyMethodStandardNetTypes()
        {
            TestConfiguration.DataObject.GetContractPropertyMethodStandardNetTypes();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetContractPropertyMethodOfDataObject()
        {
            TestConfiguration.DataObject.GetContractPropertyMethodOfDataObject();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void GetContractPropertyMethodOfDataList()
        {
            TestConfiguration.DataObject.GetContractPropertyMethodOfDataList();
        }    
    }
}
