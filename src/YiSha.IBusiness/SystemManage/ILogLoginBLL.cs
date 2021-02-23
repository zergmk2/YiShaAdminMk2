using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IBusiness.SystemManage
{
    public interface ILogLoginBLL
    {
        #region 获取数据

        Task<TData<List<LogLoginEntity>>> GetList(LogLoginListParam param);

        Task<TData<List<LogLoginEntity>>> GetPageList(LogLoginListParam param, Pagination pagination);

        Task<TData<LogLoginEntity>> GetEntity(long id);

        #endregion

        #region 提交数据

        Task<TData<string>> SaveForm(LogLoginEntity entity);

        Task<TData> DeleteForm(string ids);

        #endregion
    }
}
