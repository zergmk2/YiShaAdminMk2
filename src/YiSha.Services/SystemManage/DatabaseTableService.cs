using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion.DatabaseAccessor.Extensions;
using Furion.DependencyInjection;
using YiSha.IService.SystemManage;
using YiSha.Model.Result;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Service.SystemManage
{
    public class DatabaseTableService : IDatabaseTableService, ITransient
    {
        #region 公有方法

        public async Task<bool> DatabaseBackup(string database, string backupPath)
        {
            var backupFile = string.Format("{0}\\{1}_{2}.bak", backupPath, database,
                DateTime.Now.ToString("yyyyMMddHHmmss"));
            var strSql = string.Format(" backup database [{0}] to disk = '{1}'", database, backupFile);

            var result = await strSql.SqlNonQueryAsync();
            return result > 0 ? true : false;
        }

        #endregion

        #region 获取数据

        public async Task<List<TableInfo>> GetTableList(string tableName)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT id Id,name TableName FROM sysobjects WHERE xtype = 'u' order by name");
            IEnumerable<TableInfo> list = await strSql.ToString().SqlQueryAsync<TableInfo>();

            if (!tableName.IsEmpty())
                list = list.Where(p => p.TableName.Contains(tableName));

            await SetTableDetail(list);
            return list.ToList();
        }

        public async Task<List<TableInfo>> GetTablePageList(string tableName, Pagination pagination)
        {
            var strSql = new StringBuilder();

            strSql.Append(@"SELECT id Id,name TableName FROM sysobjects WHERE xtype = 'u'");

            if (!tableName.IsEmpty())
                strSql.Append(" AND name like @tableName");

            IEnumerable<TableInfo> list = await strSql.ToString()
                .SqlQueryAsync<TableInfo>(new Dictionary<string, object> {{"tableName", "%" + tableName + "%"}});
            pagination.TotalCount = list.Count();
            list = list.Skip(pagination.PageSize * (pagination.PageIndex - 1)).Take(pagination.PageSize);

            await SetTableDetail(list);
            return list.ToList();
        }

        public async Task<List<TableFieldInfo>> GetTableFieldList(string tableName)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT
                                  TableColumn = rtrim(b.name),
                                  TableIdentity = CASE WHEN h.id IS NOT NULL  THEN 'PK' ELSE '' END,
                                  Datatype = type_name(b.xusertype)+CASE WHEN b.colstat&1=1 THEN '[ID(' + CONVERT(varchar, ident_seed(a.name))+','+CONVERT(varchar,ident_incr(a.name))+')]' ELSE '' END,
                                  FieldLength = b.length,
                                  IsNullable = CASE b.isnullable WHEN 0 THEN 'N' ELSE 'Y' END,
                                  FieldDefault = ISNULL(e.text, ''),
                                  Remark = (SELECT ep.value FROM sys.columns sc LEFT JOIN sys.extended_properties ep ON ep.major_id = sc.object_id AND ep.minor_id = sc.column_id
										                    WHERE sc.object_id = a.id AND sc.name = b.name)
                            FROM sysobjects a, syscolumns b
                            LEFT OUTER JOIN syscomments e ON b.cdefault = e.id
                            LEFT OUTER JOIN (Select g.id, g.colid FROM sysindexes f, sysindexkeys g Where (f.id=g.id)AND(f.indid=g.indid)AND(f.indid>0)AND(f.indid<255)AND(f.status&2048)<>0) h ON (b.id=h.id)AND(b.colid=h.colid)
                            Where (a.id=b.id)AND(a.id=object_id(@TableName))
                                  ORDER BY b.colid");

            var list = await strSql.ToString().SqlQueryAsync<TableFieldInfo>(new {TableName = tableName});
            return list;
        }

        #endregion

        #region 私有方法

        /// <summary>
        ///     获取所有表的主键、主键名称、记录数
        /// </summary>
        /// <returns></returns>
        private async Task<List<TableInfo>> GetTableDetailList()
        {
            var strSql = @"SELECT (SELECT name FROM sysobjects as t WHERE xtype = 'U' and t.id = sc.id) TableName,
                                     sc.id Id,sc.name TableKey,sysobjects.name TableKeyName,sysindexes.rows TableCount
                                     FROM syscolumns sc ,sysobjects,sysindexes,sysindexkeys
                                     WHERE sysobjects.xtype = 'PK'
                                           AND sysobjects.parent_obj = sc.id
                                           AND sysindexes.id = sc.id
                                           AND sysobjects.name = sysindexes.name AND sysindexkeys.id = sc.id
                                           AND sysindexkeys.indid = sysindexes.indid
                                           AND sc.colid = sysindexkeys.colid;";

            var list = await strSql.SqlQueryAsync<TableInfo>();
            return list;
        }

        /// <summary>
        ///     赋值表的主键、主键名称、记录数
        /// </summary>
        /// <param name="list"></param>
        private async Task SetTableDetail(IEnumerable<TableInfo> list)
        {
            var detailList = await GetTableDetailList();
            foreach (var table in list)
            {
                table.TableKey = string.Join(",", detailList.Where(p => p.Id == table.Id).Select(p => p.TableKey));
                var tableInfo = detailList.Where(p => p.TableName == table.TableName).FirstOrDefault();
                if (tableInfo != null)
                {
                    table.TableKeyName = tableInfo.TableKeyName;
                    table.TableCount = tableInfo.TableCount;
                    table.Remark = tableInfo.Remark;
                }
            }
        }

        #endregion
    }
}
