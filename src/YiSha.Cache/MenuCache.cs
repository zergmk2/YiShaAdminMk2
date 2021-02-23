using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using YiSha.Entity.SystemManage;
using YiSha.Enum;
using YiSha.IService.SystemManage;
using YiSha.Util;

namespace YiSha.Cache
{
    public class MenuCache : ITransient
    {
        private readonly ICache _cache;
        private readonly IMenuService _menuService;
        private readonly string MenuCacheKey = CacheKeys.MenuCache.ParseToString();

        public MenuCache(Func<string, ISingleton, object> resolveNamed, IMenuService menuService)
        {
            _cache = resolveNamed(GlobalContext.SystemConfig.CacheService, default) as ICache;
            _menuService = menuService;
        }


        public async Task<List<MenuEntity>> GetMenuCacheList()
        {
            var cacheList = _cache.Get<List<MenuEntity>>(MenuCacheKey);
            if (cacheList == null || cacheList.Count() == 0)
            {
                cacheList = await _menuService.GetList(null);
                _cache.Set(MenuCacheKey, cacheList);
            }

            return cacheList;
        }

        public void Remove()
        {
            _cache.Remove(MenuCacheKey);
        }
    }
}
