using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core;
using LightHouse.Core.Collections;
using LightHouse.Core.Elite.Core.Events;
using LightHouse.Core.Proxies;

namespace LightHouse.Core.Elite.Loading
{
    /// <summary>
    /// Officer responsible for loading proxied DataObjects from the corresponding data source.
    /// </summary>
    public class Loader
    {
        /// <summary>
        /// Occurs when the loading has been completed.
        /// </summary>
        public event LoadEventHandler OnLoadCompleted;

        /// <summary>
        /// Triggers the unxproxing of a DataObject.
        /// </summary>
        /// <param name="sender">Sender that is triggering the loading event.</param>
        /// <param name="e">Parameters of the loading event.</param>
        public delegate void LoadEventHandler(object sender, LoadEventArgs e);

        /// <summary>
        /// Loads the provided paths from the DataObject based on the provided proxy information.
        /// </summary>
        /// <param name="dataObject">DataObject that has to be partially loaded (unproxied).</param>
        /// <param name="proxyInformation">Proxy information of the DataObject to be used in the loading process.</param>
        /// <param name="paths">Paths to be loaded (unproxied) on the provided DataObject.</param>
        public void Load(IDataObject dataObject, ProxyInformation proxyInformation, IList<String> paths)
        {
            if (OnLoadCompleted != null)
            {
                OnLoadCompleted(this, new LoadEventArgs()
                {
                    DataObject = dataObject,
                    ProxyInformation = proxyInformation,
                    Paths = paths
                });
            }
        }
    }
}
