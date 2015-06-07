using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightHouse.Core.Caching
{
    /// <summary>
    /// The DataRegion holds a collection of objects by their keys.
    /// </summary>
    public class DataRegion
    {
        /// <summary>
        /// Dictionary containing all the objects of the region.
        /// </summary>
        private IDictionary<String, Object> objects = new Dictionary<String, Object>();

        /// <summary>
        /// Dictionary holding all the regions of the cache.
        /// </summary>
        public IDictionary<String, Object> Objects
        {
            get
            {
                return objects;
            }           
        } 

        /// <summary>
        /// The mutex object for locking the calls to the dictionary and making them thread safe.
        /// </summary>
        private readonly Object mutex = new Object();

        /// <summary>
        /// Name of the region.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Deletes all objects in the region.
        /// </summary>
        public void Clear()
        {
            lock (mutex)
            {
                objects.Clear();
            }
        }

        /// <summary>
        /// Adds an object to the region in the cache.
        /// </summary>
        /// <param name="key">A unique value that is used to store and retrieve the object from the cache.</param>
        /// <param name="value">The object saved to the cache.</param>
        public void Add(String key, Object value)
        {
            lock (mutex)
            {
                objects[key] = value;
            }
        }

        /// <summary>
        /// Removes an object from the region in the cache.
        /// </summary>
        /// <param name="key">A unique value that is used to store and retrieve the object from the cache.</param>
        public void Remove(String key)
        {
            lock (mutex)
            {
                objects.Remove(key);
            }
        }

        /// <summary>
        /// Gets an object from the region in the cache by using the specified key.
        /// </summary>
        /// <param name="key">The unique value that is used to identify the object in the cache.</param>
        /// <returns>The object that was cached by using the specified key. Null is returned if the key does not exist.</returns>
        public Object Get(String key)
        {
            Object cachedObject = default(Object);

            if (!String.IsNullOrEmpty(key))
            {
                lock (mutex)
                {
                    objects.TryGetValue(key, out cachedObject);
                }
            }

            return cachedObject;
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in the region.
        /// </summary>
        /// <returns>An enumerable list of all cached objects in the region.</returns>
        public IEnumerable<KeyValuePair<String, Object>> GetObjects()
        {
            lock (mutex)
            {
                return objects.ToList<KeyValuePair<String, Object>>();
            }
        }
    }
}
