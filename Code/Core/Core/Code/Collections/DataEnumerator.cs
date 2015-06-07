using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace LightHouse.Core.Collections
{
    /// <summary>
    /// Supports a simple iteration over a list of DataObjects's.
    /// </summary>
    /// <typeparam name="T">Type of the DataObject of the enumerator.</typeparam>
    public class DataEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// DataList on which the enumerator does the enumeration.
        /// </summary>
        private IDataList dataList = default(IDataList);

        /// <summary>
        /// Position on the current DataObject in the enumeration.
        /// </summary>
        private Int32 counter = -1;

        /// <summary>
        /// Total DataObjects in the current collection.
        /// </summary>
        private Int32 total = 0;

        /// <summary>
        /// Initializes a new instance of a DataEnumerator based on the provided DataList.
        /// </summary>
        /// <param name="dataList">DataList on which the enumerator does the enumeration.</param>
        public DataEnumerator(IDataList dataList)
        {
            this.dataList = dataList;
            total = dataList.Count;

            dataList.EnterReadLock();
        }

        /// <summary>
        /// Gets the current DataObject in the collection.
        /// </summary>
        public T Current
        {
            get 
            {
                return (T)dataList[counter]; 
            }
        }

        /// <summary>
        /// Disposes the DataEnumerator. The reader lock on the DataList needs to be released on disposal.
        /// </summary>
        public void Dispose()
        {
            dataList.ExitReadLock();
        }

        /// <summary>
        /// Gets current item.
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get 
            { 
                return dataList[counter]; 
            }
        }

        /// <summary>
        /// Advances the enumerator to the next DataObject of the collection.
        /// </summary>
        /// <returns>True if the enumerator was successfully advanced to the next DataObject; false if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
            counter++;

            if (counter < total)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first DataObject in the collection.
        /// </summary>
        public void Reset()
        {
            counter = 0;
        }

        /// <summary>
        /// Advances the enumerator to the next DataObject of the collection.
        /// </summary>
        /// <returns>True if the enumerator was successfully advanced to the next DataObject; false if the enumerator has passed the end of the collection.</returns>
        bool IEnumerator.MoveNext()
        {
            counter++;

            //If counter is less than total, keep looping.
            if (counter < total)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first DataObject in the collection.
        /// </summary>
        void IEnumerator.Reset()
        {
            counter = 0;
        }
    }
}
