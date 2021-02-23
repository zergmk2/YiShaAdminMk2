using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IBusiness.SystemManage
{
    public interface IMqttThemeBLL
    {
        #region 获取数据

        Task<TData<List<MqttThemeEntity>>> GetList(MqttThemeListParam param);

        Task<TData<List<MqttThemeEntity>>> GetPageList(MqttThemeListParam param, Pagination pagination);

        Task<TData<MqttThemeEntity>> GetEntity(long id);

        #endregion

        #region 提交数据

        Task<TData<string>> SaveForm(MqttThemeEntity entity);

        Task<TData> DeleteForm(string ids);

        #endregion
    }
}
