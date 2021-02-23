using YiSha.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace YiSha.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class LogOperateController : BaseController
    {
        #region 视图功能
        public IActionResult LogOperateIndex()
        {
            return View();
        }

        public IActionResult LogOperateDetail()
        {
            return View();
        }
        #endregion
    }
}
