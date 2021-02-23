using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using YiSha.Cache;
using YiSha.Entity;
using YiSha.Enum;
using YiSha.Enum.SystemManage;
using YiSha.IBusiness.SystemManage;
using YiSha.IService.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 11:16
    ///     描 述：菜单权限业务类
    /// </summary>
    public class MenuAuthorizeBLL : IMenuAuthorizeBLL, ITransient
    {
        private readonly MenuAuthorizeCache _menuAuthorizeCache;
        private readonly IMenuAuthorizeService _menuAuthorizeService;
        private readonly MenuCache _menuCache;

        public MenuAuthorizeBLL(IMenuAuthorizeService menuAuthorizeService, MenuCache menuCache,
            MenuAuthorizeCache menuAuthorizeCache)
        {
            _menuAuthorizeService = menuAuthorizeService;
            _menuCache = menuCache;
            _menuAuthorizeCache = menuAuthorizeCache;
        }

        #region 获取数据

        public async Task<TData<List<MenuAuthorizeEntity>>> GetList(MenuAuthorizeListParam param)
        {
            var obj = new TData<List<MenuAuthorizeEntity>>();
            obj.Data = await _menuAuthorizeService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<MenuAuthorizeEntity>>> GetPageList(MenuAuthorizeListParam param,
            Pagination pagination)
        {
            var obj = new TData<List<MenuAuthorizeEntity>>();
            obj.Data = await _menuAuthorizeService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<MenuAuthorizeEntity>> GetEntity(long id)
        {
            var obj = new TData<MenuAuthorizeEntity>();
            obj.Data = await _menuAuthorizeService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }


        public async Task<TData<List<MenuAuthorizeInfo>>> GetAuthorizeList(OperatorInfo user)
        {
            var obj = new TData<List<MenuAuthorizeInfo>>();
            obj.Data = new List<MenuAuthorizeInfo>();

            var authorizeList = new List<MenuAuthorizeEntity>();
            List<MenuAuthorizeEntity> userAuthorizeList = null;
            List<MenuAuthorizeEntity> roleAuthorizeList = null;

            var menuAuthorizeCacheList = await _menuAuthorizeCache.GetMenyAuthorizeList();
            var menuList = await _menuCache.GetMenuCacheList();
            var enableMenuIdList = menuList.Where(p => p.MenuStatus == (int) StatusEnum.Yes).Select(p => p.Id).ToList();

            menuAuthorizeCacheList = menuAuthorizeCacheList.Where(p => enableMenuIdList.Contains(p.MenuId)).ToList();

            // 用户
            userAuthorizeList = menuAuthorizeCacheList.Where(p =>
                p.AuthorizeId == user.UserId && p.AuthorizeType == AuthorizeTypeEnum.User.ParseToInt()).ToList();

            // 角色
            if (!string.IsNullOrEmpty(user.RoleIds))
            {
                var roleIdList = user.RoleIds.Split(',').Select(p => long.Parse(p)).ToList();
                roleAuthorizeList = menuAuthorizeCacheList.Where(p =>
                    roleIdList.Contains(p.AuthorizeId.GetValueOrDefault()) &&
                    p.AuthorizeType == AuthorizeTypeEnum.Role.ParseToInt()).ToList();
            }

            // 排除重复的记录
            if (userAuthorizeList.Count > 0)
            {
                authorizeList.AddRange(userAuthorizeList);
                roleAuthorizeList = roleAuthorizeList
                    .Where(p => !userAuthorizeList.Select(u => u.AuthorizeId).Contains(p.AuthorizeId)).ToList();
            }

            if (roleAuthorizeList != null && roleAuthorizeList.Count > 0) authorizeList.AddRange(roleAuthorizeList);

            foreach (var authorize in authorizeList)
                obj.Data.Add(new MenuAuthorizeInfo
                {
                    MenuId = authorize.MenuId,
                    AuthorizeId = authorize.AuthorizeId,
                    AuthorizeType = authorize.AuthorizeType,
                    Authorize = menuList.Where(t => t.Id == authorize.MenuId).Select(t => t.Authorize).FirstOrDefault()
                });
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(MenuAuthorizeEntity entity)
        {
            var obj = new TData<string>();
            await _menuAuthorizeService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            var obj = new TData();
            await _menuAuthorizeService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}
