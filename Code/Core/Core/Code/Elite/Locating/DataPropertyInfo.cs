using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Elite.Locating
{
    /// <summary>
    /// Property information for DataObjects.
    /// </summary>
    public class DataPropertyInfo : PropertyInfo
    {
        /// <summary>
        /// Name of the property.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Type of the property.
        /// </summary>
        public Type PropertyType { get; set; }

        /// <summary>
        /// Data type if the property isn't a standard type.
        /// </summary>
        public DataTypeInfo DataTypeInfo { get; set; }

        /// <summary>
        /// Specifies if the property is a list.
        /// </summary>
        public Boolean IsList { get; set; }
    }
}
