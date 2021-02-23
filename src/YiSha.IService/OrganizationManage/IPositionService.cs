using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Model;

namespace YiSha.IService.OrganizationManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-06 14:10
    ///     描 述：职位信息服务接口
    /// </summary>
    public interface IPositionService
    {
        #region 获取数据

        Task<List<PositionEntity>> GetList(PositionListParam param);

        Task<List<PositionEntity>> GetPageList(PositionListParam param, Pagination pagination);

        Task<PositionEntity> GetEntity(long id);

        #endregion

        #region 提交数据

        Task SaveForm(PositionEntity entity);

        Task DeleteForm(string ids);

        #endregion

        #region 私有方法

        #endregion
    }
}
