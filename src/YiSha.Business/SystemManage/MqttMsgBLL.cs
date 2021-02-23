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
    ///     日 期：2020-12-23 08:40
    ///     描 述：MQTT消息记录业务类
    /// </summary>
    public class MqttMsgBLL : IMqttMsgBLL, ITransient
    {
        private readonly IMqttMsgService _mqttMsgService;

        public MqttMsgBLL(IMqttMsgService mqttMsgService)
        {
            _mqttMsgService = mqttMsgService;
        }

        #region 获取数据

        public async Task<TData<List<MqttMsgEntity>>> GetList(MqttMsgListParam param)
        {
            var obj = new TData<List<MqttMsgEntity>>();
            obj.Data = await _mqttMsgService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<MqttMsgEntity>>> GetPageList(MqttMsgListParam param, Pagination pagination)
        {
            var obj = new TData<List<MqttMsgEntity>>();
            obj.Data = await _mqttMsgService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<MqttMsgEntity>> GetEntity(long id)
        {
            var obj = new TData<MqttMsgEntity>();
            obj.Data = await _mqttMsgService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(MqttMsgEntity entity)
        {
            var obj = new TData<string>();
            await _mqttMsgService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            var obj = new TData();
            await _mqttMsgService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}
