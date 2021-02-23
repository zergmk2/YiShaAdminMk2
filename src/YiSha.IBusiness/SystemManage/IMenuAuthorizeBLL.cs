using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IBusiness.SystemManage
{
    public interface IMenuAuthorizeBLL
    {
        #region 获取数据

        Task<TData<List<MenuAuthorizeEntity>>> GetList(MenuAuthorizeListParam param);

        Task<TData<List<MenuAuthorizeEntity>>> GetPageList(MenuAuthorizeListParam param, Pagination pagination);

        Task<TData<MenuAuthorizeEntity>> GetEntity(long id);

        #endregion

        #region 提交数据

        Task<TData<string>> SaveForm(MenuAuthorizeEntity entity);

        Task<TData> DeleteForm(string ids);
        Task<TData<List<MenuAuthorizeInfo>>> GetAuthorizeList(OperatorInfo operatorInfo);

        #endregion
    }
}
