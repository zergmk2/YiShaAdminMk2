using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IService.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-06 09:46
    ///     描 述：角色信息服务接口
    /// </summary>
    public interface IRoleService
    {
        #region 获取数据

        Task<List<RoleEntity>> GetList(RoleListParam param);

        Task<List<RoleEntity>> GetPageList(RoleListParam param, Pagination pagination);

        Task<RoleEntity> GetEntity(long id);

        #endregion

        #region 提交数据

        Task SaveForm(RoleEntity entity);

        Task DeleteForm(string ids);
        Task SaveRoleAuth(long roleId, string menuIds);

        #endregion

        #region 私有方法

        #endregion
    }
}
