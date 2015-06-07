using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core.Proxies;

namespace LightHouse.Core.Elite.Core.Events
{
    /// <summary>
    /// Event arguments for the loading event.
    /// </summary>
    public class LoadEventArgs : EventArgs
    {
        /// <summary>
        /// DataObject that has to be partially loaded (unproxied).
        /// </summary>
        public IDataObject DataObject { get; set; }

        /// <summary>
        /// Proxy information of the DataObject to be used in the loading process.
        /// </summary>
        public ProxyInformation ProxyInformation { get; set; }

        /// <summary>
        /// Paths to be loaded (unproxied) on the provided DataObject.
        /// </summary>
        public IList<String> Paths { get; set; }
    }
}
