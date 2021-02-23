using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Model;

namespace YiSha.IService.OrganizationManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 12:41
    ///     描 述：用户信息服务接口
    /// </summary>
    public interface IUserService
    {
        #region 获取数据

        Task<List<UserEntity>> GetList(UserListParam param);

        Task<List<UserEntity>> GetPageList(UserListParam param, Pagination pagination);

        Task<UserEntity> GetEntity(long id);

        #endregion

        #region 提交数据

        Task SaveForm(UserEntity entity);

        Task DeleteForm(string ids);

        Task<UserEntity> CheckLogin(string userName);

        Task<OperatorInfo> GetUserByToken(string token);

        Task UpdateLoginInfo(UserEntity entity);

        #endregion

        #region 私有方法

        #endregion
    }
}
