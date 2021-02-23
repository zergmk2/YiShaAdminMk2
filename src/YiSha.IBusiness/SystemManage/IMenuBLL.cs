using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity.SystemManage;
using YiSha.Model;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IBusiness.SystemManage
{
    public interface IMenuBLL
    {
        #region 获取数据

        Task<TData<List<MenuEntity>>> GetList(MenuListParam param);

        Task<TData<List<MenuEntity>>> GetPageList(MenuListParam param, Pagination pagination);

        Task<TData<MenuEntity>> GetEntity(long id);
        Task<TData<List<ZtreeInfo>>> GetZtreeList(MenuListParam param);

        #endregion

        #region 提交数据

        Task<TData<string>> SaveForm(MenuEntity entity);

        Task<TData> DeleteForm(string ids);

        #endregion

        Task<TData<int>> GetMaxSort(long parentId);
    }
}
