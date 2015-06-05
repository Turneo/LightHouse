using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using LightHouse.Ontology.Social.Organization.Projects;
using System.Diagnostics;
using LightHouse.Localization;

namespace LightHouse.Core.Elite.Reflecting.Testing
{
    public class Reflector
    {
        /// <summary>
        /// Checks that the properties returned by reflector contain all the runtime properties of the type.
        /// </summary>
        public void GetProperties()
        {
            LightHouse.Core.Elite.Reflecting.Reflector reflector = new LightHouse.Core.Elite.Reflecting.Reflector();
            
            IEnumerable<PropertyInfo> properties = reflector.GetProperties(typeof(Project));

            foreach(PropertyInfo propertyInfo in typeof(Project).GetRuntimeProperties())
            {
                Assert.True(properties.Where(x => x.Name == propertyInfo.Name).Count() == 1);
            }
        }

        /// <summary>
        /// Checks that the fields returned by reflector contain all the runtime fields of the type.
        /// </summary>
        public void GetFields()
        {
            LightHouse.Core.Elite.Reflecting.Reflector reflector = new LightHouse.Core.Elite.Reflecting.Reflector();

            IEnumerable<FieldInfo> fields = reflector.GetFields(typeof(Project));

            foreach (FieldInfo fieldInfo in typeof(Project).GetRuntimeFields())
            {
                Assert.True(fields.Where(x => x.Name == fieldInfo.Name).Count() == 1);
            }
        }

        /// <summary>
        /// Checks that the property returned by reflector matches the property returned by standard reflection.
        /// </summary>
        public void GetProperty()
        {
            LightHouse.Core.Elite.Reflecting.Reflector reflector = new LightHouse.Core.Elite.Reflecting.Reflector();

            PropertyInfo propertyInfo = reflector.GetProperty(typeof(Project), "Name");
            PropertyInfo runtimePropertyInfo = typeof(Project).GetRuntimeProperty("Name");

            Assert.True(propertyInfo.Name == runtimePropertyInfo.Name);
        }

        /// <summary>
        /// Checks that the property can be retrieved correctly by it's name using the reflector.
        /// </summary>
        public void GetPropertyValueByString()
        {
            LightHouse.Core.Elite.Reflecting.Reflector reflector = new LightHouse.Core.Elite.Reflecting.Reflector();

            Project project = new Project()
            {
                Name = new Localization.LocalString("LightHouse")
            };

            LocalString name = (LocalString)reflector.GetPropertyValue("Name", project);

            Assert.True(name.GetContent() == "LightHouse");
        }

        /// <summary>
        /// Checks that the property can be setted correctly by it's PropertyInfo using the reflector.
        /// </summary>
        public void SetPropertyValueByPropertyInfo()
        {
            LightHouse.Core.Elite.Reflecting.Reflector reflector = new LightHouse.Core.Elite.Reflecting.Reflector();

            Project project = new Project();

            PropertyInfo propertyInfo = reflector.GetProperty(typeof(Project), "Name");
            reflector.SetPropertyValue(propertyInfo, project, new LocalString("LightHouse"));

            Assert.True(project.Name.GetContent() == "LightHouse");

            propertyInfo = null;
            reflector.SetPropertyValue(propertyInfo, project, new LocalString("LightHouse"));
        }

        /// <summary>
        /// Checks that the property can be setted correctly by it's name using the reflector.
        /// </summary>
        public void SetPropertyValueByString()
        {
            LightHouse.Core.Elite.Reflecting.Reflector reflector = new LightHouse.Core.Elite.Reflecting.Reflector();

            Project project = new Project();

            reflector.SetPropertyValue("Name", project, new Localization.LocalString("LightHouse"));

            Assert.True(project.Name.GetContent() == "LightHouse");
        }

        /// <summary>
        /// Checks that the property can be retrieved correctly by it's PropertyInfo using the reflector.
        /// </summary>
        public void GetPropertyValueByPropertyInfo()
        {
            LightHouse.Core.Elite.Reflecting.Reflector reflector = new LightHouse.Core.Elite.Reflecting.Reflector();

            Project project = new Project()
            {
                Name = new Localization.LocalString("LightHouse")
            };

            PropertyInfo propertyInfo = reflector.GetProperty(typeof(Project), "Name");
            LocalString name = (LocalString)reflector.GetPropertyValue(propertyInfo, project);

            Assert.True(name.GetContent() == "LightHouse");

            propertyInfo = null;
            Object empty = reflector.GetPropertyValue(propertyInfo, project);

            Assert.True(empty == null);
        }

    }
}
