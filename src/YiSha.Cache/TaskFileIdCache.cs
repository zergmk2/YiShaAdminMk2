using System;
using Furion.DependencyInjection;
using YiSha.Util;
using YiSha.Enum;

namespace YiSha.Cache
{
    public class TaskFileIdCache : ITransient
    {
        private readonly ICache _cache;
        private readonly string _cacheKey = CacheKeys.TaskFileIdCache.ParseToString();

        public TaskFileIdCache(Func<string, ISingleton, object> resolveNamed)
        {
            _cache = resolveNamed(GlobalContext.SystemConfig.CacheService, default) as ICache;
        }

        public void SetTaskFileId(int taskId, string uuid)
        {
            _cache.Set($"{_cacheKey}-{taskId}", uuid, DateTime.Now.AddDays(7));
        }

        public string GetAndRemoveTaskFileId(int taskId)
        {
           var uuid = _cache.Get<string>($"{_cacheKey}-{taskId}");
           _cache.Remove($"{_cacheKey}-{taskId}");
           return uuid;
        }
    }
}
