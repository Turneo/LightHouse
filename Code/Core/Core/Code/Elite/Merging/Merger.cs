using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core.Collections;
using LightHouse.Core;

namespace LightHouse.Core.Elite.Merging
{
    /// <summary>
    /// Officer responsible for merging objects.
    /// </summary>
    public class Merger
    {
        /// <summary>
        /// Merges two DataObjects based on a set of rules.
        /// </summary>
        /// <param name="baseObject">Base DataObject to be merged.</param>
        /// <param name="mergingObject">Merging DataObject to be merged into base DataObject.</param>
        /// <param name="paths">Paths to be considered in merge.</param>
        /// <param name="proxyReferences">Should references be proxied.</param>
        /// <param name="resolveDataObject">Special resolver for binding existing DataObjects.</param>
        public void MergeDataObject(IDataObject baseObject, IDataObject mergingObject, IList<String> paths, Boolean proxyReferences, Func<String, String, IDataObject> resolveDataObject = null)
        {
            new MergeExecutor().Execute(baseObject, mergingObject, paths, proxyReferences, resolveDataObject);
        }
    }
}
