using System;
using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;
using YiSha.Util.Helper;
using Newtonsoft.Json;

namespace YiSha.Entity
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-23 08:40
    ///     描 述：MQTT消息记录实体类
    /// </summary>
    [Table("SysMqttMsg")]
    public class MqttMsgEntity : IEntity<MasterDbContextLocator>
    {
        /// <summary>
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? Id { get; set; }

        /// <summary>
        ///     主题名称
        /// </summary>
        public string ThemeName { get; set; }

        /// <summary>
        ///     消息
        /// </summary>
        public string Msg { get; set; }

        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? Time { set; get; }
    }
}
