using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using YiSha.Util;
using YiSha.Cache;
using YiSha.Entity;
using YiSha.Enum;
using YiSha.Service.SystemManage;

namespace YiSha.Business.Cache
{
    public class DataDictDetailCache : ITransient
    {
        private DataDictDetailService _dataDictDetailService;
        private readonly ICache _cache;
        private readonly string _dataDictCacheKey = CacheKeys.DataDictDetailCache.ParseToString();

        public DataDictDetailCache(Func<string, ISingleton, object> resolveNamed, DataDictDetailService dataDictDetailService)
        {
            _cache = resolveNamed(GlobalContext.SystemConfig.CacheService, default) as ICache;
            _dataDictDetailService = dataDictDetailService;
        }

        public async Task<List<DataDictDetailEntity>> GetList()
        {
            var cacheList = _cache.Get<List<DataDictDetailEntity>>(_dataDictCacheKey);
            if (cacheList == null)
            {
                var list = await _dataDictDetailService.GetList(null);
                _cache.Set(_dataDictCacheKey, list);
                return list;
            }
            else
            {
                return cacheList;
            }
        }
    }
}
