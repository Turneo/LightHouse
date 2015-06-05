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

namespace LightHouse.Core.Elite.Building.Testing
{
    public class Builder
    {
        /// <summary>
        /// Builds a person and checks that the object was built correctly.
        /// </summary>
        public void BuildsAPerson()
        {
            LightHouse.Core.Elite.Building.Builder builder = new LightHouse.Core.Elite.Building.Builder();

            Person person = builder.Get<Person>();

            Assert.True(person != null);
        }
    }
}
