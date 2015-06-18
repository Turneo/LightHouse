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
    /// Functionality to be implemented by all Objects.
    /// </summary>
    public interface IObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// Unique identifier of every object.
        /// </summary>
        String ID { get; set; }

        /// <summary>
        /// Clones the current object.
        /// </summary>
        /// <param name="proxyReferencedObjects">Should objects be proxied during clonation.</param>
        /// <param name="paths">Paths to be included in the clonation.</param>
        /// <returns>Clonated object.</returns>
        IObject Clone(Boolean proxyReferencedObjects, IList<String> paths = null);

        /// <summary>
        /// Sets the value of the provided property.
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        void SetProperty(String property, Object value);

        /// <summary>
        /// Gets the value of the provided property.
        /// </summary>
        /// <typeparam name="T">Type of the property value.</typeparam>
        /// <param name="property">Name of the property.</param>
        /// <returns>Value of the property.</returns>
        T GetProperty<T>(String property);
        
        /// <summary>
        /// Converts the current Object to the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the requested object.</typeparam>
        /// <param name="paths">Paths that need to be included in the conversion.</param>
        /// <returns>Converted object of type T.</returns>
        T As<T>(IList<String> paths = null);
    }
}
