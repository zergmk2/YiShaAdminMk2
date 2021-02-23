using System;
using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;
using YiSha.Util.Helper;
using Newtonsoft.Json;
using SqlSugar;

namespace YiSha.Entity
{
    /// <summary>
    ///     创 建：
    ///     日 期：2020-12-04 12:49
    ///     描 述：Api日志实体类
    /// </summary>
    [Table("SysLogApi")]
    [SugarTable("SysLogApi")]
    public class LogApiEntity : IEntity<MasterDbContextLocator>
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
        ///     备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///     接口地址
        /// </summary>
        public string ExecuteUrl { get; set; }

        /// <summary>
        ///     请求参数
        /// </summary>
        public string ExecuteParam { get; set; }

        /// <summary>
        ///     请求结果
        /// </summary>
        public string ExecuteResult { get; set; }

        /// <summary>
        ///     执行时间
        /// </summary>
        public int? ExecuteTime { get; set; }

        /// <summary>
        ///     IP地址
        /// </summary>
        public string IpAddress { get; set; }

        [NotMapped]
        [SugarColumn(IsIgnore = true)]
        public string IpLocation { set; get; }

        [NotMapped]
        [SugarColumn(IsIgnore = true)]
        public string UserName { get; set; }
    }
}
