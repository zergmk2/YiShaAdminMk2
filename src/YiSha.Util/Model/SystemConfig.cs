using Furion.ConfigurableOptions;
using YiSha.Util.Helper;
using Microsoft.Extensions.Configuration;
using NLog.Fluent;

namespace YiSha.Util.Model
{
    [OptionsSettings("SystemConfig")]
    public class SystemConfig : IConfigurableOptions<SystemConfig>
    {
        #region 判断参数

        /// <summary>
        ///     缓存服务
        /// </summary>
        public string CacheService
        {
            get
            {
                if (string.IsNullOrEmpty(CacheType))
                    return "UnKnown";
                if (CacheType.ToUpper() == "REDIS")
                    return "RedisCache";
                return "MemoryCache";
            }
        }

        #endregion

        #region 配置字段

        /// <summary>
        /// Default file storage location
        /// </summary>
        public string DefaultFileStorage { set; get; }

        /// <summary>
        ///     用户默认密码
        /// </summary>
        public string DefaultUserPWD { set; get; }

        /// <summary>
        ///     背景图片资源请求url
        /// </summary>
        public string[] BackgroundGetUrl { set; get; }

        public string PageFolder { get; set; }

        public string IgnoreToken { get; set; }

        public string LogAllApi { get; set; }

        public int SnowFlakeWorkerId { get; set; } = 1;

        /// <summary>
        ///     Redis链接串
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        ///     缓存使用类型
        /// </summary>
        public string CacheType { get; set; }

        /// <summary>
        ///     是否启用MQTT服务
        /// </summary>
        public bool MqttIsOpen { set; get; } = false;

        /// <summary>
        ///     MQTT服务地址
        /// </summary>
        public string MqttTcpServer { set; get; }

        /// <summary>
        ///     MQTT服务端口
        /// </summary>
        public int? MqttTcpHost { get; set; } = 80;

        /// <summary>
        ///     MQTT用户名
        /// </summary>
        public string MqttUserName { set; get; }

        /// <summary>
        ///     MQTT密码
        /// </summary>
        public string MqttPasswrod { set; get; }

        /// <summary>
        /// 是否是调试模式
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// 允许一个用户在多个电脑同时登录
        /// </summary>
        public bool LoginMultiple { get; set; }

        /// <summary>
        /// 网站虚拟目录
        /// </summary>
        public string VirtualDirectory { get; set; }

        /// <summary>
        /// api地址
        /// </summary>
        public string ApiSite { get; set; }

        public string LoginProvider { get; set; }
        public string ServerUrl { get; set; }

        #endregion

        public void PostConfigure(SystemConfig options, IConfiguration configuration)
        {
        }
    }
}
