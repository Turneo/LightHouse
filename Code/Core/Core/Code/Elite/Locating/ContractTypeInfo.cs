using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Elite.Locating
{
    /// <summary>
    /// Type information for ContractObjects.
    /// </summary>
    public class ContractTypeInfo : TypeInfo
    {
        /// <summary>
        /// Type of the ContractObject.
        /// </summary>
        public Type ContractType { get; set; }

        /// <summary>
        /// Is there a DataObject for this ContractObject in the assembly.
        /// </summary>
        public Boolean IsDataObjectInAssembly { get; set; }

        /// <summary>
        /// List of type information of DataObjects that are used by this ContractObject.
        /// </summary>
        public IList<DataTypeInfo> DataTypeInfos { get; set; }
    }
}
