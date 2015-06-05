using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using LightHouse.Core.Caching;
using LightHouse.Core.Collections;
using LightHouse.Core.Notifications;
using LightHouse.Core.Elite.Locating;
using LightHouse.Core.Proxies;

namespace LightHouse.Core
{
    /// <summary>
    /// Represents the base class of all DataObject's. A DataObject holds the data of the application.
    public class DataObject : IDataObject
    {
        /// <summary>
        /// Initializes a new instance with no specific wrapper and proxy state false.
        /// </summary>
        public DataObject()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of DataObject class using a specified contractobject and setting the Proxystate to false.
        /// </summary>
        /// <param name="contractObject">Contract object to wrap the data object</param>
        public DataObject(IContractObject contractObject)
            : this(contractObject, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of DataObject class using a specified proxy state.
        /// </summary>
        /// <param name="isProxy">Boolean setting the proxy state of the data object</param>
        public DataObject(Boolean isProxy)
            : this(null, isProxy)
        {
        }

        /// <summary>
        /// Initializes a new instance setting the given proxy state and wrapper (contract object).
        /// </summary>
        /// <param name="isProxy">Sets inner proxing informations.</param>
        /// <param name="contractObject"></param>
        public DataObject(IContractObject contractObject, Boolean isProxy)
        {
            defaultIsProxy = isProxy;

            if (GetType() == typeof(DataObject))
            {
                proxyInformation = new DataObjectProxyInformation();

                SetProxyInformation(isProxy);
            }

            if (contractObject != null)
            {
                AddContractObjectToCache(contractObject.GetType().FullName, contractObject);
            }

            //LightHouse.Elite.Core.Notifier.AddHandler(UpdateProperty_Handler);
        }

        /// <summary>
        /// Keeps the proxy information of the DataObject.
        /// </summary>
        public class DataObjectProxyInformation : ProxyInformation
        {
        }

        /// <summary>
        /// Actual proxy information of the current DataObject.
        /// </summary>
        protected ProxyInformation proxyInformation;

        /// <summary>
        /// Returns the DataObjectProxyInformation containing the current proxy states
        /// /// </summary>
        /// <returns>DataObjectProxyInformation containing the current proxy states</returns>
        private DataObjectProxyInformation GetProxyInformation()
        {
            return (DataObjectProxyInformation)proxyInformation;
        }

        /// <summary>
        /// Set the proxy information. Currently, this method is empty
        /// </summary>
        /// <param name="isProxy">Boolean describing the new proxy state</param>
        protected virtual void SetProxyInformation(Boolean isProxy)
        {
        }

        /// <summary>
        /// Cache that holds the information if the DataObject usage is dynamic.
        /// </summary>
        protected DataCache dynamicCache = new DataCache();
        
        /// <summary>
        /// Adds the ContractObject that hold this DataObject to the cache.
        /// </summary>
        /// <param name="name">Name of the ContractObject.</param>
        /// <param name="contractObject">ContractObject to be added to the cache.</param>
        protected void AddContractObjectToCache(String name, IContractObject contractObject)
        {
            dynamicCache.Add(name, contractObject, "ContractObjects");
        }

        /// <summary>
        /// Gets a ContractObject that holds this DataObject from the cachen.
        /// </summary>
        /// <param name="name">Name of the ContractObject.</param>
        /// <returns>ContractObject contained in the cache.</returns>
        protected IContractObject GetContractObjectFromCache(String name)
        {
            return (IContractObject)dynamicCache.Get(name, "ContractObjects");
        }
        
        /// <summary>
        /// Adds property value to cache.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        protected void AddPropertyToCache(String name, Object value)
        {
            dynamicCache.Add(name, value, "Properties");
        }

        /// <summary>
        /// Gets property value from the cache.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <returns>Value of the property.</returns>
        protected Object GetPropertyFromCache(String name)
        {
            return dynamicCache.Get(name, "Properties");
        }

        /// <summary>
        /// Adds the proxy state to the cache.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <param name="isProxy">Proxy state of the property.</param>
        protected void AddProxyToCache(String name, Boolean isProxy)
        {
            dynamicCache.Add(name, isProxy, "Proxies");
        }

        /// <summary>
        /// Gets the proxy state from the cache.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        /// <returns>Proxy state of the property</returns>
        protected Boolean GetProxyFromCache(String name)
        {
            Object proxy = dynamicCache.Get(name, "Proxies");

            if (proxy == null)
            {
                proxy = defaultIsProxy;
            }

            return (Boolean)proxy;
        }

        /// <summary>
        /// Gets all the ContractObject's that hold this DataObject.
        /// </summary>
        /// <returns>Returns the ContractObject's in a KeyValuePair (Name and ContractObject) combination.</returns>
        protected IEnumerable<KeyValuePair<String, Object>> GetObjectsInContractObjects()
        {
            return dynamicCache.GetObjectsInRegion("ContractObjects");
        }

        /// <summary>
        /// Gets all the property values that hold this DataObject.
        /// </summary>
        /// <returns>Returns the property values in a KeyValuePair (Name and Value) combination.</returns>
        protected IEnumerable<KeyValuePair<String, Object>> GetObjectsInProperties()
        {
            return dynamicCache.GetObjectsInRegion("Properties");
        }

        /// <summary>
        /// Gets all the proxy states that hold this DataObject.
        /// </summary>
        /// <returns>Returns the proxy states in a KeyValuePair (Name and Value) combination.</returns>
        protected IEnumerable<KeyValuePair<String, Object>> GetObjectsInProxies()
        {
            return dynamicCache.GetObjectsInRegion("Proxies");
        }

        /// <summary>
        /// Occurs when a property of the ContractObject has changed.
        /// </summary>
        public PropertyChangedEventHandler ContractPropertyChanged { get; set; }

        /// <summary>
        /// Occurs when a property of the ContractObject is changing.
        /// </summary>
        public PropertyChangingEventHandler ContractPropertyChanging { get; set; }

        /// <summary>
        /// Stores the default proxy status for the properties.
        /// </summary>
        [DataMember]
        protected Boolean defaultIsProxy = false;

        /// <summary>
        /// Unique identifier of the DataObject.
        /// </summary>      
        private String id;

        /// <summary>
        /// Unique identifier of the DataObject.
        /// </summary>
        public virtual String ID
        {
            get { return id; }
            set 
            {
                if (value != id)
                {
                    var oldValue = id;
                    OnPropertyChanging("ID", value, oldValue);
                    id = value;
                    OnPropertyChanged("ID", value, oldValue);
                }
            }
        }

        /// <summary>
        /// Information about the Type if the DataObject has been generated dynamically.
        /// </summary>
        public String DynamicType { get; set; }

        /// <summary>
        /// Gets the value of the provided property.
        /// </summary>
        /// <typeparam name="T">Type of the property value.</typeparam>
        /// <param name="property">Name of the property.</param>
        /// <returns>Value of the property.</returns>
        public virtual T GetProperty<T>(String property)
        {
            switch (property)
            {
                case "ID":

                    return (T)(Object)this.ID;

                default:

                    Object propertyValue = GetPropertyFromCache(property);
                    return (T)propertyValue;
            }
        }

        /// <summary>
        /// Get the properties (static and dynamic) of the this object.
        /// </summary>
        /// <returns>List of property names.</returns>
        public virtual IList<String> GetProperties()
        {
            IList<String> properties = new List<String>();

            foreach(PropertyInfo propertyInfo in LightHouse.Elite.Core.Reflector.GetProperties(GetType()))
            {
                properties.Add(propertyInfo.Name);
            }

            foreach (KeyValuePair<String, Object> dynamicProperty in GetObjectsInProperties())
            {
                if (!properties.Contains(dynamicProperty.Key))
                {
                    properties.Add(dynamicProperty.Key);
                }
            }

            return properties;
        }

        /// <summary>
        /// Sets the value of the provided property.
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        public virtual void SetProperty(String property, Object value)
        {
            switch (property)
            {
                case "ID":

                    this.ID = (String)value;
                    AddProxyToCache(property, false);
                    break;

                default:

                    var propertyValue = GetProperty<Object>(property);
                    OnPropertyChanging(property, value, propertyValue);

                    AddPropertyToCache(property, value);
                    AddProxyToCache(property, false);

                    OnPropertyChanged(property, value, propertyValue);
                    break;
            }
            
        }

        /// <summary>
        /// Triggers a OnPropertyChanged event.
        /// </summary>
        /// <param name="name">Name of the property that has changed.</param>
        /// <param name="newValue">New value of the property.</param>
        /// <param name="oldValue">Old value of the property.</param>
        protected void OnPropertyChanged(String name, Object newValue, Object oldValue)
        {
            if ((oldValue == null && newValue != null) || (oldValue != null && !oldValue.Equals(newValue)))
            {
                LightHouse.Elite.Core.Notifier.Notify(this, new PropertyChangedEventArgs(name));

                foreach (KeyValuePair<String, Object> pair in GetObjectsInContractObjects())
                {
                    LightHouse.Elite.Core.Notifier.Notify(pair.Value, new PropertyChangedEventArgs(name));
                }
            }
        }

        /// <summary>
        /// Triggers a OnPropertyChanging event.
        /// </summary>
        /// <param name="name">Name of the property that is changing.</param>
        /// <param name="newValue">New value of the property.</param>
        /// <param name="oldValue">Old value of the property.</param>
        protected void OnPropertyChanging(String name, Object newValue, Object oldValue)
        {
            if ((oldValue == null && newValue != null) || (oldValue != null && !oldValue.Equals(newValue)))
            {
                LightHouse.Elite.Core.Notifier.Notify(this, new PropertyChangingEventArgs(name, newValue, oldValue));

                foreach (KeyValuePair<String, Object> pair in GetObjectsInContractObjects())
                {
                    LightHouse.Elite.Core.Notifier.Notify(pair.Value, new PropertyChangingEventArgs(name, newValue, oldValue));
                }
            }
        }

        /// <summary>
        /// Checks if the DataObject holds references to provided types.
        /// </summary>
        /// <param name="types">Types to be checked.</param>
        /// <returns>If the DataObject holds a reference to the provided types.</returns>
        public virtual Boolean IsHoldingContractObjectTypes(Type[] types)
        {
            foreach (KeyValuePair<String, Object> pair in GetObjectsInContractObjects())
            {
                Type existingContractObjectType = pair.Value.GetType();
        
                foreach (var type in types)
                {
                    if (type.GetTypeInfo().IsAssignableFrom(existingContractObjectType.GetTypeInfo()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Converts the current DataObject to the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the requested object.</typeparam>
        /// <param name="paths">Paths that need to be included in the conversion.</param>
        /// <returns>Converted object of type T.</returns>
        public T ConvertTo<T>(IList<string> paths = null)
        {
            if (typeof(IContractObject).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
            {
                //TODO: Improve performance of ContractList by holding the ContractObject already

                IContractObject contractObject = GetContractObjectFromCache(typeof(T).FullName);

                if ((contractObject != null))
                {
                    contractObject = (ContractObject)(Object)contractObject;
                }
                else
                {
                    IContractObject existingContractObject = null;

                    foreach (KeyValuePair<String, Object> pair in GetObjectsInContractObjects())
                    {
                        Type existingContractObjectType = pair.Value.GetType();
                        if (typeof(T).GetTypeInfo().IsAssignableFrom(existingContractObjectType.GetTypeInfo()))
                        {
                            existingContractObject = (ContractObject)pair.Value;
                            break;
                        }
                    }

                    if (existingContractObject != null)
                    {
                        AddContractObjectToCache(typeof(T).FullName, existingContractObject);
                        return (T)existingContractObject;
                    }

                    DataTypeInfo dataObjectInfo = LightHouse.Elite.Core.Locator.GetDataTypeInfo(this.GetDataType());

                    if (dataObjectInfo.ContractTypeInfos != null)
                    {
                        foreach (ContractTypeInfo contractObjectInfo in dataObjectInfo.ContractTypeInfos)
                        {
                            if (typeof(T).GetTypeInfo().IsAssignableFrom(contractObjectInfo.ContractType.GetTypeInfo()))
                            {
                                contractObject = (ContractObject)Activator.CreateInstance(contractObjectInfo.ContractType, new Object[] { this });
                                AddContractObjectToCache(contractObjectInfo.ContractType.FullName, contractObject);

                                return (T)contractObject;
                            }
                        }
                    }

                    if (!typeof(T).GetTypeInfo().IsAbstract)
                    {
                        contractObject = (ContractObject)Activator.CreateInstance(typeof(T), new Object[] { this });
                        AddContractObjectToCache(typeof(T).FullName, contractObject);
                    }

                }

                return (T)contractObject;
            }
            else if (typeof(ISurrogateObject).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
            {
                return (T)LightHouse.Elite.Core.Converter.ConvertTo<T>(this, paths);
            }

            return default(T);
        }

        /// <summary>
        /// Gets the property value corresponding to the ContractObject holding this DataObject.
        /// </summary>
        /// <typeparam name="T">Type of the value to be returned.</typeparam>
        /// <param name="property">Name of the property on the ContractObject.</param>
        /// <returns>Value of the requested property.</returns>
        public virtual T GetContractProperty<T>(String property)
        {
            switch (property)
            {
                case "LightHouse.Core.ContractObject.ID":
                    return (T)(Object)this.ID;
                case "ID":
                    return (T)(Object)this.ID;
                default: 
                    return GetDynamicProperty<T>(property);
            }
        }

        /// <summary>
        /// Sets the property value corresponding to the ContractObject holding this DataObject.
        /// </summary>
        /// <param name="property">Name of the property on the ContractObject.</param>
        /// <param name="value">Value of the property.</param>
        public virtual void SetContractProperty(String property, Object value)
        {
            switch (property)
            {
                case "LightHouse.Core.ContractObject.ID":
                    this.ID = (String)value;
                    break;
                case "ID":
                    this.ID = (String)value;
                    break;
                default:                

                    var propertyName = property.Contains(".") ? property.Substring(property.LastIndexOf('.') + 1) : property;
                    
                    Object propertyValue = GetPropertyFromCache(propertyName);
                    OnPropertyChanging(property, value, propertyValue);

                    SetDynamicProperty(propertyName, value);
                    AddProxyToCache(propertyName, false);

                    OnPropertyChanged(property, value, propertyValue);

                    break;
            }
            
        }

        /// <summary>
        /// Gets the data type, either dynamic or the real type.
        /// </summary>
        /// <returns>The data type of the DataObject.</returns>
        public virtual String GetDataType()
        {
            if(!String.IsNullOrEmpty(this.DynamicType))
            {
                return this.DynamicType;
            }
            else
            {
                return this.GetType().FullName;
            }
        }

        /// <summary>
        /// Gets the proxy state of the provided property.
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <returns>State of the provided property.</returns>
        public virtual Boolean GetProxyState(String property)
        {
            PropertyInfo propertyInfo = null;
            try
            {
                propertyInfo = this.GetProxyInformation().GetType().GetRuntimeProperty(property);
            }
            catch (AmbiguousMatchException)
            {
                propertyInfo = this.GetProxyInformation().GetType().GetRuntimeProperties().Where(p => p.Name == property).FirstOrDefault();
            }

            if (propertyInfo != null)
            {
                return (Boolean)propertyInfo.GetValue(this.GetProxyInformation());
            }

            Boolean proxy = GetProxyFromCache(property);

            return proxy;
        }

        /// <summary>
        /// Sets the proxy state of the provided property.
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <param name="proxy">Proxy state of the property.</param>
        public virtual void SetProxyState(String property, Boolean proxy)
        {
            PropertyInfo propertyInfo = this.GetProxyInformation().GetType().GetRuntimeProperty(property);

            if (propertyInfo != null)
            {
                propertyInfo.SetValue(this.GetProxyInformation(), proxy);
            }
            else
            {
                AddProxyToCache(property, proxy);
            }
        }

        /// <summary>
        /// Sets the dynamic property value for the given property name.
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        private void SetDynamicProperty(String property, Object value)
        {
            String propertyName = property.Contains(".") ? property.Substring(property.LastIndexOf('.') + 1) : property;

            if (value != null)
            {
                if ((value.GetType().GenericTypeArguments.Length > 0)
                && (typeof(ContractObject).GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo().GenericTypeArguments[0].GetTypeInfo()))
                && (typeof(IContractList<>).MakeGenericType(value.GetType().GenericTypeArguments[0]).GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo())))
                {
                    var propertyValue = ((IContractList)value).ToDataList();
                    AddPropertyToCache(propertyName, propertyValue);
                }
                else if (typeof(ContractObject).GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
                {
                    var propertyValue = ((ContractObject)value).ConvertTo<IDataObject>();
                    AddPropertyToCache(propertyName, propertyValue);
                }
                else
                {
                    AddPropertyToCache(propertyName, value);
                }
            }
            else
            {
                AddPropertyToCache(propertyName, value);
            }
        }


        /// <summary>
        /// Gets the property value of the given property. If the property is not found withing the dynamic properties, the default value will be returned (default(T)).
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="property">Name of the property.</param>
        /// <returns>Value of the given property as the given type. If the property is not found, default(T) is returned.</returns>
        private T GetDynamicProperty<T>(String property)
        {
            String propertyName = property.Contains(".") ? property.Substring(property.LastIndexOf('.') + 1) : property;

            T returnValue = default(T);
            
            Boolean isProxy = GetProxyFromCache(propertyName);

            if (isProxy)
            {
                IList<String> paths = new List<String>();
                paths.Add(propertyName);

                LightHouse.Elite.Core.Loader.Load(this, GetProxyInformation(), paths);
                AddProxyToCache(propertyName, false);
            }

            Object propertyValue = GetPropertyFromCache(propertyName);

            if (propertyValue != null)
            {
                if ((propertyValue.GetType().GenericTypeArguments.Length > 0)
                && (typeof(DataObject).GetTypeInfo().IsAssignableFrom(propertyValue.GetType().GenericTypeArguments[0].GetTypeInfo()))
                && (typeof(IDataList<>).MakeGenericType(propertyValue.GetType().GenericTypeArguments[0]).GetTypeInfo().IsAssignableFrom(propertyValue.GetType().GetTypeInfo())))
                {
                    if (typeof(T).GenericTypeArguments.Length == 0)
                    {
                        returnValue = (T)(Object)LightHouse.Elite.Core.Builder.GetContractList(typeof(ContractObject), ((IDataList)propertyValue));
                    }
                    else
                    {
                        returnValue = (T)(Object)LightHouse.Elite.Core.Builder.GetContractList(typeof(T).GenericTypeArguments[0], ((IDataList)propertyValue));
                    }
                }
                else if (typeof(IDataObject).GetTypeInfo().IsAssignableFrom(propertyValue.GetType().GetTypeInfo()))
                {
                    IDataObject dataObject = (IDataObject)propertyValue;

                    if (typeof(IDataObject).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
                    {
                        returnValue = (T)(Object)dataObject;
                    }
                    else
                    {
                        returnValue = dataObject.ConvertTo<T>();
                    }
                }
                else if(typeof(T) == typeof(Int32))
                {
                    returnValue = (T)(Object)Convert.ToInt32((Object)propertyValue);
                }
                else if(typeof(T) == typeof(Int64))
                {
                    returnValue = (T)(Object)Convert.ToInt64((Object)propertyValue);
                }
                else
                {
                    returnValue = (T)propertyValue;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Clones the current DataObject.
        /// </summary>
        /// <param name="proxyReferencedObjects">Should objects be proxied during clonation.</param>
        /// <param name="paths">Paths to be included in the clonation.</param>
        /// <returns>Clonated DataObject.</returns>
        public IObject Clone(Boolean proxyReferencedObjects, IList<String> paths = null)
        {
            //TODO: Implement Version without proxyReferencedObjects = currently all are proxied
            return LightHouse.Elite.Core.Cloner.CloneDataObject(this, paths, proxyReferencedObjects);
        }

        /// <summary>
        /// Handler for triggering the changes in properties.
        /// </summary>
        /// <param name="sender">Sender that tirggers the update.</param>
        /// <param name="args">Arguments of the update.</param>
        private void UpdateProperty_Handler(object sender, UpdateObjectPropertyEventArgs args)
        {
            if (this.ID != null && !Object.ReferenceEquals(this, args.Source) && this.GetType() == args.Source.GetType())
            {
                DataObject dataObject = (DataObject)args.Source;

                if (this.ID == dataObject.ID)
                {
                    var value = dataObject.GetProperty<object>(args.Property);
                    var oldValue = this.GetProperty<object>(args.Property);
                    if ((oldValue == null && value != null) || (oldValue != null && !oldValue.Equals(value)))
                    {
                        this.SetProperty(args.Property, value);
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when a property of the SurrogateObject has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when a property of the SurrogateObject is changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;
   }
}
