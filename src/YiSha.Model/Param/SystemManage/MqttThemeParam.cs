namespace YiSha.Model.Param.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-23 08:24
    ///     描 述：MQTT主题订阅实体查询类
    /// </summary>
    public class MqttThemeListParam
    {
        /// <summary>
        ///     订阅主题名称
        /// </summary>
        /// <returns></returns>
        public string ThemeName { get; set; }


        /// <summary>
        ///     是否订阅
        /// </summary>
        public bool? IsSubscribe { get; set; }
    }
}
