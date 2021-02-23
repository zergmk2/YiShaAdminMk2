using System.Collections.Generic;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using YiSha.Util.Model;
using YiSha.Entity;
using YiSha.Entity.SystemManage;
using YiSha.IBusiness.SystemManage;
using YiSha.Model.Param.SystemManage;
using Microsoft.AspNetCore.Mvc;
using YiSha.WebApi.Areas;

namespace YiSha.Web.Areas.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 11:25
    ///     描 述：菜单控制器类
    /// </summary>
    [Route("SystemManage/[controller]")]
    public class MenuController : BaseController
    {
        private readonly IApiAuthorizeBLL _apiAuthorizeBLL;
        private readonly IMenuBLL _menuBLL;

        public MenuController(IMenuBLL menuBLL, IApiAuthorizeBLL apiAuthorizeBLL)
        {
            _menuBLL = menuBLL;
            _apiAuthorizeBLL = apiAuthorizeBLL;
        }

        #region 获取数据

        /// <summary>
        ///     条件查询
        /// </summary>
        [HttpGet]
        public async Task<TData<List<MenuEntity>>> GetListJson([FromQuery] MenuListParam param)
        {
            var obj = await _menuBLL.GetList(param);
            return obj;
        }

        /// <summary>
        ///     条件查询-分页
        /// </summary>
        [HttpGet]
        public async Task<TData<List<MenuEntity>>> GetPageListJson([FromQuery] MenuListParam param,
            [FromQuery] Pagination pagination)
        {
            var obj = await _menuBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        ///     根据ID查询
        /// </summary>
        [HttpGet]
        public async Task<TData<MenuEntity>> GetFormJson([FromQuery] long id)
        {
            var obj = await _menuBLL.GetEntity(id);
            return obj;
        }

        /// <summary>
        ///     根据权限标识查询菜单URL
        /// </summary>
        [HttpGet]
        public async Task<TData<List<ApiAuthorizeEntity>>> GetAccessByMenu([FromQuery] ApiAuthorizeListParam param)
        {
            var obj = await _apiAuthorizeBLL.GetList(param);
            return obj;
        }

        #endregion

        #region 提交数据

        /// <summary>
        ///     新增/修改 数据
        /// </summary>
        [HttpPost]
        public async Task<TData<string>> SaveFormJson([FromForm] MenuEntity entity)
        {
            var obj = await _menuBLL.SaveForm(entity);
            return obj;
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        [HttpPost]
        public async Task<TData> DeleteFormJson([FromForm] string ids)
        {
            var obj = await _menuBLL.DeleteForm(ids);
            return obj;
        }

        /// <summary>
        ///     保存权限标识对应的url
        /// </summary>
        [HttpPost]
        [UnitOfWork]
        public async Task<TData> SaveAccess([FromForm] ApiAuthorizeSaveParam param)
        {
            var obj = await _apiAuthorizeBLL.SaveAccess(param.Authorize, param.Urls);
            return obj;
        }

        #endregion
    }
}
