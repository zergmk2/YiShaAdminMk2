using YiSha.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace YiSha.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class AreaController : BaseController
    {
        #region 视图功能
        public IActionResult AreaIndex()
        {
            return View();
        }
        #endregion
    }
}
