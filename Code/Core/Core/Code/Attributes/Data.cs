using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightHouse.Core.Attributes
{
    /// <summary>
    /// Defines the data that should be used for this type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class Data : Attribute
    {
        /// <summary>
        /// Type of the data to be used for this type.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Defines if the contract should be use as a default when creating new instances of this data type.
        /// </summary>
        public Boolean IsDefault { get; set; }

        /// <summary>
        /// Default constructor with parameters for the data attribute.
        /// </summary>
        /// <param name="type">Type of the data to be used for this type.</param>
        /// <param name="isDefault">Defines if the data should be use as a default when creating new instances of this type.</param>
        public Data(Type type, Boolean isDefault = true)
        {
            this.Type = type;
            this.IsDefault = isDefault;
        }
    }
}
