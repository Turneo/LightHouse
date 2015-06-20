using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core;
using LightHouse.Core.Caching;
using LightHouse.Core.Collections;
using LightHouse.Core.Elite.Locating;
using LightHouse.Core.Notifications;

namespace LightHouse.Core.Elite.Converting
{
    /// <summary>
    /// Officer responsible for converting different types of objects. Supports conversion between DataObjects, SurrogateObjects and ContractObjects.
    /// </summary>
    public class Converter
    {
        /// <summary>
        /// Converts any object to SurrogateObject, DataObject or ContractObject. The given dynamic type defines the resulting object.
        /// </summary>
        /// <typeparam name="T">Type of the requested object.</typeparam>
        /// <param name="convertingObject">Object that requires to be converted.</param>
        /// <param name="paths">Paths that need to be included in the conversion.</param>
        /// <returns>Converted object of type T.</returns>
        public T ConvertTo<T>(IObject convertingObject, IList<String> paths = null)
        {
            Type convertingType = convertingObject.GetType();

            if (typeof(T) == typeof(ISurrogateObject))
            {
                return (T)ConvertToDynamicSurrogateObject(convertingObject, paths);
            }
            else if(typeof(ISurrogateObject).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
            {
                return ConvertToSurrogateObject<T>(convertingObject, paths);
            }
            else if(typeof(IDataObject).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
            {
                if (convertingType == typeof(ISurrogateObject))
                {
                    return ConvertDynamicSurrogateObjectToDataObject<T>((ISurrogateObject)convertingObject, paths);
                }
                else if (typeof(ISurrogateObject).GetTypeInfo().IsAssignableFrom(convertingType.GetTypeInfo()))
                {
                    return ConvertSurrogateObjectToDataObject<T>((ISurrogateObject)convertingObject, paths);
                }
                else if (typeof(IDataObject).GetTypeInfo().IsAssignableFrom(convertingType.GetTypeInfo()))
                {
                    return ConvertDataObjectToDataObject<T>((IDataObject)convertingObject, paths);
                }
                else if (typeof(IContractObject).GetTypeInfo().IsAssignableFrom(convertingType.GetTypeInfo()))
                {
                    return ConvertContractObjectToDataObject<T>((IContractObject)convertingObject, paths);
                }
            }
            else if (typeof(IContractObject).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
            {
                if (convertingType == typeof(ISurrogateObject))
                {
                    return ConvertDynamicSurrogateObjectToContractObject<T>((ISurrogateObject)convertingObject, paths);
                }
                else if (typeof(ISurrogateObject).GetTypeInfo().IsAssignableFrom(convertingType.GetTypeInfo()))
                {
                    return ConvertSurrogateObjectToContractObject<T>((ISurrogateObject)convertingObject, paths);
                }
                else if (typeof(IDataObject).GetTypeInfo().IsAssignableFrom(convertingType.GetTypeInfo()))
                {
                    return ConvertDataObjectToContractObject<T>((IDataObject)convertingObject, paths);
                }
                else if (typeof(IContractObject).GetTypeInfo().IsAssignableFrom(convertingType.GetTypeInfo()))
                {
                    return ConvertContractObjectToContractObject<T>((IContractObject)convertingObject, paths);
                }                
            }

            return default(T);
        }

        /// <summary>
        /// Converts the provided SurrogateObject to a DataObject.
        /// </summary>
        /// <param name="surrogateObject">SurrogateObject that requires to be converted.</param>
        /// <param name="paths">Paths that need to be included in the conversion.</param>
        /// <returns>Converted DataObject of type T.</returns>
        private T ConvertSurrogateObjectToDataObject<T>(ISurrogateObject surrogateObject, IList<string> paths)
        {
            IDataObject dataObject = LightHouse.Elite.Core.Builder.GetDataObject(typeof(T), true);
            dataObject.ID = surrogateObject.ID;

            return (T)dataObject;
        }

        /// <summary>
        /// Converts the provided dynamic SurrogateObject to a DataObject.
        /// </summary>
        /// <param name="surrogateObject">Dynamic SurrogateObject that requires to be converted.</param>
        /// <param name="paths">Paths that need to be included in the conversion.</param>
        /// <returns>Converted DataObject of type T.</returns>
        private T ConvertDynamicSurrogateObjectToDataObject<T>(ISurrogateObject surrogateObject, IList<string> paths)
        {
            IDataObject dataObject = LightHouse.Elite.Core.Builder.GetDataObject(typeof(T), true);
            dataObject.ID = surrogateObject.ID;

            return (T)dataObject;
        }

        /// <summary>
        /// Converts the provided SurrogateObject to a ContractObject.
        /// </summary>
        /// <param name="surrogateObject">SurrogateObject that requires to be converted.</param>
        /// <param name="paths">Paths that need to be included in the conversion.</param>
        /// <returns>Converted ContractObject of type T.</returns>
        private T ConvertSurrogateObjectToContractObject<T>(ISurrogateObject surrogateObject, IList<string> paths)
        {
            IContractObject contractObject = default(IContractObject);

            if (typeof(T) == typeof(IContractObject))
            {
                if(surrogateObject.TypeInfo is DataTypeInfo)
                {                    
                    contractObject = LightHouse.Elite.Core.Builder.GetContractObject(((DataTypeInfo)surrogateObject.TypeInfo).ContractTypeInfos.FirstOrDefault().ContractType, true);
                }
                else if(surrogateObject.TypeInfo is ContractTypeInfo)
                {
                    contractObject = LightHouse.Elite.Core.Builder.GetContractObject(((ContractTypeInfo)surrogateObject.TypeInfo).ContractType, true);
                }
            }
            else
            {
                contractObject = LightHouse.Elite.Core.Builder.GetContractObject(typeof(T), true);                
            }

            if (contractObject != null)
            {
                contractObject.ID = surrogateObject.ID;
            }

            return (T)contractObject;
        }

        /// <summary>
        /// Converts the provided dynamic SurrogateObject to a ContractObject.
        /// </summary>
        /// <param name="surrogateObject">Dynamic SurrogateObject that requires to be converted.</param>
        /// <param name="paths">Paths that need to be included in the conversion.</param>
        /// <returns>Converted ContractObject of type T.</returns>
        private T ConvertDynamicSurrogateObjectToContractObject<T>(ISurrogateObject surrogateObject, IList<string> paths)
        {
            IContractObject contractObject = LightHouse.Elite.Core.Builder.GetContractObject(typeof(T), true);
            contractObject.ID = surrogateObject.ID;

            return (T)contractObject;
        }

        /// <summary>
        /// Converts the provided DataObject to a ContractObject.
        /// </summary>
        /// <param name="dataObject">DataObject that requires to be converted.</param>
        /// <param name="paths">Paths that need to be included in the conversion.</param>
        /// <returns>Converted ContractObject of type T.</returns>
        private T ConvertDataObjectToContractObject<T>(IDataObject dataObject, IList<string> paths)
        {
            return LightHouse.Elite.Core.Builder.GetContractObject<T>(dataObject);            
        }

        /// <summary>
        /// Converts the provided DataObject to another DataObject.
        /// </summary>
        /// <param name="dataObject">DataObject that requires to be converted.</param>
        /// <param name="paths">Paths that need to be included in the conversion.</param>
        /// <returns>Converted DataObject of type T.</returns>
        private T ConvertDataObjectToDataObject<T>(IDataObject dataObject, IList<string> paths)
        {
            IDataObject returnDataObject = LightHouse.Elite.Core.Builder.GetDataObject(typeof(T), true);
            returnDataObject.ID = dataObject.ID;

            return (T)returnDataObject;
        }

        /// <summary>
        /// Converts the provided ContractObject to another ContractObject.
        /// </summary>
        /// <param name="contractObject">ContractObject that requires to be converted.</param>
        /// <param name="paths">Paths that need to be included in the conversion.</param>
        /// <returns>Converted ContractObject of type T.</returns>
        private T ConvertContractObjectToContractObject<T>(IContractObject contractObject, IList<string> paths)
        {
            return LightHouse.Elite.Core.Builder.GetContractObject<T>(contractObject.As<IDataObject>());   
        }

        /// <summary>
        /// Converts the provided ContractObject to a DataObject.
        /// </summary>
        /// <param name="contractObject">ContractObject that requires to be converted.</param>
        /// <param name="paths">Paths that need to be included in the conversion.</param>
        /// <returns>Converted DataObject of type T.</returns>
        private T ConvertContractObjectToDataObject<T>(IContractObject contractObject, IList<string> paths)
        {
            return contractObject.As<T>();
        }        
        

        /// <summary>
        /// Converts the provided object to a specific SurrogateObject.
        /// </summary>
        /// <typeparam name="T">SurrogateObject type to be converted to.</typeparam>
        /// <param name="convertingObject">Object that requires to be converted.</param>
        /// <param name="paths">Paths that need to be included in the conversion.</param>
        /// <returns>Converted object of type T.</returns>
        private T ConvertToSurrogateObject<T>(IObject convertingObject, IList<String> paths = null)
        {
            //TODO: Keep this information in a central place and not retrieve it every time.
            IDictionary<String, String> propertyPaths = new Dictionary<String, String>();

            ISurrogateObject surrogateObject = (ISurrogateObject)LightHouse.Elite.Core.Builder.Get<T>();
            surrogateObject.ID = convertingObject.ID;

            foreach(System.Reflection.PropertyInfo propertyInfo in LightHouse.Elite.Core.Reflector.GetProperties(typeof(T)))
            {
                foreach (Attribute attribute in propertyInfo.GetCustomAttributes(true))
                {
                    if(attribute is LightHouse.Core.Attributes.Path)
                    {
                        propertyPaths[propertyInfo.Name] = ((LightHouse.Core.Attributes.Path)attribute).Value;
                    }
                }
            }

            foreach (KeyValuePair<String, String> propertyPath in propertyPaths)
            {
                surrogateObject.SetProperty(propertyPath.Key, GetSurrogateValue(LightHouse.Elite.Core.Reflector.GetPathValue(convertingObject, propertyPath.Value)));
            }

            return (T)surrogateObject;
        }

        /// <summary>
        /// Converts the provided object to a dynamic SurrogateObject.
        /// </summary>
        /// <param name="convertingObject">Object that requires to be converted.</param>
        /// <param name="paths">Paths that need to be included in the conversion.</param>
        /// <returns>Converted object of type T.</returns>
        private ISurrogateObject ConvertToDynamicSurrogateObject(IObject convertingObject, IList<String> paths = null)
        {
            LightHouse.Core.Dynamic.SurrogateObject surrogateObject = new LightHouse.Core.Dynamic.SurrogateObject()
            {
                ID = convertingObject.ID
            };

            if (paths != null)
            {
                foreach (String path in paths)
                {
                    surrogateObject.SetProperty(path, GetSurrogateValue(LightHouse.Elite.Core.Reflector.GetPathValue(convertingObject, path)));
                }
            }

            return surrogateObject;
        }

        /// <summary>
        /// Converts the SurrogateObject if it's a DataObject into a ContractObject containing the DataObject otherwise just returns the provided value.
        /// </summary>
        /// <param name="value">Value to be converted.</param>
        /// <returns>If the provided value is a DataObject, will return a converted ContractObject containing the DataObject otherwise the provided value itself.</returns>
        private Object GetSurrogateValue(Object value)
        {
            if (value != null)
            {
                if (typeof(IDataObject).GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
                {
                    DataTypeInfo dataTypeInfo = LightHouse.Elite.Core.Locator.GetDataTypeInfo(((IDataObject)value).DynamicType);

                    if((dataTypeInfo.ContractTypeInfos != null) && (dataTypeInfo.ContractTypeInfos.First() != null))
                    {
                        return LightHouse.Elite.Core.Builder.GetContractObject(dataTypeInfo.ContractTypeInfos.First().ContractType, ((IDataObject)value)).ToString();
                    }
                }
            }

            return value;
        }
    }
}
