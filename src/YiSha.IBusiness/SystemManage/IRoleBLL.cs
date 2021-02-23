using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IBusiness.SystemManage
{
    public interface IRoleBLL
    {
        #region 获取数据

        Task<TData<List<RoleEntity>>> GetList(RoleListParam param);

        Task<TData<List<RoleEntity>>> GetPageList(RoleListParam param, Pagination pagination);

        Task<TData<RoleEntity>> GetEntity(long id);

        #endregion

        #region 提交数据

        Task<TData<string>> SaveForm(RoleEntity entity);

        Task<TData> DeleteForm(string ids);
        Task<TData> SaveRoleAuth(long v, string menuIds);

        #endregion
    }
}
