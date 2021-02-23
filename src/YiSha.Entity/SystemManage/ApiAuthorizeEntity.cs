using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;
using YiSha.Util.Helper;
using Newtonsoft.Json;

namespace YiSha.Entity
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 16:22
    ///     描 述：接口权限实体类
    /// </summary>
    [Table("SysApiAuthorize")]
    public class ApiAuthorizeEntity : IEntity<MasterDbContextLocator>
    {
        /// <summary>
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? Id { get; set; }

        /// <summary>
        ///     请求接口
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     菜单权限标识
        /// </summary>
        public string Authorize { get; set; }
    }
}
