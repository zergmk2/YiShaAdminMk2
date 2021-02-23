using System;
using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;
using YiSha.Util.Helper;
using Newtonsoft.Json;

namespace YiSha.Entity
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 12:55
    ///     描 述：登陆日志实体类
    /// </summary>
    [Table("SysLogLogin")]
    public class LogLoginEntity : IEntity<MasterDbContextLocator>
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
        ///     执行状态(0失败 1成功)
        /// </summary>
        public int? LogStatus { get; set; }

        /// <summary>
        ///     ip地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        ///     ip位置
        /// </summary>
        public string IpLocation { get; set; }

        /// <summary>
        ///     浏览器
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        ///     操作系统
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///     额外备注
        /// </summary>
        public string ExtraRemark { get; set; }

        [NotMapped] public string UserName { get; set; }
    }
}
