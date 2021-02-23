using System.Collections.Generic;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using YiSha.Entity;
using YiSha.IBusiness.OrganizationManage;
using YiSha.IService.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Business.OrganizationManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 13:08
    ///     描 述：用户关联信息业务类
    /// </summary>
    public class UserBelongBLL : IUserBelongBLL, ITransient
    {
        private readonly IUserBelongService _userBelongService;

        public UserBelongBLL(IUserBelongService userBelongService)
        {
            _userBelongService = userBelongService;
        }

        #region 获取数据

        public async Task<TData<List<UserBelongEntity>>> GetList(UserBelongListParam param)
        {
            var obj = new TData<List<UserBelongEntity>>();
            obj.Data = await _userBelongService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<UserBelongEntity>>> GetPageList(UserBelongListParam param, Pagination pagination)
        {
            var obj = new TData<List<UserBelongEntity>>();
            obj.Data = await _userBelongService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<UserBelongEntity>> GetEntity(long id)
        {
            var obj = new TData<UserBelongEntity>();
            obj.Data = await _userBelongService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(UserBelongEntity entity)
        {
            var obj = new TData<string>();
            await _userBelongService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            var obj = new TData();
            await _userBelongService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}
