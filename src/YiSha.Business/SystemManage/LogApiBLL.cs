using System.Collections.Generic;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using YiSha.Entity;
using YiSha.IBusiness.SystemManage;
using YiSha.IService.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    /// <summary>
    ///     创 建：
    ///     日 期：2020-12-04 12:49
    ///     描 述：Api日志业务类
    /// </summary>
    public class LogApiBLL : ILogApiBLL, ITransient
    {
        private readonly ILogApiService _logApiService;

        public LogApiBLL(ILogApiService logApiService)
        {
            _logApiService = logApiService;
        }

        #region 获取数据

        public async Task<TData<List<LogApiEntity>>> GetList(LogApiListParam param)
        {
            var obj = new TData<List<LogApiEntity>>();
            obj.Data = await _logApiService.GetList(param);
            obj.Data.ForEach(a => a.IpLocation = IpLocationHelper.GetIpLocation(a.IpAddress));
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<LogApiEntity>>> GetPageList(LogApiListParam param, Pagination pagination)
        {
            var obj = new TData<List<LogApiEntity>>();
            obj.Data = await _logApiService.GetPageList(param, pagination);

            obj.Data.ForEach(a => a.IpLocation = IpLocationHelper.GetIpLocation(a.IpAddress));

            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<LogApiEntity>> GetEntity(long id)
        {
            var obj = new TData<LogApiEntity>();
            obj.Data = await _logApiService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(LogApiEntity entity)
        {
            var obj = new TData<string>();
            await _logApiService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            var obj = new TData();
            await _logApiService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}
