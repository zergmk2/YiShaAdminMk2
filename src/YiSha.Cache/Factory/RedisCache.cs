using System;
using System.Collections.Generic;
using System.Linq;
using Furion.DependencyInjection;
using YiSha.Util;
using YiSha.Util.Helper;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace YiSha.Cache.Factory
{
    /// <summary>
    ///     redis缓存
    /// </summary>
    public class RedisCache : ISingleton, ICache
    {
        private readonly IDatabase cache;
        private readonly ConnectionMultiplexer connection;

        public RedisCache()
        {
            try
            {
                connection = ConnectionMultiplexer.Connect(GlobalContext.SystemConfig.RedisConnectionString);
                cache = connection.GetDatabase();
            }
            catch (Exception ex)
            {
                LogHelper.Error("redis连接失败：", ex);
                throw new Exception("redis连接失败，请查看日志文件！");
            }
        }

        /// <summary>
        ///     新增/修改 缓存
        /// </summary>
        public void Set<T>(string key, T value, DateTime? expireTime = null)
        {
            var jsonOption = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var strValue = JsonConvert.SerializeObject(value, jsonOption);

            if (expireTime == null)
                cache.StringSet(key, strValue);
            else
                cache.StringSet(key, strValue, expireTime.Value - DateTime.Now);
        }

        /// <summary>
        ///     删除缓存
        /// </summary>
        public void Remove(string key)
        {
            cache.KeyDelete(key);
        }

        /// <summary>
        ///     获取缓存
        /// </summary>
        public T Get<T>(string key)
        {
            var t = default(T);
            try
            {
                var value = cache.StringGet(key);
                if (string.IsNullOrEmpty(value)) return t;
                t = JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return t;
        }

        public List<string> GetAllKey()
        {
            var options = ConfigurationOptions.Parse(GlobalContext.SystemConfig.RedisConnectionString);
            var keys = connection.GetServer(options.EndPoints.First()).Keys();
            var list = keys.Select(a => a.ParseToString()).ToList();
            return list;
        }

        public void Dispose()
        {
            if (connection != null) connection.Close();
            GC.SuppressFinalize(this);
        }
    }
}
