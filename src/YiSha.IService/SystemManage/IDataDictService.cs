using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IService.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-18 16:04
    ///     描 述：数据字典服务接口
    /// </summary>
    public interface IDataDictService
    {
        #region 获取数据

        Task<List<DataDictEntity>> GetList(DataDictListParam param);

        Task<List<DataDictEntity>> GetPageList(DataDictListParam param, Pagination pagination);

        Task<DataDictEntity> GetEntity(long id);

        #endregion

        #region 提交数据

        Task SaveForm(DataDictEntity entity);

        Task DeleteForm(string ids);

        #endregion

        #region 私有方法

        #endregion
    }
}
