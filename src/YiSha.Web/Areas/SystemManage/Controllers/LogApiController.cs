using YiSha.Business.SystemManage;
using YiSha.IBusiness.SystemManage;
using YiSha.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace YiSha.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class LogApiController : BaseController
    {
        #region 视图功能
        public IActionResult LogApiIndex()
        {
            return View();
        }

        public IActionResult LogApiDetail()
        {
            return View();
        }
        #endregion
    }
}
