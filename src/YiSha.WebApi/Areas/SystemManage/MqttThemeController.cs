using System.Collections.Generic;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using YiSha.Entity;
using YiSha.IBusiness.SystemManage;
using YiSha.Model.Param.SystemManage;
using Microsoft.AspNetCore.Mvc;
using YiSha.Mqtt;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.WebApi.Areas.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-23 08:24
    ///     描 述：MQTT主题订阅控制器类
    /// </summary>
    [Route("SystemManage/[controller]")]
    public class MqttThemeController : BaseController
    {
        private readonly MqttClientCenter _mqttClientCenter;
        private readonly IMqttMsgBLL _mqttMsgBLL;
        private readonly IMqttThemeBLL _mqttThemeBLL;

        public MqttThemeController(IMqttThemeBLL mqttThemeBLL, IMqttMsgBLL mqttMsgBLL,
            MqttClientCenter mqttClientCenter)
        {
            _mqttThemeBLL = mqttThemeBLL;
            _mqttMsgBLL = mqttMsgBLL;
            _mqttClientCenter = mqttClientCenter;
        }

        #region 获取数据

        /// <summary>
        ///     条件查询
        /// </summary>
        [HttpGet]
        public async Task<TData<List<MqttThemeEntity>>> GetListJson([FromQuery] MqttThemeListParam param)
        {
            var obj = await _mqttThemeBLL.GetList(param);
            return obj;
        }

        /// <summary>
        ///     条件查询-分页
        /// </summary>
        [HttpGet]
        public async Task<TData<List<MqttThemeEntity>>> GetPageListJson([FromQuery] MqttThemeListParam param,
            [FromQuery] Pagination pagination)
        {
            var obj = await _mqttThemeBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        ///     消息记录条件查询-分页
        /// </summary>
        [HttpGet]
        public async Task<TData<List<MqttMsgEntity>>> GetMsgPageListJson([FromQuery] MqttMsgListParam param,
            [FromQuery] Pagination pagination)
        {
            var obj = await _mqttMsgBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        ///     根据ID查询
        /// </summary>
        [HttpGet]
        public async Task<TData<MqttThemeEntity>> GetFormJson([FromQuery] long id)
        {
            var obj = await _mqttThemeBLL.GetEntity(id);
            return obj;
        }

        #endregion

        #region 提交数据

        /// <summary>
        ///     新增/修改 数据
        /// </summary>
        [HttpPost]
        [UnitOfWork]
        public async Task<TData<string>> SaveFormJson([FromForm] MqttThemeEntity entity)
        {
            var obj = await _mqttThemeBLL.SaveForm(entity);

            // 执行订阅
            if (!GlobalContext.SystemConfig.MqttIsOpen)
            {
                obj.Message = "数据操作成功！但是MQTT服务在配置文件中禁用，此处实际不生效！";
                obj.Tag = 0;
            }
            else
            {
                if (entity.IsSubscribe.GetValueOrDefault())
                    await _mqttClientCenter.Subscribe(entity.ThemeName);
                else
                    await _mqttClientCenter.Unsubscribe(entity.ThemeName);
            }

            return obj;
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        [HttpPost]
        public async Task<TData> DeleteFormJson([FromForm] string ids)
        {
            var obj = await _mqttThemeBLL.DeleteForm(ids);
            return obj;
        }

        #endregion
    }
}
