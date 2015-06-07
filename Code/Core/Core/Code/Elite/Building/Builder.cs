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

using System.Diagnostics;

namespace LightHouse.Core.Elite.Building
{
    /// <summary>
    /// Officer responsible for building different types of objects.
    /// </summary>
    public class Builder
    {
        /// <summary>
        /// Cache for holding building related information, like a list of compiled instance activators.
        /// </summary>
        DataCache dataCache = new DataCache();

        /// <summary>
        /// Delegate for activating instances of objects.
        /// </summary>
        /// <param name="args">Arguments of the construtor.</param>
        /// <returns>A new instance of the object.</returns>
        delegate Object InstanceActivator(params object[] args);

        /// <summary>
        /// Gets a compiled instance activator for creating objects faster.
        /// </summary>
        /// <param name="objectType">Type of the object to be created.</param>
        /// <param name="definition">Constructor parameters as string.</param>
        /// <returns>An instance activator to be used for activating new instances of the object.</returns>
        private InstanceActivator GetInstanceActivator(Type objectType, String definition)
        {
            InstanceActivator instanceActivator = dataCache.Get<InstanceActivator>(objectType.FullName, definition);

            if (instanceActivator == null)
            {
                try
                {
                    instanceActivator = CompileInstanceActivator(objectType, definition);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(String.Format("Compiling an instance activator throwed an error: {0}", ex.ToString()));
                }

                dataCache.Add(objectType.FullName, instanceActivator, definition);
            }

            return instanceActivator;
        }

        /// <summary>
        /// Compiles an instance activator.
        /// </summary>
        /// <param name="objectType">Type of the object to be created.</param>
        /// <param name="definition">Constructor parameters as string.</param>
        /// <returns>An compiled instance activator to be used for activating new instances of the object.</returns>
        private static InstanceActivator CompileInstanceActivator(Type objectType, String definition)
        {
            ConstructorInfo foundConstructor = default(ConstructorInfo);

            foreach (ConstructorInfo constructorInfo in objectType.GetTypeInfo().DeclaredConstructors)
            {
                StringBuilder constructorDefinition = new StringBuilder();

                for (int i = 0; i < constructorInfo.GetParameters().Length; i++)
		        {
                    if(i > 0)
                    {
                        constructorDefinition.Append(", ");		 
                    }
                    
                    constructorDefinition.Append(constructorInfo.GetParameters()[i].ParameterType.FullName);		 
		        }

                if (definition == constructorDefinition.ToString())
                {
                    foundConstructor = constructorInfo;
                }
            }

            Type type = foundConstructor.DeclaringType;
            ParameterInfo[] paramsInfo = foundConstructor.GetParameters();

            ParameterExpression parameter = Expression.Parameter(typeof(object[]), "args");

            Expression[] argumentExpressions = new Expression[paramsInfo.Length];

            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp = Expression.ArrayIndex(parameter, index);
                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);

                argumentExpressions[i] = paramCastExp;
            }

            NewExpression newExpression = Expression.New(foundConstructor, argumentExpressions);
            LambdaExpression lambda = Expression.Lambda(typeof(InstanceActivator), newExpression, parameter);

            return (InstanceActivator)lambda.Compile();
        }

        /// <summary>
        /// Builds a ContractList of the provided type.
        /// </summary>
        /// <typeparam name="T">Type of the ContractList to be built.</typeparam>
        /// <returns>A ContractList of the provided type.</returns>
        public IContractList<T> GetContractList<T>()
        {
            IContractList<T> contractList = (IContractList<T>)GetInstanceActivator(typeof(ContractList<>).MakeGenericType(typeof(T)), "")();
            LightHouse.Elite.Core.Notifier.Notify(contractList, new ObjectCreatedEventArgs());

            return contractList;
        }

        /// <summary>
        /// Builds a DataObject of the provided type and proxy configuration.
        /// </summary>
        /// <param name="dataType">Type of the DataObject to be built.</param>
        /// <param name="proxied">Should the DataObject be proxied by default or not.</param>
        /// <returns>A DataObject of the provided type and proxy configuration.</returns>
        public IDataObject GetDataObject(Type dataType, Boolean proxied)
        {
            DataObject dataObject = (DataObject)GetInstanceActivator(dataType, "System.Boolean")(new object[] { proxied });
            LightHouse.Elite.Core.Notifier.Notify(dataObject, new ObjectCreatedEventArgs());

            return dataObject;
        }

        /// <summary>
        /// Builds a DataObject with the provided ContractObject and proxy configuration.
        /// </summary>
        /// <param name="contractObject">ContractObject to be used to built the DataObject.</param>
        /// <param name="proxied">Should the DataObject be proxied by default or not.</param>
        /// <returns>A DataObject using the provided ContractObject and proxy configuration.</returns>
        public IDataObject GetDataObject(IContractObject contractObject, Boolean proxied = false)
        {
            IDataObject dataObject = default(IDataObject);
            ContractTypeInfo contractObjectInfo = LightHouse.Elite.Core.Locator.GetContractTypeInfo(contractObject.GetType());

            if (contractObjectInfo != null)
            {
                dataObject = (IDataObject)GetInstanceActivator(contractObjectInfo.DataTypeInfos.First().DataType, String.Format("{0}, {1}", typeof(IContractObject).FullName, typeof(System.Boolean).FullName))(new object[] { contractObject, proxied });
                dataObject.DynamicType = contractObjectInfo.DataTypeInfos.First().DynamicType;

                LightHouse.Elite.Core.Notifier.Notify(dataObject, new ObjectCreatedEventArgs());
            }
            else
            {
                throw new Exception(String.Format("Couldn't create DataObject for ContractObject {0}", contractObject.GetType()));
            }

            return dataObject;
        }

        /// <summary>
        /// Builds a DataList of the provided type.
        /// </summary>
        /// <typeparam name="T">Type of the DataList to be built.</typeparam>
        /// <returns>A DataList of the provided type.</returns>
        public IDataList<T> GetDataList<T>(IDataList dataList)
        {
            Type dataObjectListType = typeof(DataList<>).MakeGenericType(typeof(T));
            IDataList<T> newDataList = (IDataList<T>)GetInstanceActivator(dataObjectListType, "")();

            foreach(DataObject dataObject in dataList)
            {
                newDataList.Add(dataObject);
            }

            return newDataList;
        }

        /// <summary>
        /// Builds a DataList of the provided type.
        /// </summary>
        /// <param name="type">Type of the DataList to be built.</param>
        /// <returns>A DataList of the provided type.</returns>
        public IDataList GetDataList(Type type)
        {
            Type dataObjectType = null;
            String dataObjectDynamicType = "";

            if (type == typeof(ContractObject) || type.GetTypeInfo().IsSubclassOf(typeof(ContractObject)))
            {
                ContractTypeInfo contractObjectInfo = LightHouse.Elite.Core.Locator.GetContractTypeInfo(type);

                foreach(DataTypeInfo dataObjectInfo in contractObjectInfo.DataTypeInfos)
                {
                    dataObjectType = dataObjectInfo.DataType;
                    dataObjectDynamicType = dataObjectInfo.DynamicType;
                }
            }
            else if (type == typeof(DataObject) || type.GetTypeInfo().IsSubclassOf(typeof(DataObject)))
            {
                dataObjectType = type;
            }

            IDataList dataList = null;
            Type dataObjectListType = typeof(DataList<>).MakeGenericType(dataObjectType);

            dataList = (IDataList)GetInstanceActivator(dataObjectListType, "")();
            dataList.DynamicType = dataObjectDynamicType;

            return dataList;
        }

        /// <summary>
        /// Builds a ContractObject of the provided type with the given DataObject.
        /// </summary>
        /// <typeparam name="T">Type of the ContractObject to be built.</typeparam>
        /// <param name="dataObject">DataObject to be used to built the ContractObject.</param>
        /// <returns>A ContractObject using the provided type and given DataObject.</returns>
        public T GetContractObject<T>(IDataObject dataObject)
        {
            if (dataObject == null)
            {
                return default(T);
            }

            return dataObject.ConvertTo<T>();
        }

        /// <summary>
        /// Builds a ContractObject of the provided type with the given DataObject.
        /// </summary>
        /// <param name="contractType">Type of the ContractObject to be built.</param>
        /// <param name="dataObject">DataObject to be used to built the ContractObject.</param>
        /// <returns>A ContractObject using the provided type and given DataObject.</returns>
        public IContractObject GetContractObject(Type contractType, IDataObject dataObject)
        {
            if (dataObject == null)
            {
                return default(ContractObject);
            }

            MethodInfo method = typeof(IObject).GetTypeInfo().GetDeclaredMethod("ConvertTo");
            MethodInfo generic = method.MakeGenericMethod(contractType);
            
            return (IContractObject)generic.Invoke(dataObject, new Object[] { null });
        }

        /// <summary>
        /// Builds a ContractObject with the provided type and proxy configuration.
        /// </summary>
        /// <param name="contractType">Type of the ContractObject to be built.</param>
        /// <param name="isProxy">Should the ContractObject be proxied by default or not.</param>
        /// <returns>A ContractObject using the provided type and proxy configuration.</returns>
        public IContractObject GetContractObject(Type contractType, Boolean isProxy)
        {
            ContractObject contractObject = (ContractObject)GetInstanceActivator(contractType, "System.Boolean")(new object[] { isProxy });
            LightHouse.Elite.Core.Notifier.Notify(contractObject, new ObjectCreatedEventArgs());

            return contractObject;
        }

        /// <summary>
        /// Builds a ContractList of the provided type and using the given DataList.
        /// </summary>
        /// <typeparam name="T">Type of the ContractList to be built.</typeparam>
        /// <param name="dataList">DataList to be used for building the ContractList.</param>
        /// <returns>A ContractList of the provided type and containing the given DataList.</returns>
        public T GetContractList<T>(IDataList dataList)
        {
            if ((typeof(T).GenericTypeArguments.Count() > 0)
            && (typeof(T) == typeof(IContractList<>).MakeGenericType(typeof(T).GenericTypeArguments[0])))
            {
                if (dataList != null)
                {
                    return (T)GetInstanceActivator(typeof(ContractList<>).MakeGenericType(typeof(T).GenericTypeArguments[0]), "LightHouse.Core.Collections.IDataList")(new object[] { dataList });
                }
            }

            return default(T);
        }

        /// <summary>
        /// Builds a ContractList of the provided type and using the given DataList.
        /// </summary>
        /// <param name="contractType">Type of the ContractList to be built.</param>
        /// <param name="dataList">DataList to be used for building the ContractList.</param>
        /// <returns>A ContractList of the provided type and containing the given DataList.</returns>
        public IContractList GetContractList(Type contractType, IDataList dataList)
        {
            if ((contractType != null))
            {
                if (dataList != null)
                {
                    return (IContractList)GetInstanceActivator(typeof(ContractList<>).MakeGenericType(contractType), "LightHouse.Core.Collections.IDataList")(new object[] { dataList });
                }
                else
                {
                    return (IContractList)GetInstanceActivator(typeof(ContractList<>).MakeGenericType(contractType), "")(new object[] { });
                }
            }

            return default(IContractList);
        }

        /// <summary>
        /// Builds an object of the provided type.
        /// </summary>
        /// <typeparam name="T">Type of the object to be built.</typeparam>
        /// <returns>An instance of the provided type.</returns>
        public T Get<T>()
        {
            T instance = (T)GetInstanceActivator(typeof(T), "")();
            LightHouse.Elite.Core.Notifier.Notify(instance, new ObjectCreatedEventArgs());

            return instance;
        }

        /// <summary>
        /// Builds an object of the provided type.
        /// </summary>
        /// <param name="type">Type of the object to be built.</param>
        /// <returns>An instance of the provided type.</returns>
        public Object Get(Type type)
        {
            Object instance = GetInstanceActivator(type, "")();
            LightHouse.Elite.Core.Notifier.Notify(instance, new ObjectCreatedEventArgs());

            return instance;
        }

        /// <summary>
        /// Builds an object of the provided type.
        /// </summary>
        /// <param name="fullName">Type as FullName of the object to be built.</param>
        /// <returns>An instance of the provided type.</returns>
        public Object Get(String fullName)
        {
            return Get(LightHouse.Elite.Core.Locator.GetType(fullName));
        }
    }
}
