using LightHouse.Ontology.Social.Demography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LightHouse.Core.Testing
{
    public class ContractObject
    {
        /// <summary>
        /// The time for creating 10000 persons shouldn't be more than 1.5 second.
        /// </summary>
        public void Create10000Person()
        {
            DateTime startTime = DateTime.Now;

            IList<Person> people = new List<Person>();

            for (int i = 0; i < 10000; i++)
            {
                Person person = new Person()
                {
                    Reference = i.ToString(),
                    FirstName = new Localization.LocalString(String.Format("Bart{0}", i)),
                    LastName = new Localization.LocalString(String.Format("Simpson{0}", i)),
                    Birthday = DateTime.Now.AddDays(-1 * i)
                };
                
                people.Add(person);
            }

            TimeSpan timeDifference = DateTime.Now.Subtract(startTime);

            Assert.True(timeDifference.TotalSeconds < 1.5);
            Assert.Equal(people.Count, 10000);
        }
    }
}
