using System.Collections.Generic;

namespace YiSha.Util.Model
{
    public class BaseConfigModel
    {
        /// <summary>
        ///     数据库表名sys_menu
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///     表名首字母大写SysMenu
        /// </summary>
        public string TableNameUpper { get; set; }

        public FileConfigModel FileConfig { get; set; }
        public OutputConfigModel OutputConfig { get; set; }
        public PageIndexModel PageIndex { get; set; }
        public PageFormModel PageForm { get; set; }

        public string Code { set; get; }

        /// <summary>
        ///     需要侧边栏权限
        /// </summary>
        public int? NeedSidebar { get; set; }

        /// <summary>
        ///     需要生成控制器
        /// </summary>
        public int? NeedConroller { get; set; }

        /// <summary>
        ///     需要生成页面代码
        /// </summary>
        public int? NeedHtml { get; set; }

        public string ClassName { get; set; }
    }

    public class FileConfigModel
    {
        public string ClassPrefix { get; set; }
        public string ClassDescription { get; set; }
        public string CreateName { get; set; }
        public string CreateDate { get; set; }

        public string EntityName { get; set; }
        public string EntityMapName { get; set; }
        public string EntityParamName { get; set; }

        public string BusinessName { get; set; }
        public string ServiceName { get; set; }

        public string ControllerName { get; set; }
        public string PageIndexName { get; set; }
        public string PageFormName { get; set; }
    }

    public class OutputConfigModel
    {
        public List<KeyValue> ModuleList { get; set; }
        public string OutputModule { get; set; }
        public string OutputEntity { get; set; }
        public string OutputBusiness { get; set; }
        public string OutputWeb { get; set; }
    }

    public class PageIndexModel
    {
        /// <summary>
        ///     是否有搜索
        /// </summary>
        public int IsSearch { get; set; }

        /// <summary>
        ///     工具栏按钮（新增 修改 删除）
        /// </summary>
        public List<string> ButtonList { get; set; }

        /// <summary>
        ///     是否有分页
        /// </summary>
        public int IsPagination { get; set; }

        public List<string> ColumnList { get; set; }

        /// <summary>
        ///     查询菜单
        /// </summary>
        public List<string> SearchField { set; get; }
    }

    public class PageFormModel
    {
        /// <summary>
        ///     1表示显示成1列，2表示显示成2列
        /// </summary>
        public int ShowMode { get; set; }

        public List<string> FieldList { get; set; }
    }
}
