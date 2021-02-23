using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IService.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 12:28
    ///     描 述：代码模板服务接口
    /// </summary>
    public interface ICodeTempletService
    {
        #region 获取数据

        Task<List<CodeTempletEntity>> GetList(CodeTempletListParam param);

        Task<List<CodeTempletEntity>> GetPageList(CodeTempletListParam param, Pagination pagination);

        Task<CodeTempletEntity> GetEntity(long id);

        #endregion

        #region 提交数据

        Task SaveForm(CodeTempletEntity entity);

        Task DeleteForm(string ids);

        #endregion

        #region 私有方法

        #endregion
    }
}
