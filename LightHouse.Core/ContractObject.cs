using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using LightHouse.Core.Localization;
using LightHouse.Core.Model;

namespace LightHouse.Core
{
    /// <summary>
    /// Represents the base class of all contract objects. A contract is similar to an interface but provides
    /// several key improvements (more details in the knowledge pages) that allows LightHouse to be a modular 
    /// and composable framework.
    /// </summary>
    [DataContract]
    public abstract class ContractObject : INotifyPropertyChanged, IComparable
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Holds the data object which is wrapped by this contract object (in this case the contractObject field
        /// will be empty). 
        /// </summary>
        protected DataObject dataObject;

        /// <summary>
        /// Holds the contract object which is wrapped by this contract object (in this case the dataObject field
        /// will be empty). 
        /// </summary>
        protected ContractObject contractObject;

        /// <summary>
        /// Gets or sets the unique identifier of the the contract object.
        /// </summary>
        [DataMember]
        public virtual String ID
        {
            get { return GetContractProperty<String>(String.Format("{0}.{1}",MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name.Substring(4))); }
            set { SetContractProperty(String.Format("{0}.{1}",MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name.Substring(4)), value); }
        }

        /// <summary>
        /// Constructs a contract object without a specific data object.
        /// </summary>
        public ContractObject()
        {
            dataObject = LightHouse.Base.Core.Operator.GetDataObject(this);
        }

        /// <summary>
        /// Constructs a contract object wrapping a specific data object.
        /// </summary>
        /// <param name="dataObject">Data object that is wrapped by this contract object.</param>
        public ContractObject(DataObject dataObject)
        {
            this.dataObject = dataObject;
        }

        /// <summary>
        /// Constructs a contract object wrapping a specific contract object.
        /// </summary>
        /// <param name="dataObject">Contract object that is wrapped by this contract object.</param>
        public ContractObject(ContractObject contractObject)
        {
            this.contractObject = contractObject;
        }

        /// <summary>
        /// Gets the wrapped data object or data object of the wrapped contract object.
        /// </summary>
        /// <returns>Wrapped data object or data object of the wrapped contract object.</returns>
        public virtual DataObject GetDataObject()
        {
            if (dataObject != null)
            {
                return dataObject;
            }
            else
            {
                if (contractObject != null)
                {
                    contractObject.GetDataObject();
                }
            }

            return default(DataObject);
        }

        /// <summary>
        /// Is raised when the value of a property has been changed. 
        /// </summary>
        /// <param name="name">Name of the property that has been changed</param>
        protected void OnPropertyChanged(String name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Gets the value of a property from this contract object (faster than reflection).
        /// </summary>
        /// <typeparam name="T">Type of property to be returned.</typeparam>
        /// <param name="property">Name of property to be returned.</param>
        /// <returns>Value of the property.</returns>
        public T GetProperty<T>(String property)
        {
            //TODO: Review if it's a good idea to set this as an abstract so that all inherited classes requiere to override it 
            //(only if the property is not found it should call this base class)

            //TODO: Create Generic GetPropertyValue<T> on Core.Operator and use it here.
            return (T)LightHouse.Base.Core.Operator.GetPropertyValue(property, this);
        }

        /// <summary>
        /// Sets the value of a property from the this contract object faster than reflection).
        /// </summary>
        /// <param name="property">Name of property.</param>
        /// <param name="value">New value for the property.</param>
        public void SetProperty(String property, Object value)
        {
            //TODO: Review if it's a good idea to set this as an abstract so that all inherited classes requiere to override it 
            //(only if the property is not found it should call this base class)

            LightHouse.Base.Core.Operator.SetPropertyValue(property, this, value);
        }

        /// <summary>
        /// Gets the property value from the wrapped element (either data object or contract object).
        /// </summary>
        /// <typeparam name="T">Type of property to be returned.</typeparam>
        /// <param name="property">Name of property to be returned.</param>
        /// <returns>Value of the property.</returns>
        protected T GetContractProperty<T>(String property)
        {
            DataObject dataObject = GetDataObject();

            if (dataObject != null)
            {
                return (T)dataObject.GetContractProperty<T>(property);
            }
            else if (contractObject != null)
            {
                return (T)contractObject.GetContractProperty<T>(property);
            }

            return default(T);
        }

        /// <summary>
        /// Sets the property value from the wrapped element (either data object or contract object).
        /// </summary>
        /// <param name="property">Name of the property.</param>
        /// <param name="value">New value for the property.</param>
        protected void SetContractProperty(String property, Object value)
        {
            DataObject dataObject = GetDataObject();

            if (dataObject != null)
            {
                dataObject.SetContractProperty(property, value);
                
                var propertyName = property.Contains('.') ? property.Substring(property.LastIndexOf('.') + 1) : property;
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
            DataObject dataObject = GetDataObject();

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
        /// <param name="proxy">Proxy State of the property (true of false).</param>
        public void SetProxyState(String property, Boolean proxy)
        {
            DataObject dataObject = GetDataObject();

            if (dataObject != null)
            {
                dataObject.SetProxyState(property, proxy);
            }
        }

        /// <summary>
        /// Converts the contract object to another contract object type. 
        /// The wrapped data object is passed to the new contract object..
        /// </summary>
        /// <typeparam name="T">Type of the new contract object.</typeparam>
        /// <returns>New contract object of the specified type.</returns>
        public virtual T ConvertTo<T>() where T : ContractObject
        {
            return (T)Activator.CreateInstance(typeof(T), new Object[] { this.GetDataObject() });
        }

        /// <summary>
        /// Returns an object of the requested type.
        /// 
        /// In the case the object is a category object:
        /// 
        /// .As<CategoryObject> = Returns the category object if it can be casted to the requested type.
        /// .As<EntityObject> = Returns the entity that has the category assigned if it can be casted to the requested type.
        /// 
        /// In the case the object is a not a category object:
        /// 
        /// .As<CategoryObject> = Returns the category object if the contract object has the category assigned.
        /// .As<EntityObject> = Returns the entity object if it can be casted to the requested type.       
        /// 
        /// </summary>
        /// <typeparam name="T">The type of object that is requested.</typeparam>
        /// <returns>Element object based on the given conditions.</returns>
        public virtual T As<T>() where T : ElementObject
        {
            //TODO: Review the procedures for proxy elements which where removed in the modification of the base structure.

            if (typeof(CategoryObject).IsAssignableFrom(typeof(T)))
            {
                if (this is CategoryObject)
                {
                    if (typeof(T).IsAssignableFrom(GetType()))
                    {
                        return (T)(Object)this;
                    }
                    else
                    {
                        EntityObject entityObject = default(EntityObject);

                        String propertyName = GetType().FullName.Replace(GetType().BaseType.FullName, "");

                        entityObject = GetProperty<EntityObject>(propertyName);

                        if ((entityObject != null) && (entityObject.Categories != null))
                        {
                            foreach (CategoryObject categoryObject in entityObject.Categories)
                            {
                                if (typeof(T).IsAssignableFrom(categoryObject.GetType()))
                                {
                                    return (T)(Object)categoryObject;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (this is ElementObject)
                    {
                        if (((ElementObject)this).Categories != null)
                        {
                            foreach (CategoryObject categoryObject in ((ElementObject)this).Categories)
                            {
                                if (typeof(T).IsAssignableFrom(categoryObject.GetType()))
                                {
                                    return (T)(Object)categoryObject;
                                }
                            }
                        }
                    }
                }

                return default(T);
            }
            else
            {
                if (typeof(T).IsAssignableFrom(GetType()))
                {
                    return (T)(Object)this;
                }
                else
                {
                    if ((typeof(EntityObject).IsAssignableFrom(typeof(T))) && (this is CategoryObject))
                    {
                        PropertyInfo propertyInfo = GetType().GetProperty(typeof(T).Name);
                        if (propertyInfo != null)
                        {
                            return (T)propertyInfo.GetValue(this, new Object[] { });
                        }
                    }
                }
            }

            return default(T);
        }

        /// <summary>
        /// Checks if the object is of the requested type.
        /// 
        /// In the case the object is a category object:
        /// 
        /// .As<CategoryObject> = Checks if the category object can be casted to the requested type.
        /// .As<EntityObject> = Checks if the entity has the category assigned and if it can be casted to the requested type.
        /// 
        /// In the case the object is a not a category object:
        /// 
        /// .As<CategoryObject> = Checks if the contract object has the category assigned.
        /// .As<EntityObject> = Checks if the entity object can be casted to the requested type.
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual Boolean Is<T>()
        {
            //TODO: Review the procedures for proxy elements which where removed in the modification of the base structure.

            if ((typeof(T) != typeof(CategoryObject)) && (typeof(CategoryObject).IsAssignableFrom(typeof(T))) && (!typeof(CategoryObject).IsAssignableFrom(GetType())))
            {
                if (typeof(T).BaseType == typeof(CategoryObject))
                {
                    return GetProperty<Boolean>("Is" + typeof(T).Name);
                }
                else
                {
                    return GetProperty<Boolean>("Is" + typeof(T).BaseType.Name);
                }
            }
            else
            {
                if (typeof(T).IsAssignableFrom(GetType()))
                {
                    return true;
                }
                else
                {
                    if ((typeof(EntityObject).IsAssignableFrom(typeof(T))) && (this is CategoryObject))
                    {
                        PropertyInfo propertyInfo = GetType().GetProperty(typeof(T).Name);
                        if (propertyInfo != null)
                        {
                            return true;
                        }
                    }
                }                
            }

            return false;
        }

        /// <summary>
        /// Returns the string representing the contract object. If not override will return the name property.
        /// Should be overriden by each object if another String represents this contract object.
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            if ((this.GetType().GetProperty("Name") != null)
            && (this.GetProperty<LocalString>("Name") != null))
            {
                return this.GetProperty<LocalString>("Name").ToString();
            }

            return base.ToString();
        }

        /// <summary>
        /// Gets the HashCode of the current object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (this.ID != null ? this.ID.GetHashCode() : 0);
        }

        /// <summary>
        /// Clones the current contract object.
        /// </summary>
        /// <returns>Cloned contract object.</returns>
        public virtual ContractObject Clone()
        {
            Type clonedType;
            ContractObject implementation;

            clonedType = GetType();
            implementation = this;

            ElementObject clonedObject = (ElementObject)LightHouse.Base.Core.Operator.CreateInstance(clonedType);
            foreach (FieldInfo fieldInfo in LightHouse.Base.Core.Operator.GetFields(implementation.GetType(), true))
            {
                Object fieldValue = fieldInfo.GetValue(implementation);

                if (fieldValue == null || fieldValue.GetType() == typeof(System.Byte[]))
                {
                    continue;
                }

                if (fieldInfo.FieldType.IsSubclassOf(typeof(ValueObject)))
                {
                    fieldInfo.SetValue(clonedObject, fieldValue);
                }
                else if (fieldInfo.FieldType.IsSubclassOf(typeof(EntityObject)))
                {
                    fieldInfo.SetValue(clonedObject, fieldValue);
                }
                else if (fieldInfo.FieldType.IsSubclassOf(typeof(EntityObject)))
                {
                    fieldInfo.SetValue(clonedObject, fieldValue);
                }
                else if (fieldValue is IList)
                {
                    IList newList = null;

                    if (newList == null)
                    {
                        newList = (IList)LightHouse.Base.Core.Operator.CreateInstance(fieldValue.GetType());
                    }

                    for (int i = 0; i < ((IList)fieldValue).Count; i++)
                    {
                        newList.Add(((IList)fieldValue)[i]);
                    }

                    fieldInfo.SetValue(clonedObject, newList);
                }
                else
                {
                    fieldInfo.SetValue(clonedObject, fieldValue);
                }
            }

            return clonedObject;
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
                    return LightHouse.Core.Extensions.CoreHelper.DataObjectReferenceEquals(this, compareObject);
                }

                return this.ID == compareObject.ID;
            }

            return false;
        }

        /// <summary>
        /// Compares the current object to the provided object using the provided comparision service.
        /// </summary>
        /// <param name="obj">Object which is compared to the current object.</param>
        /// <returns>The result of the comparision (Int32).</returns>
        public int CompareTo(object obj)
        {
            Element element = LightHouse.Base.Storage.Operator.Get<Element>(this.GetType().FullName);

            if((element != null) && (element.Comparer != null))
            {
                dynamic results = LightHouse.Base.Services.Operator.Invoke(element.Comparer, new List<Object>() { this, obj });
                return (int)results.Result;
            }

            return -1;
        }

        /// <summary>
        /// Overides the operator (==) for comparing two contract objects.
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
        /// Overides the operator (!=) for comparing two contract objects.
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
        /// Represents the model of this contract object.
        /// </summary>
        private Element model;

        /// <summary>
        /// Gets the model of this contract object. Cached on the model field.
        /// </summary>
        /// <returns>Model of the object.</returns>
        public virtual Element GetModel()
        {
            if ((model == null) 
            && (LightHouse.Base.Storage.Operator != null))
            {
                if (this.Is<CategoryObject>())
                {
                    if (this.GetType().BaseType != null)
                    {
                        if (this.GetType().BaseType == typeof(CategoryObject))
                        {
                            model = LightHouse.Base.Storage.Operator.Get<Category>(this.GetType().FullName);
                        }
                        else
                        {
                            model = LightHouse.Base.Storage.Operator.Get<Category>(this.GetType().BaseType.FullName);
                        }
                    }
                    else
                    {
                        model = LightHouse.Base.Storage.Operator.Get<Category>(this.GetType().FullName);
                    }
                }
                else if (this.Is<EntityObject>())
                {
                    model = LightHouse.Base.Storage.Operator.Get<Entity>(this.GetType().FullName);
                }
                else
                {
                    model = LightHouse.Base.Storage.Operator.Get<Element>(this.GetType().FullName);
                }
            }

            return model;
        }
    }
}
