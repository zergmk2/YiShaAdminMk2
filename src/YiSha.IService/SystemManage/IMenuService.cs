using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IService.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 11:25
    ///     描 述：菜单服务接口
    /// </summary>
    public interface IMenuService
    {
        #region 获取数据

        Task<List<MenuEntity>> GetList(MenuListParam param);

        Task<List<MenuEntity>> GetPageList(MenuListParam param, Pagination pagination);

        Task<MenuEntity> GetEntity(long id);

        #endregion

        #region 提交数据

        Task SaveForm(MenuEntity entity);

        Task DeleteForm(string ids);

        #endregion

        #region 私有方法

        #endregion

        Task<int> GetMaxSort(long parentId);
    }
}
