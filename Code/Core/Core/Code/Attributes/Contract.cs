using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightHouse.Core.Attributes
{
    /// <summary>
    /// Defines the contract that should be used for this data type. Several contracts can be mapped to one data type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class Contract : Attribute
    {
        /// <summary>
        /// Type of the contract to be used for this data type.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Defines if the contract should be use as a default when creating new instances of this data type.
        /// </summary>
        public Boolean IsDefault { get; set; }

        /// <summary>
        /// Default constructor with parameters for the contract attribute.
        /// </summary>
        /// <param name="type">Type of the contract to be used for this data type.</param>
        /// <param name="isDefault">Defines if the contract should be use as a default when creating new instances of this data type.</param>
        public Contract(Type type, Boolean isDefault = true)
        {
            this.Type = type;
            this.IsDefault = isDefault;
        }
    }
}
