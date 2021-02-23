using System;
using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;
using YiSha.Util.Helper;
using Newtonsoft.Json;

namespace YiSha.Entity
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-06 09:46
    ///     描 述：角色信息实体类
    /// </summary>
    [Table("SysRole")]
    public class RoleEntity : IEntity<MasterDbContextLocator>
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
        ///     角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        ///     角色排序
        /// </summary>
        public int? RoleSort { get; set; }

        /// <summary>
        ///     角色状态(0禁用 1启用)
        /// </summary>
        public int? RoleStatus { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 角色对应的菜单，页面和按钮
        /// </summary>
        [NotMapped]
        public string MenuIds { get; set; }
    }
}
