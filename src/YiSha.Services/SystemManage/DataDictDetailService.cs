using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
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
    ///     日 期：2020-12-19 08:59
    ///     描 述：数据字典值服务类
    /// </summary>
    public class DataDictDetailService : IDataDictDetailService, ITransient
    {
        private readonly IRepository<DataDictDetailEntity> _dataDictDetailEntityDB;

        public DataDictDetailService(IRepository<DataDictDetailEntity> dataDictDetailEntityDB)
        {
            _dataDictDetailEntityDB = dataDictDetailEntityDB;
        }

        #region 获取数据

        /// <summary>
        ///     带条件查询所有
        /// </summary>
        public async Task<List<DataDictDetailEntity>> GetList(DataDictDetailListParam param)
        {
            #region 查询条件

            var query = _dataDictDetailEntityDB.AsQueryable();

            // 字典类型(外键)
            if (!string.IsNullOrEmpty(param.DictType))
                query = query.Where(p => p.DictType.Contains(param.DictType));
            // 字典键(一般从1开始)
            if (param.DictKey.HasValue)
                query = query.Where(p => p.DictKey == param.DictKey);
            // 字典值
            if (!string.IsNullOrEmpty(param.DictValue))
                query = query.Where(p => p.DictValue.Contains(param.DictValue));

            #endregion

            var data = await query.ToListAsync();
            return data;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public async Task<List<DataDictDetailEntity>> GetPageList(DataDictDetailListParam param, Pagination pagination)
        {
            #region 查询条件

            var query = _dataDictDetailEntityDB.AsQueryable();
            /*
                  // 字典类型(外键)
                  if (!string.IsNullOrEmpty(param.DictType))
                      query = query.Where(p => p.DictType.Contains(param.DictType));
                  // 字典键(一般从1开始)
                  if (param.DictKey.HasValue)
                      query = query.Where(p => p.DictKey == param.DictKey);
                  // 字典值
                  if (!string.IsNullOrEmpty(param.DictValue))
                      query = query.Where(p => p.DictValue.Contains(param.DictValue));

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
        public async Task<DataDictDetailEntity> GetEntity(long id)
        {
            var list = await _dataDictDetailEntityDB.AsAsyncEnumerable(p => p.Id == id);
            return list.FirstOrDefault();
        }

        public async Task<int> GetMaxSort()
        {
            object result = await "SELECT MAX(positionSort) FROM DataDictDetailEntity".SqlNonQueryAsync();
            var sort = result.ParseToInt();
            sort++;
            return sort;
        }

        /// <summary>
        ///     查询多个ID主键数据
        /// </summary>
        public async Task<List<DataDictDetailEntity>> GetListByIds(string ids)
        {
            if (ids.IsNullOrEmpty())
                throw new Exception("参数不合法！");

            var idArr = TextHelper.SplitToArray<long>(ids, ',').ToList();
            var data = await _dataDictDetailEntityDB.AsAsyncEnumerable(a => idArr.Contains(a.Id.GetValueOrDefault()));

            return data;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(DataDictDetailEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                // 默认赋值
                entity.Id = IdGeneratorHelper.Instance.GetId();
                await _dataDictDetailEntityDB.InsertNowAsync(entity);
            }
            else
            {
                await _dataDictDetailEntityDB.UpdateNowAsync(entity, ignoreNullValues: true);
            }
        }

        public async Task DeleteForm(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                throw new Exception("参数不合法！");

            var _ids = ids.Split(",");
            await _dataDictDetailEntityDB.BatchDeleteAsync(_ids);
        }

        #endregion

        #region 私有方法

        #endregion
    }
}
