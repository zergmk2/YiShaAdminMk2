using System;
using System.Text;
using System.Threading.Tasks;
using Furion;
using Furion.DependencyInjection;
using YiSha.Entity;
using YiSha.IBusiness.SystemManage;
using YiSha.IService.SystemManage;
using YiSha.Model.Param.SystemManage;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using YiSha.Util;
using YiSha.Util.Helper;

namespace YiSha.Mqtt
{
    public class MqttClientCenter : ISingleton
    {
        /// <summary>
        ///     MQTT实例
        /// </summary>
        private static IMqttClient _mqttClient;

        /// <summary>
        ///     MQTT服务器连接
        /// </summary>
        public async Task ConnectMqttServerAsync()
        {
            // 配置文件设置了不启用MQTT服务
            if (!GlobalContext.SystemConfig.MqttIsOpen)
                return;

            try
            {
                var factory = new MqttFactory();
                _mqttClient = factory.CreateMqttClient();

                var options = new MqttClientOptionsBuilder()
                    .WithTcpServer(GlobalContext.SystemConfig.MqttTcpServer, GlobalContext.SystemConfig.MqttTcpHost)
                    .WithCredentials(GlobalContext.SystemConfig.MqttUserName, GlobalContext.SystemConfig.MqttPasswrod)
                    .WithClientId(Guid.NewGuid().ToString().Substring(0, 5))
                    .Build();

                // 接受消息
                _mqttClient.UseApplicationMessageReceivedHandler(async e =>
                {
                    try
                    {
                        // 获取收到的信息
                        var msg = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        // 响应主题
                        var topic = e.ApplicationMessage.Topic;
                        if (string.IsNullOrEmpty(msg))
                            return;

                        // 日志记录
                        var mqttMsgBll = App.ServiceProvider.GetService<IMqttMsgBLL>();
                        await mqttMsgBll.SaveForm(new MqttMsgEntity
                            {Msg = msg, ThemeName = topic, Time = DateTime.Now});

                        // 调用业务方法需要： var bll = App.ServiceProvider.GetService<业务类名>();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("接受到MQTT消息后处理失败！", ex);
                    }
                });

                // 重连机制
                _mqttClient.UseDisconnectedHandler(async e =>
                {
                    Console.WriteLine("与服务器断开连接！");
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    try
                    {
                        await _mqttClient.ConnectAsync(options);
                    }
                    catch (Exception exp)
                    {
                        LogHelper.Error($"重新连接服务器失败 Msg：{exp}");
                    }
                });

                await _mqttClient.ConnectAsync(options);

                // 批量订阅
                var mqttThemeBll = App.ServiceProvider.GetService<IMqttThemeService>();
                var themes = await mqttThemeBll.GetList(new MqttThemeListParam {IsSubscribe = true});
                foreach (var topic in themes)
                    await Subscribe(topic.ThemeName);
            }
            catch (Exception exp)
            {
                LogHelper.Error("连接服务器失败", exp);
            }
        }

        /// <summary>
        ///     订阅
        /// </summary>
        /// <param name="topicName"></param>
        public async Task Subscribe(string topicName)
        {
            var topic = topicName.Trim();
            if (string.IsNullOrEmpty(topic))
            {
                LogHelper.Error("订阅主题不能为空！");
                return;
            }

            Console.WriteLine("订阅主题：" + topicName + "成功");

            if (!_mqttClient.IsConnected)
            {
                LogHelper.Error("MQTT客户端尚未连接！");
                return;
            }

            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
        }

        /// <summary>
        ///     取消订阅
        /// </summary>
        /// <param name="topicName"></param>
        public async Task Unsubscribe(string topicName)
        {
            var topic = topicName.Trim();
            if (string.IsNullOrEmpty(topic))
            {
                LogHelper.Error("订阅主题不能为空！");
                return;
            }

            if (!_mqttClient.IsConnected)
            {
                LogHelper.Error("MQTT客户端尚未连接！");
                return;
            }

            await _mqttClient.UnsubscribeAsync(topic);
        }
    }
}
