using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Cache;
using YiSha.Entity;
using YiSha.Enum;
using YiSha.Service.SystemManage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace YiSha.Business.Cache
{
    public class DataDictCache : ITransient
    {
        private DataDictService _dataDictService;
        private readonly ICache _cache;
        private readonly string _dataDictCacheKey = CacheKeys.DataDictCache.ParseToString();

        public DataDictCache(Func<string, ISingleton, object> resolveNamed, DataDictService dataDictService)
        {
            _dataDictService = dataDictService;
            _cache = resolveNamed(GlobalContext.SystemConfig.CacheService, default) as ICache;
        }


        public async Task<List<DataDictEntity>> GetList()
        {
            var cacheList = _cache.Get<List<DataDictEntity>>(_dataDictCacheKey);
            if (cacheList == null)
            {
                var list = await _dataDictService.GetList(null);
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
