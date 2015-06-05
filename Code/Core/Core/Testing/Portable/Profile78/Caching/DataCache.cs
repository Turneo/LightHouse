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
    public class DataCache
    {
        /// <summary>
        /// Checks if a person can be added to the cache and then retrieved from it.
        /// </summary>
        public void AddAndGetPerson()
        {
            LightHouse.Core.Caching.DataCache dataCache = new LightHouse.Core.Caching.DataCache();

            String id = Guid.NewGuid().ToString();

            Person person = new Person()
            {
                ID = id,
                Reference = "1671",
                FirstName = new Localization.LocalString("Homer"),
                LastName = new Localization.LocalString("Simpson"),
            };

            dataCache.Add(person.ID, person);

            person.FirstName = new Localization.LocalString("Bart");

            Person cachedPerson = dataCache.Get<Person>(id);

            Assert.Equal(cachedPerson.FirstName.GetContent(), "Bart");
        }

        /// <summary>
        /// Checks if a person with a region can be added and then retrieved from it. 
        /// </summary>
        public void AddAndGetPersonWithRegion()
        {
            LightHouse.Core.Caching.DataCache dataCache = new LightHouse.Core.Caching.DataCache();

            String id = Guid.NewGuid().ToString();

            Person person = new Person()
            {
                ID = id,
                Reference = "1671",
                FirstName = new Localization.LocalString("Homer"),
                LastName = new Localization.LocalString("Simpson"),
            };

            String region = "Family";
            String falseRegion = "Acquaintances";

            dataCache.Add(person.ID, person, region);

            person.FirstName = new Localization.LocalString("Bart");

            Person cachedPerson = dataCache.Get<Person>(id, region);
            Person falsePerson = dataCache.Get<Person>(id, falseRegion);

            Assert.NotNull(cachedPerson);
            Assert.Null(falsePerson);
            Assert.Equal(cachedPerson.FirstName.GetContent(), "Bart");
        }

        /// <summary>
        /// Checks if a region can be created and then clears anything in the region.
        /// </summary>
        public void CreateAndClearRegion()
        {
            LightHouse.Core.Caching.DataCache dataCache = new LightHouse.Core.Caching.DataCache();

            String id = Guid.NewGuid().ToString();

            Person person = new Person()
            {
                ID = id,
                Reference = "1671",
                FirstName = new Localization.LocalString("Homer"),
                LastName = new Localization.LocalString("Simpson"),
            };

            String dataRegionName = "Family";

            Assert.True(dataCache.CreateRegion(dataRegionName));
            
            dataCache.Add(person.ID, person, dataRegionName);
            
            LightHouse.Core.Caching.DataRegion dataRegion = dataCache.Regions[dataRegionName];

            Assert.True((dataRegion.Objects.Count == 1), "Item was not added to region");
            
            dataCache.ClearRegion(dataRegionName);

            Assert.True((dataRegion.Objects.Count == 0), "Region was not cleared");
        }

        /// <summary>
        /// Checks if an object can be retrieved from a default region.
        /// </summary>
        public void GetObjectFromDefaultRegion()
        {
            LightHouse.Core.Caching.DataCache dataCache = new LightHouse.Core.Caching.DataCache();

            String id = Guid.NewGuid().ToString();

            Person person = new Person()
            {
                ID = id,
                Reference = "1671",
                FirstName = new Localization.LocalString("Homer"),
                LastName = new Localization.LocalString("Simpson"),
            };
            
            dataCache.Add(person.ID, person);

            Assert.True(dataCache.Regions.ContainsKey("default"), "default region was not created");

            LightHouse.Core.Caching.DataRegion defaultDataRegion = dataCache.Regions["default"];

            Assert.True((defaultDataRegion.Objects.Count == 1), "Item was not added to region");

            Person cachedPerson = dataCache.Get<Person>(id);

            Assert.NotNull(cachedPerson);
        }

        /// <summary>
        /// Checks if an object can be retrieved from a region.
        /// </summary>
        public void GetObjectFromRegion()
        {
            LightHouse.Core.Caching.DataCache dataCache = new LightHouse.Core.Caching.DataCache();

            String id = Guid.NewGuid().ToString(); 

            Person person = new Person()
            {
                ID = id,
                Reference = "1671",
                FirstName = new Localization.LocalString("Homer"),
                LastName = new Localization.LocalString("Simpson"),
            };

            String dataRegionName = "Family";

            dataCache.Add(person.ID, person, dataRegionName);

            Assert.True(dataCache.Regions.ContainsKey(dataRegionName), "Region was not created");

            LightHouse.Core.Caching.DataRegion dataRegion = dataCache.Regions[dataRegionName];

            Assert.True((dataRegion.Objects.Count == 1), "Item was not added to region");

            Person cachedperson = dataCache.Get<Person>(id, dataRegionName);

            Assert.NotNull(cachedperson);
        }

        /// <summary>
        /// Checks if multiple objects can be retrieved from a region.
        /// </summary>
        public void GetObjectsFromRegion()
        {
            LightHouse.Core.Caching.DataCache dataCache = new LightHouse.Core.Caching.DataCache();

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

            String dataRegionName = "Family";

            dataCache.Add(person.ID, person, dataRegionName);
            dataCache.Add(person2.ID, person2, dataRegionName);
            dataCache.Add(person3.ID, person3, dataRegionName);

            Assert.True(dataCache.Regions.ContainsKey(dataRegionName), "Region was not created");

            LightHouse.Core.Caching.DataRegion dataRegion = dataCache.Regions[dataRegionName];

            Assert.True((dataRegion.Objects.Count == 3), "Items were not added to region");

            IEnumerable<KeyValuePair<String, Object>> objects = dataCache.GetObjectsInRegion(dataRegionName);

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

        /// <summary>
        /// Checks that when a region is created twice, the CreateRegion method returns false.
        /// </summary>
        public void CreateDuplicateRegion()
        {
            LightHouse.Core.Caching.DataCache dataCache = new LightHouse.Core.Caching.DataCache();

            String dataRegionName = "Family";

            Assert.True(dataCache.CreateRegion(dataRegionName));
            Assert.False(dataCache.CreateRegion(dataRegionName));
        }

        /// <summary>
        /// Checks that an object is removed from a specific region.
        /// </summary>
        public void RemoveObjectFromSpecificRegion()
        {
            LightHouse.Core.Caching.DataCache dataCache = new LightHouse.Core.Caching.DataCache();

            String id = Guid.NewGuid().ToString();

            Person person = new Person()
            {
                ID = id,
                Reference = "1671",
                FirstName = new Localization.LocalString("Homer"),
                LastName = new Localization.LocalString("Simpson"),
            };

            String dataRegionName = "Family";

            Assert.True(dataCache.CreateRegion(dataRegionName));

            dataCache.Add(person.ID, person, dataRegionName);

            Person cachedPerson = dataCache.Get<Person>(person.ID, dataRegionName);

            Assert.NotNull(cachedPerson);

            dataCache.Remove(person.ID, dataRegionName);

            Person removedPerson = dataCache.Get<Person>(person.ID, dataRegionName);

            Assert.Null(removedPerson);           
        }

        /// <summary>
        /// Checks that an object is removed from a default region.
        /// </summary>
        public void RemoveObjectFromDefaultRegion()
        {
            LightHouse.Core.Caching.DataCache dataCache = new LightHouse.Core.Caching.DataCache();

            String id = Guid.NewGuid().ToString();

            Person person = new Person()
            {
                ID = id,
                Reference = "1671",
                FirstName = new Localization.LocalString("Homer"),
                LastName = new Localization.LocalString("Simpson"),
            };

            dataCache.Add(person.ID, person);

            Assert.True(dataCache.Regions.ContainsKey("default"), "default region was not created");

            LightHouse.Core.Caching.DataRegion defaultDataRegion = dataCache.Regions["default"];

            Person cachedPerson = dataCache.Get<Person>(person.ID);

            Assert.NotNull(cachedPerson);

            dataCache.Remove(person.ID);

            Person removedPerson = dataCache.Get<Person>(person.ID);

            Assert.Null(removedPerson);
        }

        /// <summary>
        ///  REVIEW: Checks that an object from a region is removed. If the region does not exist, it is created.
        /// </summary>
        public void CreateRegionIfDoesNotExist()
        {
            LightHouse.Core.Caching.DataCache dataCache = new LightHouse.Core.Caching.DataCache();
             
            dataCache.Remove("key","region");

            Assert.True(dataCache.ContainsRegion("region"));

        }

        /// <summary>
        /// Checks that a region exists in the DataCache.
        /// </summary>
        public void CheckRegion()
        {
            LightHouse.Core.Caching.DataCache dataCache = new LightHouse.Core.Caching.DataCache();

            String id = Guid.NewGuid().ToString();

            Person person = new Person()
            {
                ID = id,
                Reference = "1671",
                FirstName = new Localization.LocalString("Homer"),
                LastName = new Localization.LocalString("Simpson"),
            };

            String dataRegionName = "Family";

            dataCache.Add(person.ID, person, dataRegionName);

            Assert.True(dataCache.ContainsRegion(dataRegionName));

        }
    }
}
