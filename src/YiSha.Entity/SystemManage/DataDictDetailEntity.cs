using System;
using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;
using YiSha.Util.Helper;
using Newtonsoft.Json;

namespace YiSha.Entity
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-19 08:59
    ///     描 述：数据字典值实体类
    /// </summary>
    [Table("SysDataDictDetail")]
    public class DataDictDetailEntity : IEntity<MasterDbContextLocator>
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
        ///     字典类型(外键)
        /// </summary>
        public string DictType { get; set; }

        /// <summary>
        ///     字典排序
        /// </summary>
        public int? DictSort { get; set; }

        /// <summary>
        ///     字典键(一般从1开始)
        /// </summary>
        public int? DictKey { get; set; }

        /// <summary>
        ///     字典值
        /// </summary>
        public string DictValue { get; set; }

        /// <summary>
        ///     显示样式(default primary success info warning danger)
        /// </summary>
        public string ListClass { get; set; }

        /// <summary>
        ///     字典状态(0禁用 1启用)
        /// </summary>
        public int? DictStatus { get; set; }

        /// <summary>
        ///     默认选中(0不是 1是)
        /// </summary>
        public int? IsDefault { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Remark { get; set; }
    }
}
