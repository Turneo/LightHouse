using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core
{
    /// <summary>
    /// Functionality to be implemented by all ContractObjects.
    /// </summary>
    public interface IContractObject : IObject
    {
        /// <summary>
        /// Gets the property value corresponding to the ContractObject holding this ContractObject.
        /// </summary>
        /// <typeparam name="T">Type of the value to be returned.</typeparam>
        /// <param name="property">Name of the property on the ContractObject.</param>
        /// <returns>Value of the requested property.</returns>
        T GetContractProperty<T>(String property);

        /// <summary>
        /// Sets the property value corresponding to the ContractObject holding this DataObject.
        /// </summary>
        /// <param name="property">Name of the property on the ContractObject.</param>
        /// <param name="value">Value of the property.</param>
        void SetContractProperty(String property, Object value);       
    }
}
