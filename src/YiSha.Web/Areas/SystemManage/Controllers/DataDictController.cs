using YiSha.Business.SystemManage;
using YiSha.Web.Controllers;
using YiSha.WebApi.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace YiSha.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class DataDictController : BaseController
    {
        private DataDictBLL _dataDictBLL;

        public DataDictController(DataDictBLL dataDictBLL)
        {
            _dataDictBLL = dataDictBLL;
        }

        #region 视图功能
        public IActionResult DataDictIndex()
        {
            return View();
        }

        public IActionResult DataDictForm()
        {
            return View();
        }
        #endregion

    }
}
