using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;

namespace YiSha.Data.DbContexts
{
    /// <summary>
    ///     默认数据库上下文
    /// </summary>
    [AppDbContext("DefaultConnectionString")]
    public class DefaultDbContext : AppDbContext<DefaultDbContext, MasterDbContextLocator>
    {
        /// <summary>
        ///     继承父类构造函数
        /// </summary>
        /// <param name="options"></param>
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
            EnabledEntityChangedListener = true;
        }
    }
}
