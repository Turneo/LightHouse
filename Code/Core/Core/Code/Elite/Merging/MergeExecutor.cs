using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core.Collections;
using LightHouse.Core;
using LightHouse.Core.Caching;

namespace LightHouse.Core.Elite.Merging
{
    /// <summary>
    /// Responsible for executing a merge. Holds all the required information needed for the merge execution.
    /// </summary>
    internal class MergeExecutor
    {
        /// <summary>
        /// Cache of the already merged objects during the merge execution.
        /// </summary>
        private DataCache convertedObjectsCache = new DataCache();

        /// <summary>
        /// Base DataObject to be merged.
        /// </summary>
        public IDataObject BaseObject { get; set; }
        /// <summary>
        /// Merging DataObject to be merged into base DataObject.
        /// </summary>
        public IDataObject MergingObject { get; set; }
        /// <summary>
        /// Paths to be considered in merge.
        /// </summary>
        public IList<String> Paths { get; set; }
        /// <summary>
        /// Should references be proxied.
        /// </summary>
        public Boolean ProxyReferences { get; set; }
        /// <summary>
        /// Special resolver for binding existing DataObjects.
        /// </summary>
        public Func<String, String, IDataObject> ResolveDataObject { get; set; }

        /// <summary>
        /// Executes a merge procedure.
        /// </summary>
        /// <param name="baseObject">Base DataObject to be merged.</param>
        /// <param name="mergingObject">Merging DataObject to be merged into base DataObject.</param>
        /// <param name="paths">Paths to be considered in merge.</param>
        /// <param name="proxyReferences">Should references be proxied.</param>
        /// <param name="resolveDataObject">Special resolver for binding existing DataObjects.</param>
        public void Execute(IDataObject baseObject, IDataObject mergingObject, IList<String> paths, Boolean proxyReferences, Func<String, String, IDataObject> resolveDataObject = null)
        {
            this.BaseObject = baseObject;
            this.MergingObject = mergingObject;
            this.Paths = paths;
            this.ProxyReferences = proxyReferences;
            this.ResolveDataObject = resolveDataObject;

            MergeDataObject(this.BaseObject, this.MergingObject, true);
        }

        /// <summary>
        /// Merges dynamic and static properties of DataObject.
        /// </summary>
        /// <param name="baseObject">Base DataObject to be merged.</param>
        /// <param name="mergingObject">Merging DataObject to be merged into base DataObject.</param>
        /// <param name="checkPaths">Consider paths in the merging.</param>
        private void MergeDataObject(IDataObject baseObject, IDataObject mergingObject, Boolean checkPaths)
        {
            if (!String.IsNullOrEmpty(mergingObject.DynamicType))
            {
                MergeDynamicProperties(baseObject, mergingObject, checkPaths);
            }

            MergeProperties(baseObject, mergingObject, checkPaths);
        }

        /// <summary>
        /// Get fields from the ContractObject.
        /// </summary>
        /// <returns>Fields of the ContractObject.</returns>
        private FieldInfo GetContractObjectsField()
        {
            return typeof(DataObject).GetTypeInfo().DeclaredFields.FirstOrDefault(f => f.Name == "contractObjects"); ;
        }

        /// <summary>
        /// Merges static properties of DataObject.
        /// </summary>
        /// <param name="baseObject">Base DataObject to be merged.</param>
        /// <param name="mergingObject">Merging DataObject to be merged into base DataObject.</param>
        /// <param name="checkPaths">Consider paths in the merging.</param>
        private void MergeProperties(IDataObject baseObject, IDataObject mergingObject, Boolean checkPaths)
        {
            foreach (PropertyInfo propertyInfo in mergingObject.GetType().GetRuntimeProperties())
            {
                Boolean includePath = true;

                if (this.Paths != null)
                {
                    includePath = !checkPaths;

                    foreach (String path in this.Paths)
                    {
                        if (propertyInfo.Name == path)
                        {
                            includePath = true;
                        }
                    }
                }

                if ((propertyInfo.CanWrite) && (!mergingObject.GetProxyState(propertyInfo.Name)) && (includePath))
                {
                    if (IsDataList(propertyInfo.PropertyType))
                    {
                        IDataList propertyValue = (IDataList)propertyInfo.GetValue(mergingObject, new Object[] { });

                        if (propertyValue != null)
                        {
                            IDataList dataList = (IDataList)LightHouse.Elite.Core.Builder.Get(propertyValue.GetType());

                            for (int i = 0; i < propertyValue.Count; i++)
                            {
                                if (IsReferenceObject((IDataObject)propertyValue[i]))
                                {
                                    dataList.Add(GetReferencedObject((IDataObject)propertyValue[i]));
                                }
                                else
                                {
                                    IDataObject referencedObject = GetProxyObject((IDataObject)propertyValue[i]);
                                    dataList.Add(referencedObject);
                                }
                            }

                            propertyInfo.SetValue(baseObject, dataList, new Object[] { });
                        }
                    }
                    else if (IsDataObject(propertyInfo.PropertyType))
                    {
                        DataObject propertyValue = (DataObject)propertyInfo.GetValue(mergingObject, new Object[] { });

                        if (propertyValue != null)
                        {
                            if (IsReferenceObject(propertyValue))
                            {
                                // If the Object hasn't been saved, it is persisted as such to the database
                                if (String.IsNullOrEmpty(propertyValue.ID))
                                {
                                    IDataObject referencedObject = GetProxyObject(propertyValue);
                                    propertyInfo.SetValue(baseObject, referencedObject, new Object[] { });
                                }
                                else
                                {
                                    propertyInfo.SetValue(baseObject, GetReferencedObject(propertyValue), new Object[] { });
                                }
                            }
                            else 
                            {
                                IDataObject referencedObject = GetProxyObject(propertyValue);
                                propertyInfo.SetValue(baseObject, referencedObject, new Object[] { });
                            }
                        }
                        else
                        {
                            propertyInfo.SetValue(baseObject, null, new Object[] { });
                        }
                    }
                    else
                    {
                        propertyInfo.SetValue(baseObject, propertyInfo.GetValue(mergingObject, new Object[] { }), new Object[] { });
                    }
                }
            }
        }

        /// <summary>
        /// Merges dynamic properties of DataObject.
        /// </summary>
        /// <param name="baseObject">Base DataObject to be merged.</param>
        /// <param name="mergingObject">Merging DataObject to be merged into base DataObject.</param>
        /// <param name="checkPaths">Consider paths in the merging.</param>
        private void MergeDynamicProperties(IDataObject baseObject, IDataObject mergingObject, Boolean checkPaths)
        {
            baseObject.DynamicType = mergingObject.DynamicType;

            DataCache dynamicCache = (DataCache)typeof(DataObject).GetTypeInfo().GetDeclaredField("dynamicCache").GetValue(mergingObject);

            foreach (KeyValuePair<String, Object> propertyValue in dynamicCache.GetObjectsInRegion("Properties"))
            {
                Boolean includePath = true;

                if (this.Paths != null)
                {
                    includePath = !checkPaths;

                    foreach (String path in this.Paths)
                    {
                        if (propertyValue.Key == path)
                        {
                            includePath = true;
                        }
                    }
                }

                if ((!mergingObject.GetProxyState(propertyValue.Key)) && (includePath))
                {
                    if (propertyValue.Value != null)
                    {
                        Type propertyType = propertyValue.Value.GetType();

                        if (IsDataList(propertyType))
                        {
                            IDataList mergingList = ((IDataList)propertyValue.Value);
                            IDataList dataList = (IDataList)LightHouse.Elite.Core.Builder.Get(propertyValue.Value.GetType());

                            for (int i = 0; i < mergingList.Count; i++)
                            {
                                IDataObject cloningObject = (IDataObject)mergingList[i];

                                if (IsReferenceObject(cloningObject))
                                {
                                    //Proxy on new generated object should be set to false, otherwise will bet unproxied on storing

                                    if (String.IsNullOrEmpty(cloningObject.ID))
                                    {
                                        IDataObject referencedObject = GetProxyObject(cloningObject);
                                        dataList.Add(referencedObject);
                                    }
                                    else
                                    {

                                        dataList.Add(GetReferencedObject(cloningObject));
                                    }
                                }
                                else
                                {
                                    IDataObject referencedObject = GetProxyObject(cloningObject);
                                    dataList.Add(referencedObject);
                                }
                            }

                            baseObject.SetProperty(propertyValue.Key, dataList);
                        }
                        else if (IsDataObject(propertyType))
                        {
                            DataObject cloningObject = ((DataObject)propertyValue.Value);

                            if (IsReferenceObject(cloningObject))
                            {
                                // If the Object hasn't been saved, it is persisted as such to the database
                                if (String.IsNullOrEmpty(cloningObject.ID))
                                {
                                    IDataObject referencedObject = GetProxyObject(cloningObject);
                                    baseObject.SetProperty(propertyValue.Key, referencedObject);
                                }
                                else
                                {
                                    baseObject.SetProperty(propertyValue.Key, GetReferencedObject(cloningObject));
                                }
                            }
                            else
                            {
                                IDataObject referencedObject = GetProxyObject(cloningObject);
                                baseObject.SetProperty(propertyValue.Key, referencedObject);
                            }
                        }
                        else
                        {
                            baseObject.SetProperty(propertyValue.Key, propertyValue.Value);
                        }
                    }
                    else
                    {
                        baseObject.SetProperty(propertyValue.Key, propertyValue.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets reference object from the DataObject.
        /// </summary>
        /// <param name="cloningObject">Object to be cloned.</param>
        /// <returns>Cloned DataObject.</returns>
        private IDataObject GetReferencedObject(IDataObject cloningObject)
        {
            IDataObject referencedObject = default(IDataObject);

            if (this.ResolveDataObject != null)
            {
                referencedObject = ResolveDataObject(cloningObject.GetDataType(), cloningObject.ID);
            }

            if (referencedObject == null)
            {
                referencedObject = LightHouse.Elite.Core.Builder.GetDataObject(cloningObject.GetType(), this.ProxyReferences);
                referencedObject.ID = cloningObject.ID;
                referencedObject.DynamicType = cloningObject.DynamicType;
            }

            return referencedObject;
        }
        
        /// <summary>
        /// Gets proxy object from source object.
        /// </summary>
        /// <param name="sourceObject">Source DataObject.</param>
        /// <returns>Proxied DataObject.</returns>
        private IDataObject GetProxyObject(IDataObject sourceObject)
        {
            IDataObject destinationObject = convertedObjectsCache.Get<DataObject>(sourceObject.GetHashCode().ToString());

            if (destinationObject != null)
            {
                if (IsReferenceObject(sourceObject))
                {
                    IDataObject referencedObject = LightHouse.Elite.Core.Builder.GetDataObject(sourceObject.GetType(), false);
                    referencedObject.ID = sourceObject.ID;
                    referencedObject.DynamicType = sourceObject.DynamicType;

                    return referencedObject;
                }
            }
            else
            {
                destinationObject = LightHouse.Elite.Core.Builder.GetDataObject(sourceObject.GetType(), false);
                convertedObjectsCache.Add(sourceObject.GetHashCode().ToString(), destinationObject);
            }

            MergeDataObject(destinationObject, sourceObject, false);

            return destinationObject;
        }

        /// <summary>
        /// Checks if the DataObject is a reference object or not.
        /// </summary>
        /// <param name="dataObject">DataObject to bec checked.</param>
        /// <returns>Wheter the DataObject is reference or not.</returns>
        private static Boolean IsReferenceObject(IDataObject dataObject)
        {
            if (typeof(IReferenceObject).GetTypeInfo().IsAssignableFrom(dataObject.GetType().GetTypeInfo()))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the property type is a DataObject or not.
        /// </summary>
        /// <param name="propertyType">Property type to be checked.</param>
        /// <returns>Wheter the property type is a DataObject or not.</returns>
        private static bool IsDataObject(Type propertyType)
        {
            return typeof(DataObject).GetTypeInfo().IsAssignableFrom(propertyType.GetTypeInfo());
        }

        /// <summary>
        /// Checks if the property type is a DataList or not.
        /// </summary>
        /// <param name="propertyType">Property type to be checked.</param>
        /// <returns>Wheter the property type is a DataList or not.</returns>
        private static bool IsDataList(Type propertyType)
        {
            return (propertyType.GenericTypeArguments.Length > 0) && (typeof(DataObject).GetTypeInfo().IsAssignableFrom(propertyType.GenericTypeArguments[0].GetTypeInfo()));
        }
    }
}
