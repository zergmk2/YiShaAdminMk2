using System;
using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;
using YiSha.Util.Helper;
using Newtonsoft.Json;

namespace YiSha.Entity
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 12:41
    ///     描 述：用户信息实体类
    /// </summary>
    [Table("SysUser")]
    public class UserEntity : IEntity<MasterDbContextLocator>
    {
        /// <summary>
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? Id { get; set; }

        /// <summary>
        /// </summary>
        public int? IsDelete { get; set; }

        /// <summary>
        /// </summary>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? CreatorId { get; set; }

        /// <summary>
        ///     用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     密码盐值
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        ///     姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        ///     所属部门Id
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? DepartmentId { get; set; }

        /// <summary>
        ///     性别(0未知 1男 2女)
        /// </summary>
        public int? Gender { get; set; }

        /// <summary>
        ///     出生日期
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        ///     头像
        /// </summary>
        public string Portrait { get; set; }

        /// <summary>
        ///     Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     手机
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        ///     QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        ///     微信
        /// </summary>
        public string WeChat { get; set; }

        /// <summary>
        ///     登录次数
        /// </summary>
        public int? LoginCount { get; set; }

        /// <summary>
        ///     用户状态(0禁用 1启用)
        /// </summary>
        public int? UserStatus { get; set; }

        /// <summary>
        ///     系统用户(0不是 1是[系统用户拥有所有的权限])
        /// </summary>
        public int? IsSystem { get; set; }

        /// <summary>
        ///     在线(0不是 1是)
        /// </summary>
        public int? IsOnline { get; set; }

        /// <summary>
        ///     首次登录时间
        /// </summary>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? FirstVisit { get; set; }

        /// <summary>
        ///     上一次登录时间
        /// </summary>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? PreviousVisit { get; set; }

        /// <summary>
        ///     最后一次登录时间
        /// </summary>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? LastVisit { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///     ApiToken
        /// </summary>
        public string ApiToken { get; set; }

        /// <summary>
        ///     相片
        /// </summary>
        public string Picture { get; set; }

        [NotMapped] public string RoleIds { get; set; }

        [NotMapped] public string PositionIds { get; set; }

        [NotMapped] public string DepartmentName { get; set; }
    }
}
