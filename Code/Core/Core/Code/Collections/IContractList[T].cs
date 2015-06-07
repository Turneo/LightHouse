using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LightHouse.Core.Collections
{
    /// <summary>
    /// A thread safe collection of ContractObjects.
    /// </summary>
    /// <typeparam name="T">Type of the ContractObjects included in the collection.</typeparam>
    public interface IContractList<T> : IQueryableList<T>, IList<T>, IContractList
    {
        /// <summary>
        /// Adds the ContractObjects from another ContractList into the current collection.
        /// </summary>
        /// <param name="items">ContractList to be added to this collection.</param>
        void Add(IContractList<T> items);
    }
}
