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
        /// REVIEW: Source IObject where the Path can be found
        /// </summary>
        public IObject Source { get; set; }

        /// <summary>
        /// REVIEW: Path where the Value can be found in the Source
        /// </summary>
        public String Path { get; set; }
    }
}
