using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IService.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-23 08:24
    ///     描 述：MQTT主题订阅服务接口
    /// </summary>
    public interface IMqttThemeService
    {
        #region 获取数据

        Task<List<MqttThemeEntity>> GetList(MqttThemeListParam param);

        Task<List<MqttThemeEntity>> GetPageList(MqttThemeListParam param, Pagination pagination);

        Task<MqttThemeEntity> GetEntity(long id);

        #endregion

        #region 提交数据

        Task SaveForm(MqttThemeEntity entity);

        Task DeleteForm(string ids);

        #endregion

        #region 私有方法

        #endregion
    }
}
