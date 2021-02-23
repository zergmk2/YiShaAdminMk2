using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IBusiness.SystemManage
{
    public interface IDataDictDetailBLL
    {
        #region 获取数据

        Task<TData<List<DataDictDetailEntity>>> GetList(DataDictDetailListParam param);

        Task<TData<List<DataDictDetailEntity>>> GetPageList(DataDictDetailListParam param, Pagination pagination);

        Task<TData<DataDictDetailEntity>> GetEntity(long id);

        #endregion

        #region 提交数据

        Task<TData<string>> SaveForm(DataDictDetailEntity entity);

        Task<TData> DeleteForm(string ids);

        #endregion
    }
}
