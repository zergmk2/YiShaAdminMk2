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
    ///     创 建：song
    ///     日 期：2020-12-04 12:55
    ///     描 述：登陆日志控制器类
    /// </summary>
    [Route("SystemManage/[controller]")]
    public class LogLoginController : BaseController
    {
        private readonly ILogLoginBLL _logLoginBLL;

        public LogLoginController(ILogLoginBLL logLoginBLL)
        {
            _logLoginBLL = logLoginBLL;
        }

        #region 获取数据

        /// <summary>
        ///     条件查询
        /// </summary>
        [HttpGet]
        public async Task<TData<List<LogLoginEntity>>> GetListJson([FromQuery] LogLoginListParam param)
        {
            var obj = await _logLoginBLL.GetList(param);
            return obj;
        }

        /// <summary>
        ///     条件查询-分页
        /// </summary>
        [HttpGet]
        public async Task<TData<List<LogLoginEntity>>> GetPageListJson([FromQuery] LogLoginListParam param,
            [FromQuery] Pagination pagination)
        {
            var obj = await _logLoginBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        ///     根据ID查询
        /// </summary>
        [HttpGet]
        public async Task<TData<LogLoginEntity>> GetFormJson([FromQuery] long id)
        {
            var obj = await _logLoginBLL.GetEntity(id);
            return obj;
        }

        #endregion

        #region 提交数据

        /// <summary>
        ///     新增/修改 数据
        /// </summary>
        [HttpPost]
        public async Task<TData<string>> SaveFormJson([FromForm] LogLoginEntity entity)
        {
            var obj = await _logLoginBLL.SaveForm(entity);
            return obj;
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        [HttpPost]
        public async Task<TData> DeleteFormJson([FromForm] string ids)
        {
            var obj = await _logLoginBLL.DeleteForm(ids);
            return obj;
        }

        #endregion
    }
}
