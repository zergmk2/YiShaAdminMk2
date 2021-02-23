using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using YiSha.Entity;
using YiSha.Enum;
using YiSha.IService.SystemManage;
using YiSha.Util;

namespace YiSha.Cache
{
    public class ApiAuthorizeCache : ITransient
    {
        private readonly IApiAuthorizeService _apiAuthorizeService;

        private readonly ICache _cache;
        private readonly string _cacheKey = CacheKeys.ApiAuthorizeCache.ParseToString();

        public ApiAuthorizeCache(Func<string, ISingleton, object> resolveNamed,
            IApiAuthorizeService apiAuthorizeService)
        {
            _cache = resolveNamed(GlobalContext.SystemConfig.CacheService, default) as ICache;
            _apiAuthorizeService = apiAuthorizeService;
        }

        /// <summary>
        ///     根据url获取权限标识
        /// </summary>
        public async Task<List<ApiAuthorizeEntity>> GetAuthorizeByUrl(string url = "")
        {
            var list = _cache.Get<List<ApiAuthorizeEntity>>(_cacheKey);
            if (list == null || list.Count() == 0)
            {
                list = await _apiAuthorizeService.GetList(null);
                _cache.Set(_cacheKey, list);
            }

            if (url.IsEmpty())
                return list;

            return list.Where(a => a.Url.ToLower() == url.ToLower()).ToList();
        }
    }
}
