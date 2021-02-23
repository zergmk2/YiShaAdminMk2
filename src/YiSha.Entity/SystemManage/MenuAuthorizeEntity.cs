using System;
using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;
using YiSha.Util.Helper;
using Newtonsoft.Json;

namespace YiSha.Entity
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 11:16
    ///     描 述：菜单权限实体类
    /// </summary>
    [Table("SysMenuAuthorize")]
    public class MenuAuthorizeEntity : IEntity<MasterDbContextLocator>
    {
        /// <summary>
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? CreatorId { get; set; }

        /// <summary>
        ///     菜单Id
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? MenuId { get; set; }

        /// <summary>
        ///     授权Id(角色Id或者用户Id)
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? AuthorizeId { get; set; }

        /// <summary>
        ///     授权类型(1角色 2用户)
        /// </summary>
        public int? AuthorizeType { get; set; }
    }
}
