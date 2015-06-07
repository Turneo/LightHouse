using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace LightHouse.Core.Collections
{
    /// <summary>
    /// A thread safe collection of ContractObjects.
    /// </summary>
    /// <typeparam name="T">Type of the ContractObjects included in the collection.</typeparam>
    public class ContractList<T> : QueryableList<T>, IContractList<T>, INotifyCollectionChanged
    {
        /// <summary>
        /// The wrapped DataList containing the actual DataObjects.
        /// </summary>
        IDataList dataList = default(IDataList);

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                dataList.CollectionChanged += value;
            }
            remove
            {
                dataList.CollectionChanged -= value;
            }
        }

        /// <summary>
        /// Initializes a new instance of ContractList using a provided DataList.
        /// </summary>
        /// <param name="dataList">DataList to be used for constructing the ContractList.</param>
        public ContractList(IDataList dataList)
        {
            this.dataList = dataList;
        }

        /// <summary>
        /// Initializes a new instance of ContractList using a new DataList.
        /// </summary>
        public ContractList()
        {
            this.dataList = new DataList<DataObject>();
        }

        /// <summary>
        /// Gets the contained DataList.
        /// </summary>
        /// <returns>DataList contained in the ContractList.</returns>
        public IDataList ToDataList()
        {
            return dataList;
        }

        /// <summary>
        /// Adds a ContractObject to the collection. Internally the ContractObject is converted into a DataObject and added to the DataList.
        /// </summary>
        /// <param name="item">ContractObject to be added.</param>
        public void Add(T item)
        {
            dataList.Add(((IContractObject)(Object)item).ConvertTo<IDataObject>());            
        }

        /// <summary>
        /// Adds a new ContractObject to the collection. Internally the ContractObject is converted into a DataObject and added to the DataList.
        /// </summary>
        /// <param name="contractObject">ContractObject to be added.</param>
        public void Add(IContractObject contractObject)
        {
            dataList.Add(contractObject.ConvertTo<IDataObject>());
        }

        /// <summary>
        /// Adds the ContractObjects from another ContractList into the current collection.
        /// </summary>
        /// <param name="items">ContractList to be added to this collection.</param>
        public void Add(IContractList<T> items)
        {
            foreach (T item in items)
            {
                dataList.Add(((IContractObject)(Object)item).ConvertTo<IDataObject>());
            }
        }

        /// <summary>
        /// Returns the index of the specified ContractObject.
        /// </summary>
        /// <param name="item">ContractObject to lockup inside the collection.</param>
        /// <returns>Index of the ContractObject within the collection.</returns>
        public int IndexOf(T item)
        {
            return dataList.IndexOf(((IContractObject)(Object)item).ConvertTo<IDataObject>());
        }

        /// <summary>
        /// Inserts a ContractObject to the collection at the specified index.
        /// </summary>
        /// <param name="index">Position at which the ContractObject should be inserted.</param>
        /// <param name="item">ContractObject to be inserted.</param>
        public void Insert(int index, T item)
        {
            dataList.Insert(index, ((IContractObject)(Object)item).ConvertTo<IDataObject>());
        }

        /// <summary>
        /// Removes the ContractObject at the specified index.
        /// </summary>
        /// <param name="index">Index of ContractObject to be removed.</param>
        public void RemoveAt(int index)
        {
            dataList.RemoveAt(index);
        }

        /// <summary>
        /// Gets the ContractObject at the specified index.
        /// </summary>
        /// <param name="index">Index of the ContractObject to be returned.</param>
        /// <returns>ContractObject at the specified index.</returns>
        public IContractObject GetItem(int index)
        {
            return (IContractObject)(Object)LightHouse.Elite.Core.Builder.GetContractObject<T>((DataObject)dataList[index]);
        }

        /// <summary>
        /// Removes all the ContractObjects in the collection.
        /// </summary>
        public void Clear()
        {
            dataList.Clear();
        }

        /// <summary>
        /// Checks if the collection contains the specified ContractObject.
        /// </summary>
        /// <param name="item">ContractObject which has to be searched in the collection.</param>
        /// <returns>True if the ContractObject is found in this collection; otherwise false.</returns>
        public bool Contains(T item)
        {
            return dataList.Contains(((IContractObject)(object)item).ConvertTo<IDataObject>());
        }

        /// <summary>
        /// Copies the ContractObject to the index at the provided array.
        /// </summary>
        /// <param name="array">Array which should be modified at the provided index.</param>
        /// <param name="arrayIndex">Position of ContractObject that needs to be copied to the provided array.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            array[arrayIndex] = this[arrayIndex];
        }

        /// <summary>
        /// Gets the number of ContractObjects contained in the collection. 
        /// </summary>
        public int Count
        {
            get 
            { 
                return dataList.Count; 
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
        /// Removes the specified ContractObject from the collection.
        /// </summary>
        /// <param name="item">ContractObject to be removed.</param>
        /// <returns>True if the ContractObject was removed successfully; otherwise false.</returns>
        public bool Remove(T item)
        {
            dataList.Remove(((IContractObject)(Object)item).ConvertTo<IDataObject>());

            return true;
        }
         
        /// <summary>
        /// Adds the specified ContractObject to the collection and returns the index of the added ContractObject.
        /// </summary>
        /// <param name="value">ContractObject to be added to the collection.</param>
        /// <returns>Index of the newly added ContractObject in the collection.</returns>
        public int Add(object value)
        {
            if (value != null)
            {
                if (typeof(DataObject).GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
                {
                    dataList.Add((DataObject)value);
                }
                else if (typeof(IContractObject).GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
                {
                    dataList.Add(((IContractObject)value).ConvertTo<IDataObject>());
                }
                else if (typeof(ISurrogateObject).GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
                {
                    dataList.Add(((ISurrogateObject)value).ConvertTo<IDataObject>());
                }
            } 
            
            return dataList.Count - 1;
        }

        /// <summary>
        /// Checks if the collection contains the specified collection.
        /// </summary>
        /// <param name="value">ContractObject which has to be searched in the collection.</param>
        /// <returns>True if the ContractObject is contained in the ContractList; otherwise false.</returns>
        public bool Contains(object value)
        {
            if ((value != null) && (value is IContractObject))
            {
                IContractObject item = value as IContractObject;
                return dataList.Contains(item.ConvertTo<IDataObject>());
            }

            return false;
        }

        /// <summary>
        /// Returns the index of the specified ContractObject.
        /// </summary>
        /// <param name="value">ContractObject to lockup inside the collection.</param>
        /// <returns>Index of the ContractObject within the collection.</returns>
        public int IndexOf(object value)
        {
            if ((value != null) && (value is IContractObject))
            {
                IContractObject item = value as IContractObject;
                return dataList.IndexOf(item.ConvertTo<IDataObject>());
            }

            return -1;
        }

        /// <summary>
        /// Inserts a ContractObject to the collection at the specified index.
        /// </summary>
        /// <param name="index">Position at which the ContractObject should be inserted.</param>
        /// <param name="value">ContractObject to be inserted.</param>
        public void Insert(int index, object value)
        {
            if ((value != null) && (value is IContractObject))
            {
                IContractObject item = value as IContractObject;
                dataList.Insert(index, item.ConvertTo<IDataObject>());
            }
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
        /// Removes the specified ContractObject from the collection.
        /// </summary>
        /// <param name="value">ContractObject to be removed.</param>
        public void Remove(IContractObject value)
        {
            if ((value != null) && (value is IContractObject))
            {
                IContractObject item = value as IContractObject;
                dataList.Remove(item.ConvertTo<IDataObject>());
            }         
        }

        /// <summary>
        /// Copies the ContractObject to the index at the provided array.
        /// </summary>
        /// <param name="array">Array which should be modified at the provided index.</param>
        /// <param name="index">Position of ContractObject that needs to be copied to the provided array.</param>
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
                return ((IList)dataList).IsSynchronized;
            }

        }

        /// <summary>
        /// Adds the ContractObjects from another ContractList into the current ContractList.
        /// </summary>
        /// <param name="contractList">ContractList to be added to this ContractList.</param>
        public void AddRange(IContractList<T> contractList)
        {
            this.Add(contractList);
        }

        /// <summary>
        /// Converts the current ContractList to a new ContractList of a different type.
        /// </summary>
        /// <typeparam name="U">Type of the new ContractList.</typeparam>
        /// <returns>A ContractList of the specified type.</returns>
        public IContractList<U> ToContractList<U>() where U : IContractObject
        {
            return new ContractList<U>(dataList);
        }

        /// <summary>
        /// Converts the current ContractList to a new ContractList of a different type.
        /// </summary>
        /// <param name="type">Type of the new ContractList.</param>
        /// <returns>A ContractList of the specified type.</returns>
        public IContractList ToContractList(Type type)
        {
            return LightHouse.Elite.Core.Builder.GetContractList(type, this.dataList);
        }

        /// <summary>
        /// Adds the ContractObjects from another ContractList into the current ContractList.
        /// </summary>
        /// <param name="items">ContractList to be added to this ContractList.</param>
        public void Add(IContractList items)
        {
            this.Add(items);
        }

        /// <summary>
        /// Gets the ContractEnumerator for the collection.
        /// </summary>
        /// <returns>A ContractEnumerator for the collection.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new ContractEnumerator<T>(new DataEnumerator<IDataObject>(dataList));
        }

        /// <summary>
        /// Gets the ContractEnumerator for the collection.
        /// </summary>
        /// <returns>A ContractEnumerator for the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ContractEnumerator<T>(new DataEnumerator<IDataObject>(dataList));
        }
        
        /// <summary>
        /// Gets the type of the Queryable for the current collection.
        /// </summary>
        public override Type ElementType
        {
            get 
            {
                return GetQueryable<T>().ElementType;
            }
        }

        /// <summary>
        /// Gets the expression of the Queryable for the current collection.
        /// </summary>
        public override System.Linq.Expressions.Expression Expression
        {
            get
            {
                return GetQueryable<T>().Expression;
            }
        }

        /// <summary>
        /// Gets the query provider of the Queryable for the current collection.
        /// </summary>
        public override IQueryProvider Provider
        {
            get 
            {
                return GetQueryable<T>().Provider;
            }
        }

        /// <summary>
        /// Returns the Queryable for the current collection.
        /// </summary>
        /// <returns>Queryable for the current collection.</returns>
        private IQueryable GetQueryable<T>()
        {
            IList<T> list = new List<T>();

            if ((dataList != null) && (dataList.Count > 0))
            {
                foreach(IDataObject item in dataList)
                {
                    list.Add(item.ConvertTo<T>());
                }
            }

            return list.AsQueryable();
        }

        /// <summary>
        /// Gets the number of ContractObjects in the current collection.
        /// </summary>
        /// <returns>Number of ContractObjects in the current collection.</returns>
        public int GetCount()
        {
            return this.Count;
        }

        /// <summary>
        /// Removes the specified ContractObject from the collection.
        /// </summary>
        /// <param name="value">ContractObject to be removed.</param>
        public void Remove(object value)
        {
            dataList.Remove(((IContractObject)(Object)value).ConvertTo<IDataObject>());
        }

        /// <summary>
        /// Gets or sets the ContractObject at the specified index.
        /// </summary>
        /// <param name="index">Index of the ContractObject to be returned.</param>
        /// <returns>ContractObject at the specified index.</returns>
        public T this[int index]
        {
            get
            {
                return LightHouse.Elite.Core.Builder.GetContractObject<T>((DataObject)dataList[index]);
            }
            set
            {
                dataList[index] = ((IContractObject)(Object)value).ConvertTo<IDataObject>();
            }
        }
    }
}
