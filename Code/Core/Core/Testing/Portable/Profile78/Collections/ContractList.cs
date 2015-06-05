using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Ontology.Social.Demography;

using Xunit;
using LightHouse.Core.Collections;

namespace LightHouse.Core.Collections.Testing
{
    public class ContractList
    {
        /// <summary>
        /// Checks if a dynamic type can be set in a new ContractList and if the returned ContractList is the same (so it can be used in subsequent fluent calls).
        /// </summary>
        public void SetDynamicTypeOfContractList()
        {
            String personData = String.Format("{0}Data", typeof(Person).FullName);

            IQueryableList<Person> queryableList = new ContractList<Person>();
            IQueryableList<Person> fluentList = queryableList.OfDynamicType(personData);

            Assert.True(ReferenceEquals(queryableList, fluentList), "The returned object isn't the same after assigning the dynamic type.");
            Assert.True(fluentList.GetType().GetTypeInfo().BaseType.GetTypeInfo().GetDeclaredField("dynamicType").GetValue(fluentList).ToString() == personData, "The returned object doesn't contain the correct dynamic type.");
        }

        /// <summary>
        /// Checks if the paths can be included in a new ContractList and if the returned ContractList is the same (so it can be used in subsequent fluent calls).
        /// </summary>
        public void SetIncludeInContractList()
        {
            QueryableList<Person> queryableList = new ContractList<Person>();
            IQueryableList<Person> fluentList = queryableList.Include("LastName");
            IQueryableList<Person> secondFluentList = fluentList.Include("FirstName").Include("Birthday");
            IQueryableList<Person> thirdFluentList = fluentList.Include(new List<String>() { "Salutation", "Gender" });

            Assert.True(ReferenceEquals(queryableList, fluentList), "The returned object isn't the same after assigning the dynamic type.");
            Assert.True(ReferenceEquals(queryableList, secondFluentList), "The returned object isn't the same after assigning the dynamic type.");

            ICollection<String> paths = (ICollection<String>)secondFluentList.GetType().GetTypeInfo().BaseType.GetTypeInfo().GetDeclaredField("paths").GetValue(secondFluentList);
            Assert.True(paths.Contains("LastName"), "The returned object doesn't include the added path.");
            Assert.True(paths.Contains("Birthday"), "The returned object doesn't include the added path.");
            Assert.True(paths.Contains("Salutation"), "The returned object doesn't include the added path.");
            Assert.True(paths.Contains("Gender"), "The returned object doesn't include the added path.");
        }
    }
}
