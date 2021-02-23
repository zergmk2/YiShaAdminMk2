using System.Collections.Generic;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using YiSha.Entity;
using YiSha.IBusiness.SystemManage;
using YiSha.Model;
using YiSha.Model.Param.SystemManage;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util.Model;

namespace YiSha.WebApi.Areas.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-06 09:46
    ///     描 述：角色信息控制器类
    /// </summary>
    [Route("SystemManage/[controller]")]
    public class RoleController : BaseController
    {
        private readonly IMenuBLL _menuBLL;
        private readonly IRoleBLL _roleBLL;

        public RoleController(IRoleBLL roleBLL, IMenuBLL menuBLL)
        {
            _roleBLL = roleBLL;
            _menuBLL = menuBLL;
        }

        #region 获取数据

        /// <summary>
        ///     条件查询
        /// </summary>
        [HttpGet]
        public async Task<TData<List<RoleEntity>>> GetListJson([FromQuery] RoleListParam param)
        {
            var obj = await _roleBLL.GetList(param);
            return obj;
        }

        /// <summary>
        ///     条件查询-分页
        /// </summary>
        [HttpGet]
        public async Task<TData<List<RoleEntity>>> GetPageListJson([FromQuery] RoleListParam param,
            [FromQuery] Pagination pagination)
        {
            var obj = await _roleBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        ///     根据ID查询
        /// </summary>
        [HttpGet]
        public async Task<TData<RoleEntity>> GetFormJson([FromQuery] long id)
        {
            var obj = await _roleBLL.GetEntity(id);
            return obj;
        }

        /// <summary>
        ///     查询权限菜单树
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<List<ZtreeInfo>>> QueryRoleAuthTree([FromQuery] long id)
        {
            var allMenu = await _menuBLL.GetZtreeList(null);
            var roleInfo = await _roleBLL.GetEntity(id);

            if (string.IsNullOrEmpty(roleInfo.Data.MenuIds))
                return allMenu;

            foreach (var ztree in allMenu.Data)
            foreach (var _Id in roleInfo.Data.MenuIds.Split(','))
                if (long.Parse(_Id) == ztree.id)
                    ztree.@checked = true;

            return allMenu;
        }

        #endregion

        #region 提交数据

        /// <summary>
        ///     新增/修改 数据
        /// </summary>
        [HttpPost]
        public async Task<TData<string>> SaveFormJson([FromForm] RoleEntity entity)
        {
            var obj = await _roleBLL.SaveForm(entity);
            return obj;
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        [HttpPost]
        public async Task<TData> DeleteFormJson([FromForm] string ids)
        {
            var obj = await _roleBLL.DeleteForm(ids);
            return obj;
        }

        /// <summary>
        ///     保存权限
        /// </summary>
        /// <param name="RoleId">角色ID</param>
        /// <param name="MenuIds">菜单ID</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async Task<TData> SaveRoleAuth([FromForm] long RoleId, [FromForm] string MenuIds)
        {
            var data = await _roleBLL.SaveRoleAuth(RoleId, MenuIds);
            return data;
        }

        #endregion
    }
}
