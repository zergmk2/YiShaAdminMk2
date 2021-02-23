using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.IBusiness.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util.Model;
using YiSha.Business.SystemManage;
using YiSha.Cache;
using YiSha.Model.Result.SystemManage;

namespace YiSha.WebApi.Areas.OrganizationManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 12:41
    ///     描 述：用户信息控制器类
    /// </summary>
    [Route("OrganizationManage/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserBLL _userBLL;
        private readonly OperatorCache _operator;
        private readonly MenuAuthorizeBLL _menuAuthorizeBll;
        public UserController(IUserBLL userBLL, OperatorCache operatorCache, MenuAuthorizeBLL menuAuthorizeBll)
        {
            _userBLL = userBLL;
            _operator = operatorCache;
            _menuAuthorizeBll = menuAuthorizeBll;
        }

        #region 获取数据

        /// <summary>
        ///     条件查询
        /// </summary>
        [HttpGet]
        public async Task<TData<List<UserEntity>>> GetListJson([FromQuery] UserListParam param)
        {
            var obj = await _userBLL.GetList(param);
            return obj;
        }

        /// <summary>
        ///     条件查询-分页
        /// </summary>
        [HttpGet]
        public async Task<TData<List<UserEntity>>> GetPageListJson([FromQuery] UserListParam param,
            [FromQuery] Pagination pagination)
        {
            var obj = await _userBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        ///     根据ID查询
        /// </summary>
        [HttpGet]
        public async Task<TData<UserEntity>> GetFormJson([FromQuery] long id)
        {
            var obj = await _userBLL.GetEntity(id);
            return obj;
        }

        /// <summary>
        ///     页面信息初始化
        /// </summary>
        [HttpGet]
        public async Task<TData<object>> UserPageLoad()
        {
            var obj = await _userBLL.UserPageLoad();
            return obj;
        }

        /// <summary>
        ///     获取用户关联信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<object>> GetUserDlcInfo([FromQuery] long userId)
        {
            var user = new UserEntity {Id = userId};
            var obj = new TData<object>();

            // 查询关联信息
            await _userBLL.GetUserBelong(user);

            obj.Tag = 1;
            obj.Data = new {RoleIds = user.RoleIds ?? "", PositionIds = user.PositionIds ?? ""};

            return obj;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUserAuthorizeJson()
        {
            TData<UserAuthorizeInfo> obj = new TData<UserAuthorizeInfo>();
            OperatorInfo operatorInfo = await _operator.Current();
            TData<List<MenuAuthorizeInfo>> objMenuAuthorizeInfo = await _menuAuthorizeBll.GetAuthorizeList(operatorInfo);
            obj.Data = new UserAuthorizeInfo();
            obj.Data.IsSystem = operatorInfo.IsSystem;
            if (objMenuAuthorizeInfo.Tag == 1)
            {
                obj.Data.MenuAuthorize = objMenuAuthorizeInfo.Data;
            }
            obj.Tag = 1;
            return Json(obj);
        }

        #endregion

        #region 提交数据

        /// <summary>
        ///     新增/修改 数据
        /// </summary>
        [HttpPost]
        public async Task<TData<string>> SaveFormJson([FromForm] UserEntity entity)
        {
            var obj = await _userBLL.SaveForm(entity);
            return obj;
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        [HttpPost]
        public async Task<TData> DeleteFormJson([FromForm] string ids)
        {
            var obj = await _userBLL.DeleteForm(ids);
            return obj;
        }

        #endregion
    }
}
