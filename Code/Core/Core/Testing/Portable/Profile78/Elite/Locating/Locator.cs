using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using LightHouse.Localization;
using LightHouse.Ontology.Social.Organization.Projects;
using LightHouse.Ontology.Social.Demography;
using LightHouse.Ontology.Social.Business.Accounting.Documents;
using LightHouse.Ontology.Social.Geography;
using LightHouse.Ontology.Social.Business.Accounting.Documents.Sales;
using LightHouse.Ontology.Social.Economics;

namespace LightHouse.Core.Elite.Locating.Testing
{
    public class Locator
    {
        /// <summary>
        /// Checks that the GetType method of locator provides the correct type.
        /// </summary>
        public new void GetType()
        {
            LightHouse.Core.Elite.Locating.Locator locator = new LightHouse.Core.Elite.Locating.Locator();

            Type projectType = locator.GetType(typeof(Project).FullName);

            Assert.True(projectType.FullName == typeof(Project).FullName);
        }

        /// <summary>
        /// Checks that the correct base types of dynamic objects are returned.
        /// </summary>
        public void GetBaseTypesOfDynamicObject()
        {
            LightHouse.Core.Elite.Locating.Locator locator = new LightHouse.Core.Elite.Locating.Locator();

            Project project = new Project();
            IDataObject dataObject = project.ConvertTo<IDataObject>();

            IList<String> baseTypes = locator.GetBaseTypes(dataObject);

            Assert.True(baseTypes.Count == 2);
            Assert.True(baseTypes[0] == typeof(LightHouse.Storage.EntityObject).FullName);
            Assert.True(baseTypes[1] == typeof(LightHouse.Storage.ElementObject).FullName);
        }

        /// <summary>
        /// Checks that the correct base types of non dynamic objects are returned.
        /// </summary>
        public void GetBaseTypesOfNonDynamicObject()
        {
            LightHouse.Core.Elite.Locating.Locator locator = new LightHouse.Core.Elite.Locating.Locator();

            LightHouse.Storage.EntityObject entityObject = new LightHouse.Storage.EntityObject();

            IList<String> baseTypes = locator.GetBaseTypes(entityObject);

            Assert.True(baseTypes.Count == 1);
            Assert.True(baseTypes[0] == typeof(LightHouse.Storage.ElementObject).FullName);
        }

        /// <summary>
        /// Compares the performance of GetType of the locator with the GetType from the .NET Framework reflection.
        /// </summary>
        public void GetTypePerformance()
        {
            LightHouse.Core.Elite.Locating.Locator locator = new LightHouse.Core.Elite.Locating.Locator();
           
            Type projectType = default(Type);
            Type personType = default(Type);
            Type documentType = default(Type);
            Type countryType = default(Type);
            Type invoiceType = default(Type);
            Type moneyType = default(Type);
            Type entityObjectType = default(Type);

            Stopwatch reflectionStopwatch = new Stopwatch();
            reflectionStopwatch.Start();
            projectType = Type.GetType(typeof(Project).AssemblyQualifiedName);
            personType = Type.GetType(typeof(Person).AssemblyQualifiedName);
            documentType = Type.GetType(typeof(Document).AssemblyQualifiedName);
            countryType = Type.GetType(typeof(Country).AssemblyQualifiedName);
            invoiceType = Type.GetType(typeof(Invoice).AssemblyQualifiedName);
            moneyType = Type.GetType(typeof(Money).AssemblyQualifiedName);

            for (int i = 0; i < 100000; i++)
            {
                entityObjectType = Type.GetType(typeof(EntityObject).AssemblyQualifiedName);
            }

            reflectionStopwatch.Stop();

            Assert.True(projectType != null);
            Assert.True(personType != null);
            Assert.True(documentType != null);
            Assert.True(countryType != null);
            Assert.True(invoiceType != null);
            Assert.True(moneyType != null);
            Assert.True(entityObjectType != null); 
            
            Stopwatch locatorStopwatch = new Stopwatch();
            locatorStopwatch.Start();
            
            locator.Load();

            projectType = locator.GetType(typeof(Project).FullName);
            personType = locator.GetType(typeof(Person).FullName);
            documentType = locator.GetType(typeof(Document).FullName);
            countryType = locator.GetType(typeof(Country).FullName);
            invoiceType = locator.GetType(typeof(Invoice).FullName);
            moneyType = locator.GetType(typeof(Money).FullName);

            for (int i = 0; i < 100000; i++)
            {
                entityObjectType = locator.GetType(typeof(EntityObject).FullName);               
            }

            locatorStopwatch.Stop();

            Assert.True(projectType != null);
            Assert.True(personType != null);
            Assert.True(documentType != null);
            Assert.True(countryType != null);
            Assert.True(invoiceType != null);
            Assert.True(moneyType != null);
            Assert.True(entityObjectType != null);

            projectType = default(Type);
            personType = default(Type);
            documentType = default(Type);
            countryType = default(Type);
            invoiceType = default(Type);
            moneyType = default(Type);
            entityObjectType = default(Type);

            Assert.True(locatorStopwatch.Elapsed < reflectionStopwatch.Elapsed, "If the .NET Framework reflection is faster then the cached version of the locator, the implementation of GetType should be revised.");
        }
    }
}
