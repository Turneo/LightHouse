using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Collections
{
    /// <summary>
    /// A thread safe collection of DataObjects.
    /// </summary>
    /// <typeparam name="T">Type of the DataObjects included in the collection.</typeparam>
    public interface IDataList<T> : IQueryableList<T>, IDataList 
    {
        /// <summary>
        /// Adds the DataObjects from another DataList into the current collection.
        /// </summary>
        /// <param name="items">DataList to be added to this collection.</param>
        void Add(IDataList<T> items);
    }
}
