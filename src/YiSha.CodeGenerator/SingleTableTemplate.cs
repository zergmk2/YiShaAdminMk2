using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;
using YiSha.Entity;
using YiSha.Entity.SystemManage;
using YiSha.Enum.SystemManage;

namespace YiSha.CodeGenerator
{
    /// <summary>
    ///     代码生成
    /// </summary>
    public class SingleTableTemplate : ITransient
    {
        public static readonly string projectName = "MyProject";

        public static readonly string templetPath =
            GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + "Templet";

        public static readonly string[] tableFixs = {"Sys"};
        private readonly IRepository<CodeTempletEntity> _codeRepository;
        private readonly IRepository<MenuEntity> _sysMenuRepository;

        public SingleTableTemplate(IRepository<MenuEntity> sysMenuRepository,
            IRepository<CodeTempletEntity> codeRepository)
        {
            _sysMenuRepository = sysMenuRepository;
            _codeRepository = codeRepository;
        }

        #region GetBaseConfig

        public BaseConfigModel GetBaseConfig(string path, string tableName, string tableDescription,
            List<string> tableFieldList)
        {
            path = GetProjectRootPath(path);

            var defaultField = 2; // 默认显示2个字段

            var baseConfigModel = new BaseConfigModel();
            baseConfigModel.TableName = tableName;
            baseConfigModel.TableNameUpper = tableName;

            #region FileConfigModel

            baseConfigModel.FileConfig = new FileConfigModel();
            baseConfigModel.FileConfig.ClassPrefix = TableMappingHelper.GetClassNamePrefix(tableName);
            baseConfigModel.FileConfig.ClassDescription = tableDescription;
            baseConfigModel.FileConfig.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            baseConfigModel.FileConfig.EntityName = string.Format("{0}", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.EntityMapName = string.Format("{0}Map", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.EntityParamName =
                string.Format("{0}Param", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.BusinessName = string.Format("{0}BLL", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.ServiceName =
                string.Format("{0}Service", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.ControllerName =
                string.Format("{0}Controller", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.PageIndexName =
                string.Format("{0}Index", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.PageFormName = string.Format("{0}Form", baseConfigModel.FileConfig.ClassPrefix);

            #endregion

            #region OutputConfigModel

            baseConfigModel.OutputConfig = new OutputConfigModel();
            baseConfigModel.OutputConfig.OutputModule = string.Empty;
            baseConfigModel.OutputConfig.OutputEntity = path;
            baseConfigModel.OutputConfig.OutputBusiness = path;
            baseConfigModel.OutputConfig.OutputWeb = Path.Combine(path, "YiSha.WebApi");
            var areasModule = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas");
            if (Directory.Exists(areasModule))
            {
                var areas = Directory.GetDirectories(areasModule).Select(p => Path.GetFileName(p))
                    .Where(p => p != "DemoManage");
                var modules = new List<KeyValue>();
                foreach (var area in areas)
                    modules.Add(new KeyValue
                    {
                        Key = area,
                        Value = area
                    });

                baseConfigModel.OutputConfig.ModuleList = modules;
            }
            else
            {
                baseConfigModel.OutputConfig.ModuleList = new List<KeyValue>
                {
                    new()
                    {
                        Key = "TestManage",
                        Value = "TestManage"
                    }
                };
            }

            #endregion

            #region PageIndexModel

            baseConfigModel.PageIndex = new PageIndexModel();
            baseConfigModel.PageIndex.IsSearch = 1;
            baseConfigModel.PageIndex.IsPagination = 1;
            baseConfigModel.PageIndex.ButtonList = new List<string>();
            baseConfigModel.PageIndex.ColumnList = new List<string>();
            baseConfigModel.PageIndex.ColumnList.AddRange(tableFieldList.Take(defaultField));

            #endregion

            #region PageFormModel

            baseConfigModel.PageForm = new PageFormModel();
            baseConfigModel.PageForm.ShowMode = 1;
            baseConfigModel.PageForm.FieldList = new List<string>();
            baseConfigModel.PageForm.FieldList.AddRange(tableFieldList.Take(defaultField));

            #endregion

            baseConfigModel.ClassName = GetClassName(tableName);
            ;

            return baseConfigModel;
        }

        #endregion

        #region BuildEntity 创建实体类

        public string BuildEntity(BaseConfigModel baseConfigModel, DataTable dt)
        {
            // 读取模板
            var sb = ReadTemplet("Entity", baseConfigModel);

            #region 描述

            var describe = new StringBuilder();
            SetClassDescription("实体类", baseConfigModel, describe);
            sb = sb.Replace("{描述}", describe.ToString());

            #endregion

            #region 类名

            sb = sb.Replace("{类名}", baseConfigModel.FileConfig.EntityName);

            #endregion

            #region 表名称

            sb = sb.Replace("{表名称}", baseConfigModel.TableName);

            #endregion

            #region 字段

            var tableColum = new StringBuilder();
            var column = string.Empty;
            var remark = string.Empty;
            var datatype = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                column = dr["TableColumn"].ToString();
                remark = dr["Remark"].ToString();
                datatype = dr["Datatype"].ToString();
                datatype = TableMappingHelper.GetPropertyDatatype(datatype);
                tableColum.AppendLine("        /// <summary>");
                tableColum.AppendLine("        /// " + remark);
                tableColum.AppendLine("        /// </summary>");
                switch (datatype)
                {
                    case "long?":
                        tableColum.AppendLine("        [JsonConverter(typeof(StringJsonConverter))]");
                        break;

                    case "DateTime?":
                        tableColum.AppendLine("        [JsonConverter(typeof(DateTimeJsonConverter))]");
                        break;
                }

                tableColum.AppendLine("        public " + datatype + " " + column + " { get; set; }");
            }

            sb = sb.Replace("{字段}", tableColum.ToString());

            #endregion

            return sb.ToString();
        }

        #endregion

        #region BuildEntityParam 实体查询类

        public string BuildEntityParam(BaseConfigModel baseConfigModel, DataTable dt)
        {
            // 读取模板
            var sb = ReadTemplet("ListParam", baseConfigModel);

            #region 描述

            var describe = new StringBuilder();
            SetClassDescription("实体查询类", baseConfigModel, describe);
            sb = sb.Replace("{描述}", describe.ToString());

            #endregion

            #region 类名

            sb.Replace("{类名}", baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam"));

            #endregion

            #region 字段

            var tableColum = new StringBuilder();

            if (baseConfigModel.PageIndex.SearchField != null)
                foreach (var serachStr in baseConfigModel.PageIndex.SearchField)
                foreach (DataRow dr in dt.Rows)
                {
                    var column = dr["TableColumn"].ToString();
                    var remark = dr["Remark"].ToString();
                    var datatype = dr["Datatype"].ToString();
                    if (column == serachStr)
                    {
                        datatype = TableMappingHelper.GetPropertyDatatype(datatype);
                        tableColum.AppendLine("        /// <summary>");
                        tableColum.AppendLine("        /// " + remark);
                        tableColum.AppendLine("        /// </summary>");
                        tableColum.AppendLine("        /// <returns></returns>");
                        switch (datatype)
                        {
                            case "long?":
                                tableColum.AppendLine("        [JsonConverter(typeof(StringJsonConverter))]");
                                break;

                            case "DateTime?":
                                tableColum.AppendLine("        [JsonConverter(typeof(DateTimeJsonConverter))]");
                                break;
                        }

                        tableColum.AppendLine("        public " + datatype + " " + column + " { get; set; }");

                        break;
                    }
                }

            sb = sb.Replace("{字段}", tableColum.ToString());

            #endregion

            return sb.ToString();
        }

        #endregion

        #region BuildController 控制器

        public string BuildController(BaseConfigModel baseConfigModel)
        {
            // 读取模板
            var sb = ReadTemplet("Controller", baseConfigModel);

            #region 描述

            var describe = new StringBuilder();
            SetClassDescription("控制器类", baseConfigModel, describe);
            sb = sb.Replace("{描述}", describe.ToString());

            #endregion

            #region 类名

            sb = sb.Replace("{类名}", baseConfigModel.FileConfig.ControllerName);

            #endregion

            #region 业务类名

            sb = sb.Replace("{业务类名}", baseConfigModel.FileConfig.BusinessName);
            sb = sb.Replace("{驼峰业务类名}", TextHelper.StrFirstCharToLower(baseConfigModel.FileConfig.BusinessName));

            #endregion

            #region 实体类名

            sb = sb.Replace("{实体类名}", baseConfigModel.FileConfig.EntityName);

            #endregion

            #region 查询类名

            sb = sb.Replace("{查询类名}", baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam"));

            #endregion

            return sb.ToString();
        }

        #endregion

        #region BuildIndex 列表页面

        public string BuildIndex(BaseConfigModel baseConfigModel, DataTable dt)
        {
            #region 初始化集合

            if (baseConfigModel.PageIndex.ButtonList == null)
                baseConfigModel.PageIndex.ButtonList = new List<string>();

            if (baseConfigModel.PageIndex.ColumnList == null)
                baseConfigModel.PageIndex.ColumnList = new List<string>();

            #endregion

            // 读取模板
            var sb = ReadTemplet("Index", baseConfigModel);


            #region 是否启用搜索

            if (baseConfigModel.PageIndex.IsSearch == 1)
                sb = sb.Replace("{启用搜索}", "");
            else
                sb = sb.Replace("{启用搜索}", ".searchbar{ display:none; }");

            #endregion

            #region 是否显示工具栏

            if (baseConfigModel.PageIndex.ButtonList.Where(p => p != "btnSearch").Any())
            {
                if (baseConfigModel.PageIndex.ButtonList.Where(p => p == "btnAdd").Any())
                    sb = sb.Replace("{新增按钮}",
                        "<a id=\"btnAdd\" class=\"layui-btn layui-btn-sm icon-btn\" lay-event=\"add\"><i class=\"layui-icon\">&#xe654;</i>新增</a>");
                else
                    sb = sb.Replace("{新增按钮}", "");

                if (baseConfigModel.PageIndex.ButtonList.Where(p => p == "btnEdit").Any())
                    sb = sb.Replace("{修改按钮}",
                        "<a id=\"btnEdit\" class=\"layui-btn layui-btn-warm layui-btn-sm icon-btn\" lay-event=\"edit\"><i class=\"layui-icon\">&#xe642;</i>修改</a>");
                else
                    sb = sb.Replace("{修改按钮}", "");

                if (baseConfigModel.PageIndex.ButtonList.Where(p => p == "btnDelete").Any())
                    sb = sb.Replace("{删除按钮}",
                        "<a id=\"btnDelete\" class=\"layui-btn layui-btn-danger layui-btn-sm icon-btn\" lay-event=\"del\"><i class=\"layui-icon\">&#xe640;</i>删除</a>");
                else
                    sb = sb.Replace("{删除按钮}", "");
            }

            #endregion

            #region 表格列

            var columnList = new StringBuilder();
            foreach (var column in baseConfigModel.PageIndex.ColumnList)
            {
                var remark = string.Empty;
                foreach (DataRow dr in dt.Rows)
                    if (column == dr["TableColumn"].ToString())
                    {
                        remark = dr["Remark"].ToString();
                        break;
                    }

                remark = string.IsNullOrWhiteSpace(remark) ? column : remark;
                columnList.AppendLine("                    { field: '" + column + "', title: '" + remark +
                                      "', sort: true },");
            }

            sb = sb.Replace("{表格列}", columnList.ToString());

            #endregion

            #region 查询条件

            var condition = new StringBuilder();
            if (baseConfigModel.PageIndex.SearchField != null)
                foreach (var serachStr in baseConfigModel.PageIndex.SearchField)
                foreach (DataRow dr in dt.Rows)
                {
                    var column = dr["TableColumn"].ToString();
                    var remark = dr["Remark"].ToString();
                    var datatype = dr["Datatype"].ToString();
                    if (column == serachStr)
                    {
                        if (string.IsNullOrWhiteSpace(remark))
                            remark = column;

                        condition.AppendLine("                        <div class=\"layui-inline\">");
                        condition.AppendLine("                            <label class=\"layui-form-label\">" + remark +
                                             ":</label>");
                        condition.AppendLine("                            <div class=\"layui-input-inline\">");
                        datatype = TableMappingHelper.GetPropertyDatatype(datatype);
                        switch (datatype)
                        {
                            case "long?":
                            case "int?":
                            case "decimal?":
                                condition.AppendLine("                                <input name=\"" + column +
                                                     "\" class=\"layui-input\" placeholder=\"输入\" type=\"number\" />");
                                break;

                            default:
                                condition.AppendLine("                                <input name=\"" + column +
                                                     "\" class=\"layui-input\" placeholder=\"输入\" type=\"text\" />");
                                break;
                        }

                        condition.AppendLine("                            </div>");
                        condition.AppendLine("                        </div>");

                        break;
                    }
                }

            sb = sb.Replace("{查询条件}", condition.ToString());

            #endregion

            return sb.ToString();
        }

        #endregion

        #region BuildForm 表单页面

        public string BuildForm(BaseConfigModel baseConfigModel, DataTable dt)
        {
            // 初始化集合
            if (baseConfigModel.PageForm.FieldList == null)
                baseConfigModel.PageForm.FieldList = new List<string>();

            // 读取模板
            var sb = ReadTemplet("Form", baseConfigModel);

            #region 表单代码

            var fieldList = new StringBuilder();
            if (baseConfigModel.PageForm.FieldList.Count > 0)
            {
                var field = string.Empty;
                var fieldLower = string.Empty;
                var col = baseConfigModel.PageForm.ShowMode == 1 ? "12" : "6";

                for (var i = 0; i < baseConfigModel.PageForm.FieldList.Count; i++)
                {
                    // 第一个参数做一个必填示范
                    var req = "";
                    var req2 = " lay-verType=\"tips\" ";
                    if (i == 0)
                    {
                        req = " layui-form-required";
                        req2 = " lay-verType=\"tips\" lay-verify=\"required\" required ";
                    }

                    field = baseConfigModel.PageForm.FieldList[i];
                    var remark = string.Empty;
                    foreach (DataRow dr in dt.Rows)
                        if (field == dr["TableColumn"].ToString())
                        {
                            remark = dr["Remark"].ToString();
                            break;
                        }

                    remark = string.IsNullOrWhiteSpace(remark) ? field : remark;

                    fieldLower = TableMappingHelper.FirstLetterLowercase(field);

                    fieldList.AppendLine("            <div class=\"layui-col-sm" + col + "\">");
                    fieldList.AppendLine("                <div class=\"layui-form-item\">");
                    fieldList.AppendLine("                    <label class=\"layui-form-label" + req + "\">" + remark +
                                         "</label>");
                    fieldList.AppendLine("                    <div class=\"layui-input-block\">");
                    fieldList.AppendLine("                        <input id=\"" + fieldLower + "\" name=\"" + field +
                                         "\" autocomplete=\"off\" type=\"text\" placeholder=\"请输入\" class=\"layui-input\" " +
                                         req2 + " >");
                    fieldList.AppendLine("                    </div>");
                    fieldList.AppendLine("                </div>");
                    fieldList.AppendLine("            </div>");
                }
            }

            sb = sb.Replace("{表单控件}", fieldList.ToString());

            #endregion

            return sb.ToString();
        }

        #endregion

        #region BuildMenu

        public string BuildMenu(BaseConfigModel baseConfigModel)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("  菜单路径:" + baseConfigModel.OutputConfig.OutputModule + "/" +
                          baseConfigModel.FileConfig.ClassPrefix + "/" + baseConfigModel.FileConfig.PageIndexName);
            sb.AppendLine();
            var modulePrefix = GetModulePrefix(baseConfigModel);
            var classPrefix = baseConfigModel.FileConfig.ClassPrefix.ToLower();
            sb.AppendLine("  页面显示权限：" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "view"));
            sb.AppendLine();
            var list = GetButtonAuthorizeList();

            if (baseConfigModel.PageIndex.IsSearch == 1)
            {
                var button = list.Where(p => p.Key == "btnSearch").FirstOrDefault();
                sb.AppendLine("  按钮" + button.Description + "权限：" +
                              string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, button.Value));
            }

            foreach (var btn in baseConfigModel.PageIndex.ButtonList)
            {
                var button = list.Where(p => p.Key == btn).FirstOrDefault();
                sb.AppendLine("  按钮" + button.Description + "权限：" +
                              string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, button.Value));
            }

            sb.AppendLine();
            return sb.ToString();
        }

        #endregion

        #region BuildService 数据库服务

        public string BuildService(BaseConfigModel baseConfigModel, DataTable dt)
        {
            // 读取模板
            var sb = ReadTemplet("Service", baseConfigModel);

            #region 描述

            var describe = new StringBuilder();
            SetClassDescription("服务类", baseConfigModel, describe);
            sb = sb.Replace("{描述}", describe.ToString());

            #endregion

            #region 类名

            sb = sb.Replace("{类名}", baseConfigModel.FileConfig.ServiceName);

            #endregion

            #region 表名称

            sb = sb.Replace("{表名称}", baseConfigModel.TableName);

            #endregion

            #region 实体类名

            sb = sb.Replace("{实体类名}", baseConfigModel.FileConfig.EntityName);
            sb = sb.Replace("{驼峰实体类名}", TextHelper.StrFirstCharToLower(baseConfigModel.FileConfig.EntityName));

            #endregion

            #region 查询类名

            sb = sb.Replace("{查询类名}", baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam"));

            #endregion

            #region 查询条件

            var condition = new StringBuilder();
            if (baseConfigModel.PageIndex.SearchField != null)
                foreach (var serachStr in baseConfigModel.PageIndex.SearchField)
                foreach (DataRow dr in dt.Rows)
                {
                    var column = dr["TableColumn"].ToString();
                    var remark = dr["Remark"].ToString();
                    var datatype = dr["Datatype"].ToString();
                    if (column == serachStr)
                    {
                        condition.AppendLine("            // " + remark);
                        datatype = TableMappingHelper.GetPropertyDatatype(datatype);
                        switch (datatype)
                        {
                            case "long?":
                            case "int?":
                            case "decimal?":
                            case "DateTime?":
                            case "bool?":
                                condition.AppendLine("            if (param." + column + ".HasValue)");
                                condition.AppendLine("                query = query.Where(p => p." + column +
                                                     " == param." + column + ");");
                                break;

                            case "string":
                                condition.AppendLine("            if (!string.IsNullOrEmpty(param." + column + "))");
                                condition.AppendLine("                query = query.Where(p => p." + column +
                                                     ".Contains(param." + column + "));");
                                break;
                        }

                        break;
                    }
                }

            sb = sb.Replace("{查询条件}", condition.ToString());

            #endregion

            return sb.ToString();
        }

        public string BuildIService(BaseConfigModel baseConfigModel, DataTable dt)
        {
            // 读取模板
            var sb = ReadTemplet("IService", baseConfigModel);

            #region 描述

            var describe = new StringBuilder();
            SetClassDescription("服务接口", baseConfigModel, describe);
            sb = sb.Replace("{描述}", describe.ToString());

            #endregion

            #region 类名

            sb = sb.Replace("{类名}", baseConfigModel.FileConfig.ServiceName);

            #endregion

            #region 实体类名

            sb = sb.Replace("{实体类名}", baseConfigModel.FileConfig.EntityName);

            #endregion

            #region 查询类名

            sb = sb.Replace("{查询类名}", baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam"));

            #endregion

            return sb.ToString();
        }

        #endregion

        #region BuildBusiness 业务层

        public string BuildBusiness(BaseConfigModel baseConfigModel)
        {
            // 读取模板
            var sb = ReadTemplet("BLL", baseConfigModel);

            #region 描述

            var describe = new StringBuilder();
            SetClassDescription("业务类", baseConfigModel, describe);
            sb = sb.Replace("{描述}", describe.ToString());

            #endregion

            #region 类名

            sb = sb.Replace("{类名}", baseConfigModel.FileConfig.BusinessName);

            #endregion

            #region 数据服务类名

            sb = sb.Replace("{数据服务类名}", baseConfigModel.FileConfig.ServiceName);
            sb = sb.Replace("{驼峰数据服务类名}", TextHelper.StrFirstCharToLower(baseConfigModel.FileConfig.ServiceName));

            #endregion

            #region 实体类名

            sb = sb.Replace("{实体类名}", baseConfigModel.FileConfig.EntityName);

            #endregion

            #region 查询类名

            sb = sb.Replace("{查询类名}", baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam"));

            #endregion

            return sb.ToString();
        }

        public string BuildIBusiness(BaseConfigModel baseConfigModel)
        {
            // 读取模板
            var sb = ReadTemplet("IBLL", baseConfigModel);

            #region 描述

            var describe = new StringBuilder();
            SetClassDescription("业务类", baseConfigModel, describe);
            sb = sb.Replace("{描述}", describe.ToString());

            #endregion

            #region 类名

            sb = sb.Replace("{类名}", baseConfigModel.FileConfig.BusinessName);

            #endregion

            #region 数据服务类名

            sb = sb.Replace("{数据服务类名}", baseConfigModel.FileConfig.ServiceName);
            sb = sb.Replace("{驼峰数据服务类名}", TextHelper.StrFirstCharToLower(baseConfigModel.FileConfig.ServiceName));

            #endregion

            #region 实体类名

            sb = sb.Replace("{实体类名}", baseConfigModel.FileConfig.EntityName);

            #endregion

            #region 查询类名

            sb = sb.Replace("{查询类名}", baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam"));

            #endregion

            return sb.ToString();
        }

        #endregion

        #region CreateCode 写入代码

        public async Task<List<KeyValue>> CreateCode(BaseConfigModel baseConfigModel, string code)
        {
#if DEBUG
            LogHelper.Info("生成了代码");
#else
            throw new Exception("只有调试状态下可以生成代码！");
#endif

            var result = new List<KeyValue>();
            var param = code.ToJObject();

            #region 实体类

            if (!string.IsNullOrEmpty(param["CodeEntity"].ParseToString()))
            {
                var codeEntity = HttpUtility.HtmlDecode(param["CodeEntity"].ToString());
                var codePath = Path.Combine(baseConfigModel.OutputConfig.OutputEntity, projectName + ".Entity",
                    baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.EntityName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeEntity);
                    result.Add(new KeyValue {Key = "实体类", Value = codePath});
                }
            }

            #endregion

            #region 实体查询类

            if (!param["CodeEntityParam"].IsEmpty())
            {
                var codeListEntity = HttpUtility.HtmlDecode(param["CodeEntityParam"].ToString());
                var codePath = Path.Combine(baseConfigModel.OutputConfig.OutputEntity, projectName + ".Model", "Param",
                    baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.EntityParamName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeListEntity);
                    result.Add(new KeyValue {Key = "实体查询类", Value = codePath});
                }
            }

            #endregion

            #region 服务类

            if (!param["CodeService"].IsEmpty())
            {
                var codeService = HttpUtility.HtmlDecode(param["CodeService"].ToString());
                var codePath = Path.Combine(baseConfigModel.OutputConfig.OutputBusiness, projectName + ".Service",
                    baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.ServiceName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeService);
                    result.Add(new KeyValue {Key = "服务类", Value = codePath});
                }
            }

            if (!param["CodeIService"].IsEmpty())
            {
                var codeService = HttpUtility.HtmlDecode(param["CodeIService"].ToString());
                var codePath = Path.Combine(baseConfigModel.OutputConfig.OutputBusiness, projectName + ".IService",
                    baseConfigModel.OutputConfig.OutputModule, "I" + baseConfigModel.FileConfig.ServiceName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeService);
                    result.Add(new KeyValue {Key = "服务接口", Value = codePath});
                }
            }

            #endregion

            #region 业务类

            if (!param["CodeBusiness"].IsEmpty())
            {
                var codeBusiness = HttpUtility.HtmlDecode(param["CodeBusiness"].ToString());
                var codePath = Path.Combine(baseConfigModel.OutputConfig.OutputBusiness, projectName + ".Business",
                    baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.BusinessName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeBusiness);
                    result.Add(new KeyValue {Key = "业务类", Value = codePath});
                }
            }

            if (!param["CodeIBusiness"].IsEmpty())
            {
                var codeBusiness = HttpUtility.HtmlDecode(param["CodeIBusiness"].ToString());
                var codePath = Path.Combine(baseConfigModel.OutputConfig.OutputBusiness, projectName + ".IBusiness",
                    baseConfigModel.OutputConfig.OutputModule, "I" + baseConfigModel.FileConfig.BusinessName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeBusiness);
                    result.Add(new KeyValue {Key = "业务接口", Value = codePath});
                }
            }

            #endregion

            #region 控制器

            if (!param["CodeController"].IsEmpty() && baseConfigModel.NeedConroller == 1)
            {
                var codeController = HttpUtility.HtmlDecode(param["CodeController"].ToString());
                var codePath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas",
                    baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.ControllerName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeController);
                    result.Add(new KeyValue {Key = "控制器", Value = codePath});
                }
            }

            #endregion

            #region 菜单生成

            if (baseConfigModel.NeedSidebar == 1)
            {
                // 生成菜单

                var buttonAuthorizeList = GetButtonAuthorizeList();
                var menuUrl =
                    "#/" + TextHelper.StrFirstCharToLower(
                        baseConfigModel.OutputConfig.OutputModule.Replace("Manage", "")) + "/"
                    + TextHelper.StrFirstCharToLower(baseConfigModel.FileConfig.ClassPrefix) + "/"
                    + TextHelper.StrFirstCharToLower(baseConfigModel.FileConfig.PageIndexName);

                var modulePrefix = GetModulePrefix(baseConfigModel);
                var classPrefix = baseConfigModel.FileConfig.ClassPrefix.ToLower();
                var menuEntity = new MenuEntity
                {
                    MenuName = baseConfigModel.FileConfig.ClassDescription,
                    MenuUrl = menuUrl,
                    MenuType = (int) MenuTypeEnum.Menu,
                    Authorize = string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "view")
                };
                var obj = await AddMenu(menuEntity);
                if (obj.Tag == 1)
                {
                    result.Add(new KeyValue {Key = "菜单(刷新页面可见)", Value = menuUrl});
                    if (baseConfigModel.PageIndex.IsSearch == 1)
                    {
                        // 按钮搜索权限
                        var button = buttonAuthorizeList.Where(p => p.Key == "btnSearch").FirstOrDefault();
                        var buttonEntity = new MenuEntity
                        {
                            ParentId = menuEntity.Id,
                            MenuName = baseConfigModel.FileConfig.ClassDescription + button.Description,
                            MenuType = (int) MenuTypeEnum.Button,
                            Authorize = string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, button.Value)
                        };
                        await AddMenu(buttonEntity);
                    }

                    foreach (var btn in baseConfigModel.PageIndex.ButtonList)
                    {
                        var button = buttonAuthorizeList.Where(p => p.Key == btn).FirstOrDefault();
                        var buttonEntity = new MenuEntity
                        {
                            ParentId = menuEntity.Id,
                            MenuName = baseConfigModel.FileConfig.ClassDescription + button.Description,
                            MenuType = (int) MenuTypeEnum.Button,
                            Authorize = string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, button.Value)
                        };
                        await AddMenu(buttonEntity);
                    }
                }
            }

            #endregion

            #region 列表页

            if (!param["CodeIndex"].IsEmpty() && baseConfigModel.NeedHtml == 1)
            {
                var codeIndex = HttpUtility.HtmlDecode(param["CodeIndex"].ToString());
                var pageFolder = GlobalContext.SystemConfig.PageFolder;
                var codePath = Path.Combine(
                    baseConfigModel.OutputConfig.OutputWeb.Replace("Api", ""),
                    "wwwroot",
                    pageFolder,
                    TextHelper.StrFirstCharToLower(baseConfigModel.OutputConfig.OutputModule.Replace("Manage", "")),
                    TextHelper.StrFirstCharToLower(baseConfigModel.FileConfig.ClassPrefix),
                    TextHelper.StrFirstCharToLower(baseConfigModel.FileConfig.PageIndexName)
                    + ".html");

                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeIndex);
                    result.Add(new KeyValue {Key = "列表页", Value = codePath});
                }
            }

            #endregion

            #region 表单页

            if (!param["CodeForm"].IsEmpty() && baseConfigModel.NeedHtml == 1)
            {
                var codeSave = HttpUtility.HtmlDecode(param["CodeForm"].ToString());
                var pageFolder = GlobalContext.SystemConfig.PageFolder;
                var codePath = Path.Combine(
                    baseConfigModel.OutputConfig.OutputWeb.Replace("Api", ""),
                    "wwwroot",
                    pageFolder,
                    TextHelper.StrFirstCharToLower(baseConfigModel.OutputConfig.OutputModule.Replace("Manage", "")),
                    TextHelper.StrFirstCharToLower(baseConfigModel.FileConfig.ClassPrefix),
                    TextHelper.StrFirstCharToLower(baseConfigModel.FileConfig.PageFormName)
                    + ".html");

                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeSave);
                    result.Add(new KeyValue {Key = "表单页", Value = codePath});
                }
            }

            #endregion

            return result;
        }

        private async Task<TData> AddMenu(MenuEntity menuEntity)
        {
            var obj = new TData();
            IEnumerable<MenuEntity> menuList = _sysMenuRepository.Entities;

            if (!menuList.Where(p => p.MenuName == menuEntity.MenuName && p.Authorize == menuEntity.Authorize).Any())
            {
                menuEntity.MenuSort = menuList.Max(p => p.MenuSort) + 1;
                menuEntity.MenuStatus = 1;
                menuEntity.Id = IdGeneratorHelper.Instance.GetId();

                await _sysMenuRepository.InsertNowAsync(menuEntity);
                obj.Tag = 1;
            }

            return obj;
        }

        #endregion

        #region 私有方法

        /// <summary>
        ///     替换通用字符串
        /// </summary>
        private StringBuilder ReplaceCode(StringBuilder sb, BaseConfigModel baseConfigModel)
        {
            // 项目名称
            sb = sb.Replace("{项目名称}", projectName);

            // 命名空间
            sb = sb.Replace("{命名空间}", baseConfigModel.OutputConfig.OutputModule);

            // 类名前缀
            sb = sb.Replace("{类名前缀}", baseConfigModel.FileConfig.ClassPrefix);
            sb = sb.Replace("{驼峰类名前缀}", TextHelper.StrFirstCharToLower(baseConfigModel.FileConfig.ClassPrefix));

            return sb;
        }

        /// <summary>
        ///     获取项目路径
        /// </summary>
        private string GetProjectRootPath(string path)
        {
            path = path.ParseToString();
            path = path.Trim('\\');

#if DEBUG
            // 向上找一级
            path = Directory.GetParent(path).FullName;
#endif
            return path;
        }

        /// <summary>
        ///     类属性注释
        /// </summary>
        private void SetClassDescription(string type, BaseConfigModel baseConfigModel, StringBuilder sb)
        {
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 创 建：" + baseConfigModel.FileConfig.CreateName);
            sb.AppendLine("    /// 日 期：" + baseConfigModel.FileConfig.CreateDate);
            sb.AppendLine("    /// 描 述：" + baseConfigModel.FileConfig.ClassDescription + type);
            sb.AppendLine("    /// </summary>");
        }

        /// <summary>
        ///     权限按钮
        /// </summary>
        private List<KeyValue> GetButtonAuthorizeList()
        {
            var list = new List<KeyValue>();
            list.Add(new KeyValue {Key = "btnSearch", Value = "search", Description = "搜索"});
            list.Add(new KeyValue {Key = "btnAdd", Value = "add", Description = "新增"});
            list.Add(new KeyValue {Key = "btnEdit", Value = "edit", Description = "修改"});
            list.Add(new KeyValue {Key = "btnDelete", Value = "delete", Description = "删除"});
            return list;
        }

        /// <summary>
        ///     模块文件夹后缀
        /// </summary>
        /// <param name="baseConfigModel"></param>
        /// <returns></returns>
        private string GetModulePrefix(BaseConfigModel baseConfigModel)
        {
            return baseConfigModel.OutputConfig.OutputModule.Replace("Manage", string.Empty).ToLower();
        }

        /// <summary>
        ///     获取类名
        /// </summary>
        private string GetClassName(string tableName)
        {
            foreach (var fix in tableFixs)
                if (tableName.StartsWith(fix))
                    return tableName.Remove(0, fix.Length);
            return tableName;
        }

        /// <summary>
        ///     读取代码模板
        /// </summary>
        private StringBuilder ReadTemplet(string flag, BaseConfigModel bc)
        {
            var item = _codeRepository.AsQueryable().Where(a => a.Flag == flag).FirstOrDefault();
            if (item == null)
                throw new Exception("找不到相应的模板：" + flag);

            var sb = new StringBuilder(item.Code);
            sb = ReplaceCode(sb, bc);
            return sb;
        }

        #endregion
    }
}
