using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;
using YiSha.Util.Helper;
using Newtonsoft.Json;

namespace YiSha.Entity
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-23 08:24
    ///     描 述：MQTT主题订阅实体类
    /// </summary>
    [Table("SysMqttTheme")]
    public class MqttThemeEntity : IEntity<MasterDbContextLocator>
    {
        /// <summary>
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? Id { get; set; }

        /// <summary>
        ///     订阅主题名称
        /// </summary>
        public string ThemeName { get; set; }

        /// <summary>
        ///     是否订阅
        /// </summary>
        public bool? IsSubscribe { get; set; }
    }
}
