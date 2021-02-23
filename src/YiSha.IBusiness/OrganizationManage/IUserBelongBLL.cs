using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Model;

namespace YiSha.IBusiness.OrganizationManage
{
    public interface IUserBelongBLL
    {
        #region 获取数据

        Task<TData<List<UserBelongEntity>>> GetList(UserBelongListParam param);

        Task<TData<List<UserBelongEntity>>> GetPageList(UserBelongListParam param, Pagination pagination);

        Task<TData<UserBelongEntity>> GetEntity(long id);

        #endregion

        #region 提交数据

        Task<TData<string>> SaveForm(UserBelongEntity entity);

        Task<TData> DeleteForm(string ids);

        #endregion
    }
}
