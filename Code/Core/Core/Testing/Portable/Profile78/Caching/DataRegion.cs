using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using LightHouse.Ontology.Social.Demography;

namespace LightHouse.Core.Caching.Testing
{
    public class DataRegion
    {
        /// <summary>
        /// Checks to see if the region can be cleared.
        /// </summary>
        public void ClearRegion()
        {
            LightHouse.Core.Caching.DataRegion dataRegion = new LightHouse.Core.Caching.DataRegion()
            {
                Name = "Family",
            };
            String id = Guid.NewGuid().ToString();

            Person person = new Person()
            {
                ID = id,
                Reference = "1671",
                FirstName = new Localization.LocalString("Homer"),
                LastName = new Localization.LocalString("Simpson"),
            };

            dataRegion.Add(person.ID, person);

            Assert.True((dataRegion.Objects.Count == 1), "Item was not added to region");

            dataRegion.Clear();

            Assert.True((dataRegion.Objects.Count == 0), "Region was not cleared");

        }

        /// <summary>
        /// Checks if an object can be added to a region.
        /// </summary>
        public void AddObjectToRegion()
        {
            LightHouse.Core.Caching.DataRegion dataRegion = new LightHouse.Core.Caching.DataRegion()
            {
                Name = "Family",
            };

            String id = Guid.NewGuid().ToString();

            Person person = new Person()
            {
                ID = id,
                Reference = "1671",
                FirstName = new Localization.LocalString("Homer"),
                LastName = new Localization.LocalString("Simpson"),
            };

            dataRegion.Add(person.ID, person);

            Assert.True((dataRegion.Objects.Count == 1), "Item was not added to region");
        }

        /// <summary>
        /// Checks if an object can be retrieved.
        /// </summary>
        public void GetObject()
        {
            LightHouse.Core.Caching.DataRegion dataRegion = new LightHouse.Core.Caching.DataRegion()
            {
                Name = "Family",
            };

            String id = Guid.NewGuid().ToString();

            Person person = new Person()
            {
                ID = id,
                Reference = "1671",
                FirstName = new Localization.LocalString("Homer"),
                LastName = new Localization.LocalString("Simpson"),
            };

            dataRegion.Add(person.ID, person);

            Assert.True((dataRegion.Objects.Count == 1), "Item was not added to region");

            Person cachedperson = (Person)dataRegion.Get(id);

            Assert.NotNull(cachedperson);
        }

        /// <summary>
        /// Checks if multiple objects can be retrieved.
        /// </summary>
        public void GetObjects()
        {
            LightHouse.Core.Caching.DataRegion dataRegion = new LightHouse.Core.Caching.DataRegion()
            {
                Name = "Family",
            };

            String id = Guid.NewGuid().ToString();

            Person person = new Person()
            {
                ID = id,
                Reference = "1617",
                FirstName = new Localization.LocalString("Homer"),
                LastName = new Localization.LocalString("Simpson"),
            };

            String id2 = Guid.NewGuid().ToString();

            Person person2 = new Person()
            {
                ID = id2,
                Reference = "1618",
                FirstName = new Localization.LocalString("Marge"),
                LastName = new Localization.LocalString("Simpson"),
            };

            String id3 = Guid.NewGuid().ToString();

            Person person3 = new Person()
            {
                ID = id3,
                Reference = "1619",
                FirstName = new Localization.LocalString("Bart"),
                LastName = new Localization.LocalString("Simpson"),
            };

            dataRegion.Add(person.ID, person);
            dataRegion.Add(person2.ID, person2);
            dataRegion.Add(person3.ID, person3);

            Assert.True((dataRegion.Objects.Count == 3), "Items were not added to region");

            IEnumerable<KeyValuePair<String, Object>> objects = dataRegion.GetObjects();

            Assert.NotNull(objects);

            Int32 foreachCounter = 0;

            foreach (KeyValuePair<String, Object> itemKeyValue in objects)
            {
                Person loadedPerson = itemKeyValue.Value as Person;

                Assert.NotNull(loadedPerson);

                if ((loadedPerson.ID != id) && (loadedPerson.ID != id2) && (loadedPerson.ID != id3))
                {
                    Assert.False(true, "Wrong ID detected");
                }

                foreachCounter++;
            }

            Assert.True(foreachCounter == 3, "Wrong number of objects when looping with a foreach");
            Assert.True(objects.Count() == 3, "Wrong number of objects when using the Count() Method extension");
        }
    }
}
