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

namespace LightHouse.Core.Dynamic
{
    /// <summary>
    /// Dynamic version of the SurrogateObject. Used in case none SurrogateObject is provided.
    /// </summary>
    public class SurrogateObject : ISurrogateObject
    {
        /// <summary>
        /// Initializes a new SurrogateObject.
        /// </summary>
        public SurrogateObject()
        {
        }

        /// <summary>
        /// Cache for holding information about the dynamic properties.
        /// </summary>
        private DataCache dynamicCache = new DataCache();

        /// <summary>
        /// Occurs when a property has been changed.
        /// </summary>
        private EventHandler<PropertyChangedEventArgs> propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (!propertyChanged.ContainsHandler<PropertyChangedEventArgs>(value))
                {
                    propertyChanged += value.MakeWeak<PropertyChangedEventArgs>(e => { propertyChanged -= e; });
                }
            }
            remove
            {

            }
        }

        /// <summary>
        /// Occurs when a property is changing.
        /// </summary>
        private EventHandler<PropertyChangingEventArgs> propertyChanging;
        public event PropertyChangingEventHandler PropertyChanging
        {
            add
            {
                if (!propertyChanging.ContainsHandler<PropertyChangingEventArgs>(value))
                {
                    propertyChanging += value.MakeWeak<PropertyChangingEventArgs>(e => { propertyChanging -= e; });
                }
            }
            remove
            {

            }
        }

        /// <summary>
        /// Unique identifier of the SurrogateObject.
        /// </summary>
        public String ID { get; set; }

        /// <summary>
        /// Information about the ContractType or DataType that represents the SurrogageObject.
        /// </summary>
        public LightHouse.Core.Elite.Locating.TypeInfo TypeInfo { get; set; }

        /// <summary>
        /// Sets the value of the provided property.
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        public void SetProperty(String name, Object value)
        {
            switch (name)
            {
                case "ID":
                    this.ID = value.ToString();
                    break;
                default:
                    dynamicCache.Add(name, value, "Properties");
                    break;
            }

        }

        /// <summary>
        /// Gets the value of the provided property.
        /// </summary>
        /// <typeparam name="T">Type of the property value.</typeparam>
        /// <param name="property">Name of the property.</param>
        /// <returns>Value of the property.</returns>
        public T GetProperty<T>(String name)
        {
            switch (name)
            {
                case "ID":
                    return (T)(Object)this.ID;                    
                default:
                    return dynamicCache.Get<T>(name, "Properties");                    
            }
        }

        /// <summary>
        /// Triggers a OnPropertyChanged event.
        /// </summary>
        /// <param name="name">Name of the property that has changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Converts the current SurrogateObject to the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the requested object.</typeparam>
        /// <param name="paths">Paths that need to be included in the conversion.</param>
        /// <returns>Converted object of type T.</returns>
        public T ConvertTo<T>(IList<string> paths = null)
        {
            return LightHouse.Elite.Core.Converter.ConvertTo<T>(this, paths);            
        }

        /// <summary>
        /// Clones the current SurrogateObject.
        /// </summary>
        /// <param name="proxyReferencedObjects">Should objects be proxied during clonation.</param>
        /// <param name="paths">Paths to be included in the clonation.</param>
        /// <returns>Clonated DataObject.</returns>
        public IObject Clone(bool proxyReferencedObjects, IList<string> paths = null)
        {
            throw new NotImplementedException();
        }
    }
}
