using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Business.SystemManage;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;
using YiSha.Web.Controllers;
using YiSha.WebApi.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace YiSha.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class DataDictDetailController : BaseController
    {
        #region 视图功能
        public IActionResult DataDictDetailIndex()
        {
            return View();
        }
        public IActionResult DataDictDetailForm()
        {
            return View();
        }
        #endregion
    }
}
