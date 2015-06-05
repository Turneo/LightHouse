using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightHouse.Core.Attributes
{
    /// <summary>
    /// Defines the path for the surrogate property to get/set the value from the related contract or data object. Use in combination with the contract attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class Path : Attribute
    {
        /// <summary>
        /// The effective value of the path attribute.
        /// </summary>
        public String Value { get; set; }

        /// <summary>
        /// Default constructor with parameters for the path attribute.
        /// </summary>
        /// <param name="path">Path of the surrogate property to get/set the value from the related contract or data object. Use in combination with the contract attribute.</param>
        public Path(String path)
        {
            this.Value = path;
        }
    }
}
