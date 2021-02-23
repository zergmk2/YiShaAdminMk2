using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;
using YiSha.Util.Helper;
using Newtonsoft.Json;

namespace YiSha.Entity
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 12:28
    ///     描 述：代码模板实体类
    /// </summary>
    [Table("SysCodeTemplet")]
    public class CodeTempletEntity : IEntity<MasterDbContextLocator>
    {
        /// <summary>
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? Id { get; set; }

        /// <summary>
        ///     代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///     标识
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        ///     语言类型
        /// </summary>
        public string Type { get; set; }
    }
}
