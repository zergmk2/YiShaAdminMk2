using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.IBusiness.SystemManage;
using YiSha.Model.Result;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util.Model;

namespace YiSha.WebApi.Areas.SystemManage
{
    [Route("SystemManage/[controller]")]
    public class DatabaseController : BaseController
    {
        private readonly IDatabaseTableBLL _databaseTableBLL;

        public DatabaseController(IDatabaseTableBLL databaseTableBLL)
        {
            _databaseTableBLL = databaseTableBLL;
        }

        #region 获取数据

        [HttpGet]
        public async Task<TData<List<TableInfo>>> GetTableListJson([FromQuery] string tableName)
        {
            var obj = await _databaseTableBLL.GetTableList(tableName);
            return obj;
        }

        [HttpGet]
        public async Task<TData<List<TableInfo>>> GetTablePageListJson([FromQuery] string tableName,
            [FromQuery] Pagination pagination)
        {
            var obj = await _databaseTableBLL.GetTablePageList(tableName, pagination);
            return obj;
        }

        [HttpGet]
        public async Task<TData<List<TableFieldInfo>>> GetTableFieldListJson([FromQuery] string tableName)
        {
            var obj = await _databaseTableBLL.GetTableFieldList(tableName);
            return obj;
        }

        #endregion
    }
}
