using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core;
using LightHouse.Core.Caching;
using LightHouse.Core.Collections;

namespace LightHouse.Core.Elite.Locating
{
    /// <summary>
    /// Officer responsible for locating types and type informations.
    /// </summary>
    public class Locator
    {
        /// <summary>
        /// Defines it the type informations are being loaded.
        /// </summary>
        private Boolean infosLoading = false;

        /// <summary>
        /// Defines it the type informations have been loaded.
        /// </summary>
        private Boolean infosLoaded = false;

        /// <summary>
        /// Defines it the known types are being loaded.
        /// </summary>
        private Boolean knownTypesLoading = false;

        /// <summary>
        /// Defines it the known types have been loaded.
        /// </summary>
        private Boolean knownTypesLoaded = false;

        /// <summary>
        /// Thread locker for blocking multiple threads for doing the same work.
        /// </summary>
        private static readonly Object infosLocker = new Object();

        /// <summary>
        /// Cache holding the type informations.
        /// </summary>
        private DataCache dataCache = new DataCache();

        /// <summary>
        /// Loads the locator officer.
        /// </summary>
        public void Load()
        {
            LoadKnownTypes();
        }

        /// <summary>
        /// Gets a type by it's full name.
        /// </summary>
        /// <param name="fullName">Full name of the type.</param>
        /// <returns>Type matching the provided full name.</returns>
        public Type GetType(String fullName)
        {
            var loadKnowingTypesTask = LoadKnownTypes();

            if (loadKnowingTypesTask.Result)
            {
                return dataCache.Get<Type>(fullName, "KnownTypes");
            }
            else
            {
                return default(Type);
            }
        }

        /// <summary>
        /// Loads all type informations, by analyzing the classes in the assemblies.
        /// </summary>
        private void LoadInfos()
        {
            lock (infosLocker)
            {
                if (!infosLoading && !infosLoaded)
                {
                    infosLoading = true;

                    LoadKnownTypes();

                    foreach (KeyValuePair<String, Object> knownTypeValuePair in dataCache.GetObjectsInRegion("KnownTypes"))
                    {
                        Type knownType = (Type)knownTypeValuePair.Value;

                        if (typeof(DataObject).GetTypeInfo().IsAssignableFrom(knownType.GetTypeInfo()) && (typeof(DataObject) != knownType))
                        {
                            AddInfos(null, knownType, knownType.GetTypeInfo().BaseType, "", "", true);

                            foreach (LightHouse.Core.Attributes.Contract attribute in knownType.GetTypeInfo().GetCustomAttributes(typeof(LightHouse.Core.Attributes.Contract), false))
                            {
                                AddInfos(attribute.Type, knownType, knownType.GetTypeInfo().BaseType, "", "", true);
                            }
                        }
                    }

                    foreach (KeyValuePair<String, Object> knownTypeValuePair in dataCache.GetObjectsInRegion("KnownTypes"))
                    {
                        Type knownType = (Type)knownTypeValuePair.Value;

                        if ((typeof(ContractObject).GetTypeInfo().IsAssignableFrom(knownType.GetTypeInfo())) && (dataCache.Get(knownType.FullName, "DataTypeInfos") == null))
                        {
                            String dynamicBaseType = GetDynamicBaseType(knownType);
                            Type dataType = GetDataObjectDataType(knownType.GetTypeInfo().BaseType);
                            Type baseType = null;

                            if (String.IsNullOrEmpty(dynamicBaseType))
                            {
                                baseType = GetDataObjectBaseType(knownType.GetTypeInfo().BaseType);
                            }

                            AddInfos(knownType, dataType, baseType, String.Format("{0}Data", knownType.FullName), dynamicBaseType, false);
                        }
                    }

                    foreach (KeyValuePair<String, Object> dataTypeInfoValuePair in dataCache.GetObjectsInRegion("DataTypeInfos"))
                    {
                        DataTypeInfo dataTypeInfo = (DataTypeInfo)dataTypeInfoValuePair.Value;

                        if ((dataTypeInfo.ContractTypeInfos != null) && (dataTypeInfo.ContractTypeInfos.Count > 0))
                        {
                            foreach (System.Reflection.PropertyInfo propertyInfo in LightHouse.Elite.Core.Reflector.GetProperties(dataTypeInfo.ContractTypeInfos.First().ContractType))
                            {
                                Type propertyType = default(Type);
                                DataTypeInfo propertyDataTypeInfo = default(DataTypeInfo);
                                Boolean isList = false;

                                if (typeof(IContractObject).GetTypeInfo().IsAssignableFrom(propertyInfo.PropertyType.GetTypeInfo()))
                                {
                                    propertyDataTypeInfo = dataCache.Get<ContractTypeInfo>(propertyInfo.PropertyType.FullName, "ContractTypeInfos").DataTypeInfos.First();
                                }
                                else if (typeof(IContractList).GetTypeInfo().IsAssignableFrom(propertyInfo.PropertyType.GetTypeInfo()))
                                {
                                    propertyDataTypeInfo = dataCache.Get<ContractTypeInfo>(propertyInfo.PropertyType.GenericTypeArguments[0].FullName, "ContractTypeInfos").DataTypeInfos.First();
                                    isList = true;
                                }
                                else
                                {
                                    propertyType = propertyInfo.PropertyType;
                                }

                                dataTypeInfo.PropertyInfos.Add(new DataPropertyInfo()
                                {
                                    Name = propertyInfo.Name,
                                    PropertyType = propertyType,
                                    DataTypeInfo = propertyDataTypeInfo,
                                    IsList = isList
                                });
                            }
                        }

                        if (!String.IsNullOrEmpty(dataTypeInfo.DynamicType))
                        {
                            IList<DataTypeInfo> childrenTypes = new List<DataTypeInfo>();
                            dataCache.Add(dataTypeInfo.DynamicType, childrenTypes, "DynamicChildrenTypes");

                            PopulateDynamicChildrenTypes(dataTypeInfo, childrenTypes);
                        }
                    }

                    infosLoading = false;
                    infosLoaded = true;
                }
            }
        }

        /// <summary>
        /// Populates the dynamic base type information in the type informations of the DataObjects.
        /// </summary>
        /// <param name="dynamicBaseType">Dynamic base types to be analyzed for dynamic children types.</param>
        /// <param name="childrenTypes">Dynamic children types of the provided base type.</param>
        private void PopulateDynamicChildrenTypes(DataTypeInfo baseDataTypeInfo, IList<DataTypeInfo> childrenTypes)
        {
            LoadInfos();

            foreach(KeyValuePair<String, Object> dataObjectKeyValuePair in dataCache.GetObjectsInRegion("DataTypeInfos"))
            {
                DataTypeInfo dataTypeInfo = (DataTypeInfo)dataObjectKeyValuePair.Value;

                if (dataTypeInfo.DynamicBaseType == baseDataTypeInfo.DynamicType)
                {
                    childrenTypes.Add(dataTypeInfo);
                    PopulateDynamicChildrenTypes(dataTypeInfo, childrenTypes);
                }
            }
        }

        /// <summary>
        /// Adds type information to the locator cache.
        /// </summary>
        /// <param name="contractType">Type of the ContractObject to be added.</param>
        /// <param name="dataType">Type of the DataObject to be added.</param>
        /// <param name="baseType">Base type of the object to be added.</param>
        /// <param name="dynamicType">Dynamic type of the object to be added.</param>
        /// <param name="dynamicBaseType">Dynamic base type of the object to be added.</param>
        /// <param name="isDataObjectInAssembly">Is the provided object in the assembly.</param>
        private void AddInfos(Type contractType, Type dataType, Type baseType, String dynamicType, String dynamicBaseType, Boolean isDataObjectInAssembly)
        {
            String searchType = dynamicType;

            if(String.IsNullOrEmpty(searchType))
            {
                searchType = dataType.FullName;
            }

            DataTypeInfo dataTypeInfo = dataCache.Get<DataTypeInfo>(searchType, "DataTypeInfos");

            if (dataTypeInfo == null)
            {
                dataTypeInfo = new DataTypeInfo()
                {
                    DataType = dataType,
                    BaseType = baseType,
                    DynamicType = dynamicType,
                    DynamicBaseType = dynamicBaseType,
                    ContractTypeInfos = new List<ContractTypeInfo>(),
                    PropertyInfos = new List<DataPropertyInfo>(),
                    IsInAssembly = isDataObjectInAssembly
                };

                dataCache.Add(searchType, dataTypeInfo, "DataTypeInfos");
            }

            if (contractType != null)
            {
                ContractTypeInfo contractTypeInfo = dataCache.Get<ContractTypeInfo>(contractType.FullName, "ContractTypeInfos");

                if (contractTypeInfo == null)
                {
                    contractTypeInfo = new ContractTypeInfo()
                    {
                        ContractType = contractType,
                        IsDataObjectInAssembly = isDataObjectInAssembly,
                        DataTypeInfos = new List<DataTypeInfo>()
                    };

                    contractTypeInfo.DataTypeInfos.Add(dataTypeInfo);
                    dataTypeInfo.ContractTypeInfos.Add(contractTypeInfo);

                    dataCache.Add(contractType.FullName, contractTypeInfo, "ContractTypeInfos");
                }
            }
        }

        /// <summary>
        /// Gets the data type from the provided type.
        /// </summary>
        /// <param name="type">Type to be analyzed for the data type.</param>
        /// <returns>Data type of the provided type.</returns>
        private Type GetDataObjectDataType(Type type)
        {
            LoadInfos();

            ContractTypeInfo ContractTypeInfo = dataCache.Get<ContractTypeInfo>(type.FullName, "ContractTypeInfos");

            if ((ContractTypeInfo != null) && (ContractTypeInfo.IsDataObjectInAssembly))
            {
                return ContractTypeInfo.DataTypeInfos.Where(x => x.IsInAssembly).First().DataType;
            }
            else if((type != typeof(ContractObject))
            && ((type != typeof(Object))))
            {
                return GetDataObjectDataType(type.GetTypeInfo().BaseType);
            }

            return typeof(DataObject);
        }

        /// <summary>
        /// Gets the DataObject base type from the provided type.
        /// </summary>
        /// <param name="type">Type to be analyzed for base type.</param>
        /// <returns>Base type of the provided type.</returns>
        private Type GetDataObjectBaseType(Type type)
        {
            LoadInfos();

            ContractTypeInfo ContractTypeInfo = dataCache.Get<ContractTypeInfo>(type.FullName, "ContractTypeInfos");

            if ((ContractTypeInfo != null) && (ContractTypeInfo.IsDataObjectInAssembly))
            {
                return ContractTypeInfo.DataTypeInfos.Where(x => x.IsInAssembly).First().DataType;
            }

            return default(Type);
        }

        /// <summary>
        /// Gets dynamic base type for ContractObject type.
        /// </summary>
        /// <param name="contractObjectType">Type of the ContractObject to be analyzed.</param>
        /// <returns>Dynamic base type as string.</returns>
        private String GetDynamicBaseType(Type contractObjectType)
        {
            if ((contractObjectType != typeof(ContractObject))
            && (contractObjectType.GetTypeInfo().BaseType != typeof(ContractObject)))
            {
                ContractTypeInfo ContractTypeInfo = dataCache.Get<ContractTypeInfo>(contractObjectType.GetTypeInfo().BaseType.FullName, "ContractTypeInfos");

                if ((ContractTypeInfo == null) || (!ContractTypeInfo.IsDataObjectInAssembly))
                {
                    return String.Format("{0}Data", contractObjectType.GetTypeInfo().BaseType.FullName);
                }
            }

            return "";
        }

        /// <summary>
        /// Gets the dynamic children types of the provided dynamic type.
        /// </summary>
        /// <param name="dynamicType">Dynamic type to be analyzed for children types.</param>
        /// <returns>List of children types as DataTypeInfos.</returns>
        public IList<DataTypeInfo> GetDynamicChildrenTypes(DataTypeInfo dataTypeInfo)
        {
            LoadInfos();

            return dataCache.Get<IList<DataTypeInfo>>(dataTypeInfo.DynamicType, "DynamicChildrenTypes");
        }

        /// <summary>
        /// Loads all known types by examining the assemblies in the application path.
        /// </summary>
        /// <returns>Task with the result of true if the loading was succesful; otherwise a task with the result of false.</returns>
        private async Task<Boolean> LoadKnownTypes()
        {
            if (knownTypesLoading)
            {
                while (!knownTypesLoaded)
                {
                    await Task.Delay(100);
                }
            }

            if (!knownTypesLoaded)
            {
                knownTypesLoading = true;

                IList<Assembly> assemblies = LightHouse.Bootstrap.Assemblies.GetAll();

                Assembly executingAssembly = (Assembly)typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly").Invoke(null, new object[0]);

                if (executingAssembly != null)
                {
                    assemblies.Add(executingAssembly);
                }

                foreach (Assembly assembly in assemblies.OrderBy(a => a.FullName))
                {
                    GetKnownTypesOfAssembly(assembly);
                }

                knownTypesLoaded = true;
            }

            return true;
        }

        /// <summary>
        /// Gets the known types from the provided assembly.
        /// </summary>
        /// <param name="assembly">Assembly to be analyzed for known types.</param>
        private void GetKnownTypesOfAssembly(Assembly assembly)
        {
            try
            {
                foreach (System.Reflection.TypeInfo typeInfo in assembly.DefinedTypes.OrderBy(t => t.FullName))
                {
                    dataCache.Add(typeInfo.FullName, typeInfo.AsType(), "KnownTypes");
                }
            }
            catch (ReflectionTypeLoadException reflectionTypeLoadException)
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (Exception loaderException in reflectionTypeLoadException.LoaderExceptions)
                {
                    stringBuilder.AppendLine(String.Format("Loading type {0} failed. Loader Exception: {1}.", assembly.FullName, loaderException.Message));                    
                }             

                Debug.WriteLine(stringBuilder.ToString());                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Loading assembly {0} failed. Exception: {1}.", assembly.FullName, ex.Message);                
            }
        }

        /// <summary>
        /// Get all known types.
        /// </summary>
        /// <returns>A collection of known types.</returns>
        public ICollection<Type> GetKnownTypes()
        {
            LoadKnownTypes();

            IList<Type> knownTypes = new List<Type>();

            foreach (KeyValuePair<String, Object> knownTypeKeyValuePair in dataCache.GetObjectsInRegion("KnownTypes"))
            {
                knownTypes.Add((Type)knownTypeKeyValuePair.Value);
            }

            return knownTypes;
        }

        /// <summary>
        /// Gets the type information of a ContractObjectby it's type.
        /// </summary>
        /// <param name="contractObjectType">Type of the ContractObject.</param>
        /// <returns>Type information of the ContractObject.</returns>
        public ContractTypeInfo GetContractTypeInfo(Type contractObjectType)
        {
            return GetContractTypeInfo(contractObjectType.FullName);
        }

        /// <summary>
        /// Gets the type information of a ContractObject by it's full name.
        /// </summary>
        /// <param name="dataObjectFullName">Full Name of the ContractObject.</param>
        /// <returns>Type information of the ContractObject.</returns>

        public ContractTypeInfo GetContractTypeInfo(String contractObjectFullName)
        {
            LoadInfos();

            return dataCache.Get<ContractTypeInfo>(contractObjectFullName, "ContractTypeInfos");
        }

        /// <summary>
        /// Gets the type information of a DataObject by it's type.
        /// </summary>
        /// <param name="dataObjectType">Type of the DataObject.</param>
        /// <returns>Type information of the DataObject.</returns>
        public DataTypeInfo GetDataTypeInfo(Type dataObjectType)
        {
            return GetDataTypeInfo(dataObjectType.FullName);
        }

        /// <summary>
        /// Gets the type information of a DataObject by it's full name.
        /// </summary>
        /// <param name="dataObjectFullName">Full Name of the DataObject.</param>
        /// <returns>Type information of the DataObject.</returns>
        public DataTypeInfo GetDataTypeInfo(String dataObjectFullName)
        {
            LoadInfos();

            return dataCache.Get<DataTypeInfo>(dataObjectFullName, "DataTypeInfos");
        }

        /// <summary>
        /// Gets the list of base types. Should the object be dynamic, the DynamicBaseTypes would be returned.
        /// </summary>
        /// <param name="dataObject">DataObject that needs to be analyzed for base types.</param>
        /// <returns>List of base types of the provided DataObject as list of string.</returns>       
        public virtual IList<String> GetBaseTypes(IDataObject dataObject)
        {
            if (!String.IsNullOrEmpty(dataObject.DynamicType))
            {
                DataTypeInfo dataTypeInfo = GetDataTypeInfo(dataObject.DynamicType);

                return GetBaseTypes(dataTypeInfo);
            }
            else
            {
                IList<String> baseTypes = new List<String>();

                GetBaseTypes(dataObject.GetType().GetTypeInfo().BaseType, baseTypes);

                return baseTypes;
            }
        }

        /// <summary>
        /// Returns the list of base types from the provided type information.
        /// </summary>
        /// <param name="dataTypeInfo">Type information of the DataObject that needs to be analyzed for base types.</param>
        /// <returns>List of base types of the provided type information as list of string.</returns>       
        public virtual IList<String> GetBaseTypes(DataTypeInfo dataTypeInfo)
        {
            IList<String> baseTypes = new List<String>();

            GetDynamicBaseTypes(dataTypeInfo, baseTypes);

            return baseTypes;
        }

        /// <summary>
        /// Gets the dynamic base types of the provided type information. 
        /// </summary>
        /// <param name="dataTypeInfo">Type information of the DataObject that needs to be analyzed for base types.</param>
        /// <param name="baseTypes">List of based types, to which the new base types have to be added.</param>
        private void GetDynamicBaseTypes(DataTypeInfo dataTypeInfo, IList<String> baseTypes)
        {
            if (dataTypeInfo.DynamicBaseType == "")
            {
                GetBaseTypes(dataTypeInfo.BaseType, baseTypes);
            }
            else
            {
                DataTypeInfo baseDataTypeInfo = GetDataTypeInfo(dataTypeInfo.DynamicBaseType);

                baseTypes.Add(dataTypeInfo.DynamicType);

                GetDynamicBaseTypes(baseDataTypeInfo, baseTypes);
            }
        }

        /// <summary>
        /// Gets base types of the provided type.
        /// </summary>
        /// <param name="type">Child type that needs to be analyzed for base types.</param>
        /// <param name="baseTypes">List of based types, to which the new base types have to be added.</param>
        private void GetBaseTypes(Type type, IList<String> baseTypes)
        {
            baseTypes.Add(type.FullName);

            if ((type.GetTypeInfo().BaseType != typeof(DataObject)) && (type.GetTypeInfo().BaseType != typeof(Object)))
            {
                GetBaseTypes(type.GetTypeInfo().BaseType, baseTypes);
            }
        }
    }
}
