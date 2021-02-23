using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IBusiness.SystemManage
{
    public interface ILogApiBLL
    {
        #region 获取数据

        Task<TData<List<LogApiEntity>>> GetList(LogApiListParam param);

        Task<TData<List<LogApiEntity>>> GetPageList(LogApiListParam param, Pagination pagination);

        Task<TData<LogApiEntity>> GetEntity(long id);

        #endregion

        #region 提交数据

        Task<TData<string>> SaveForm(LogApiEntity entity);

        Task<TData> DeleteForm(string ids);

        #endregion
    }
}
