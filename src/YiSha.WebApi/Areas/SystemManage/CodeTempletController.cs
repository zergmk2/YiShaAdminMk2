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
    ///     日 期：2020-12-04 12:28
    ///     描 述：代码模板控制器类
    /// </summary>
    [Route("SystemManage/[controller]")]
    public class CodeTempletController : BaseController
    {
        private readonly ICodeTempletBLL _codeTempletBLL;

        public CodeTempletController(ICodeTempletBLL codeTempletBLL)
        {
            _codeTempletBLL = codeTempletBLL;
        }

        #region 获取数据

        /// <summary>
        ///     条件查询
        /// </summary>
        [HttpGet]
        public async Task<TData<List<CodeTempletEntity>>> GetListJson([FromQuery] CodeTempletListParam param)
        {
            var obj = await _codeTempletBLL.GetList(param);
            return obj;
        }

        /// <summary>
        ///     条件查询-分页
        /// </summary>
        [HttpGet]
        public async Task<TData<List<CodeTempletEntity>>> GetPageListJson([FromQuery] CodeTempletListParam param,
            [FromQuery] Pagination pagination)
        {
            var obj = await _codeTempletBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        ///     根据ID查询
        /// </summary>
        [HttpGet]
        public async Task<TData<CodeTempletEntity>> GetFormJson([FromQuery] long id)
        {
            var obj = await _codeTempletBLL.GetEntity(id);
            return obj;
        }

        #endregion

        #region 提交数据

        /// <summary>
        ///     新增/修改 数据
        /// </summary>
        [HttpPost]
        public async Task<TData<string>> SaveFormJson([FromForm] CodeTempletEntity entity)
        {
            var obj = await _codeTempletBLL.SaveForm(entity);
            return obj;
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        [HttpPost]
        public async Task<TData> DeleteFormJson([FromForm] string ids)
        {
            var obj = await _codeTempletBLL.DeleteForm(ids);
            return obj;
        }

        #endregion
    }
}
