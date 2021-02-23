using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Model;

namespace YiSha.IBusiness.OrganizationManage
{
    public interface IPositionBLL
    {
        #region 获取数据

        Task<TData<List<PositionEntity>>> GetList(PositionListParam param);

        Task<TData<List<PositionEntity>>> GetPageList(PositionListParam param, Pagination pagination);

        Task<TData<PositionEntity>> GetEntity(long id);

        #endregion

        #region 提交数据

        Task<TData<string>> SaveForm(PositionEntity entity);

        Task<TData> DeleteForm(string ids);

        #endregion
    }
}
