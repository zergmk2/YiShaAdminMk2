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
    ///     创 建：
    ///     日 期：2020-12-04 12:49
    ///     描 述：Api日志服务类
    /// </summary>
    public class LogApiService : ILogApiService, ITransient
    {
        private readonly IRepository<LogApiEntity> _logApiEntityDB;
        private readonly IRepository<UserEntity> _userEntityDB;

        public LogApiService(IRepository<LogApiEntity> logApiEntityDB, IRepository<UserEntity> userEntityDB)
        {
            _logApiEntityDB = logApiEntityDB;
            _userEntityDB = userEntityDB;
        }

        #region 私有方法

        private IQueryable<LogApiEntity> ListFilter(LogApiListParam param)
        {
            var query = from a in _logApiEntityDB.AsQueryable()
                join b in _userEntityDB.AsQueryable() on a.CreatorId equals b.Id
                    into ab
                from res in ab.DefaultIfEmpty()
                select new LogApiEntity
                {
                    Id = a.Id,
                    Remark = a.Remark,
                    UserName = res.UserName,
                    ExecuteParam = a.ExecuteParam,
                    ExecuteResult = a.ExecuteResult,
                    ExecuteTime = a.ExecuteTime,
                    ExecuteUrl = a.ExecuteUrl,
                    LogStatus = a.LogStatus,
                    IpAddress = a.IpAddress,
                    IpLocation = IpLocationHelper.GetIpLocation(a.IpAddress),
                    CreateTime = a.CreateTime
                };

            if (!string.IsNullOrEmpty(param.UserName))
                query = query.Where(p => p.UserName.Contains(param.UserName));

            if (param.LogStatus > -1)
                query = query.Where(p => p.LogStatus == param.LogStatus);

            if (!string.IsNullOrEmpty(param.IpAddress))
                query = query.Where(p => p.IpAddress.Contains(param.IpAddress));

            if (param.StartTime.HasValue)
                query = query.Where(p => p.CreateTime >= param.StartTime);

            if (param.EndTime.HasValue)
                query = query.Where(p => p.CreateTime <= param.EndTime.Value.AddDays(1));

            return query;
        }

        #endregion

        #region 获取数据

        /// <summary>
        ///     带条件查询所有
        /// </summary>
        public async Task<List<LogApiEntity>> GetList(LogApiListParam param)
        {
            #region 查询条件

            var query = _logApiEntityDB.AsQueryable();
            /*
            // 执行状态(0失败 1成功)
            if (param.LogStatus.HasValue)
                query = query.Where(p => p.LogStatus == param.LogStatus);
            // 接口地址
            if (!string.IsNullOrEmpty(param.ExecuteUrl))
                query = query.Where(p => p.ExecuteUrl.Contains(param.ExecuteUrl));

            */

            #endregion

            var data = await query.ToListAsync();
            return data;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public async Task<List<LogApiEntity>> GetPageList(LogApiListParam param, Pagination pagination)
        {
            #region 查询条件

            var query = ListFilter(param);

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
        public async Task<LogApiEntity> GetEntity(long id)
        {
            var list = await _logApiEntityDB.AsAsyncEnumerable(p => p.Id == id);
            return list.FirstOrDefault();
        }

        /// <summary>
        ///     查询多个ID主键数据
        /// </summary>
        public async Task<List<LogApiEntity>> GetListByIds(string ids)
        {
            if (ids.IsNullOrEmpty())
                throw new Exception("参数不合法！");

            var idArr = TextHelper.SplitToArray<long>(ids, ',').ToList();
            var data = await _logApiEntityDB.AsAsyncEnumerable(a => idArr.Contains(a.Id.GetValueOrDefault()));

            return data;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(LogApiEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                // 默认赋值
                entity.Id = IdGeneratorHelper.Instance.GetId();
                ;
                await _logApiEntityDB.InsertNowAsync(entity);
            }
            else
            {
                await _logApiEntityDB.UpdateNowAsync(entity, ignoreNullValues: true);
            }
        }

        public async Task DeleteForm(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                throw new Exception("参数不合法！");

            var sql = "Delete From SysLogApi Where Id in (" + ids + ")";
            await _logApiEntityDB.SqlNonQueryAsync(sql);
        }

        #endregion
    }
}
