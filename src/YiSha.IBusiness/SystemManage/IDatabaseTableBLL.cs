using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Model;
using YiSha.Model.Result;
using YiSha.Util.Model;

namespace YiSha.IBusiness.SystemManage
{
    public interface IDatabaseTableBLL
    {
        #region 提交数据

        Task<string> DatabaseBackup(string backupPath);

        #endregion

        #region 获取数据

        Task<TData<List<TableInfo>>> GetTableList(string tableName);

        Task<TData<List<TableInfo>>> GetTablePageList(string tableName, Pagination pagination);

        /// <summary>
        ///     获取表字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<TData<List<TableFieldInfo>>> GetTableFieldList(string tableName);

        /// <summary>
        ///     获取表字段，去掉基础字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<TData<List<TableFieldInfo>>> GetTableFieldPartList(string tableName);

        Task<TData<List<ZtreeInfo>>> GetTableFieldZtreeList(string tableName);

        #endregion
    }
}
