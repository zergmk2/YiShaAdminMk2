using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.LinqBuilder;
using YiSha.Entity;
using Microsoft.EntityFrameworkCore;
using YiSha.IService.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Service.OrganizationManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-06 14:12
    ///     描 述：部门信息服务类
    /// </summary>
    public class DepartmentService : IDepartmentService, ITransient
    {
        private readonly IRepository<DepartmentEntity> _departmentEntityDB;

        public DepartmentService(IRepository<DepartmentEntity> departmentEntityDB)
        {
            _departmentEntityDB = departmentEntityDB;
        }

        #region 获取数据

        /// <summary>
        ///     带条件查询所有
        /// </summary>
        public async Task<List<DepartmentEntity>> GetList(DepartmentListParam param)
        {
            #region 查询条件

            var query = _departmentEntityDB.AsQueryable();
            /*
            // 部门名称
            if (!string.IsNullOrEmpty(param.DepartmentName))
                query = query.Where(p => p.DepartmentName.Contains(param.DepartmentName));

            */

            #endregion

            var data = await query.ToListAsync();
            return data;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public async Task<List<DepartmentEntity>> GetPageList(DepartmentListParam param, Pagination pagination)
        {
            #region 查询条件

            var query = _departmentEntityDB.AsQueryable();
            /*
            // 部门名称
            if (!string.IsNullOrEmpty(param.DepartmentName))
                query = query.Where(p => p.DepartmentName.Contains(param.DepartmentName));

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
        public async Task<DepartmentEntity> GetEntity(long id)
        {
            var list = await _departmentEntityDB.AsAsyncEnumerable(p => p.Id == id);
            return list.FirstOrDefault();
        }

        /// <summary>
        ///     查询多个ID主键数据
        /// </summary>
        public async Task<List<DepartmentEntity>> GetListByIds(string ids)
        {
            if (ids.IsNullOrEmpty())
                throw new Exception("参数不合法！");

            var idArr = TextHelper.SplitToArray<long>(ids, ',').ToList();
            var data = await _departmentEntityDB.AsAsyncEnumerable(a => idArr.Contains(a.Id.GetValueOrDefault()));

            return data;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(DepartmentEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                // 默认赋值
                entity.Id = IdGeneratorHelper.Instance.GetId();
                await _departmentEntityDB.InsertNowAsync(entity);
            }
            else
            {
                await _departmentEntityDB.UpdateNowAsync(entity, ignoreNullValues: true);
            }
        }

        public async Task DeleteForm(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                throw new Exception("参数不合法！");

            var _ids = ids.Split(",");
            await _departmentEntityDB.BatchDeleteAsync(_ids);
        }

        #endregion

        #region 私有方法

        #endregion
    }
}
