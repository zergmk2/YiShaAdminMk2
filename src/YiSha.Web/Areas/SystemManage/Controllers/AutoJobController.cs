using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity.SystemManage;
using YiSha.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace YiSha.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class AutoJobController : BaseController
    {

        #region 视图功能
        public IActionResult AutoJobIndex()
        {
            return View();
        }
        public IActionResult AutoJobForm()
        {
            return View();
        }
        #endregion
    }
}
