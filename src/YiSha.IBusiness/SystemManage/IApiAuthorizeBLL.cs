using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IBusiness.SystemManage
{
    public interface IApiAuthorizeBLL
    {
        #region 获取数据

        Task<TData<List<ApiAuthorizeEntity>>> GetList(ApiAuthorizeListParam param);

        Task<TData<List<ApiAuthorizeEntity>>> GetPageList(ApiAuthorizeListParam param, Pagination pagination);

        Task<TData<ApiAuthorizeEntity>> GetEntity(long id);

        #endregion

        #region 提交数据

        Task<TData<string>> SaveForm(ApiAuthorizeEntity entity);

        Task<TData> DeleteForm(string ids);
        Task<TData> SaveAccess(string authorize, List<string> urls);

        #endregion
    }
}
