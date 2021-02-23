using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.IBusiness.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util.Model;

namespace YiSha.WebApi.Areas.OrganizationManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-06 14:10
    ///     描 述：职位信息控制器类
    /// </summary>
    [Route("OrganizationManage/[controller]")]
    public class PositionController : BaseController
    {
        private readonly IPositionBLL _positionBLL;

        public PositionController(IPositionBLL positionBLL)
        {
            _positionBLL = positionBLL;
        }

        #region 获取数据

        /// <summary>
        ///     条件查询
        /// </summary>
        [HttpGet]
        public async Task<TData<List<PositionEntity>>> GetListJson([FromQuery] PositionListParam param)
        {
            var obj = await _positionBLL.GetList(param);
            return obj;
        }

        /// <summary>
        ///     条件查询-分页
        /// </summary>
        [HttpGet]
        public async Task<TData<List<PositionEntity>>> GetPageListJson([FromQuery] PositionListParam param,
            [FromQuery] Pagination pagination)
        {
            var obj = await _positionBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        ///     根据ID查询
        /// </summary>
        [HttpGet]
        public async Task<TData<PositionEntity>> GetFormJson([FromQuery] long id)
        {
            var obj = await _positionBLL.GetEntity(id);
            return obj;
        }

        #endregion

        #region 提交数据

        /// <summary>
        ///     新增/修改 数据
        /// </summary>
        [HttpPost]
        public async Task<TData<string>> SaveFormJson([FromForm] PositionEntity entity)
        {
            var obj = await _positionBLL.SaveForm(entity);
            return obj;
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        [HttpPost]
        public async Task<TData> DeleteFormJson([FromForm] string ids)
        {
            var obj = await _positionBLL.DeleteForm(ids);
            return obj;
        }

        #endregion
    }
}
