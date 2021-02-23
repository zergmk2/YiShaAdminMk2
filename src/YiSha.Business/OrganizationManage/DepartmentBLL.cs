using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using YiSha.Cache;
using YiSha.Entity;
using YiSha.IBusiness.OrganizationManage;
using YiSha.IService.OrganizationManage;
using YiSha.Model;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Business.OrganizationManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-06 14:12
    ///     描 述：部门信息业务类
    /// </summary>
    public class DepartmentBLL : IDepartmentBLL, ITransient
    {
        private readonly IDepartmentService _departmentService;
        private readonly OperatorCache _operatorCache;
        private readonly IUserService _userService;

        public DepartmentBLL(IDepartmentService departmentService, OperatorCache operatorCache,
            IUserService userService)
        {
            _departmentService = departmentService;
            _operatorCache = operatorCache;
            _userService = userService;
        }

        #region 公共方法

        /// <summary>
        ///     获取当前部门及下面所有的部门
        /// </summary>
        /// <param name="departmentList"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<List<long>> GetChildrenDepartmentIdList(List<DepartmentEntity> departmentList,
            long departmentId)
        {
            if (departmentList == null) departmentList = await _departmentService.GetList(null);
            var departmentIdList = new List<long>();
            departmentIdList.Add(departmentId);
            GetChildrenDepartmentIdList(departmentList, departmentId, departmentIdList);
            return departmentIdList;
        }

        #endregion

        #region 获取数据

        public async Task<TData<List<DepartmentEntity>>> GetList(DepartmentListParam param)
        {
            var obj = new TData<List<DepartmentEntity>>();
            obj.Data = await _departmentService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<DepartmentEntity>>> GetPageList(DepartmentListParam param, Pagination pagination)
        {
            var obj = new TData<List<DepartmentEntity>>();
            obj.Data = await _departmentService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<DepartmentEntity>> GetEntity(long id)
        {
            var obj = new TData<DepartmentEntity>();
            obj.Data = await _departmentService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }


        public async Task<TData<List<ZtreeInfo>>> GetZtreeUserList(DepartmentListParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Data = new List<ZtreeInfo>();
            var departmentList = await _departmentService.GetList(param);
            var operatorInfo = await _operatorCache.Current();
            if (operatorInfo.IsSystem != 1)
            {
                var childrenDepartmentIdList =
                    await GetChildrenDepartmentIdList(departmentList, operatorInfo.DepartmentId.Value);
                departmentList = departmentList.Where(p => childrenDepartmentIdList.Contains(p.Id.Value)).ToList();
            }

            var userList = await _userService.GetList(null);
            foreach (var department in departmentList)
            {
                obj.Data.Add(new ZtreeInfo
                {
                    id = department.Id,
                    pId = department.ParentId,
                    name = department.DepartmentName
                });
                var userIdList = userList.Where(t => t.DepartmentId == department.Id).Select(t => t.Id.Value).ToList();
                foreach (var user in userList.Where(t => userIdList.Contains(t.Id.Value)))
                    obj.Data.Add(new ZtreeInfo
                    {
                        id = user.Id,
                        pId = department.Id,
                        name = user.RealName
                    });
            }

            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeDepartmentList(DepartmentListParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Data = new List<ZtreeInfo>();
            List<DepartmentEntity> departmentList = await _departmentService.GetList(param);
            OperatorInfo operatorInfo = await _operatorCache.Current();
            if (operatorInfo.IsSystem != 1)
            {
                List<long> childrenDepartmentIdList = await GetChildrenDepartmentIdList(departmentList, operatorInfo.DepartmentId.Value);
                departmentList = departmentList.Where(p => childrenDepartmentIdList.Contains(p.Id.Value)).ToList();
            }
            foreach (DepartmentEntity department in departmentList)
            {
                obj.Data.Add(new ZtreeInfo
                {
                    id = department.Id,
                    pId = department.ParentId,
                    name = department.DepartmentName
                });
            }
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(DepartmentEntity entity)
        {
            var obj = new TData<string>();
            await _departmentService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            var obj = new TData();
            await _departmentService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 私有方法

        /// <summary>
        ///     获取该部门下面所有的子部门
        /// </summary>
        /// <param name="departmentList"></param>
        /// <param name="departmentId"></param>
        /// <param name="departmentIdList"></param>
        private void GetChildrenDepartmentIdList(List<DepartmentEntity> departmentList, long departmentId,
            List<long> departmentIdList)
        {
            var children = departmentList.Where(p => p.ParentId == departmentId).Select(p => p.Id.Value).ToList();
            if (children.Count > 0)
            {
                departmentIdList.AddRange(children);
                foreach (var id in children) GetChildrenDepartmentIdList(departmentList, id, departmentIdList);
            }
        }

        private void GetDepartmentIdList(List<DepartmentEntity> departmentList, long departmentId,
            List<long> departmentIdList)
        {
            var children = departmentList.Where(p => p.ParentId == departmentId).Select(p => p.Id.Value).ToList();
            if (children.Count > 0)
            {
                departmentIdList.AddRange(children);
                foreach (var id in children) GetDepartmentIdList(departmentList, id, departmentIdList);
            }
        }

        #endregion
    }
}
