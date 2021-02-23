using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using YiSha.Util.Model;
using YiSha.Business.OrganizationManage;
using YiSha.Entity;
using YiSha.Model;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Web.Controllers;
using YiSha.WebApi.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace YiSha.Admin.Web.Areas.OrganizationManage.Controllers
{
    [Area("OrganizationManage")]
    public class DepartmentController : BaseController
    {
        private DepartmentBLL _departmentBLL;

        public DepartmentController(DepartmentBLL departmentBLL)
        {
            _departmentBLL = departmentBLL;
        }
        
        #region 视图功能
        // [AuthorizeFilter("organization:department:view")]
        public IActionResult DepartmentIndex()
        {
            return View();
        }
        public IActionResult DepartmentForm()
        {
            return View();
        }
        #endregion

        // #region 获取数据
        // [HttpGet]
        // [AuthorizeFilter("organization:department:search,organization:user:search")]
        // public async Task<IActionResult> GetListJson(DepartmentListParam param)
        // {
        //     TData<List<DepartmentEntity>> obj = await departmentBLL.GetList(param);
        //     return Json(obj);
        // }
        //
        // [HttpGet]
        // [AuthorizeFilter("organization:department:search,organization:user:search")]
        // public async Task<IActionResult> GetDepartmentTreeListJson(DepartmentListParam param)
        // {
        //     TData<List<ZtreeInfo>> obj = await departmentBLL.GetZtreeDepartmentList(param);
        //     return Json(obj);
        // }
        //
        // [HttpGet]
        // public async Task<IActionResult> GetUserTreeListJson(DepartmentListParam param)
        // {
        //     TData<List<ZtreeInfo>> obj = await departmentBLL.GetZtreeUserList(param);
        //     return Json(obj);
        // }
        //
        // [HttpGet]
        // public async Task<IActionResult> GetFormJson(long id)
        // {
        //     TData<DepartmentEntity> obj = await departmentBLL.GetEntity(id);
        //     return Json(obj);
        // }
        //
        // [HttpGet]
        // public async Task<IActionResult> GetMaxSortJson()
        // {
        //     TData<int> obj = await departmentBLL.GetMaxSort();
        //     return Json(obj);
        // }
        // #endregion
        //
        // #region 提交数据
        // [HttpPost]
        // [AuthorizeFilter("organization:department:add,organization:department:edit")]
        // public async Task<IActionResult> SaveFormJson(DepartmentEntity entity)
        // {
        //     TData<string> obj = await departmentBLL.SaveForm(entity);
        //     return Json(obj);
        // }
        //
        // [HttpPost]
        // [AuthorizeFilter("organization:department:delete")]
        // public async Task<IActionResult> DeleteFormJson(string ids)
        // {
        //     TData obj = await departmentBLL.DeleteForm(ids);
        //     return Json(obj);
        // }
        // #endregion
        //
    }
}