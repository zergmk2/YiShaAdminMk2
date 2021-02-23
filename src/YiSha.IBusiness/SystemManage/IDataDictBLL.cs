using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;
using YiSha.Model.Result.SystemManage;

namespace YiSha.IBusiness.SystemManage
{
    public interface IDataDictBLL
    {
        #region 获取数据

        Task<TData<List<DataDictEntity>>> GetList(DataDictListParam param);

        Task<TData<List<DataDictEntity>>> GetPageList(DataDictListParam param, Pagination pagination);

        Task<TData<DataDictEntity>> GetEntity(long id);

        #endregion

        #region 提交数据

        Task<TData<string>> SaveForm(DataDictEntity entity);

        Task<TData> DeleteForm(string ids);

        #endregion

        Task<TData<List<DataDictInfo>>> GetDataDictList();
    }
}
