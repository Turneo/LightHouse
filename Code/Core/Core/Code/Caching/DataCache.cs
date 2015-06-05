using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightHouse.Core.Caching
{
    /// <summary>
    /// The object that is used by cache-enabled applications for storing and retrieving objects from the cache.
    /// </summary>
    public class DataCache
    {
        private IDictionary<String, DataRegion> regions = new Dictionary<String, DataRegion>();
        /// <summary>
        /// Dictionary holding all the regions of the cache.
        /// </summary>
        public IDictionary<String, DataRegion> Regions 
        { 
            get
            {
                return regions;
            }
        }

        /// <summary>
        /// The mutex object for locking the calls to the regions dictionary and making them thread safe.
        /// </summary>
        private readonly Object mutex = new Object();

        /// <summary>
        /// The default time that an object is kept in the cache before it expires.
        /// </summary>
        public TimeSpan DefaultExpiryPeriod { get; set; }

        /// <summary>
        /// Adds an object to the default region in the cache.
        /// </summary>
        /// <param name="key">A unique value that is used to store and retrieve the object from the cache.</param>
        /// <param name="value">The object saved to the cache.</param>
        public void Add(String key, Object value)
        {
            Add(key, value, "default");
        }

        /// <summary>
        /// Adds an object to a region in the cache. If the region doesn't exist it's created.
        /// </summary>
        /// <param name="key">A unique value that is used to store and retrieve the object from the cache. </param>
        /// <param name="value">The object saved to the cache.</param>
        /// <param name="region">The name of the region to save the object in.</param>
        public void Add(String key, Object value, String region)
        {
            DataRegion dataRegion = default(DataRegion);

            lock (mutex)
            {
                if (!regions.TryGetValue(region, out dataRegion))
                {
                    dataRegion = new DataRegion();
                    regions[region] = dataRegion;
                }
            }

            dataRegion.Add(key, value);
        }

        /// <summary>
        /// Removes an object to the default region in the cache.
        /// </summary>
        /// <param name="key">A unique value that is used to store and retrieve the object from the cache.</param>
        public void Remove(String key)
        {
            Remove(key, "default");
        }

        /// <summary>
        /// Removes an object to a region in the cache. If the region doesn't exist it's created.
        /// </summary>
        /// <param name="key">A unique value that is used to store and retrieve the object from the cache. </param>
        /// <param name="region">The name of the region to remove the object from.</param>
        public void Remove(String key, String region)
        {
            DataRegion dataRegion = default(DataRegion);

            lock (mutex)
            {
                if (!regions.TryGetValue(region, out dataRegion))
                {
                    dataRegion = new DataRegion();
                    regions[region] = dataRegion;
                }
            }

            dataRegion.Remove(key);
        }

        /// <summary>
        /// Deletes all objects in the specified region.
        /// </summary>
        /// <param name="region">The name of the region whose objects are removed.</param>
        public void ClearRegion(String region)
        {
            DataRegion dataRegion = default(DataRegion);

            lock (mutex)
            {
                regions.TryGetValue(region, out dataRegion);
            }

            if (dataRegion != null)
            {
                dataRegion.Clear();
            }
        }

        /// <summary>
        /// Creates a region.
        /// </summary>
        /// <param name="region">The name of the region that is created.</param>
        /// <returns>If the region has been created successfully or not. Should the region already exist, this method will return false.</returns>
        public Boolean CreateRegion(String region)
        {
            lock (mutex)
            {
                if (!regions.ContainsKey(region))
                {
                    regions[region] = new DataRegion();

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the region exists.
        /// </summary>
        /// <param name="region">The name of the region that needs to be checked.</param>
        /// <returns>If the region exists or not.</returns>
        public Boolean ContainsRegion(String region)
        {
            lock (mutex)
            {
                return regions.ContainsKey(region);
            }
        }

        /// <summary>
        /// Gets an object from the cache using the specified key from the default region.
        /// </summary>
        /// <param name="key">The unique value that is used to identify the object in the cache.</param>
        /// <returns>The object that was cached by using the specified key. Null is returned if the key does not exist.</returns>
        public Object Get(String key)
        {
            return Get(key, "default");
        }

        /// <summary>
        /// Gets an object from the specified region by using the specified key. 
        /// </summary>
        /// <param name="key">The unique value that is used to identify the object in the region.</param>
        /// <param name="region">The name of the region where the object resides.</param>
        /// <returns>The object that was cached by using the specified key. Null is returned if the key does not exist.</returns>
        public Object Get(String key, String region)
        {
            DataRegion dataRegion = default(DataRegion);

            lock (mutex)
            {
                if (!regions.TryGetValue(region, out dataRegion))
                {
                    dataRegion = new DataRegion();
                    regions[region] = dataRegion;
                }
            }

            return dataRegion.Get(key);
        }

        /// <summary>
        /// Gets an object from the cache using the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the object to be retrieved.</typeparam>
        /// <param name="key">The unique value that is used to identify the object in the region.</param>
        /// <returns>The object that was cached by using the specified key. Null is returned if the key does not exist.</returns>
        public T Get<T>(String key)
        {
            return (T)Get(key);
        }

        /// <summary>
        /// Gets an object from the cache using the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the object to be retrieved.</typeparam>
        /// <param name="key">The unique value that is used to identify the object in the region.</param>
        /// <param name="region">The name of the region where the object resides.</param>
        /// <returns>The object that was cached by using the specified key. Null is returned if the key does not exist.</returns>
        public T Get<T>(String key, String region)
        {
            return (T)Get(key, region);
        }

        /// <summary>
        /// Gets an enumerable list of all cached objects in the specified region.
        /// </summary>
        /// <param name="region">The name of the region for which to return a list of all resident objects.</param>
        /// <returns>An enumerable list of all cached objects in the specified region.</returns>
        public IEnumerable<KeyValuePair<String, Object>> GetObjectsInRegion(String region)
        {
            DataRegion dataRegion = default(DataRegion);

            lock (mutex)
            {
                if (!regions.TryGetValue(region, out dataRegion))
                {
                    dataRegion = new DataRegion();
                    regions[region] = dataRegion;
                }
            }

            return dataRegion.GetObjects();
        }
    }
}
