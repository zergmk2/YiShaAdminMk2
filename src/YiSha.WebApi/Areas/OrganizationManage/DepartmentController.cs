using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.IBusiness.OrganizationManage;
using YiSha.Model;
using YiSha.Model.Param.OrganizationManage;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util.Model;

namespace YiSha.WebApi.Areas.OrganizationManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-06 14:12
    ///     描 述：部门信息控制器类
    /// </summary>
    [Route("OrganizationManage/[controller]")]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentBLL _departmentBLL;
        private readonly IUserBLL _userBLL;

        public DepartmentController(IDepartmentBLL departmentBLL, IUserBLL userBLL)
        {
            _departmentBLL = departmentBLL;
            _userBLL = userBLL;
        }

        #region 获取数据

        /// <summary>
        ///     条件查询
        /// </summary>
        [HttpGet]
        public async Task<TData<List<DepartmentEntity>>> GetListJson([FromQuery] DepartmentListParam param)
        {
            var obj = await _departmentBLL.GetList(param);
            return obj;
        }

        /// <summary>
        ///     条件查询-分页
        /// </summary>
        [HttpGet]
        public async Task<TData<List<DepartmentEntity>>> GetPageListJson([FromQuery] DepartmentListParam param,
            [FromQuery] Pagination pagination)
        {
            var obj = await _departmentBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        ///     根据ID查询
        /// </summary>
        [HttpGet]
        public async Task<TData<DepartmentEntity>> GetFormJson([FromQuery] long id)
        {
            var obj = await _departmentBLL.GetEntity(id);
            return obj;
        }

        /// <summary>
        ///     用户下拉框用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<List<UserEntity>>> GetUserListJson()
        {
            var obj = await _userBLL.GetList(null);
            return obj;
        }

        [HttpGet]
        public async Task<TData<List<ZtreeInfo>>> GetDepartmentTreeListJson([FromQuery] DepartmentListParam param)
        {
            TData<List<ZtreeInfo>> obj = await _departmentBLL.GetZtreeDepartmentList(param);
            return obj;
        }

        #endregion

        #region 提交数据

        /// <summary>
        ///     新增/修改 数据
        /// </summary>
        [HttpPost]
        public async Task<TData<string>> SaveFormJson([FromForm] DepartmentEntity entity)
        {
            var obj = await _departmentBLL.SaveForm(entity);
            return obj;
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        [HttpPost]
        public async Task<TData> DeleteFormJson([FromForm] string ids)
        {
            var obj = await _departmentBLL.DeleteForm(ids);
            return obj;
        }

        #endregion
    }
}
