using System;
using System.Collections.Generic;
using System.Linq;
using Furion.DependencyInjection;
using YiSha.Cache;
using YiSha.Model.Param.SystemManage;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util;
using YiSha.Util.Model;
using StackExchange.Profiling.Internal;

namespace YiSha.WebApi.Areas.SystemManage
{
    [Route("SystemManage/[controller]")]
    public class CacheController : BaseController
    {
        private readonly ICache _cache;

        public CacheController(Func<string, ISingleton, object> resolveNamed)
        {
            _cache = resolveNamed(GlobalContext.SystemConfig.CacheService, default) as ICache;
        }

        #region 获取数据

        /// <summary>
        ///     条件查询-分页
        /// </summary>
        [HttpGet]
        public TData<IEnumerable<object>> GetPageListJson([FromQuery] CacheListParam param,
            [FromQuery] Pagination pagination)
        {
            var obj = new TData<IEnumerable<object>>();
            var list = _cache.GetAllKey();

            if (param != null)
                if (!param.Key.IsEmpty())
                    list = list.Where(a => a.Contains(param.Key)).ToList();

            obj.Total = list.Count();
            obj.Tag = 1;
            obj.Data = list.Skip(pagination.PageSize * (pagination.PageIndex - 1)).Take(pagination.PageSize)
                .Select(a => new {Key = a});

            return obj;
        }

        /// <summary>
        ///     获取指定键的值
        /// </summary>
        [HttpGet]
        public TData<string> GetDetail([FromQuery] CacheListParam param)
        {
            var obj = new TData<string>();
            obj.Tag = 1;

            if (param == null || param.Key.IsEmpty())
                return obj;

            obj.Data = _cache.Get<object>(param.Key).ToJson();
            return obj;
        }

        #endregion

        #region 提交数据

        /// <summary>
        ///     删除数据
        /// </summary>
        [HttpPost]
        public TData DeleteFormJson([FromForm] List<string> Keys)
        {
            var obj = new TData();

            if (Keys != null && Keys.Count() > 0)
                foreach (var key in Keys)
                    _cache.Remove(key);

            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        ///     新增/修改数据
        /// </summary>
        [HttpPost]
        public TData SaveFormJson([FromForm] string Key, [FromForm] string Value, [FromForm] DateTime? Time)
        {
            var obj = new TData();

            if (Key.IsEmpty() || Value.IsEmpty())
                throw new Exception("参数不合法！");

            if (Time.HasValue)
                _cache.Set(Key, Value, Time);
            else
                _cache.Set(Key, Value);

            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}
