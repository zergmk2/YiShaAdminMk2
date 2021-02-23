using System;
using System.Collections.Generic;

namespace YiSha.Cache
{
    /// <summary>
    ///     缓存服务接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        ///     设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        void Set<T>(string key, T value, DateTime? expireTime = null);

        /// <summary>
        ///     移除缓存
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        ///     获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        ///     获取所有缓存键
        /// </summary>
        /// <returns></returns>
        List<string> GetAllKey();
    }
}
