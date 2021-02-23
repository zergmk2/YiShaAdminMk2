using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion;
using Furion.DependencyInjection;
using YiSha.IBusiness.SystemManage;
using YiSha.IService.SystemManage;
using YiSha.Model;
using YiSha.Model.Result;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    public class DatabaseTableBLL : IDatabaseTableBLL, ITransient
    {
        #region 提交数据

        public async Task<string> DatabaseBackup(string backupPath)
        {
            var database = HtmlHelper.Resove(App.Configuration["ConnectionStrings:DBConnectionString"].ToLower(),
                "database=", ";");
            await _databaseTableService.DatabaseBackup(database, backupPath);
            return backupPath;
        }

        #endregion

        #region 构造函数

        private readonly IDatabaseTableService _databaseTableService;

        public DatabaseTableBLL(IDatabaseTableService databaseTableService)
        {
            _databaseTableService = databaseTableService;
        }

        #endregion

        #region 获取数据

        public async Task<TData<List<TableInfo>>> GetTableList(string tableName)
        {
            var obj = new TData<List<TableInfo>>();
            var list = await _databaseTableService.GetTableList(tableName);
            obj.Data = list;
            obj.Total = list.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<TableInfo>>> GetTablePageList(string tableName, Pagination pagination)
        {
            var obj = new TData<List<TableInfo>>();
            var list = await _databaseTableService.GetTablePageList(tableName, pagination);
            obj.Data = list;
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        ///     获取表字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<TData<List<TableFieldInfo>>> GetTableFieldList(string tableName)
        {
            var obj = new TData<List<TableFieldInfo>>();
            var list = await _databaseTableService.GetTableFieldList(tableName);
            obj.Data = list;
            obj.Total = list.Count;
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        ///     获取表字段，去掉基础字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<TData<List<TableFieldInfo>>> GetTableFieldPartList(string tableName)
        {
            var obj = new TData<List<TableFieldInfo>>();
            var list = await _databaseTableService.GetTableFieldList(tableName);
            obj.Data = list;
            obj.Data.RemoveAll(p => BaseField.BaseFieldList.Contains(p.TableColumn));
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ZtreeInfo>>> GetTableFieldZtreeList(string tableName)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Data = new List<ZtreeInfo>();
            if (string.IsNullOrEmpty(tableName)) return obj;
            var list = await _databaseTableService.GetTableFieldList(tableName);
            obj.Data.Add(new ZtreeInfo {id = 1, pId = 0, name = tableName});
            var sName = string.Empty;
            for (var i = 0; i < list.Count; i++)
            {
                sName = list[i].TableColumn;
                obj.Data.Add(new ZtreeInfo
                {
                    id = i + 2,
                    pId = 1,
                    name = sName
                });
            }

            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}
