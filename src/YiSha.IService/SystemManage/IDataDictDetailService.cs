using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IService.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-19 08:59
    ///     描 述：数据字典值服务接口
    /// </summary>
    public interface IDataDictDetailService
    {
        #region 获取数据

        Task<List<DataDictDetailEntity>> GetList(DataDictDetailListParam param);

        Task<List<DataDictDetailEntity>> GetPageList(DataDictDetailListParam param, Pagination pagination);

        Task<DataDictDetailEntity> GetEntity(long id);

        #endregion

        #region 提交数据

        Task SaveForm(DataDictDetailEntity entity);

        Task DeleteForm(string ids);

        #endregion

        #region 私有方法

        #endregion
    }
}
