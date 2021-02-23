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
    public class MenuAuthorizeCache : ITransient
    {
        private readonly ICache _cache;
        private readonly IMenuAuthorizeService _menuAuthorizeService;
        private readonly string MenuAuthorizeCacheKey = CacheKeys.MenuAuthorizeCache.ParseToString();

        public MenuAuthorizeCache(Func<string, ISingleton, object> resolveNamed,
            IMenuAuthorizeService menuAuthorizeService)
        {
            _cache = resolveNamed(GlobalContext.SystemConfig.CacheService, default) as ICache;
            _menuAuthorizeService = menuAuthorizeService;
        }

        /// <summary>
        ///     查询缓存中的权限
        /// </summary>
        public async Task<List<MenuAuthorizeEntity>> GetMenyAuthorizeList()
        {
            var cacheList = _cache.Get<List<MenuAuthorizeEntity>>(MenuAuthorizeCacheKey);
            if (cacheList == null || cacheList.Count() == 0)
            {
                cacheList = await _menuAuthorizeService.GetList(null);
                _cache.Set(MenuAuthorizeCacheKey, cacheList);
            }

            return cacheList;
        }

        public void Remove()
        {
            _cache.Remove(MenuAuthorizeCacheKey);
        }
    }
}
