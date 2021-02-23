using System.Collections.Generic;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using YiSha.Cache;
using YiSha.Entity;
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
    ///     日 期：2020-12-06 09:46
    ///     描 述：角色信息业务类
    /// </summary>
    public class RoleBLL : IRoleBLL, ITransient
    {
        private MenuAuthorizeCache _menuAuthorizeCache;
        private readonly IMenuAuthorizeService _menuAuthorizeService;
        private readonly IRoleService _roleService;

        public RoleBLL(IRoleService roleService, IMenuAuthorizeService menuAuthorizeService,
            MenuAuthorizeCache menuAuthorizeCache)
        {
            _roleService = roleService;
            _menuAuthorizeService = menuAuthorizeService;
            _menuAuthorizeCache = menuAuthorizeCache;
        }

        #region 获取数据

        public async Task<TData<List<RoleEntity>>> GetList(RoleListParam param)
        {
            var obj = new TData<List<RoleEntity>>();
            obj.Data = await _roleService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<RoleEntity>>> GetPageList(RoleListParam param, Pagination pagination)
        {
            var obj = new TData<List<RoleEntity>>();
            obj.Data = await _roleService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<RoleEntity>> GetEntity(long id)
        {
            var obj = new TData<RoleEntity>();
            obj.Data = await _roleService.GetEntity(id);

            var menuIds = await _menuAuthorizeService.GetMenuIdList(new MenuListParam
            {
                AuthorizeId = id,
                AuthorizeType = AuthorizeTypeEnum.Role.ParseToInt()
            });

            // 获取角色对应的权限
            obj.Data.MenuIds = string.Join(",", menuIds);

            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        ///     保存权限
        /// </summary>
        public async Task<TData> SaveRoleAuth(long roleId, string menuIds)
        {
            var obj = new TData();
            await _roleService.SaveRoleAuth(roleId, menuIds);

            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(RoleEntity entity)
        {
            var obj = new TData<string>();
            await _roleService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            var obj = new TData();
            await _roleService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}
