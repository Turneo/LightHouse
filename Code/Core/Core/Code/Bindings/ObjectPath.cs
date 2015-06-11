using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Bindings
{
    public class ObjectPath
    {
        /// <summary>
        /// Source of the binding as IObject.
        /// </summary>
        public IObject Source { get; set; }

        /// <summary>
        /// Path where the value can be found in the Source.
        /// </summary>
        public String Path { get; set; }
    }
}
