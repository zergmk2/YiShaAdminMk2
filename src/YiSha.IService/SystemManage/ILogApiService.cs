using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IService.SystemManage
{
    /// <summary>
    ///     创 建：
    ///     日 期：2020-12-04 12:49
    ///     描 述：Api日志服务接口
    /// </summary>
    public interface ILogApiService
    {
        #region 获取数据

        Task<List<LogApiEntity>> GetList(LogApiListParam param);

        Task<List<LogApiEntity>> GetPageList(LogApiListParam param, Pagination pagination);

        Task<LogApiEntity> GetEntity(long id);

        #endregion

        #region 提交数据

        Task SaveForm(LogApiEntity entity);

        Task DeleteForm(string ids);

        #endregion

        #region 私有方法

        #endregion
    }
}
