using LightHouse.Core.Bindings;
using LightHouse.Core.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;

namespace LightHouse.Core.Collections
{
    /// <summary>
    /// A thread safe collection of DataObjects.
    /// </summary>
    /// <typeparam name="T">Type of the DataObjects included in the collection.</typeparam>
    public class DataList<T> : QueryableList<T>, IDataList<T>
    {
        /// <summary>
        /// REVIEW: Query that the DataList is bound to
        /// </summary>
        protected IQuery query;

        /// <summary>
        /// REVIEW: ObjectPath that the DataList is bound to
        /// </summary>
        protected ObjectPath objectPath;

        /// <summary>
        /// Internal list containing the DataObjects.
        /// </summary>
        private IList<T> dataList = new List<T>();

        /// <summary>
        /// Lock to be used for supporting single writers and multiple readers to the DataList.
        /// </summary>
        private ReaderWriterLockSlim readWriterLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// Initializes a new instance of a DataList.
        /// </summary>
        public DataList()
        {
        }

        /// <summary>
        /// Initializes a new instance of a DataList based on the provided query.
        /// </summary>
        /// <param name="query">Query object containing required querying information.</param>
        public DataList(IQuery query)
        {
            this.query = query;
        }

        /// <summary>
        /// Initializes a new instance of a DataList based on the provided ObjectPath.
        /// </summary>
        /// <param name="objectPath">ObjectPath containing required binding information.</param>
        public DataList(ObjectPath objectPath)
        {
            this.objectPath = objectPath;
        }

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;
      
        /// <summary>
        /// DynamicType of the DataList. To be used in the case the DataObject are dynamic and not yet generated.
        /// </summary>
        public String DynamicType { get; set; }

        /// <summary>
        /// Acquires a reader lock, for thread-safe reading operations on the collection.
        /// </summary>
        public void EnterReadLock()
        {
            readWriterLock.EnterReadLock();
        }

        /// <summary>
        /// Releases the reader lock that has been used for thread-safe reading operations on the collection.
        /// </summary>
        public void ExitReadLock()
        {
            readWriterLock.ExitReadLock();
        }

        /// <summary>
        /// Adds a DataObject to the collection. 
        /// </summary>
        /// <param name="item">DataObject to be added.</param>
        public void Add(T item)
        {
            dataList.Add((T)item);
        }

        /// <summary>
        /// Returns the index of the specified DataObject.
        /// </summary>
        /// <param name="item">DataObject to lockup inside the collection.</param>
        /// <returns>Index of the DataObject within the collection.</returns>
        public int IndexOf(T item)
        {
            return dataList.IndexOf(item);
        }

        /// <summary>
        /// Inserts a DataObject to the collection at the specified index.
        /// </summary>
        /// <param name="index">Position at which the DataObject should be inserted.</param>
        /// <param name="item">DataObject to be inserted.</param>
        public void Insert(int index, T item)
        {
            dataList.Insert(index, item);    
        }

        /// <summary>
        /// Removes the DataObject at the specified index.
        /// </summary>
        /// <param name="index">Index of DataObject to be removed.</param>
        public void RemoveAt(int index)
        {
            dataList.RemoveAt(index);
        }

        /// <summary>
        // Gets or sets the DataObject at the specified index.
        /// </summary>
        /// <param name="index">Index of the DataObject to be returned.</param>
        /// <returns>DataObject at the specified index.</returns>
        public T this[int index]
        {
            get
            {
                return dataList[index];
            }
            set
            {
                dataList[index] = value;
            }
        }

        /// <summary>
        /// Removes all the DataObjects in the collection.
        /// </summary>
        public void Clear()
        {
            dataList.Clear();
        }

        /// <summary>
        ///  Checks if the collection contains the specified DataObject.
        /// </summary>
        /// <param name="item">DataObject which has to be searched in the collection.</param>
        /// <returns>True if the DataObject is found in this collection; otherwise false.</returns>
        public bool Contains(T item)
        {
            return dataList.Contains(item);
        }

        /// <summary>
        /// Copies the DataObject to the index at the provided array.
        /// </summary>
        /// <param name="array">Array which should be modified at the provided index.</param>
        /// <param name="arrayIndex">Position of DataObject that needs to be copied to the provided array.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            array[arrayIndex] = this[arrayIndex];
        }

        /// <summary>
        /// Gets the number of DataObjects contained in the collection. 
        /// </summary>
        public int Count
        {
            get 
            { 
                if (this.query != null)
                {
                    return  GetQueryable().Count();
                }

                return dataList.Count(); 
            }
        }

        /// <summary>
        /// Gets whether the collection is read-only or not. 
        /// </summary>
        public bool IsReadOnly
        {
            get 
            { 
                return false; 
            }
        }

        /// <summary>
        /// Removes the specified DataObject from the collection.
        /// </summary>
        /// <param name="item">DataObject to be removed.</param>
        /// <returns>True if the DataObject was removed successfully; otherwise false.</returns>
        public bool Remove(T item)
        {
            return dataList.Remove(item);
        }

        /// <summary>
        /// Adds the DataObjects from another DataList into the current collection.
        /// </summary>
        /// <param name="items">DataList to be added to this collection.</param>
        public void Add(IDataList<T> items)
        {
            foreach (T item in items)
            {
                dataList.Add(item);
            }
        }

        /// <summary>
        /// Adds a DataObject to the collection.
        /// </summary>
        /// <param name="item">DataObject to be added.</param>
        public void Add(object item)
        {
            dataList.Add((T)item);
        }

        /// <summary>
        /// Adds a DataObject to the collection.
        /// </summary>
        /// <param name="value">DataObject to be added.</param>
        /// <returns>Index of the newly added DataObject in the collection.</returns>
        int System.Collections.IList.Add(object value)
        {
            dataList.Add((T)value);
            return 1;
        }

        /// <summary>
        /// Checks if the collection contains the specified DataObject.
        /// </summary>
        /// <param name="value">DataObject which has to be searched in the collection.</param>
        /// <returns>True if the DataObject is contained in the collection; otherwise false.</returns>
        public bool Contains(object value)
        {
            return dataList.Contains((T)value);
        }

        /// <summary>
        /// Returns the index of the specified DataObject.
        /// </summary>
        /// <param name="value">DataObject to lockup inside the collection.</param>
        /// <returns>Index of the DataObject within the collection.</returns>
        public int IndexOf(object value)
        {
            return dataList.IndexOf((T)value);
        }

        /// <summary>
        /// Inserts a DataObject to the collection at the specified index.
        /// </summary>
        /// <param name="index">Position at which the DataObject should be inserted.</param>
        /// <param name="value">DataObject to be inserted.</param>
        public void Insert(int index, object value)
        {
            dataList.Insert(index, (T)value);    
        }

        /// <summary>
        /// Get a value indicating whether the collection is fix in size.
        /// </summary>
        public bool IsFixedSize
        {
            get 
            { 
                return false; 
            }
        }

        /// <summary>
        /// Removes the specified DataObject from the collection.
        /// </summary>
        /// <param name="value">DataObject to be removed.</param>
        public void Remove(object value)
        {
            dataList.Remove((T)value);
        }

        /// <summary>
        /// Gets or sets the DataObject at the specified index.
        /// </summary>
        /// <param name="index">Index of the DataObject to be returned.</param>
        /// <returns>DataObject at the specified index.</returns>
        object System.Collections.IList.this[int index]
        {
            get
            {
                if (index < dataList.Count)
                {
                    return dataList[index];
                }

                return null;
            }
            set
            {
                if (index < dataList.Count)
                {
                    dataList[index] = (T)value;
                }
            }
        }

        /// <summary>
        /// Copies the DataObject to the index at the provided array.
        /// </summary>
        /// <param name="array">Array which should be modified at the provided index.</param>
        /// <param name="index">Position of DataObject that needs to be copied to the provided array.</param>
        public void CopyTo(Array array, int index)
        {
            ((T[])array)[index] = this[index];
        }

        /// <summary>
        /// Gets whether the collection is synchronized or not.
        /// </summary>
        public bool IsSynchronized
        {
            get 
            { 
                return ((IList)dataList).IsSynchronized; 
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the collection.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return ((IList)dataList).SyncRoot;
            }
        }

        /// <summary>
        /// Gets the Query for the collection.
        /// </summary>
        /// <returns>A Query for the collection.</returns>
        public IQueryable<T> Query()
        {
            return (IQueryable<T>)this.query;            
        }

        /// <summary>
        /// Gets the DataEnumerator for the collection.
        /// </summary>
        /// <returns>A DataEnumerator for the collection.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (query != null)
            {
                return  GetQueryable().GetEnumerator();
            }

            return new DataEnumerator<T>(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            if (query != null)
            {
                return  GetQueryable().GetEnumerator();
            }

            return new DataEnumerator<T>(this);
        }

        /// <summary>
        /// Gets the type of the Queryable for the current collection.
        /// </summary>
        public override Type ElementType
        {
            get
            {
                return GetQueryable().ElementType;
            }
        }

        /// <summary>
        /// Gets the expression of the Queryable for  the current collection.
        /// </summary>
        public override System.Linq.Expressions.Expression Expression
        {
            get
            {
                return GetQueryable().Expression;
            }
        }

        /// <summary>
        /// Gets the query provider of the Queryable for the current collection.
        /// </summary>
        public override IQueryProvider Provider
        {
            get
            {
                return GetQueryable().Provider;
            }
        }

        /// <summary>
        /// Returns the Queryable for the current collection.
        /// </summary>
        /// <returns>Queryable for the current collection.</returns>
        private IQueryable<T> GetQueryable()
        {
            if (this.query != null)
            {
                return (IQueryable<T>)this.query;
            }

            IList<T> list = new List<T>();

            if ((dataList != null) && (dataList.Count > 0))
            {
                foreach (IDataObject item in dataList)
                {
                    list.Add(item.ConvertTo<T>());
                }
            }

            return list.AsQueryable<T>();
        }

        /// <summary>
        /// Converts the current ContractList to a new SurrogateList of U which must be inherited from ISurrogateObject.
        /// </summary>
        /// <typeparam name="U">Type of the new SurrogateList.</typeparam>
        /// <returns>A SurrogateList of the specified type of surrogate objects.</returns>
        public ISurrogateList<U> ToSurrogateList<U>() where U : ISurrogateObject
        {
            return new SurrogateList<U>(this.query.As<U>());
        }
    }
}
