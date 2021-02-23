using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.LinqBuilder;
using YiSha.Entity;
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
    ///     日 期：2020-12-04 11:16
    ///     描 述：菜单权限服务类
    /// </summary>
    public class MenuAuthorizeService : IMenuAuthorizeService, ITransient
    {
        private readonly IRepository<MenuAuthorizeEntity> _menuAuthorizeEntityDB;

        public MenuAuthorizeService(IRepository<MenuAuthorizeEntity> menuAuthorizeEntityDB)
        {
            _menuAuthorizeEntityDB = menuAuthorizeEntityDB;
        }

        #region 获取数据

        /// <summary>
        ///     带条件查询所有
        /// </summary>
        public async Task<List<MenuAuthorizeEntity>> GetList(MenuAuthorizeListParam param)
        {
            #region 查询条件

            var query = _menuAuthorizeEntityDB.AsQueryable();
            /*

            */

            #endregion

            var data = await query.ToListAsync();
            return data;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public async Task<List<MenuAuthorizeEntity>> GetPageList(MenuAuthorizeListParam param, Pagination pagination)
        {
            #region 查询条件

            var query = _menuAuthorizeEntityDB.AsQueryable();
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
        public async Task<MenuAuthorizeEntity> GetEntity(long id)
        {
            var list = await _menuAuthorizeEntityDB.AsAsyncEnumerable(p => p.Id == id);
            return list.FirstOrDefault();
        }

        /// <summary>
        ///     查询多个ID主键数据
        /// </summary>
        public async Task<List<MenuAuthorizeEntity>> GetListByIds(string ids)
        {
            if (ids.IsNullOrEmpty())
                throw new Exception("参数不合法！");

            var idArr = TextHelper.SplitToArray<long>(ids, ',').ToList();
            var data = await _menuAuthorizeEntityDB.AsAsyncEnumerable(a => idArr.Contains(a.Id.GetValueOrDefault()));

            return data;
        }

        /// <summary>
        ///     通过权限条件 获取菜单id
        /// </summary>
        /// <param name="menuListParam"></param>
        /// <returns></returns>
        public async Task<List<long>> GetMenuIdList(MenuListParam param)
        {
            var query = _menuAuthorizeEntityDB.AsQueryable();
            if (param != null)
            {
                if (param.AuthorizeId.HasValue)
                    query = query.Where(t => t.AuthorizeId == param.AuthorizeId);

                if (param.AuthorizeType.HasValue)
                    query = query.Where(t => t.AuthorizeType == param.AuthorizeType);

                if (!param.AuthorizeIds.IsEmpty())
                {
                    var authorizeIdArr = TextHelper.SplitToArray<long>(param.AuthorizeIds, ',');
                    query = query.Where(t => authorizeIdArr.Contains(t.AuthorizeId.Value));
                }
            }

            var menuIds = await query.Select(a => a.MenuId.GetValueOrDefault()).ToListAsync();
            return menuIds;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(MenuAuthorizeEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                // 默认赋值
                entity.Id = IdGeneratorHelper.Instance.GetId();

                await _menuAuthorizeEntityDB.InsertNowAsync(entity);
            }
            else
            {
                // 默认赋值

                await _menuAuthorizeEntityDB.UpdateNowAsync(entity, ignoreNullValues: true);
            }
        }

        public async Task DeleteForm(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                throw new Exception("参数不合法！");

            var sql = "Delete From SysMenuAuthorize Where Id in (" + ids + ")";
            await _menuAuthorizeEntityDB.SqlNonQueryAsync(sql);
        }

        #endregion

        #region 私有方法

        #endregion
    }
}
