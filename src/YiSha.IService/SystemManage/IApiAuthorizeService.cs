using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IService.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 16:22
    ///     描 述：接口权限服务接口
    /// </summary>
    public interface IApiAuthorizeService
    {
        #region 获取数据

        Task<List<ApiAuthorizeEntity>> GetList(ApiAuthorizeListParam param);

        Task<List<ApiAuthorizeEntity>> GetPageList(ApiAuthorizeListParam param, Pagination pagination);

        Task<ApiAuthorizeEntity> GetEntity(long id);

        #endregion

        #region 提交数据

        Task SaveForm(ApiAuthorizeEntity entity);

        Task DeleteForm(string ids);
        Task DeleteByAuthorize(string authorize);
        Task AddAccess(List<ApiAuthorizeEntity> apiAuthorizes);

        #endregion

        #region 私有方法

        #endregion
    }
}
