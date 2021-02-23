using System.Linq;
using Furion;
using Furion.DatabaseAccessor;
using YiSha.Data.DbContexts;
using YiSha.Util.Helper;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace YiSha.Data
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // 数据库访问注册
            services.AddDatabaseAccessor(options =>
            {
                // 注册数据库访问上下文
                options.AddDbPool<DefaultDbContext>(DbProvider.Sqlite);
            },"YiSha.Database.Migrations");

            // 注册 SqlSugar 服务
            services.AddSqlSugar(new ConnectionConfig
            {
                ConnectionString = App.Configuration["ConnectionStrings:DefaultConnectionString"],
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            },
            db =>
            {
                //处理日志事务
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    LogHelper.Debug(sql);
                    LogHelper.Debug((string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value))));
                    LogHelper.Debug("========================");
                };
            });
        }
    }
}
