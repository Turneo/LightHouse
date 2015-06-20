using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Elite.Locating
{
    /// <summary>
    /// Type information for DataObjects.
    /// </summary>
    public class DataTypeInfo : TypeInfo
    {
        /// <summary>
        /// Type of the DataObject.
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// Base type of the DataObject.
        /// </summary>
        public Type BaseType { get; set; }

        /// <summary>
        /// Dynamic type of the type, to be used in the case the DataObject are dynamic and not yet generated.
        /// </summary>
        public String DynamicType { get; set; }

        /// <summary>
        /// Dynamic base type of the type, to be used in the case the DataObject are dynamic and not yet generated.
        /// </summary>
        public String DynamicBaseType { get; set; }

        /// <summary>
        /// List of property information of this DataObject.
        /// </summary>
        public IList<DataPropertyInfo> PropertyInfos { get; set; }

        /// <summary>
        /// List of type information of ContractObjects that are used by this DataObject.
        /// </summary>
        public IList<ContractTypeInfo> ContractTypeInfos { get; set; }
        
        /// <summary>
        /// Is there a ContractObject for this DataObject in the assembly.
        /// </summary>
        public Boolean IsInAssembly { get; set; }
    }
}
