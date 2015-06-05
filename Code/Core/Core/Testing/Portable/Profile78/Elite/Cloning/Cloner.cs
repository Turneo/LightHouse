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

namespace LightHouse.Core.Elite.Cloning.Testing
{
    public class Cloner
    {
        /// <summary>
        /// Clones a person converted that has been coverted to a DataObject and checks that all properties have been cloned correctly.
        /// </summary>
        public void ClonePersonAsDataObject()
        {
            LightHouse.Core.Elite.Cloning.Cloner cloner = new LightHouse.Core.Elite.Cloning.Cloner();

            Person person = new Person()
            {
                FirstName = new LocalString("Homer"),
                LastName = new LocalString("Simpson")
            };

            IDataObject clonedDataObject = cloner.CloneDataObject(person.ConvertTo<IDataObject>(), null, true, null);
            Person clonedPerson = new Person(clonedDataObject);

            Assert.True(clonedPerson.FirstName.GetContent() == "Homer");
            Assert.True(clonedPerson.LastName.GetContent() == "Simpson");
        }
    }
}
