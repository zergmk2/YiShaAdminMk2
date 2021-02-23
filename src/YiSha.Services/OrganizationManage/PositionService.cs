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
    ///     日 期：2020-12-06 14:10
    ///     描 述：职位信息服务类
    /// </summary>
    public class PositionService : IPositionService, ITransient
    {
        private readonly IRepository<PositionEntity> _positionEntityDB;

        public PositionService(IRepository<PositionEntity> positionEntityDB)
        {
            _positionEntityDB = positionEntityDB;
        }

        #region 获取数据

        /// <summary>
        ///     带条件查询所有
        /// </summary>
        public async Task<List<PositionEntity>> GetList(PositionListParam param)
        {
            #region 查询条件

            var query = _positionEntityDB.AsQueryable();
            /*
            // 职位名称
            if (!string.IsNullOrEmpty(param.PositionName))
                query = query.Where(p => p.PositionName.Contains(param.PositionName));

            */

            #endregion

            var data = await query.ToListAsync();
            return data;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public async Task<List<PositionEntity>> GetPageList(PositionListParam param, Pagination pagination)
        {
            #region 查询条件

            var query = _positionEntityDB.AsQueryable();
            /*
            // 职位名称
            if (!string.IsNullOrEmpty(param.PositionName))
                query = query.Where(p => p.PositionName.Contains(param.PositionName));

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
        public async Task<PositionEntity> GetEntity(long id)
        {
            var list = await _positionEntityDB.AsAsyncEnumerable(p => p.Id == id);
            return list.FirstOrDefault();
        }

        /// <summary>
        ///     查询多个ID主键数据
        /// </summary>
        public async Task<List<PositionEntity>> GetListByIds(string ids)
        {
            if (ids.IsNullOrEmpty())
                throw new Exception("参数不合法！");

            var idArr = TextHelper.SplitToArray<long>(ids, ',').ToList();
            var data = await _positionEntityDB.AsAsyncEnumerable(a => idArr.Contains(a.Id.GetValueOrDefault()));

            return data;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(PositionEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                // 默认赋值
                entity.Id = IdGeneratorHelper.Instance.GetId();
                await _positionEntityDB.InsertNowAsync(entity);
            }
            else
            {
                await _positionEntityDB.UpdateNowAsync(entity, ignoreNullValues: true);
            }
        }

        public async Task DeleteForm(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                throw new Exception("参数不合法！");

            var sql = "Delete From SysPosition Where Id in (" + ids + ")";
            await _positionEntityDB.SqlNonQueryAsync(sql);
        }

        #endregion

        #region 私有方法

        #endregion
    }
}
