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
    ///     日 期：2020-12-23 08:24
    ///     描 述：MQTT主题订阅服务类
    /// </summary>
    public class MqttThemeService : IMqttThemeService, ITransient
    {
        private readonly IRepository<MqttThemeEntity> _mqttThemeEntityDB;

        public MqttThemeService(IRepository<MqttThemeEntity> mqttThemeEntityDB)
        {
            _mqttThemeEntityDB = mqttThemeEntityDB;
        }

        #region 获取数据

        /// <summary>
        ///     带条件查询所有
        /// </summary>
        public async Task<List<MqttThemeEntity>> GetList(MqttThemeListParam param)
        {
            #region 查询条件

            var query = _mqttThemeEntityDB.AsQueryable();

            // 订阅主题名称
            if (!string.IsNullOrEmpty(param.ThemeName))
                query = query.Where(p => p.ThemeName.Contains(param.ThemeName));
            if (param.IsSubscribe.HasValue)
                query = query.Where(p => p.IsSubscribe == param.IsSubscribe);

            #endregion

            var data = await query.ToListAsync();
            return data;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public async Task<List<MqttThemeEntity>> GetPageList(MqttThemeListParam param, Pagination pagination)
        {
            #region 查询条件

            var query = _mqttThemeEntityDB.AsQueryable();

            // 订阅主题名称
            if (!string.IsNullOrEmpty(param.ThemeName))
                query = query.Where(p => p.ThemeName.Contains(param.ThemeName));
            if (param.IsSubscribe.HasValue)
                query = query.Where(p => p.IsSubscribe == param.IsSubscribe);

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
        public async Task<MqttThemeEntity> GetEntity(long id)
        {
            var list = await _mqttThemeEntityDB.AsAsyncEnumerable(p => p.Id == id);
            return list.FirstOrDefault();
        }

        /// <summary>
        ///     查询多个ID主键数据
        /// </summary>
        public async Task<List<MqttThemeEntity>> GetListByIds(string ids)
        {
            if (ids.IsNullOrEmpty())
                throw new Exception("参数不合法！");

            var idArr = TextHelper.SplitToArray<long>(ids, ',').ToList();
            var data = await _mqttThemeEntityDB.AsAsyncEnumerable(a => idArr.Contains(a.Id.GetValueOrDefault()));

            return data;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(MqttThemeEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                // 默认赋值
                entity.Id = IdGeneratorHelper.Instance.GetId();
                await _mqttThemeEntityDB.InsertNowAsync(entity);
            }
            else
            {
                await _mqttThemeEntityDB.UpdateNowAsync(entity, ignoreNullValues: true);
            }
        }

        public async Task DeleteForm(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                throw new Exception("参数不合法！");

            var _ids = ids.Split(",");
            await _mqttThemeEntityDB.BatchDeleteAsync(_ids);
        }

        #endregion

        #region 私有方法

        #endregion
    }
}
