using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using YiSha.Cache;
using YiSha.Entity;
using YiSha.Enum;
using YiSha.Enum.OrganizationManage;
using YiSha.IBusiness.OrganizationManage;
using YiSha.IService.OrganizationManage;
using YiSha.IService.SystemManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Business.OrganizationManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 12:41
    ///     描 述：用户信息业务类
    /// </summary>
    public class UserBLL : IUserBLL, ITransient
    {
        private readonly IDepartmentService _departmentService;
        private readonly OperatorCache _operatorCache;
        private readonly IPositionService _positionService;
        private readonly IRoleService _roleService;
        private readonly IUserBelongService _userBelongService;
        private readonly IUserService _userService;


        private readonly List<DepartmentEntity> Main = new();

        public UserBLL(IUserService userService, IUserBelongService userBelongService, IRoleService roleService,
            IPositionService positionService,
            IDepartmentService departmentService, OperatorCache operatorCache)
        {
            _userService = userService;
            _userBelongService = userBelongService;
            _roleService = roleService;
            _positionService = positionService;
            _departmentService = departmentService;
            _operatorCache = operatorCache;
        }

        #region 获取数据

        public async Task<TData<List<UserEntity>>> GetList(UserListParam param)
        {
            var obj = new TData<List<UserEntity>>();
            obj.Data = await _userService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<UserEntity>>> GetPageList(UserListParam param, Pagination pagination)
        {
            var obj = new TData<List<UserEntity>>();
            obj.Data = await _userService.GetPageList(param, pagination);

            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<UserEntity>> GetEntity(long id)
        {
            var obj = new TData<UserEntity>();
            obj.Data = await _userService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        ///     登陆校验
        /// </summary>
        public async Task<TData<UserEntity>> CheckLogin(string userName, string password)
        {
            var obj = new TData<UserEntity>();
            if (userName.IsEmpty() || password.IsEmpty())
            {
                obj.Message = "用户名或密码不能为空";
                return obj;
            }

            var user = await _userService.CheckLogin(userName);
            if (user != null)
            {
                if (user.UserStatus == (int) StatusEnum.Yes)
                {
                    if (user.Password == EncryptUserPassword(password, user.Salt))
                    {
                        user.LoginCount++;
                        user.IsOnline = 1;

                        #region 设置日期

                        if (user.FirstVisit == GlobalConstant.DefaultTime)
                            user.FirstVisit = DateTime.Now;

                        if (user.PreviousVisit == GlobalConstant.DefaultTime)
                            user.PreviousVisit = DateTime.Now;

                        if (user.LastVisit != GlobalConstant.DefaultTime)
                            user.PreviousVisit = user.LastVisit;

                        user.LastVisit = DateTime.Now;

                        #endregion

                        user.ApiToken = SecurityHelper.GetGuid();

                        await GetUserBelong(user);

                        obj.Data = user;
                        obj.Message = "登录成功";
                        obj.Tag = 1;
                    }
                    else
                    {
                        obj.Message = "密码不正确，请重新输入";
                    }
                }
                else
                {
                    obj.Message = "账号被禁用，请联系管理员";
                }
            }
            else
            {
                obj.Message = "账号不存在，请重新输入";
            }

            return obj;
        }

        public async Task<TData<object>> UserPageLoad()
        {
            var data = new TData<object>();
            var roles = await _roleService.GetList(null);
            var positions = await _positionService.GetList(null);

            #region 部门信息

            var departmentList = await _departmentService.GetList(null);
            var operatorInfo = await _operatorCache.Current();
            var childrenDepartmentIdList =
                await GetChildrenDepartmentIdList(departmentList, operatorInfo.DepartmentId.Value);

            departmentList = departmentList.Where(p => childrenDepartmentIdList.Contains(p.Id.Value)).ToList();

            Main.Add(departmentList.Where(x => x.ParentId.GetValueOrDefault() == 0).FirstOrDefault()); //根节点
            AddDepartment(departmentList, Main.FirstOrDefault()); //递归

            //结果树形结构
            var treeMenu = Main;

            #endregion

            data.Tag = 1;
            data.Data = new {roles, positions, ztreeInfo = treeMenu};
            return data;
        }


        /// <summary>
        ///     获取用户的职位和角色
        /// </summary>
        /// <param name="user"></param>
        public async Task GetUserBelong(UserEntity user)
        {
            var userBelongList = await _userBelongService.GetList(new UserBelongListParam {UserId = user.Id});

            var roleBelongList = userBelongList.Where(p => p.BelongType == UserBelongTypeEnum.Role.ParseToInt())
                .ToList();
            if (roleBelongList.Count > 0)
                user.RoleIds = string.Join(",", roleBelongList.Select(p => p.BelongId).ToList());

            var positionBelongList = userBelongList.Where(p => p.BelongType == UserBelongTypeEnum.Position.ParseToInt())
                .ToList();
            if (positionBelongList.Count > 0)
                user.PositionIds = string.Join(",", positionBelongList.Select(p => p.BelongId).ToList());
        }

        #endregion

        #region 提交数据

        /// <summary>
        ///     保存用户信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<TData<string>> SaveForm(UserEntity entity)
        {
            var obj = new TData<string>();

            if (entity.IsSystem == 1)
                throw new Exception("禁止操作！");

            entity.Password = GlobalContext.SystemConfig.DefaultUserPWD;
            entity.UserStatus = 1;
            entity.Salt = GetPasswordSalt();
            entity.Password = EncryptUserPassword(entity.Password, entity.Salt);

            // 保存用户信息
            await _userService.SaveForm(entity);

            // 保存角色和职位信息
            await _userBelongService.DeleteByUserId(entity.Id.Value);
            if (!entity.RoleIds.IsEmpty())
                await _userBelongService.SaveUserRoles(entity.Id.Value,
                    entity.RoleIds.Split(",").Select(a => a.ParseToLong()).ToList());
            if (!entity.PositionIds.IsEmpty())
                await _userBelongService.SaveUserPositions(entity.Id.Value,
                    entity.PositionIds.Split(",").Select(a => a.ParseToLong()).ToList());

            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            var obj = new TData();
            await _userService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        public async Task UpdateLoginInfo(UserEntity entity)
        {
            await _userService.UpdateLoginInfo(entity);
        }

        #endregion

        #region 私有方法

        /// <summary>
        ///     密码MD5处理
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private string EncryptUserPassword(string password, string salt)
        {
            var md5Password = SecurityHelper.MD5Encrypt(password);
            var encryptPassword = SecurityHelper.MD5Encrypt(md5Password + salt);
            return encryptPassword;
        }

        /// <summary>
        ///     密码盐
        /// </summary>
        /// <returns></returns>
        private string GetPasswordSalt()
        {
            return new Random().Next(1, 100000).ToString();
        }

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

        // 递归
        public void AddDepartment(List<DepartmentEntity> all, DepartmentEntity curItem)
        {
            if (curItem == null || !curItem.Id.HasValue) return;
            var childItems = all.Where(ee => ee.ParentId == curItem.Id).ToList(); //得到子节点
            curItem.children = childItems; //将子节点加入

            //遍历子节点，进行递归，寻找子节点的子节点
            foreach (var subItem in childItems) AddDepartment(all, subItem);
        }

        #endregion
    }
}
