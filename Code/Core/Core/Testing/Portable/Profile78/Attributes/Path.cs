using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using LightHouse.Core.Testing.Surrogates;
using LightHouse.Ontology.Social.Demography;

namespace LightHouse.Core.Attributes.Testing
{
    public class Path
    {
        /// <summary>
        /// Creates a project surrogate and tests if the corresponding attribute can be read.
        /// </summary>
        public void CheckPathAttributeOnSurrogateObject()
        {
            IDictionary<String, String> propertyPaths = new Dictionary<String, String>();

            foreach (PropertyInfo propertyInfo in LightHouse.Elite.Core.Reflector.GetProperties(typeof(ProjectSurrogate)))
            {
                foreach (Attribute attribute in propertyInfo.GetCustomAttributes(true))
                {
                    if (attribute is LightHouse.Core.Attributes.Path)
                    {
                        propertyPaths[propertyInfo.Name] = ((LightHouse.Core.Attributes.Path)attribute).Value;
                    }
                }
            }

            Assert.True(propertyPaths.Count == 5);
            Assert.True(propertyPaths["Name"] == "Name");
            Assert.True(propertyPaths["LeaderFirstName"] == "Leader.FirstName");
            Assert.True(propertyPaths["LeaderLastName"] == "Leader.LastName");
            Assert.True(propertyPaths["Birthday"] == "Leader.Birthday");
            Assert.True(propertyPaths["Reference"] == "Reference");
        }
    }
}
