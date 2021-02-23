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
    ///     日 期：2020-12-04 13:08
    ///     描 述：用户关联信息服务类
    /// </summary>
    public class UserBelongService : IUserBelongService, ITransient
    {
        private readonly IRepository<UserBelongEntity> _userBelongEntityDB;

        public UserBelongService(IRepository<UserBelongEntity> userBelongEntityDB)
        {
            _userBelongEntityDB = userBelongEntityDB;
        }

        #region 获取数据

        /// <summary>
        ///     带条件查询所有
        /// </summary>
        public async Task<List<UserBelongEntity>> GetList(UserBelongListParam param)
        {
            #region 查询条件

            var query = _userBelongEntityDB.AsQueryable();

            if (param.UserId.HasValue)
                query = query.Where(a => a.UserId == param.UserId);

            #endregion

            var data = await query.ToListAsync();
            return data;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public async Task<List<UserBelongEntity>> GetPageList(UserBelongListParam param, Pagination pagination)
        {
            #region 查询条件

            var query = _userBelongEntityDB.AsQueryable();
            /*

            */
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
        public async Task<UserBelongEntity> GetEntity(long id)
        {
            var list = await _userBelongEntityDB.AsAsyncEnumerable(p => p.Id == id);
            return list.FirstOrDefault();
        }

        /// <summary>
        ///     查询多个ID主键数据
        /// </summary>
        public async Task<List<UserBelongEntity>> GetListByIds(string ids)
        {
            if (ids.IsNullOrEmpty())
                throw new Exception("参数不合法！");

            var idArr = TextHelper.SplitToArray<long>(ids, ',').ToList();
            var data = await _userBelongEntityDB.AsAsyncEnumerable(a => idArr.Contains(a.Id.GetValueOrDefault()));

            return data;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(UserBelongEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                // 默认赋值
                entity.Id = IdGeneratorHelper.Instance.GetId();
                entity.CreatorId = NetHelper.HttpContext.User.FindFirstValue("UserId").ParseToLong();
                entity.CreateTime = DateTime.Now;
                await _userBelongEntityDB.InsertNowAsync(entity);
            }
            else
            {
                await _userBelongEntityDB.UpdateNowAsync(entity, ignoreNullValues: true);
            }
        }

        public async Task DeleteForm(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                throw new Exception("参数不合法！");

            var _ids = ids.Split(",");
            await _userBelongEntityDB.BatchDeleteAsync(_ids);
        }

        public async Task SaveUserRoles(long userId, List<long> roleIds)
        {
            foreach (var roleId in roleIds.Distinct())
            {
                var entity = new UserBelongEntity();
                entity.UserId = userId;
                entity.BelongId = roleId;
                entity.BelongType = UserBelongTypeEnum.Role.ParseToInt();

                await SaveForm(entity);
            }
        }

        public async Task SaveUserPositions(long userId, List<long> pIds)
        {
            foreach (var pId in pIds.Distinct())
            {
                var entity = new UserBelongEntity();
                entity.UserId = userId;
                entity.BelongId = pId;
                entity.BelongType = UserBelongTypeEnum.Position.ParseToInt();

                await SaveForm(entity);
            }
        }

        public async Task DeleteByUserId(long userId)
        {
            var ids = await _userBelongEntityDB.Where(a => a.UserId == userId).Select(a => a.Id.GetValueOrDefault())
                .ToListAsync();
            await _userBelongEntityDB.BatchDeleteAsync(ids);
        }

        #endregion

        #region 私有方法

        #endregion
    }
}
