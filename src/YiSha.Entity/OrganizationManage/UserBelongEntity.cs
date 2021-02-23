using System;
using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;
using YiSha.Util.Helper;
using Newtonsoft.Json;

namespace YiSha.Entity
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 13:08
    ///     描 述：用户关联信息实体类
    /// </summary>
    [Table("SysUserBelong")]
    public class UserBelongEntity : IEntity<MasterDbContextLocator>
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
        ///     用户Id
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? UserId { get; set; }

        /// <summary>
        ///     职位Id或者角色Id
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? BelongId { get; set; }

        /// <summary>
        ///     所属类型(1职位 2角色)
        /// </summary>
        public int? BelongType { get; set; }
    }
}
