using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using LightHouse.Bootstrap;
using LightHouse.Core.Notifications;

namespace LightHouse.Core
{
    /// <summary>
    /// Represents the base class of all ContractObjects. A contract is similar to an interface but provides
    /// several key improvements (more details in the knowledge pages) that allows LightHouse to be a modular 
    /// and composable framework.
    /// </summary>
    public abstract class ContractObject : IContractObject
    {
        /// <summary>
        /// Occurs when a property value is changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging
        {
            add            
            {
                IDataObject dataObject = ConvertTo<IDataObject>();

                if (!dataObject.ContractPropertyChanging.ContainsHandler<PropertyChangingEventArgs>(value))
                {
                    dataObject.ContractPropertyChanging += value.MakeWeak<PropertyChangingEventArgs>(eventHandler => { dataObject.ContractPropertyChanging -= eventHandler; });
                }
            }
            remove
            {
                
            }
        }

        /// <summary>
        /// Occurs when a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                IDataObject dataObject = ConvertTo<IDataObject>();

                if (!dataObject.ContractPropertyChanged.ContainsHandler<PropertyChangedEventArgs>(value))
                {
                    dataObject.ContractPropertyChanged += value.MakeWeak<PropertyChangedEventArgs>(eventHandler => { dataObject.ContractPropertyChanged -= eventHandler; });
                }
            }
            remove
            {
                
            }
        }

        /// <summary>
        /// Holds the DataObject which is wrapped by this ContractObject (in this case the ContractObject field
        /// will be empty). 
        /// </summary>
        protected IDataObject dataObject;

        /// <summary>
        /// Holds the ContractObject which is wrapped by this ContractObject (in this case the DataObject field
        /// will be empty). 
        /// </summary>
        protected IContractObject contractObject;

        /// <summary>
        /// Unique identifier of the the ContractObject.
        /// </summary>
        [DataMember]
        public virtual String ID
        {
            get { return GetContractProperty<String>(String.Format("{0}.{1}",typeof(ContractObject).FullName, "ID")); }
            set { SetContractProperty(String.Format("{0}.{1}",typeof(ContractObject).FullName, "ID"), value); }
        }

        /// <summary>
        /// Initializes an instance of a ContractObject without a specific DataObject.
        /// </summary>
        public ContractObject() : this(false)
        {
            
        }

        /// <summary>
        /// Initializes an instance of a ContractObject with a specific proxy state.
        /// </summary>
        public ContractObject(Boolean proxyState)
        {
            dataObject = LightHouse.Elite.Core.Builder.GetDataObject(this, proxyState);
        }

        /// <summary>
        /// Initializes an instance of a ContractObject with a specific DataObject.
        /// </summary>
        /// <param name="dataObject">DataObject that is wrapped by this ContractObject.</param>
        public ContractObject(IDataObject dataObject)
        {
            this.dataObject = dataObject;
        }

        /// <summary>
        /// CInitializes an instance of a ContractObject with a specific ContractObject.
        /// </summary>
        /// <param name="dataObject">ContractObject that is wrapped by this ContractObject.</param>
        public ContractObject(IContractObject contractObject)
        {
            this.contractObject = contractObject;
        }

        /// <summary>
        /// Converts the current ContractObject to the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the requested object.</typeparam>
        /// <param name="paths">Paths that need to be included in the conversion.</param>
        /// <returns>Converted object of type T.</returns>
        public T ConvertTo<T>(IList<String> paths = null)
        {
            if(typeof(IDataObject).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
            {
                if (dataObject != null)
                {
                    return (T)dataObject;
                }
                else
                {
                    if (contractObject != null)
                    {
                        return contractObject.ConvertTo<T>();
                    }
                }
            }
            else if (typeof(ISurrogateObject).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
            {
                return (T)LightHouse.Elite.Core.Converter.ConvertTo<T>(this, paths);
            }

            return default(T);
        }

        /// <summary>
        /// Triggers a OnPropertyChanging event.
        /// </summary>
        /// <param name="name">Name of the property that is changing.</param>
        protected virtual void OnPropertyChanging(String name, Object newValue = null, Object oldValue = null)
        {
            EventHandler<PropertyChangingEventArgs> handler = ConvertTo<IDataObject>().ContractPropertyChanging;

            if (handler != null)
            {
                handler(this, new PropertyChangingEventArgs(name, newValue, oldValue));
            }
        }

        /// <summary>
        /// Triggers a OnPropertyChanged event.
        /// </summary>
        /// <param name="name">Name of the property that has changed.</param>
        protected virtual void OnPropertyChanged(String name)
        {
            EventHandler<PropertyChangedEventArgs> handler = ConvertTo<IDataObject>().ContractPropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Gets the value of the provided property.
        /// </summary>
        /// <typeparam name="T">Type of the property value.</typeparam>
        /// <param name="property">Name of the property.</param>
        /// <returns>Value of the property.</returns>
        public T GetProperty<T>(String property)
        {
            //TODO: Review if it's a good idea to set this as an abstract so that all inherited classes requiere to override it 
            //(only if the property is not found it should call this base class)

            //TODO: Create Generic GetPropertyValue<T> on Core.Operator and use it here.
            return (T)LightHouse.Elite.Core.Reflector.GetPropertyValue(property, this);
        }

        /// <summary>
        /// Sets the value of the provided property.
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        public void SetProperty(String property, Object value)
        {
            //TODO: Review if it's a good idea to set this as an abstract so that all inherited classes requiere to override it 
            //(only if the property is not found it should call this base class)

            LightHouse.Elite.Core.Reflector.SetPropertyValue(property, this, value);
        }

        /// <summary>
        /// Sets the property value corresponding to the ContractObject holding this DataObject.
        /// </summary>
        /// <param name="property">Name of the property on the ContractObject.</param>
        /// <param name="value">Value of the property.</param>
        public virtual T GetContractProperty<T>(String property)
        {
            IDataObject dataObject = ConvertTo<IDataObject>();

            if (dataObject != null)
            {
                T value = (T)dataObject.GetContractProperty<T>(property);

                return value;
            }
            else if (contractObject != null)
            {
                return (T)contractObject.GetContractProperty<T>(property);
            }

            return default(T);
        }

        /// <summary>
        /// Gets the PropertyChangedEventHandler event handler from the DataObject.
        /// </summary>
        /// <returns>PropertyChangedEventHandler form the DataObject.</returns>
        protected PropertyChangedEventHandler GetContractPropertyChangedEventHandler()
        {
            IDataObject dataObject = ConvertTo<IDataObject>();
            PropertyChangedEventHandler eventHandler = default(PropertyChangedEventHandler);

            if (dataObject != null)
            {
                eventHandler = dataObject.GetContractProperty<PropertyChangedEventHandler>(this.GetType().FullName + ".PropertyChanged");

                if (eventHandler == default(PropertyChangedEventHandler))
                {
                    dataObject.SetContractProperty(this.GetType().FullName + ".PropertyChanged", eventHandler);
                }
            }
            else if (contractObject != null)
            {
                eventHandler = contractObject.GetContractProperty<PropertyChangedEventHandler>(this.GetType().FullName + ".PropertyChanged");

                if (eventHandler == default(PropertyChangedEventHandler))
                {
                    contractObject.SetContractProperty(this.GetType().FullName + ".PropertyChanged", eventHandler);
                }
            }

            return eventHandler;
        }

        /// <summary>
        /// Gets the PropertyChangingEventHandler event handler from the DataObject.
        /// </summary>
        /// <returns>PropertyChangingEventHandler form the DataObject.</returns>
        protected PropertyChangingEventHandler GetContractPropertyChangingEventHandler()
        {
            IDataObject dataObject = ConvertTo<IDataObject>();
            PropertyChangingEventHandler eventHandler = default(PropertyChangingEventHandler);

            if (dataObject != null)
            {
                eventHandler = dataObject.GetContractProperty<PropertyChangingEventHandler>(this.GetType().FullName + ".PropertyChanging");

                if (eventHandler == default(PropertyChangingEventHandler))
                {
                    dataObject.SetContractProperty(this.GetType().FullName + ".PropertyChanging", eventHandler);
                }
            }
            else if (contractObject != null)
            {
                eventHandler = contractObject.GetContractProperty<PropertyChangingEventHandler>(this.GetType().FullName + ".PropertyChanging");

                if (eventHandler == default(PropertyChangingEventHandler))
                {
                    contractObject.SetContractProperty(this.GetType().FullName + ".PropertyChanging", eventHandler);
                }
            }

            return eventHandler;
        }

        /// <summary>
        /// Sets the property value corresponding to the ContractObject holding this DataObject.
        /// </summary>
        /// <param name="property">Name of the property on the ContractObject.</param>
        /// <param name="value">Value of the property.</
        public void SetContractProperty(String property, Object value)
        {
            IDataObject dataObject = ConvertTo<IDataObject>();

            if (dataObject != null)
            {
                var propertyName = property.Contains(".") ? property.Substring(property.LastIndexOf('.') + 1) : property;
                
                OnPropertyChanging(propertyName, value);
                dataObject.SetContractProperty(property, value);              
                OnPropertyChanged(propertyName); 
            }
        }

        /// <summary>
        /// Gets the proxy state of a property.
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <returns>Proxy State of the property (true of false).</returns>
        public Boolean GetProxyState(String property)
        {
            IDataObject dataObject = ConvertTo<IDataObject>();

            if (dataObject != null)
            {
                return dataObject.GetProxyState(property);
            }

            return true;
        }

        /// <summary>
        /// Sets the proxy state of a property.
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <param name="proxy">Proxy state of the property (true of false).</param>
        public void SetProxyState(String property, Boolean proxy)
        {
            IDataObject dataObject = ConvertTo<IDataObject>();

            if (dataObject != null)
            {
                dataObject.SetProxyState(property, proxy);
            }
        }

        /// <summary>
        /// Gets the HashCode of the current object.
        /// </summary>
        /// <returns>HashCode of the ContractObject.</returns>
        public override int GetHashCode()
        {
            return (this.ID != null ? this.ID.GetHashCode() : 0);
        }

        /// <summary>
        /// Checks if the current object compares logically to the provided object.
        /// </summary>
        /// <param name="obj">Object which is compared to the current object.</param>
        /// <returns>The result of the comparision (true or false).</returns>
        public override bool Equals(object obj)
        {
            if ((obj != null) 
            && (obj is ContractObject))
            {
                ContractObject compareObject = obj as ContractObject;

                if (this.ID == null && compareObject.ID == null)
                {
                    return DataObjectReferenceEquals(this, compareObject);
                    
                }

                return this.ID == compareObject.ID;
            }

            return false;
        }

        /// <summary>
        /// Determines wheter the specified System.Object instances are the same instance. 
        /// </summary>
        /// <param name="object1">First object to compare.</param>
        /// <param name="object2">Second object to compare.</param>
        /// <returns>The result of the comparision (true or false).</returns>
        private bool DataObjectReferenceEquals(ContractObject object1, ContractObject object2)
        {
            if (Object.ReferenceEquals(object1, null))
            {
                return Object.ReferenceEquals(object2, null);
            }
            else if (Object.ReferenceEquals(object2, null))
            {
                return false;
            }

            return Object.ReferenceEquals(object1.ConvertTo<IDataObject>(), object2.ConvertTo<IDataObject>());
        }

        /// <summary>
        /// Overides the operator (==) for comparing two ContractObjects.
        /// </summary>
        /// <param name="object1">First object to compare.</param>
        /// <param name="object2">Second object to compare.</param>
        /// <returns>The result of the comparision (true or false).</returns>
        public static bool operator ==(ContractObject object1, ContractObject object2)
        {
            if (Object.ReferenceEquals(object1, null))
            {
                return Object.ReferenceEquals(object2, null);
            }

            return object1.Equals(object2);
        }

        /// <summary>
        /// Overides the operator (!=) for comparing two ContractObjects.
        /// </summary>
        /// <param name="object1">First object to compare.</param>
        /// <param name="object2">Second object to compare.</param>
        /// <returns>The result of the comparision (true or false).</returns>
        public static bool operator !=(ContractObject object1, ContractObject object2)
        {
            if (Object.ReferenceEquals(object2, null) && !Object.ReferenceEquals(object1, null))
            {
                return true;
            }

            if (Object.ReferenceEquals(object1, null) && !Object.ReferenceEquals(object2, null))
            {
                return true;
            }

            if (Object.ReferenceEquals(object1, null) && Object.ReferenceEquals(object2, null))
            {
                return false;
            }

            return !object1.Equals(object2);
        }

        /// <summary>
        /// Clones the current ContractObject.
        /// </summary>
        /// <param name="proxyReferencedObjects">Should objects be proxied during clonation.</param>
        /// <param name="paths">Paths to be included in the clonation.</param>
        /// <returns>Clonated ContractObject.</returns>
        public IObject Clone(bool proxyReferencedObjects, IList<string> paths = null)
        {
            throw new NotImplementedException();
        }
    }
}
