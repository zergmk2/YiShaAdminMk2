using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.IBusiness.SystemManage;
using YiSha.Model.Param.SystemManage;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util.Model;
using YiSha.Model.Result.SystemManage;

namespace YiSha.WebApi.Areas.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-18 16:04
    ///     描 述：数据字典控制器类
    /// </summary>
    [Route("SystemManage/[controller]")]
    public class DataDictController : BaseController
    {
        private readonly IDataDictBLL _dataDictBLL;
        private readonly IDataDictDetailBLL _dataDictDetailBLL;

        public DataDictController(IDataDictBLL dataDictBLL, IDataDictDetailBLL dataDictDetailBLL)
        {
            _dataDictBLL = dataDictBLL;
            _dataDictDetailBLL = dataDictDetailBLL;
        }

        #region 获取数据

        /// <summary>
        ///     条件查询
        /// </summary>
        [HttpGet]
        public async Task<TData<List<DataDictEntity>>> GetListJson([FromQuery] DataDictListParam param)
        {
            var obj = await _dataDictBLL.GetList(param);
            return obj;
        }

        /// <summary>
        ///     条件查询-分页
        /// </summary>
        [HttpGet]
        public async Task<TData<List<DataDictEntity>>> GetPageListJson([FromQuery] DataDictListParam param,
            [FromQuery] Pagination pagination)
        {
            var obj = await _dataDictBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        ///     根据ID查询
        /// </summary>
        [HttpGet]
        public async Task<TData<DataDictEntity>> GetFormJson([FromQuery] long id)
        {
            var obj = await _dataDictBLL.GetEntity(id);
            return obj;
        }

        [HttpGet]
        public async Task<IActionResult> GetDataDictListJson()
        {
            TData<List<DataDictInfo>> obj = await _dataDictBLL.GetDataDictList();
            return Json(obj);
        }

        /// <summary>
        ///     用于获取字典值
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<List<DataDictDetailEntity>>> GetDataDictDetailListJson(
            [FromQuery] DataDictDetailListParam param, [FromQuery] Pagination pagination)
        {
            var obj = await _dataDictDetailBLL.GetPageList(param, pagination);
            return obj;
        }

        #endregion

        #region 提交数据

        /// <summary>
        ///     新增/修改 数据
        /// </summary>
        [HttpPost]
        public async Task<TData<string>> SaveFormJson([FromForm] DataDictEntity entity)
        {
            var obj = await _dataDictBLL.SaveForm(entity);
            return obj;
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        [HttpPost]
        public async Task<TData> DeleteFormJson([FromForm] string ids)
        {
            var obj = await _dataDictBLL.DeleteForm(ids);
            return obj;
        }

        #endregion
    }
}
