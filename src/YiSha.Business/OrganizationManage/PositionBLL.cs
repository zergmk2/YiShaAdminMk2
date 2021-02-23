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
    ///     日 期：2020-12-06 14:10
    ///     描 述：职位信息业务类
    /// </summary>
    public class PositionBLL : IPositionBLL, ITransient
    {
        private readonly IPositionService _positionService;

        public PositionBLL(IPositionService positionService)
        {
            _positionService = positionService;
        }

        #region 获取数据

        public async Task<TData<List<PositionEntity>>> GetList(PositionListParam param)
        {
            var obj = new TData<List<PositionEntity>>();
            obj.Data = await _positionService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<PositionEntity>>> GetPageList(PositionListParam param, Pagination pagination)
        {
            var obj = new TData<List<PositionEntity>>();
            obj.Data = await _positionService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<PositionEntity>> GetEntity(long id)
        {
            var obj = new TData<PositionEntity>();
            obj.Data = await _positionService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(PositionEntity entity)
        {
            var obj = new TData<string>();
            await _positionService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            var obj = new TData();
            await _positionService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}
