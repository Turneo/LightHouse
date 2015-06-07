using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace LightHouse.Core.Collections
{
    /// <summary>
    /// Supports a simple iteration over a list of ContractObjects.
    /// </summary>
    /// <typeparam name="T">Type of the ContractObject of the enumerator.</typeparam>
    public class ContractEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// Internal DataEnumerator used for enumerating through the related DataObjects.
        /// </summary>
        private IEnumerator dataEnumerator = default(IEnumerator);

        /// <summary>
        /// Initializes a new instance of a ContractEnumerator with a given DataEnumerator.
        /// </summary>
        /// <param name="dataEnumerator">DataEnumerator to be used for the iteration of ContractObjects.</param>
        public ContractEnumerator(IEnumerator dataEnumerator)
        {
            this.dataEnumerator = dataEnumerator;
        }

        /// <summary>
        /// Disposes the containing DataEnumerator.
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)dataEnumerator).Dispose();
        }

        /// <summary>
        /// Gets the current ContractObject in the collection.
        /// </summary>
        public T Current
        {
            get 
            {
                return LightHouse.Elite.Core.Builder.GetContractObject<T>((DataObject)dataEnumerator.Current); 
            }
        }

        /// <summary>
        /// Gets the current ContractObject in the collection.
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get 
            {
                return LightHouse.Elite.Core.Builder.GetContractObject<T>((DataObject)dataEnumerator.Current); 
            }
        }

        /// <summary>
        /// Advances the enumerator to the next ContractObject of the collection.
        /// </summary>
        /// <returns>True if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
            return dataEnumerator.MoveNext();
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first ContractObject in the collection.
        /// </summary>
        public void Reset()
        {
            dataEnumerator.Reset();
        }

        /// <summary>
        /// Advances the enumerator to the next ContractObject of the collection.
        /// </summary>
        /// <returns>True if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
        bool IEnumerator.MoveNext()
        {
            return dataEnumerator.MoveNext();
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first ContractObject in the collection.
        /// </summary>
        void IEnumerator.Reset()
        {
            dataEnumerator.Reset();
        }
    }
}
