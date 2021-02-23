using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.LinqBuilder;
using YiSha.Entity;
using YiSha.Enum.OrganizationManage;
using Microsoft.EntityFrameworkCore;
using YiSha.IService.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Service.OrganizationManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 12:41
    ///     描 述：用户信息服务类
    /// </summary>
    public class UserService : IUserService, ITransient
    {
        private readonly IRepository<DepartmentEntity> _departmentEntityDB;
        private readonly IRepository<UserBelongEntity> _userBelongEntityDB;
        private readonly IRepository<UserEntity> _userEntityDB;

        public UserService(IRepository<UserEntity> userEntityDB, IRepository<DepartmentEntity> departmentEntityDB,
            IRepository<UserBelongEntity> userBelongEntityDB)
        {
            _userEntityDB = userEntityDB;
            _departmentEntityDB = departmentEntityDB;
            _userBelongEntityDB = userBelongEntityDB;
        }

        #region 获取数据

        /// <summary>
        ///     带条件查询所有
        /// </summary>
        public async Task<List<UserEntity>> GetList(UserListParam param)
        {
            #region 查询条件

            var query = _userEntityDB.AsQueryable();
            /*
            // 用户名
            if (!string.IsNullOrEmpty(param.UserName))
                query = query.Where(p => p.UserName.Contains(param.UserName));

            */

            #endregion

            var data = await query.ToListAsync();
            return data;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public async Task<List<UserEntity>> GetPageList(UserListParam param, Pagination pagination)
        {
            #region 查询条件

            var query = _userEntityDB.AsQueryable();

            // 用户名
            if (!string.IsNullOrEmpty(param.UserName))
                query = query.Where(p => p.UserName.Contains(param.UserName));

            var q = from a in query
                join b in _departmentEntityDB.AsQueryable() on a.DepartmentId equals b.Id into ab
                from b in ab.DefaultIfEmpty()
                select new UserEntity
                {
                    Id = a.Id,
                    Birthday = a.Birthday,
                    CreateTime = a.CreateTime,
                    DepartmentId = a.DepartmentId,
                    Email = a.Email,
                    Gender = a.Gender,
                    IsSystem = a.IsSystem,
                    Mobile = a.Mobile,
                    Remark = a.Remark,
                    UserName = a.UserName,
                    UserStatus = a.UserStatus,
                    RealName = a.RealName,
                    DepartmentName = b == null ? string.Empty : b.DepartmentName
                };

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
        public async Task<UserEntity> GetEntity(long id)
        {
            var list = await _userEntityDB.AsAsyncEnumerable(p => p.Id == id);
            return list.FirstOrDefault();
        }

        /// <summary>
        ///     查询多个ID主键数据
        /// </summary>
        public async Task<List<UserEntity>> GetListByIds(string ids)
        {
            if (ids.IsNullOrEmpty())
                throw new Exception("参数不合法！");

            var idArr = TextHelper.SplitToArray<long>(ids, ',').ToList();
            var data = await _userEntityDB.AsAsyncEnumerable(a => idArr.Contains(a.Id.GetValueOrDefault()));

            return data;
        }

        /// <summary>
        ///     根据token获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<OperatorInfo> GetUserByToken(string token)
        {
            if (!SecurityHelper.IsSafeSqlParam(token))
                return null;

            token = token.ParseToString().Trim();

            var operatorInfo = await _userEntityDB
                .AsQueryable()
                .Where(a => a.ApiToken == token)
                .Select(a => new OperatorInfo
                {
                    UserId = a.Id,
                    UserStatus = a.UserStatus,
                    IsOnline = a.IsOnline,
                    UserName = a.UserName,
                    RealName = a.RealName,
                    Portrait = a.Portrait,
                    DepartmentId = a.DepartmentId,
                    ApiToken = a.ApiToken,
                    IsSystem = a.IsSystem
                }).FirstOrDefaultAsync();

            if (operatorInfo != null)
            {
                // 角色

                var roleIds = await _userBelongEntityDB
                    .AsQueryable()
                    .Where(a => a.UserId == operatorInfo.UserId && a.BelongType == UserBelongTypeEnum.Role.ParseToInt())
                    .Select(a => a.BelongId.ParseToString())
                    .ToListAsync();

                operatorInfo.RoleIds = string.Join(",", roleIds);

                // 部门名称
                operatorInfo.DepartmentName = await _departmentEntityDB
                    .AsQueryable()
                    .Where(a => a.Id == operatorInfo.DepartmentId)
                    .Select(a => a.DepartmentName)
                    .FirstOrDefaultAsync();
            }

            return operatorInfo;
        }


        public async Task<UserEntity> CheckLogin(string userName)
        {
            var user = await _userEntityDB.Where(t => t.UserName == userName).SingleOrDefaultAsync();
            return user;
        }

        /// <summary>
        ///     校验用户是否为管理员
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckUserIsSystemManage(long id)
        {
            var i = await _userEntityDB.AsQueryable().Where(a => a.IsSystem == 1 && a.Id == id).CountAsync();
            if (i > 0)
                return true;

            return false;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(UserEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                entity.Id = IdGeneratorHelper.Instance.GetId();
                entity.CreateTime = DateTime.Now;
                entity.CreatorId = NetHelper.HttpContext.User.FindFirstValue("UserId").ParseToLong();
                entity.IsDelete = 0;
                await _userEntityDB.InsertNowAsync(entity);
            }
            else
            {
                if (await CheckUserIsSystemManage(entity.Id.Value))
                    throw new Exception("超管账号不可修改");

                await _userEntityDB.UpdateNowAsync(entity, ignoreNullValues: true);
            }
        }

        public async Task DeleteForm(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                throw new Exception("参数不合法！");

            var _ids = ids.Split(",");

            foreach (var id in _ids)
                if (await CheckUserIsSystemManage(id.ParseToLong()))
                    throw new Exception("超管账号不删除");

            await _userEntityDB.BatchDeleteAsync(_ids);
        }

        public async Task UpdateLoginInfo(UserEntity entity)
        {
            entity.LoginCount = entity.LoginCount.Value + 1;
            entity.LastVisit = DateTime.Now;
            entity.PreviousVisit = DateTime.Now;

            await _userEntityDB.UpdateNowAsync(entity, ignoreNullValues: true);
        }

        #endregion

        #region 私有方法

        #endregion
    }
}
