using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Model;

namespace YiSha.IBusiness.OrganizationManage
{
    public interface IUserBLL
    {
        #region 获取数据

        Task<TData<List<UserEntity>>> GetList(UserListParam param);

        Task<TData<List<UserEntity>>> GetPageList(UserListParam param, Pagination pagination);

        Task<TData<UserEntity>> GetEntity(long id);

        #endregion

        #region 提交数据

        Task<TData<string>> SaveForm(UserEntity entity);

        Task<TData> DeleteForm(string ids);
        Task<TData<UserEntity>> CheckLogin(string userName, string password);
        Task<TData<object>> UserPageLoad();
        Task GetUserBelong(UserEntity user);
        Task UpdateLoginInfo(UserEntity data);

        #endregion
    }
}
