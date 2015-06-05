using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using LightHouse.Ontology.Social.Demography;

namespace LightHouse.Core.Testing
{
    public class ContractEnumerator
    {
        public void ExecuteMoveNext()
        {
            LightHouse.Core.Collections.ContractList<Person> contractList = new LightHouse.Core.Collections.ContractList<Person>()
            {
                new Person()
                {
                    Reference = "Person1"
                },
                new Person()
                {
                    Reference = "Person2"
                }                   
            };

            IEnumerator contractEnumerator = ((IEnumerable)contractList).GetEnumerator();

            Assert.Equal(contractEnumerator.MoveNext(), true);
            Assert.Equal(contractEnumerator.Current.ToString(), contractList[0].ToString());
        }

        public void ExecuteReset()
        {
            LightHouse.Core.Collections.ContractList<Person> contractList = new LightHouse.Core.Collections.ContractList<Person>()
            {
                new Person()
                {
                    Reference = "Person1"
                },
                new Person()
                {
                    Reference = "Person2"
                }                   
            };

            IEnumerator contractEnumerator = ((IEnumerable)contractList).GetEnumerator();

            contractEnumerator.MoveNext();
            contractEnumerator.Reset();

            Assert.Equal(contractEnumerator.Current.ToString(), contractList[0].ToString());
        }

    }
}
