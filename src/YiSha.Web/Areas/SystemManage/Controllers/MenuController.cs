using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util.Model;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.IBusiness.SystemManage;
using YiSha.Model;
using YiSha.Model.Param.SystemManage;
using YiSha.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace YiSha.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class MenuController : BaseController
    {
        #region 视图功能
        public IActionResult MenuIndex()
        {
            return View();
        }

        public IActionResult MenuForm()
        {
            return View();
        }

        public IActionResult MenuChoose()
        {
            return View();
        }
        public IActionResult MenuIcon()
        {
            return View();
        }
        #endregion
    }
}
