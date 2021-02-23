using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Model;

namespace YiSha.IBusiness.OrganizationManage
{
    public interface IDepartmentBLL
    {
        #region 获取数据

        Task<TData<List<DepartmentEntity>>> GetList(DepartmentListParam param);

        Task<TData<List<DepartmentEntity>>> GetPageList(DepartmentListParam param, Pagination pagination);

        Task<TData<DepartmentEntity>> GetEntity(long id);

        #endregion

        #region 提交数据

        Task<TData<string>> SaveForm(DepartmentEntity entity);

        Task<TData> DeleteForm(string ids);
        Task<TData<List<ZtreeInfo>>> GetZtreeUserList(DepartmentListParam param);

        #endregion

        Task<TData<List<ZtreeInfo>>> GetZtreeDepartmentList(DepartmentListParam departmentListParam);
    }
}
