using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core.Caching;
using LightHouse.Core.Collections;
using LightHouse.Core.Notifications;

namespace LightHouse.Core
{
    /// <summary>
    /// Object to be used as a surrogate for ContractObjects and DataObjects.
    /// </summary>
    public class SurrogateObject : ISurrogateObject
    {
        /// <summary>
        /// Initializes a new instance of a SurrogateObject.
        /// </summary>
        public SurrogateObject()
        {
        }

        /// <summary>
        /// Occurs when a property of the SurrogateObject has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Occurs when a property of the SurrogateObject is changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Unique identifier of the SurrogateObject.
        /// </summary>
        public String ID { get; set; }

        /// <summary>
        /// Information about the ContractType or DataType.
        /// </summary>
        public LightHouse.Core.Elite.Locating.TypeInfo TypeInfo { get; set; }

        /// <summary>
        /// Converts to a ContractObject or DataObject. The provided paths are only copied into the new object.
        /// </summary>
        /// <typeparam name="T">Type of the requested object.</typeparam>
        /// <param name="paths">Paths to be included in the conversion.</param>
        /// <returns>Converted object of type T.</returns>
        public T As<T>(IList<string> paths = null)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Triggers a OnPropertyChanged event.
        /// </summary>
        /// <param name="name">Name of the method calling the OnPropertyChanged event</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Sets the value of the provided property.
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        public void SetProperty(string property, object value)
        {
            LightHouse.Elite.Core.Reflector.SetPropertyValue(property, this, value);
        }

        /// <summary>
        /// Gets the value of the provided property.
        /// </summary>
        /// <typeparam name="T">Type of the property value.</typeparam>
        /// <param name="property">Name of the property.</param>
        /// <returns>Value of the property.</returns>
        public T GetProperty<T>(string property)
        {
            return (T)(Object)LightHouse.Elite.Core.Reflector.GetPropertyValue(property, this);
        }

        /// <summary>
        /// Clones the current SurrogateObject.
        /// </summary>
        /// <param name="proxyReferencedObjects">Should objects be proxied during clonation.</param>
        /// <param name="paths">Paths to be included in the clonation.</param>
        /// <returns>Cloned SurrogateObject.</returns>
        public IObject Clone(bool proxyReferencedObjects, IList<string> paths = null)
        {
            throw new NotImplementedException();
        }
    }
}
