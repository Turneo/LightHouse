using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core.Collections;
using LightHouse.Core;
using System.Reflection;

namespace LightHouse.Core.Elite.Cloning
{
    /// <summary>
    /// Officer responsible for cloning DataObjects.
    /// </summary>
    public class Cloner
    {
        /// <summary>
        /// Clones the provided DataObject taking in consideration paths and proxy configurations. A resolver can be provided for additional customizations.
        /// </summary>
        /// <param name="cloningObject">Object to be cloned.</param>
        /// <param name="paths">Paths to be included in the clonation.</param>
        /// <param name="proxyReferences">Should objects be proxied during clonation.</param>
        /// <param name="resolveDataObject">Custom resolvers for referenced DataObjects.</param>
        /// <returns>Cloned object taking in consideration paths and proxy configurations.</returns>
        public IDataObject CloneDataObject(IDataObject cloningObject, IList<String> paths, Boolean proxyReferences, Func<String, String, IDataObject> resolveDataObject = null)
        {
            IDataObject clonedObject = LightHouse.Elite.Core.Builder.GetDataObject(cloningObject.GetType(), false);

            LightHouse.Elite.Core.Merger.MergeDataObject(clonedObject, cloningObject, paths, proxyReferences, resolveDataObject);

            return clonedObject;
        }
        
    }
}
