using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.LinqBuilder;
using YiSha.Entity.SystemManage;
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
    ///     日 期：2020-12-04 11:25
    ///     描 述：菜单服务类
    /// </summary>
    public class MenuService : IMenuService, ITransient
    {
        private readonly IRepository<MenuEntity> _menuEntityDB;

        public MenuService(IRepository<MenuEntity> menuEntityDB)
        {
            _menuEntityDB = menuEntityDB;
        }

        #region 获取数据

        /// <summary>
        ///     带条件查询所有
        /// </summary>
        public async Task<List<MenuEntity>> GetList(MenuListParam param)
        {
            #region 查询条件

            var query = _menuEntityDB.AsQueryable();
            /*

            */

            #endregion

            var data = await query.ToListAsync();
            return data;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public async Task<List<MenuEntity>> GetPageList(MenuListParam param, Pagination pagination)
        {
            #region 查询条件

            var query = _menuEntityDB.AsQueryable();
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
        public async Task<MenuEntity> GetEntity(long id)
        {
            var list = await _menuEntityDB.AsAsyncEnumerable(p => p.Id == id);
            return list.FirstOrDefault();
        }

        /// <summary>
        ///     查询多个ID主键数据
        /// </summary>
        public async Task<List<MenuEntity>> GetListByIds(string ids)
        {
            if (ids.IsNullOrEmpty())
                throw new Exception("参数不合法！");

            var idArr = TextHelper.SplitToArray<long>(ids, ',').ToList();
            var data = await _menuEntityDB.AsAsyncEnumerable(a => idArr.Contains(a.Id.GetValueOrDefault()));

            return data;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(MenuEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                // 默认赋值
                entity.Id = IdGeneratorHelper.Instance.GetId();
                ;

                await _menuEntityDB.InsertNowAsync(entity);
            }
            else
            {
                // 默认赋值

                await _menuEntityDB.UpdateNowAsync(entity, ignoreNullValues: true);
            }
        }

        public async Task DeleteForm(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                throw new Exception("参数不合法！");

            var _ids = ids.Split(",");
            await _menuEntityDB.BatchDeleteAsync(_ids);
        }

        public Task<int> GetMaxSort(long parentId)
        {
            if (parentId > 0)
            {
                return Task.FromResult(_menuEntityDB.Entities.Where(e => e.ParentId == parentId)
                    .Max(entity => entity.MenuSort));
            }
            return Task.FromResult(_menuEntityDB.Entities.Max(entity => entity.MenuSort));
        }

        #endregion

        #region 私有方法

        #endregion
    }
}
