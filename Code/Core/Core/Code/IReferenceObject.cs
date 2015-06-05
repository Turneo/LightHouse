using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core
{
    /// <summary>
    /// Object that has a unique reference that can be used for identifying it.
    /// </summary>
    public interface IReferenceObject
    {
        /// <summary>
        /// Unique reference of the object.
        /// </summary>
        String Reference { get; set; }
    }
}
