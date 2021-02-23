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
    ///     创 建：song
    ///     日 期：2020-12-04 12:55
    ///     描 述：登陆日志业务类
    /// </summary>
    public class LogLoginBLL : ILogLoginBLL, ITransient
    {
        private readonly ILogLoginService _logLoginService;

        public LogLoginBLL(ILogLoginService logLoginService)
        {
            _logLoginService = logLoginService;
        }

        #region 获取数据

        public async Task<TData<List<LogLoginEntity>>> GetList(LogLoginListParam param)
        {
            var obj = new TData<List<LogLoginEntity>>();
            obj.Data = await _logLoginService.GetList(param);
            obj.Data.ForEach(a => a.IpLocation = IpLocationHelper.GetIpLocation(a.IpAddress));
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<LogLoginEntity>>> GetPageList(LogLoginListParam param, Pagination pagination)
        {
            var obj = new TData<List<LogLoginEntity>>();
            obj.Data = await _logLoginService.GetPageList(param, pagination);
            obj.Data.ForEach(a => a.IpLocation = IpLocationHelper.GetIpLocation(a.IpAddress));
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<LogLoginEntity>> GetEntity(long id)
        {
            var obj = new TData<LogLoginEntity>();
            obj.Data = await _logLoginService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(LogLoginEntity entity)
        {
            var obj = new TData<string>();
            await _logLoginService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            var obj = new TData();
            await _logLoginService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}
