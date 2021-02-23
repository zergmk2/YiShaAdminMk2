using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IService.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 11:16
    ///     描 述：菜单权限服务接口
    /// </summary>
    public interface IMenuAuthorizeService
    {
        #region 获取数据

        Task<List<MenuAuthorizeEntity>> GetList(MenuAuthorizeListParam param);

        Task<List<MenuAuthorizeEntity>> GetPageList(MenuAuthorizeListParam param, Pagination pagination);

        Task<MenuAuthorizeEntity> GetEntity(long id);

        #endregion

        #region 提交数据

        Task SaveForm(MenuAuthorizeEntity entity);

        Task DeleteForm(string ids);
        Task<List<long>> GetMenuIdList(MenuListParam menuListParam);

        #endregion

        #region 私有方法

        #endregion
    }
}
