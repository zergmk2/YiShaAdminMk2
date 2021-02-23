using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Furion.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;

namespace YiSha.Cache.Factory
{
    public class MemoryCache : ISingleton, ICache
    {
        private readonly IMemoryCache cache;

        public MemoryCache(IMemoryCache memoryCache)
        {
            cache = memoryCache;
        }

        /// <summary>
        ///     新增/修改 缓存
        /// </summary>
        public void Set<T>(string key, T value, DateTime? expireTime = null)
        {
            if (expireTime.HasValue)
                cache.Set(key, value, expireTime.Value);
            else
                cache.Set(key, value);
        }

        /// <summary>
        ///     删除缓存
        /// </summary>
        public void Remove(string key)
        {
            cache.Remove(key);
        }

        /// <summary>
        ///     获取缓存
        /// </summary>
        public T Get<T>(string key)
        {
            return cache.Get<T>(key);
        }

        public List<string> GetAllKey()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = cache.GetType().GetField("_entries", flags).GetValue(cache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                var key = cacheItem.Key.ToString();
                if (key.StartsWith("mini-profiler"))
                    continue;

                keys.Add(key);
            }

            return keys;
        }
    }
}
