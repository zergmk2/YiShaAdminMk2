using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IService.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-23 08:40
    ///     描 述：MQTT消息记录服务接口
    /// </summary>
    public interface IMqttMsgService
    {
        #region 获取数据

        Task<List<MqttMsgEntity>> GetList(MqttMsgListParam param);

        Task<List<MqttMsgEntity>> GetPageList(MqttMsgListParam param, Pagination pagination);

        Task<MqttMsgEntity> GetEntity(long id);

        #endregion

        #region 提交数据

        Task SaveForm(MqttMsgEntity entity);

        Task DeleteForm(string ids);

        #endregion

        #region 私有方法

        #endregion
    }
}
