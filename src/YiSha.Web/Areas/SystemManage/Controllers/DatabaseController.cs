using YiSha.Business.SystemManage;
using YiSha.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace YiSha.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class DatabaseController : BaseController
    {
        private DatabaseTableBLL _databaseTableBLL;

        public DatabaseController(DatabaseTableBLL databaseTableBLL)
        {
            _databaseTableBLL = databaseTableBLL;
        }

        #region 视图功能
        public IActionResult DatatableIndex()
        {
            return View();
        }
        #endregion
    }
}
