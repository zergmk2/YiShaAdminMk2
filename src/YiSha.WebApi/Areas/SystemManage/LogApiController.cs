using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.IBusiness.SystemManage;
using YiSha.Model.Param.SystemManage;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util.Model;

namespace YiSha.WebApi.Areas.SystemManage
{
    /// <summary>
    ///     创 建：
    ///     日 期：2020-12-04 12:49
    ///     描 述：Api日志控制器类
    /// </summary>
    [Route("SystemManage/LogApi")]
    public class LogApiController : BaseController
    {
        private readonly ILogApiBLL _logApiBLL;

        public LogApiController(ILogApiBLL logApiBLL)
        {
            _logApiBLL = logApiBLL;
        }

        #region 获取数据

        /// <summary>
        ///     条件查询
        /// </summary>
        [HttpGet]
        public async Task<TData<List<LogApiEntity>>> GetListJson([FromQuery] LogApiListParam param)
        {
            var obj = await _logApiBLL.GetList(param);
            return obj;
        }

        /// <summary>
        ///     条件查询-分页
        /// </summary>
        [HttpGet]
        public async Task<TData<List<LogApiEntity>>> GetPageListJson([FromQuery] LogApiListParam param,
            [FromQuery] Pagination pagination)
        {
            var obj = await _logApiBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        ///     根据ID查询
        /// </summary>
        [HttpGet]
        public async Task<TData<LogApiEntity>> GetFormJson([FromQuery] long id)
        {
            var obj = await _logApiBLL.GetEntity(id);
            return obj;
        }

        #endregion

        #region 提交数据

        /// <summary>
        ///     新增/修改 数据
        /// </summary>
        [HttpPost]
        public async Task<TData<string>> SaveFormJson([FromForm] LogApiEntity entity)
        {
            var obj = await _logApiBLL.SaveForm(entity);
            return obj;
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        [HttpPost]
        public async Task<TData> DeleteFormJson([FromForm] string ids)
        {
            var obj = await _logApiBLL.DeleteForm(ids);
            return obj;
        }

        #endregion
    }
}
