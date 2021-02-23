using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.IBusiness.SystemManage
{
    public interface IMqttMsgBLL
    {
        #region 获取数据

        Task<TData<List<MqttMsgEntity>>> GetList(MqttMsgListParam param);

        Task<TData<List<MqttMsgEntity>>> GetPageList(MqttMsgListParam param, Pagination pagination);

        Task<TData<MqttMsgEntity>> GetEntity(long id);

        #endregion

        #region 提交数据

        Task<TData<string>> SaveForm(MqttMsgEntity entity);

        Task<TData> DeleteForm(string ids);

        #endregion
    }
}
