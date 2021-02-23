using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using Furion.LinqBuilder;
using YiSha.Entity;
using YiSha.Enum.SystemManage;
using Microsoft.EntityFrameworkCore;
using YiSha.IService.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Service.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-06 09:46
    ///     描 述：角色信息服务类
    /// </summary>
    public class RoleService : IRoleService, ITransient
    {
        private readonly IRepository<MenuAuthorizeEntity> _menuAuthorizeEntityDB;
        private readonly IRepository<RoleEntity> _roleEntityDB;

        public RoleService(IRepository<RoleEntity> roleEntityDB, IRepository<MenuAuthorizeEntity> menuAuthorizeEntityDB)
        {
            _roleEntityDB = roleEntityDB;
            _menuAuthorizeEntityDB = menuAuthorizeEntityDB;
        }

        #region 获取数据

        /// <summary>
        ///     带条件查询所有
        /// </summary>
        public async Task<List<RoleEntity>> GetList(RoleListParam param)
        {
            #region 查询条件

            var query = _roleEntityDB.AsQueryable();
            /*

            */

            #endregion

            var data = await query.ToListAsync();
            return data;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public async Task<List<RoleEntity>> GetPageList(RoleListParam param, Pagination pagination)
        {
            #region 查询条件

            var query = _roleEntityDB.AsQueryable();
            /*

            */
            var data = await query.OrderByDescending(a => a.Id)
                .ToPagedListAsync(pagination.PageIndex, pagination.PageSize);

            #endregion

            // 分页参数赋值
            pagination.TotalCount = data.TotalCount;
            return data.Items.ToList();
        }

        /// <summary>
        ///     根据ID获取对象
        /// </summary>
        public async Task<RoleEntity> GetEntity(long id)
        {
            var list = await _roleEntityDB.AsAsyncEnumerable(p => p.Id == id);
            return list.FirstOrDefault();
        }

        /// <summary>
        ///     查询多个ID主键数据
        /// </summary>
        public async Task<List<RoleEntity>> GetListByIds(string ids)
        {
            if (ids.IsNullOrEmpty())
                throw new Exception("参数不合法！");

            var idArr = TextHelper.SplitToArray<long>(ids, ',').ToList();
            var data = await _roleEntityDB.AsAsyncEnumerable(a => idArr.Contains(a.Id.GetValueOrDefault()));

            return data;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(RoleEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                // 默认赋值
                entity.Id = IdGeneratorHelper.Instance.GetId();
                entity.CreatorId = NetHelper.HttpContext.User.FindFirstValue("UserId").ParseToLong();
                entity.CreateTime = DateTime.Now;
                await _roleEntityDB.InsertNowAsync(entity);
            }
            else
            {
                await _roleEntityDB.UpdateIncludeExistsNowAsync(entity,
                    new[]
                    {
                        nameof(RoleEntity.RoleName), nameof(RoleEntity.RoleSort), nameof(RoleEntity.RoleStatus),
                        nameof(RoleEntity.Remark)
                    }, false);
            }
        }

        public async Task DeleteForm(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                throw new Exception("参数不合法！");

            var _ids = ids.Split(",");
            await _roleEntityDB.BatchDeleteAsync(_ids);
        }

        public async Task SaveRoleAuth(long roleId, string menuIds)
        {
            var items = await _menuAuthorizeEntityDB.Where(a => a.AuthorizeId == roleId).ToListAsync();
            foreach (var item in items)
                await item.DeleteNowAsync();

            // 角色对应的菜单、页面和按钮权限
            if (!string.IsNullOrEmpty(menuIds))
                foreach (var menuId in TextHelper.SplitToArray<long>(menuIds, ','))
                {
                    var menuAuthorizeEntity = new MenuAuthorizeEntity();
                    menuAuthorizeEntity.AuthorizeId = roleId;
                    menuAuthorizeEntity.MenuId = menuId;
                    menuAuthorizeEntity.AuthorizeType = AuthorizeTypeEnum.Role.ParseToInt();
                    menuAuthorizeEntity.Id = IdGeneratorHelper.Instance.GetId();
                    menuAuthorizeEntity.CreatorId = NetHelper.HttpContext.User.FindFirstValue("UserId").ParseToLong();
                    menuAuthorizeEntity.CreateTime = DateTime.Now;
                    await _menuAuthorizeEntityDB.InsertNowAsync(menuAuthorizeEntity);
                }
        }

        #endregion

        #region 私有方法

        #endregion
    }
}
