using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using LightHouse.Testing.xUnit;
using LightHouse.Core.Collections;

namespace LightHouse.Core.Testing
{
    public class TestRunner
    {
        private String testString = "testString";
        private DateTime testDateTime = new DateTime(2015, 1, 1);
        private Decimal testDecimal = new Decimal(1.5);
        private Int32 testInteger = 1;
        private Boolean testBoolean = true;
        private String contractObjectID = "testContractObject999";
        private String subContractObjectID = "testSubContractObject999";

        public TestRunner()
        {
            TestConfiguration.SetEnvironment();
        }

        [TestCase]
        [CI]
        [Scenario("Standard")]
        public void TestAllContractObjects()
        {
            int knownTypeCount = 0;
            foreach (Type type in LightHouse.Elite.Core.Locator.GetKnownTypes())
            {
                knownTypeCount++;
                if (!type.GetTypeInfo().IsAbstract
                    && typeof(LightHouse.Core.ContractObject).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
                {
                    object buildedObject = null;
                    
                    try
                    {
                        buildedObject = LightHouse.Elite.Core.Builder.Get(type);
                    }
                    catch (Exception) 
                    { 
                    }

                    if (buildedObject != null)
                    {
                        LightHouse.Core.ContractObject contractObject = (LightHouse.Core.ContractObject)buildedObject;
                        contractObject.ID = contractObjectID;
                        Assert.NotNull(contractObject);

                        IEnumerable<PropertyInfo> properties = type.GetRuntimeProperties(); // get all object properties;

                        foreach (PropertyInfo property in properties)
                        {
                            SetProperty(contractObject, property);
                        }

                        foreach (PropertyInfo property in properties)
                        {
                            CheckProperty(contractObject, property);
                        }
                    }
                }
            }

            Assert.True(knownTypeCount > 0);
        }

        private void SetProperty(LightHouse.Core.ContractObject contractObject, PropertyInfo property)
        {
            if (property.PropertyType == typeof(String))
            {
                property.SetValue(contractObject, testString);
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                property.SetValue(contractObject, testDateTime);
            }
            else if (property.PropertyType == typeof(Decimal))
            {
                property.SetValue(contractObject, testDecimal);
            }
            else if (property.PropertyType == typeof(Int32))
            {
                property.SetValue(contractObject, testInteger);
            }
            else if (property.PropertyType == typeof(Boolean))
            {
                property.SetValue(contractObject, testBoolean);
            }
            else if (property.PropertyType == typeof(LightHouse.Core.ContractObject))
            {
                LightHouse.Core.ContractObject subContractObject = (LightHouse.Core.ContractObject)LightHouse.Elite.Core.Builder.Get(property.PropertyType);
                subContractObject.ID = subContractObjectID;
                property.SetValue(contractObject, subContractObject);
            }
            else if (property.PropertyType.GetTypeInfo().IsGenericType && (property.PropertyType.GetGenericTypeDefinition() == typeof(IContractList<>)))
            {
                Type typeArgument = property.PropertyType.GenericTypeArguments[0];
                Type generic = typeof(ContractList<>);
                Type specific = generic.MakeGenericType(typeArgument);

                IContractList contractList = (IContractList)LightHouse.Elite.Core.Builder.Get(specific);

                property.SetValue(contractObject, contractList);
            }
        }

        private void CheckProperty(LightHouse.Core.ContractObject contractObject, PropertyInfo property)
        {
            if (property.PropertyType == typeof(String))
            {
                String value = (String)property.GetValue(contractObject);
                Assert.NotNull(value);
                Assert.Equal(testString, value);
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                DateTime value = (DateTime)property.GetValue(contractObject);
                Assert.NotNull(value);
                Assert.Equal(testDateTime, value);
            }
            else if (property.PropertyType == typeof(Decimal))
            {
                Decimal value = (Decimal)property.GetValue(contractObject);
                Assert.NotNull(value);
                Assert.Equal(testDecimal, value);
            }
            else if (property.PropertyType == typeof(Int32))
            {
                Int32 value = (Int32)property.GetValue(contractObject);
                Assert.NotNull(value);
                Assert.Equal(testInteger, value);
            }
            else if (property.PropertyType == typeof(Boolean))
            {
                Boolean value = (Boolean)property.GetValue(contractObject);
                Assert.NotNull(value);
                Assert.Equal(testBoolean, value);
            }
            else if (property.PropertyType == typeof(LightHouse.Core.ContractObject))
            {
                LightHouse.Core.ContractObject value = (LightHouse.Core.ContractObject)property.GetValue(contractObject);
                Assert.NotNull(value);
                Assert.Equal(contractObjectID, value.ID);
            }
            else if (property.PropertyType.GetTypeInfo().IsGenericType && (property.PropertyType.GetGenericTypeDefinition() == typeof(IContractList<>)))
            {
                IContractList value = (IContractList)property.GetValue(contractObject);
                Assert.NotNull(value);
            }
        }
    }
}
