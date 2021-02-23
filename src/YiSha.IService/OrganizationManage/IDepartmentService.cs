using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Model;

namespace YiSha.IService.OrganizationManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-06 14:12
    ///     描 述：部门信息服务接口
    /// </summary>
    public interface IDepartmentService
    {
        #region 获取数据

        Task<List<DepartmentEntity>> GetList(DepartmentListParam param);

        Task<List<DepartmentEntity>> GetPageList(DepartmentListParam param, Pagination pagination);

        Task<DepartmentEntity> GetEntity(long id);

        #endregion

        #region 提交数据

        Task SaveForm(DepartmentEntity entity);

        Task DeleteForm(string ids);

        #endregion

        #region 私有方法

        #endregion
    }
}
