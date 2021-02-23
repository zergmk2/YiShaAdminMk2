using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.LinqBuilder;
using YiSha.Entity;
using Microsoft.EntityFrameworkCore;
using YiSha.IService.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Service.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-23 08:40
    ///     描 述：MQTT消息记录服务类
    /// </summary>
    public class MqttMsgService : IMqttMsgService, ITransient
    {
        private readonly IRepository<MqttMsgEntity> _mqttMsgEntityDB;

        public MqttMsgService(IRepository<MqttMsgEntity> mqttMsgEntityDB)
        {
            _mqttMsgEntityDB = mqttMsgEntityDB;
        }

        #region 获取数据

        /// <summary>
        ///     带条件查询所有
        /// </summary>
        public async Task<List<MqttMsgEntity>> GetList(MqttMsgListParam param)
        {
            #region 查询条件

            var query = _mqttMsgEntityDB.AsQueryable();
            /*
                  //
                  if (param.ThemeId.HasValue)
                      query = query.Where(p => p.ThemeId == param.ThemeId);

            */

            #endregion

            var data = await query.ToListAsync();
            return data;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public async Task<List<MqttMsgEntity>> GetPageList(MqttMsgListParam param, Pagination pagination)
        {
            #region 查询条件

            var query = _mqttMsgEntityDB.AsQueryable();

            //
            if (!param.ThemeName.IsEmpty())
                query = query.Where(p => p.ThemeName == param.ThemeName);

            var data = await query.OrderByDescending(a => a.Id)
                .ToPagedListAsync(pagination.PageIndex, pagination.PageSize);

            #endregion

            // 分页参数赋值
            pagination.TotalCount = data.TotalCount;
            return data.Items.ToList();
        }

        /// <summary>
        ///     根据ID获取对象
        /// </summary>
        public async Task<MqttMsgEntity> GetEntity(long id)
        {
            var list = await _mqttMsgEntityDB.AsAsyncEnumerable(p => p.Id == id);
            return list.FirstOrDefault();
        }

        /// <summary>
        ///     查询多个ID主键数据
        /// </summary>
        public async Task<List<MqttMsgEntity>> GetListByIds(string ids)
        {
            if (ids.IsNullOrEmpty())
                throw new Exception("参数不合法！");

            var idArr = TextHelper.SplitToArray<long>(ids, ',').ToList();
            var data = await _mqttMsgEntityDB.AsAsyncEnumerable(a => idArr.Contains(a.Id.GetValueOrDefault()));

            return data;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(MqttMsgEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                // 默认赋值
                entity.Id = IdGeneratorHelper.Instance.GetId();
                await _mqttMsgEntityDB.InsertNowAsync(entity);
            }
            else
            {
                await _mqttMsgEntityDB.UpdateNowAsync(entity, ignoreNullValues: true);
            }
        }

        public async Task DeleteForm(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                throw new Exception("参数不合法！");

            var _ids = ids.Split(",");
            await _mqttMsgEntityDB.BatchDeleteAsync(_ids);
        }

        #endregion

        #region 私有方法

        #endregion
    }
}
