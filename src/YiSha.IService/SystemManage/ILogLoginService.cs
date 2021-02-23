using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IService.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 12:55
    ///     描 述：登陆日志服务接口
    /// </summary>
    public interface ILogLoginService
    {
        #region 获取数据

        Task<List<LogLoginEntity>> GetList(LogLoginListParam param);

        Task<List<LogLoginEntity>> GetPageList(LogLoginListParam param, Pagination pagination);

        Task<LogLoginEntity> GetEntity(long id);

        #endregion

        #region 提交数据

        Task SaveForm(LogLoginEntity entity);

        Task DeleteForm(string ids);

        #endregion

        #region 私有方法

        #endregion
    }
}
