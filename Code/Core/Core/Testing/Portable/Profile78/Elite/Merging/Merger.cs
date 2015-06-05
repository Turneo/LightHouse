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

namespace LightHouse.Core.Elite.Merging.Testing
{
    public class Merger
    {
        /// <summary>
        /// Merges two person, by their LastName. The FirstName should be kept untouched.
        /// </summary>
        public void MergePersonAsDataObject()
        {
            LightHouse.Core.Elite.Merging.Merger merger = new LightHouse.Core.Elite.Merging.Merger();

            Person oldPerson = new Person()
            {
                FirstName = new LocalString("Homer"),
                LastName = new LocalString("Simpson")
            };

            Person newPerson = new Person()
            {
                FirstName = new LocalString("Bart"),
                LastName = new LocalString("Miller")
            };

            merger.MergeDataObject(oldPerson.ConvertTo<IDataObject>(), newPerson.ConvertTo<IDataObject>(), new List<String>() { "FirstName" }, true);

            Assert.True(oldPerson.FirstName.GetContent() == "Bart");
            Assert.True(oldPerson.LastName.GetContent() == "Simpson");
        }
    }
}
