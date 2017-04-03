using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Cache
{
    public class InMemoryCache : ICache
    {
        static readonly ObjectCache Cache = MemoryCache.Default;
        private static int timeout = 10;//int.Parse(ConfigurationManager.AppSettings["CacheTimeoutMinutes"]);
       
        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        public T Get<T>(string key) where T : class
        {
            try
            {
                return (T)Cache[key];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="objectToCache">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public void Add<T>(T objectToCache, string key) where T : class
        {
            Cache.Add(key, objectToCache, DateTime.Now.AddMinutes(timeout));
        }

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <param name="objectToCache">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public void Add(object objectToCache, string key)
        {
            Cache.Add(key, objectToCache, DateTime.Now.AddMinutes(timeout));
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public void Clear(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return Cache.Get(key) != null;
        }

        /// <summary>
        /// Gets all cached items as a list by their key.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAll()
        {
            return Cache.Select(keyValuePair => keyValuePair.Key).ToList();
        }
    }
}
