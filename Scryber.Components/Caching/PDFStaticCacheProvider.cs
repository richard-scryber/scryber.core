/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using Scryber;
using Scryber.Drawing;

namespace Scryber.Caching
{
    public class PDFStaticCacheProvider : IPDFCacheProvider
    {

        
        #region private class CacheEntry

        /// <summary>
        /// Inner class for holding an object and it's expiry time
        /// </summary>
        private class CacheEntry
        {
            internal object Data;
            internal DateTime Expires;

            internal bool IsExpired()
            {
                if (Expires == Scryber.Caching.PDFCacheProvider.NoAbsoluteExpiration)
                    return false;
                else
                    return DateTime.Now > Expires;
            }
        }

        #endregion


        /// <summary>
        /// Holds the references to a cache entry based on the string key.
        /// </summary>
        private static Dictionary<string, CacheEntry> _cache = new Dictionary<string,CacheEntry>(StringComparer.OrdinalIgnoreCase);
        
        /// <summary>
        /// Thread lock
        /// </summary>
        private static object _lock = new object();
        


        //
        // IPDFCacheProvider implementation
        //

        #region public bool TryRetrieveFromCache(string type, string key, out object data)

        /// <summary>
        /// Tries to get the required object from the static cache based on the specified key and type, returning true if a non-null value was found
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool TryRetrieveFromCache(string type, string key, out object data)
        {
            key = CombineKey(type, key);
            CacheEntry entry;
            lock (_lock)
            {
                _cache.TryGetValue(key, out entry);

                if (null == entry)
                    data = null;
                else if (entry.IsExpired())
                {
                    _cache.Remove(key);
                    data = null;
                }
                else
                    data = entry.Data;

            }
            return null != data;
        }

        #endregion 

        #region public void AddToCache(string type, string key, object data)

        /// <summary>
        /// Adds the item to the cache with no expriration
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void AddToCache(string type, string key, object data)
        {
            this.AddToCache(type, key, data, DateTime.MaxValue);
        }

        #endregion

        #region public void AddToCache(string type, string key, object data, TimeSpan duration)

        /// <summary>
        /// Adds the item to the cache and makes it available for the specified duration
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="duration"></param>
        public void AddToCache(string type, string key, object data, TimeSpan duration)
        {
            if (duration == TimeSpan.Zero)
                return;

            this.AddToCache(type, key, data, DateTime.Now.Add(duration));
        }

        #endregion


        //
        // support methods
        //

        #region public void AddToCache(string type, string key, object data, DateTime expires)

        /// <summary>
        /// Adds the specified object to the cache dictionary based on the type and key values, 
        /// and allows it to be retrievied until it expires.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expires"></param>
        public void AddToCache(string type, string key, object data, DateTime expires)
        {
            if (expires == Scryber.Caching.PDFCacheProvider.NoAbsoluteExpiration || expires > DateTime.Now)
            {
                key = CombineKey(type, key);

                lock (_lock)
                {
                    CacheEntry entry = new CacheEntry() { Expires = expires, Data = data };
                    _cache[key] = entry;
                }
            }
        }

        #endregion

        #region private static string CombineKey(string type, string key)

        /// <summary>
        /// joins the type and key into a unique key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string CombineKey(string type, string key)
        {
            if (null == type)
                throw RecordAndRaise.ArgumentNull("type");
            else if (null == key)
                throw RecordAndRaise.ArgumentNull("key");
            else
                return String.Concat("pdf:", type, ":", key);
        }

        #endregion
    }
}
