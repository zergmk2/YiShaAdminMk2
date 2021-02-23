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
    ///     日 期：2020-12-04 12:55
    ///     描 述：登陆日志服务类
    /// </summary>
    public class LogLoginService : ILogLoginService, ITransient
    {
        private readonly IRepository<LogLoginEntity> _logLoginEntityDB;
        private readonly IRepository<UserEntity> _userEntityDB;

        public LogLoginService(IRepository<LogLoginEntity> logLoginEntityDB, IRepository<UserEntity> userEntityDB)
        {
            _logLoginEntityDB = logLoginEntityDB;
            _userEntityDB = userEntityDB;
        }

        #region 私有方法

        private IQueryable<LogLoginEntity> ListFilter(LogLoginListParam param)
        {
            var query = from a in _logLoginEntityDB.AsQueryable()
                join b in _userEntityDB.AsQueryable() on a.CreatorId equals b.Id
                    into ab
                from res in ab.DefaultIfEmpty()
                select new LogLoginEntity
                {
                    Id = a.Id,
                    Browser = a.Browser,
                    ExtraRemark = a.ExtraRemark,
                    IpAddress = a.IpAddress,
                    IpLocation = a.IpLocation,
                    LogStatus = a.LogStatus,
                    OS = a.OS,
                    Remark = a.Remark,
                    UserName = res.UserName,
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
        public async Task<List<LogLoginEntity>> GetList(LogLoginListParam param)
        {
            #region 查询条件

            var query = _logLoginEntityDB.AsQueryable();
            /*
            // ip地址
            if (!string.IsNullOrEmpty(param.IpAddress))
                query = query.Where(p => p.IpAddress.Contains(param.IpAddress));
            // 浏览器
            if (!string.IsNullOrEmpty(param.Browser))
                query = query.Where(p => p.Browser.Contains(param.Browser));
            // 操作系统
            if (!string.IsNullOrEmpty(param.OS))
                query = query.Where(p => p.OS.Contains(param.OS));

            */

            #endregion

            var data = await query.ToListAsync();
            return data;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public async Task<List<LogLoginEntity>> GetPageList(LogLoginListParam param, Pagination pagination)
        {
            #region 查询条件

            var query = _logLoginEntityDB.AsQueryable();
            /*
            // ip地址
            if (!string.IsNullOrEmpty(param.IpAddress))
                query = query.Where(p => p.IpAddress.Contains(param.IpAddress));
            // 浏览器
            if (!string.IsNullOrEmpty(param.Browser))
                query = query.Where(p => p.Browser.Contains(param.Browser));
            // 操作系统
            if (!string.IsNullOrEmpty(param.OS))
                query = query.Where(p => p.OS.Contains(param.OS));

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
        public async Task<LogLoginEntity> GetEntity(long id)
        {
            var list = await _logLoginEntityDB.AsAsyncEnumerable(p => p.Id == id);
            return list.FirstOrDefault();
        }

        /// <summary>
        ///     查询多个ID主键数据
        /// </summary>
        public async Task<List<LogLoginEntity>> GetListByIds(string ids)
        {
            if (ids.IsNullOrEmpty())
                throw new Exception("参数不合法！");

            var idArr = TextHelper.SplitToArray<long>(ids, ',').ToList();
            var data = await _logLoginEntityDB.AsAsyncEnumerable(a => idArr.Contains(a.Id.GetValueOrDefault()));

            return data;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(LogLoginEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                // 默认赋值
                entity.Id = IdGeneratorHelper.Instance.GetId();
                ;
                await _logLoginEntityDB.InsertNowAsync(entity);
            }
            else
            {
                await _logLoginEntityDB.UpdateNowAsync(entity, ignoreNullValues: true);
            }
        }

        public async Task DeleteForm(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                throw new Exception("参数不合法！");

            var _ids = ids.Split(",");
            await _logLoginEntityDB.BatchDeleteAsync(_ids);
        }

        #endregion
    }
}
