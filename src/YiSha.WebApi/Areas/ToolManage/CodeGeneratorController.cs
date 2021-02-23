using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using YiSha.IBusiness.SystemManage;
using YiSha.Model;
using YiSha.Model.Result;
using Microsoft.AspNetCore.Mvc;
using YiSha.CodeGenerator;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.WebApi.Areas.ToolManage
{
    /// <summary>
    ///     代码生成
    /// </summary>
    [Route("ToolManage/[controller]")]
    public class CodeGeneratorController : BaseController
    {
        private readonly IDatabaseTableBLL _databaseTableBLL;
        private readonly SingleTableTemplate _singleTableTemplate;

        public CodeGeneratorController(IDatabaseTableBLL databaseTableBLL, SingleTableTemplate singleTableTemplate)
        {
            _databaseTableBLL = databaseTableBLL;
            _singleTableTemplate = singleTableTemplate;
        }

        #region 获取数据

        [HttpGet]
        public async Task<TData<List<ZtreeInfo>>> GetTableFieldTreeListJson([FromQuery] string tableName)
        {
            var obj = await _databaseTableBLL.GetTableFieldZtreeList(tableName);
            return obj;
        }

        [HttpGet]
        public async Task<TData<List<ZtreeInfo>>> GetTableFieldTreePartListJson([FromQuery] string tableName,
            [FromQuery] int upper = 0)
        {
            var obj = await _databaseTableBLL.GetTableFieldZtreeList(tableName);
            if (obj.Data != null)
                // 基础字段不显示出来
                obj.Data.RemoveAll(p => BaseField.BaseFieldList.Contains(p.name));
            return obj;
        }


        [HttpGet]
        public async Task<TData<List<TableFieldInfo>>> GetTableFieldListJson([FromQuery] string tableName)
        {
            var obj = await _databaseTableBLL.GetTableFieldList(tableName);
            if (obj.Data != null)
                // 基础字段不显示出来
                obj.Data.RemoveAll(p => BaseField.BaseFieldList.Contains(p.TableColumn));
            return obj;
        }


        [HttpGet]
        public async Task<TData<BaseConfigModel>> GetBaseConfigJson([FromQuery] string tableName)
        {
            var obj = new TData<BaseConfigModel>();

            var tableDescription = string.Empty;
            var tDataTableField = await _databaseTableBLL.GetTableFieldList(tableName);
            var columnList = tDataTableField.Data.Where(p => !BaseField.BaseFieldList.Contains(p.TableColumn))
                .Select(p => p.TableColumn).ToList();

            var serverPath = GlobalContext.HostingEnvironment.ContentRootPath;
            obj.Data = _singleTableTemplate.GetBaseConfig(serverPath, tableName, tableDescription, columnList);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        [HttpPost]
        public async Task<TData<object>> CodePreviewJson([FromForm] BaseConfigModel baseConfig)
        {
            var obj = new TData<object>();
            if (string.IsNullOrEmpty(baseConfig.OutputConfig.OutputModule))
            {
                obj.Message = "请选择输出到的模块";
                return obj;
            }

            var objTable = await _databaseTableBLL.GetTableFieldList(baseConfig.TableName);
            var dt = DataTableHelper.ListToDataTable(objTable.Data); // 用DataTable类型，避免依赖

            var codeEntity = _singleTableTemplate.BuildEntity(baseConfig, dt);
            var codeEntityParam = _singleTableTemplate.BuildEntityParam(baseConfig, dt);
            var codeService = _singleTableTemplate.BuildService(baseConfig, dt);
            var codeBusiness = _singleTableTemplate.BuildBusiness(baseConfig);
            var codeIService = _singleTableTemplate.BuildIService(baseConfig, dt);
            var codeIBusiness = _singleTableTemplate.BuildIBusiness(baseConfig);

            var codeController = _singleTableTemplate.BuildController(baseConfig);
            var codeIndex = _singleTableTemplate.BuildIndex(baseConfig, dt);
            var codeForm = _singleTableTemplate.BuildForm(baseConfig, dt);

            var json = new
            {
                CodeEntity = HttpUtility.HtmlEncode(codeEntity),
                CodeEntityParam = HttpUtility.HtmlEncode(codeEntityParam),
                CodeService = HttpUtility.HtmlEncode(codeService),
                CodeBusiness = HttpUtility.HtmlEncode(codeBusiness),
                CodeIService = HttpUtility.HtmlEncode(codeIService),
                CodeIBusiness = HttpUtility.HtmlEncode(codeIBusiness),
                CodeController = HttpUtility.HtmlEncode(codeController),
                CodeIndex = HttpUtility.HtmlEncode(codeIndex),
                CodeForm = HttpUtility.HtmlEncode(codeForm)
            };
            obj.Data = json;
            obj.Tag = 1;

            return obj;
        }

        [HttpPost]
        public async Task<TData<List<KeyValue>>> CodeGenerateJson([FromForm] BaseConfigModel baseConfig)
        {
            var obj = new TData<List<KeyValue>>();

            var result = await _singleTableTemplate.CreateCode(baseConfig, HttpUtility.UrlDecode(baseConfig.Code));
            obj.Data = result;
            obj.Tag = 1;

            return obj;
        }

        #endregion
    }
}
