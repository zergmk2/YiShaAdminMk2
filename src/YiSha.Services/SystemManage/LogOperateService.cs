using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;
using YiSha.Entity;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;

namespace YiSha.Service.SystemManage
{
    public class LogOperateService :  ILogOperateService, ITransient
    {
        private readonly IRepository<LogOperateEntity> _logOperateEntityDB;

        public LogOperateService(IRepository<LogOperateEntity> logOperateEntityDB)
        {
            _logOperateEntityDB = logOperateEntityDB;
        }

        #region 获取数据
        public async Task<List<LogOperateEntity>> GetList(LogOperateListParam param)
        {
            // var strSql = new StringBuilder();
            // List<DbParameter> filter = ListFilter(param, strSql);
            // var list = await _logOperateEntityDB.Find<LogOperateEntity>(strSql.ToString(), filter.ToArray());
            return new List<LogOperateEntity>();
        }

        public async Task<List<LogOperateEntity>> GetPageList(LogOperateListParam param, Pagination pagination)
        {
            return new List<LogOperateEntity>();
            // var strSql = new StringBuilder();
            // List<DbParameter> filter = ListFilter(param, strSql);
            // var list = await this.BaseRepository().FindList<LogOperateEntity>(strSql.ToString(), filter.ToArray(), pagination);
            // return list.ToList();
        }

        public async Task<LogOperateEntity> GetEntity(long id)
        {
            return await _logOperateEntityDB.FirstOrDefaultAsync(entity => entity.Id == id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(LogOperateEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await _logOperateEntityDB.InsertNowAsync(entity);
            }
            else
            {
                await _logOperateEntityDB.UpdateNowAsync(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            var _ids = ids.Split(",");
            await _logOperateEntityDB.BatchDeleteAsync(idArr);
        }

        public async Task RemoveAllForm()
        {
            await _logOperateEntityDB.SqlQueryAsync("truncate table SysLogOperate");
        }
        #endregion

        #region 私有方法
        // private List<DbParameter> ListFilter(LogOperateListParam param, StringBuilder strSql)
        // {
        //     strSql.Append(@"SELECT  a.Id,
        //                             a.BaseCreateTime,
        //                             a.BaseCreatorId,
        //                             a.LogStatus,
        //                             a.IpAddress,
        //                             a.IpLocation,
        //                             a.Remark,
        //                             a.ExecuteUrl,
        //                             a.ExecuteParam,
        //                             a.ExecuteResult,
        //                             a.ExecuteTime,
        //                             b.UserName,
        //                             c.DepartmentName
        //                     FROM    SysLogOperate a
        //                             LEFT JOIN SysUser b ON a.BaseCreatorId = b.Id
        //                             LEFT JOIN SysDepartment c ON b.DepartmentId = c.Id
        //                     WHERE   1 = 1");
        //     var parameter = new List<DbParameter>();
        //     if (param != null)
        //     {
        //         if (!string.IsNullOrEmpty(param.UserName))
        //         {
        //             strSql.Append(" AND b.UserName like @UserName");
        //             parameter.Add(DbParameterExtension.CreateDbParameter("@UserName", '%' + param.UserName + '%'));
        //         }
        //         if (param.LogStatus > -1)
        //         {
        //             strSql.Append(" AND a.LogStatus = @LogStatus");
        //             parameter.Add(DbParameterExtension.CreateDbParameter("@LogStatus", param.LogStatus));
        //         }
        //         if (!string.IsNullOrEmpty(param.ExecuteUrl))
        //         {
        //             strSql.Append(" AND a.ExecuteUrl like @ExecuteUrl");
        //             parameter.Add(DbParameterExtension.CreateDbParameter("@ExecuteUrl", '%' + param.ExecuteUrl + '%'));
        //         }
        //         if (!string.IsNullOrEmpty(param.StartTime.ParseToString()))
        //         {
        //             strSql.Append(" AND a.BaseCreateTime >= @StartTime");
        //             parameter.Add(DbParameterExtension.CreateDbParameter("@StartTime", param.StartTime));
        //         }
        //         if (!string.IsNullOrEmpty(param.EndTime.ParseToString()))
        //         {
        //             param.EndTime = param.EndTime.Value.Date.Add(new TimeSpan(23, 59, 59));
        //             strSql.Append(" AND a.BaseCreateTime <= @EndTime");
        //             parameter.Add(DbParameterExtension.CreateDbParameter("@EndTime", param.EndTime));
        //         }
        //     }
        //     return parameter;
        // }
        #endregion
    }

    public interface ILogOperateService
    {
    }
}
