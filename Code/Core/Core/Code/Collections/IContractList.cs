using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Collections.Specialized;

namespace LightHouse.Core.Collections
{
    /// <summary>
    /// A thread safe collection of ContractObjects.
    /// </summary>
    public interface IContractList : IEnumerable
    {
        /// <summary>
        /// Gets the contained DataList.
        /// </summary>
        /// <returns>DataList contained in the ContractList.</returns>
        IDataList ToDataList();

        /// <summary>
        /// Adds a new ContractObject to the collection. Internally the ContractObject is converted into a DataObject and added to the DataList.
        /// </summary>
        /// <param name="contractObject">ContractObject to be added.</param>
        void Add(IContractObject contractObject);

        /// <summary>
        /// Removes the specified ContractObject from the collection.
        /// </summary>
        /// <param name="value">ContractObject to be removed.</param>
        void Remove(IContractObject contractObject);

        /// <summary>
        /// Gets the number of ContractObjects in the current collection.
        /// </summary>
        /// <returns>Number of ContractObjects in the current collection.</returns>
        int GetCount();
        
        /// <summary>
        /// Gets the ContractObject at the specified index.
        /// </summary>
        /// <param name="index">Index of the ContractObject to be returned.</param>
        /// <returns>ContractObject at the specified index.</returns>
        IContractObject GetItem(Int32 index);

        /// <summary>
        /// Converts the current ContractList to a new ContractList of a different type.
        /// </summary>
        /// <typeparam name="U">Type of the new ContractList.</typeparam>
        /// <returns>A ContractList of the specified type.</returns>
        IContractList<U> ToContractList<U>() where U : IContractObject;

        /// <summary>
        /// REVIEW: Converts the current ContractList to a new SurroagteList of ISurrogateObject (or inherited).
        /// </summary>
        /// <typeparam name="U">Type of the new SurrogateList.</typeparam>
        /// <returns>A SurrogateList of the specified type of surrogate objects.</returns>
        ISurrogateList<U> ToSurrogateList<U>() where U : ISurrogateObject;
    }
}
