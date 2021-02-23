using System.Collections.Generic;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;

namespace YiSha.Util.Helper
{
    public static class IRepositoryHelper
    {
        /// <summary>
        ///     批量删除，且数据存在才根据主键删除（立即提交）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Db"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static async Task BatchDeleteAsync<T>(this IRepository<T> Db, ICollection<long> ids)
            where T : class, IPrivateEntity, new()
        {
            foreach (var id in ids) await Db.DeleteExistsNowAsync(id);
            ;
        }

        /// <summary>
        ///     批量删除，且数据存在才根据主键删除（立即提交）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Db"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static async Task BatchDeleteAsync<T>(this IRepository<T> Db, ICollection<string> ids)
            where T : class, IPrivateEntity, new()
        {
            foreach (var id in ids) await Db.DeleteExistsNowAsync(id.ParseToLong());
            ;
        }
    }
}
