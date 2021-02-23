using YiSha.Business.SystemManage;
using YiSha.IBusiness.SystemManage;
using YiSha.Web.Controllers;
using YiSha.WebApi.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace YiSha.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class RoleController : BaseController
    {
        #region 视图功能
        public IActionResult RoleIndex()
        {
            return View();
        }

        public IActionResult RoleForm()
        {
            return View();
        }
        #endregion
    }
}
