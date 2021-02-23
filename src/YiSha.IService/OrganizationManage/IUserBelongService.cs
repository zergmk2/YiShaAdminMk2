using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Model;

namespace YiSha.IService.OrganizationManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 13:08
    ///     描 述：用户关联信息服务接口
    /// </summary>
    public interface IUserBelongService
    {
        #region 获取数据

        Task<List<UserBelongEntity>> GetList(UserBelongListParam param);

        Task<List<UserBelongEntity>> GetPageList(UserBelongListParam param, Pagination pagination);

        Task<UserBelongEntity> GetEntity(long id);

        #endregion

        #region 提交数据

        Task SaveForm(UserBelongEntity entity);

        Task DeleteForm(string ids);

        Task SaveUserRoles(long value, List<long> lists);

        Task SaveUserPositions(long value, List<long> lists);
        Task DeleteByUserId(long value);

        #endregion

        #region 私有方法

        #endregion
    }
}
