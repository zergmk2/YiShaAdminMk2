using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using YiSha.Cache;
using YiSha.Entity.SystemManage;
using YiSha.IBusiness.SystemManage;
using YiSha.IService.SystemManage;
using YiSha.Model;
using YiSha.Model.Param.SystemManage;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 11:25
    ///     描 述：菜单业务类
    /// </summary>
    public class MenuBLL : IMenuBLL, ITransient
    {
        private readonly MenuCache _menuCache;
        private readonly IMenuService _menuService;

        public MenuBLL(IMenuService menuService, MenuCache menuCache)
        {
            _menuService = menuService;
            _menuCache = menuCache;
        }


        #region 私有方法

        private List<MenuEntity> ListFilter(MenuListParam param, List<MenuEntity> list)
        {
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.MenuName))
                    list = list.Where(p => p.MenuName.Contains(param.MenuName)).ToList();
                if (param.MenuStatus > 0) list = list.Where(p => p.MenuStatus == param.MenuStatus).ToList();
            }

            return list;
        }

        #endregion

        #region 获取数据

        public async Task<TData<List<MenuEntity>>> GetList(MenuListParam param)
        {
            var obj = new TData<List<MenuEntity>>();
            obj.Data = await _menuService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<MenuEntity>>> GetPageList(MenuListParam param, Pagination pagination)
        {
            var obj = new TData<List<MenuEntity>>();
            obj.Data = await _menuService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<MenuEntity>> GetEntity(long id)
        {
            var obj = new TData<MenuEntity>();
            obj.Data = await _menuService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeList(MenuListParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Data = new List<ZtreeInfo>();

            var list = await _menuCache.GetMenuCacheList();
            list = ListFilter(param, list);

            foreach (var menu in list)
                obj.Data.Add(new ZtreeInfo
                {
                    id = menu.Id,
                    pId = menu.ParentId,
                    name = menu.MenuName
                });

            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(MenuEntity entity)
        {
            var obj = new TData<string>();
            await _menuService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            var obj = new TData();
            await _menuService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        public async Task<TData<int>> GetMaxSort(long parentId)
        {
            TData<int> obj = new TData<int>();
            obj.Data = await _menuService.GetMaxSort(parentId);
            obj.Tag = 1;
            return obj;
        }
    }
}
