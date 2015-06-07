using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core.Notifications;

namespace LightHouse.Core
{
    /// <summary>
    /// Functionality to be implemented by all DataObjects.
    /// </summary>
    public interface IDataObject : IObject 
    {
        /// <summary>
        /// Occurs when a property of the ContractObject has changed.
        /// </summary>
        EventHandler<PropertyChangedEventArgs> ContractPropertyChanged { get; set; }
        
        /// <summary>
        /// Occurs when a property of the ContractObject is changing.
        /// </summary>
        EventHandler<PropertyChangingEventArgs> ContractPropertyChanging { get; set; }

        /// <summary>
        /// Information about the Type if the DataObject has been generated dynamically.
        /// </summary>
        String DynamicType { get; set; }

        /// <summary>
        /// Gets the data type, either dynamic or the real type.
        /// </summary>
        /// <returns>The data type of the DataObject.</returns>
        String GetDataType();

        /// <summary>
        /// Gets the proxy state of the provided property.
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <returns>State of the provided property.</returns>
        Boolean GetProxyState(String property);

        /// <summary>
        /// Sets the proxy state of the provided property.
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <param name="proxy">Proxy state of the property.</param>
        void SetProxyState(String property, Boolean proxy);

        /// <summary>
        /// Gets the property value corresponding to the ContractObject holding this DataObject.
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
