using YiSha.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace YiSha.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class AutoJobLogController : BaseController
    {

        #region 视图功能
        public IActionResult AutoJobLogIndex()
        {
            return View();
        }
        #endregion
    }
}
