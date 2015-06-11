using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;

namespace LightHouse.Core.Collections
{
    /// <summary>
    /// A thread safe collection of DataObjects.
    /// </summary>
    public interface IDataList : IList
    {
        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// DynamicType of the DataList. To be used in the case the DataObject are dynamic and not yet generated.
        /// </summary>
        String DynamicType { get; set;  }

        /// <summary>
        /// Acquires a reader lock, for thread-safe reading operations on the collection.
        /// </summary>
        void EnterReadLock();

        /// <summary>
        /// Releases the reader lock that has been used for thread-safe reading operations on the collection.
        /// </summary>
        void ExitReadLock();

        /// <summary>
        /// Converts the current DataList to a new SurroagteList of the provided type (inherited from ISurrogateObject).
        /// </summary>
        /// <typeparam name="U">Type of the new SurrogateList.</typeparam>
        /// <returns>A SurrogateList of the specified type of surrogate objects.</returns>
        ISurrogateList<U> ToSurrogateList<U>() where U : ISurrogateObject;
    }
}
