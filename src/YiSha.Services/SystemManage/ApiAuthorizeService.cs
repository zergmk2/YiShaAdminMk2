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
    ///     日 期：2020-12-04 16:22
    ///     描 述：接口权限服务类
    /// </summary>
    public class ApiAuthorizeService : IApiAuthorizeService, ITransient
    {
        private readonly IRepository<ApiAuthorizeEntity> _apiAuthorizeEntityDB;

        public ApiAuthorizeService(IRepository<ApiAuthorizeEntity> apiAuthorizeEntityDB)
        {
            _apiAuthorizeEntityDB = apiAuthorizeEntityDB;
        }

        #region 获取数据

        /// <summary>
        ///     带条件查询所有
        /// </summary>
        public async Task<List<ApiAuthorizeEntity>> GetList(ApiAuthorizeListParam param)
        {
            #region 查询条件

            var query = _apiAuthorizeEntityDB.AsQueryable();

            if (param != null)
            {
                if (!param.Authorize.IsEmpty())
                    query = query.Where(a => a.Authorize == param.Authorize);

                if (!param.Url.IsEmpty())
                    query = query.Where(a => a.Url == param.Url);
            }

            #endregion

            var data = await query.ToListAsync();
            return data;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public async Task<List<ApiAuthorizeEntity>> GetPageList(ApiAuthorizeListParam param, Pagination pagination)
        {
            #region 查询条件

            var query = _apiAuthorizeEntityDB.AsQueryable();
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
        public async Task<ApiAuthorizeEntity> GetEntity(long id)
        {
            var list = await _apiAuthorizeEntityDB.AsAsyncEnumerable(p => p.Id == id);
            return list.FirstOrDefault();
        }

        public async Task<int> GetMaxSort()
        {
            object result = await "SELECT MAX(positionSort) FROM ApiAuthorizeEntity".SqlNonQueryAsync();
            var sort = result.ParseToInt();
            sort++;
            return sort;
        }

        /// <summary>
        ///     查询多个ID主键数据
        /// </summary>
        public async Task<List<ApiAuthorizeEntity>> GetListByIds(string ids)
        {
            if (ids.IsNullOrEmpty())
                throw new Exception("参数不合法！");

            var idArr = TextHelper.SplitToArray<long>(ids, ',').ToList();
            var data = await _apiAuthorizeEntityDB.AsAsyncEnumerable(a => idArr.Contains(a.Id.GetValueOrDefault()));

            return data;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(ApiAuthorizeEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                // 默认赋值
                entity.Id = IdGeneratorHelper.Instance.GetId();
                await _apiAuthorizeEntityDB.InsertNowAsync(entity);
            }
            else
            {
                await _apiAuthorizeEntityDB.UpdateNowAsync(entity, ignoreNullValues: true);
            }
        }

        public async Task DeleteForm(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                throw new Exception("参数不合法！");

            var _ids = ids.Split(",");
            await _apiAuthorizeEntityDB.BatchDeleteAsync(_ids);
        }


        /// <summary>
        ///     根据权限标识删除数据
        /// </summary>
        /// <param name="authorize"></param>
        /// <returns></returns>
        public async Task DeleteByAuthorize(string authorize)
        {
            await "Delete From SysApiAuthorize Where Authorize=@authorize".SqlNonQueryAsync(new {authorize});
        }

        /// <summary>
        ///     批量插入数据
        /// </summary>
        public async Task AddAccess(List<ApiAuthorizeEntity> apiAuthorizes)
        {
            foreach (var item in apiAuthorizes)
                await item.InsertNowAsync();
        }

        #endregion

        #region 私有方法

        #endregion
    }
}
