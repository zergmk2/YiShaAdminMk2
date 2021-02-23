using System;
using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;
using YiSha.Util.Helper;
using Newtonsoft.Json;

namespace YiSha.Entity
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-18 16:04
    ///     描 述：数据字典实体类
    /// </summary>
    [Table("SysDataDict")]
    public class DataDictEntity : IEntity<MasterDbContextLocator>
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
        ///     字典类型
        /// </summary>
        public string DictType { get; set; }

        /// <summary>
        ///     字典排序
        /// </summary>
        public int? DictSort { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Remark { get; set; }
    }
}
