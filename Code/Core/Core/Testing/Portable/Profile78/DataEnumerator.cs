using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using LightHouse.Ontology.Social.Demography;

namespace LightHouse.Core.Testing
{
    public class DataEnumerator
    {
        public void ExecuteMoveNext() 
        {
            LightHouse.Core.Collections.IDataList<IDataObject> dataList = new LightHouse.Core.Collections.DataList<IDataObject>()
            {
                new Person()
                {
                    Reference = "Person1"
                }
                .ConvertTo<IDataObject>(),
                new Person()
                {
                    Reference = "Person2"
                }
                .ConvertTo<IDataObject>()
            };

            LightHouse.Core.Collections.DataEnumerator<IDataObject> dataEnumerator = new LightHouse.Core.Collections.DataEnumerator<IDataObject>(dataList);

            Assert.Equal(true, dataEnumerator.MoveNext());
            Assert.Equal(dataList[0], dataEnumerator.Current);
        }

        public void ExecuteReset()
        {
            LightHouse.Core.Collections.IDataList<IDataObject> dataList = new LightHouse.Core.Collections.DataList<IDataObject>()
            {
                new Person()
                {
                    Reference = "Person1"
                }
                .ConvertTo<IDataObject>(),
                new Person()
                {
                    Reference = "Person2"
                }
                .ConvertTo<IDataObject>()
            };

            LightHouse.Core.Collections.DataEnumerator<LightHouse.Core.DataObject> dataEnumerator = new LightHouse.Core.Collections.DataEnumerator<LightHouse.Core.DataObject>(dataList);

            dataEnumerator.MoveNext();
            dataEnumerator.Reset();

            Assert.Equal(dataList[0], dataEnumerator.Current);
        }

    }
}
