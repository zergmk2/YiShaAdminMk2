using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IBusiness.SystemManage
{
    public interface ICodeTempletBLL
    {
        #region 获取数据

        Task<TData<List<CodeTempletEntity>>> GetList(CodeTempletListParam param);

        Task<TData<List<CodeTempletEntity>>> GetPageList(CodeTempletListParam param, Pagination pagination);

        Task<TData<CodeTempletEntity>> GetEntity(long id);

        #endregion

        #region 提交数据

        Task<TData<string>> SaveForm(CodeTempletEntity entity);

        Task<TData> DeleteForm(string ids);

        #endregion
    }
}
