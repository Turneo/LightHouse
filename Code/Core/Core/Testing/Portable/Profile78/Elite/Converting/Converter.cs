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
using LightHouse.Core.Testing.Surrogates;

namespace LightHouse.Core.Elite.Converting.Testing
{
    public class Converter
    {
        /// <summary>
        /// Converts a ContractObject into a dynamic SurrogateObject.
        /// </summary>
        public void ConvertContractObjectToDynamicSurrogate()
        {
            LightHouse.Core.Elite.Converting.Converter converter = new LightHouse.Core.Elite.Converting.Converter();

            Project project = new Project()
            {
                Name = new LocalString("LightHouse"),
                Leader = new Person()
                {
                    FirstName = new LocalString("Homer"),
                    LastName = new LocalString("Simpson")
                }
            };

            ISurrogateObject surrogateObject = converter.ConvertTo<ISurrogateObject>(project, new List<String>() { "Name", "Leader.FirstName", "Leader.LastName" });

            Assert.True(surrogateObject.GetProperty<LocalString>("Name").GetContent() == "LightHouse");
            Assert.True(surrogateObject.GetProperty<LocalString>("Leader.FirstName").GetContent() == "Homer");
            Assert.True(surrogateObject.GetProperty<LocalString>("Leader.LastName").GetContent() == "Simpson");
        }

        /// <summary>
        /// Converts a ContractObject into a provided SurrogateObject.
        /// </summary>
        public void ConvertContractObjectToSurrogateObject()
        {
            LightHouse.Core.Elite.Converting.Converter converter = new LightHouse.Core.Elite.Converting.Converter();

            DateTime today = DateTime.Now;

            Project project = new Project()
            {
                Reference = "1",
                Leader = new Person()
                {
                    Birthday = today
                }
            };

            ISurrogateObject surrogateObject = converter.ConvertTo<ProjectSurrogate>(project);

            Assert.True(surrogateObject.GetProperty<String>("Reference") == "1");
            Assert.True(surrogateObject.GetProperty<DateTime>("Birthday") == today);
        }

        /// <summary>
        /// Converts a DataObject into a dynamic SurrogateObject.
        /// </summary>
        public void ConvertDataObjectToDynamicSurrogate()
        {
            LightHouse.Core.Elite.Converting.Converter converter = new LightHouse.Core.Elite.Converting.Converter();

            Project project = new Project()
            {
                Name = new LocalString("LightHouse"),
                Leader = new Person()
                {
                    FirstName = new LocalString("Homer"),
                    LastName = new LocalString("Simpson")
                }
            };

            ISurrogateObject surrogateObject = converter.ConvertTo<ISurrogateObject>(project.ConvertTo<IDataObject>(), new List<String>() { "Name", "Leader.FirstName", "Leader.LastName" });

            Assert.True(surrogateObject.GetProperty<String>("Name") == "LightHouse");
            Assert.True(surrogateObject.GetProperty<String>("Leader.FirstName") == "Homer");
            Assert.True(surrogateObject.GetProperty<String>("Leader.LastName") == "Simpson");
        }

        /// <summary>
        /// Converts a ContractObject into a provided SurrogateObject.
        /// </summary>
        public void ConvertDataObjectToSurrogateObject()
        {
            LightHouse.Core.Elite.Converting.Converter converter = new LightHouse.Core.Elite.Converting.Converter();

            DateTime today = DateTime.Now;

            Project project = new Project()
            {
                Reference = "1",
                Leader = new Person()
                {
                    Birthday = today
                }
            };

            ISurrogateObject surrogateObject = converter.ConvertTo<ProjectSurrogate>(project.ConvertTo<IDataObject>());

            Assert.True(surrogateObject.GetProperty<String>("Reference") == "1");
            Assert.True(surrogateObject.GetProperty<DateTime>("Birthday") == today);
        }

        /// <summary>
        /// Converts a ContractObject into a String. Should return a default(String).
        /// </summary>
        public void ConvertContractObjectToString()
        {
            LightHouse.Core.Elite.Converting.Converter converter = new LightHouse.Core.Elite.Converting.Converter();

            Project project = new Project()
            {
                Reference = "1"
            };

            String defaultString = converter.ConvertTo<String>(project);

            Assert.True(defaultString == default(String));
        }

        /// <summary>
        /// Converts a DataObject into a String. Should return a default(String).
        /// </summary>
        public void ConvertDataObjectToString()
        {
            LightHouse.Core.Elite.Converting.Converter converter = new LightHouse.Core.Elite.Converting.Converter();

            Project project = new Project()
            {
                Reference = "1"
            };

            String defaultString = converter.ConvertTo<String>(project.ConvertTo<IDataObject>());

            Assert.True(defaultString == default(String));
        }
    }
}
