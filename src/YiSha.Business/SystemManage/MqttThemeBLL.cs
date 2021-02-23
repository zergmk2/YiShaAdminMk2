using System.Collections.Generic;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using YiSha.Entity;
using YiSha.IBusiness.SystemManage;
using YiSha.IService.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-23 08:24
    ///     描 述：MQTT主题订阅业务类
    /// </summary>
    public class MqttThemeBLL : IMqttThemeBLL, ITransient
    {
        private readonly IMqttThemeService _mqttThemeService;

        public MqttThemeBLL(IMqttThemeService mqttThemeService)
        {
            _mqttThemeService = mqttThemeService;
        }

        #region 获取数据

        public async Task<TData<List<MqttThemeEntity>>> GetList(MqttThemeListParam param)
        {
            var obj = new TData<List<MqttThemeEntity>>();
            obj.Data = await _mqttThemeService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<MqttThemeEntity>>> GetPageList(MqttThemeListParam param, Pagination pagination)
        {
            var obj = new TData<List<MqttThemeEntity>>();
            obj.Data = await _mqttThemeService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<MqttThemeEntity>> GetEntity(long id)
        {
            var obj = new TData<MqttThemeEntity>();
            obj.Data = await _mqttThemeService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(MqttThemeEntity entity)
        {
            var obj = new TData<string>();
            await _mqttThemeService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            var obj = new TData();
            await _mqttThemeService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}
