using System.Collections.Generic;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using YiSha.Entity;
using YiSha.IBusiness.SystemManage;
using YiSha.IService.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-19 08:59
    ///     描 述：数据字典值业务类
    /// </summary>
    public class DataDictDetailBLL : IDataDictDetailBLL, ITransient
    {
        private readonly IDataDictDetailService _dataDictDetailService;

        public DataDictDetailBLL(IDataDictDetailService dataDictDetailService)
        {
            _dataDictDetailService = dataDictDetailService;
        }

        #region 获取数据

        public async Task<TData<List<DataDictDetailEntity>>> GetList(DataDictDetailListParam param)
        {
            var obj = new TData<List<DataDictDetailEntity>>();
            obj.Data = await _dataDictDetailService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<DataDictDetailEntity>>> GetPageList(DataDictDetailListParam param,
            Pagination pagination)
        {
            var obj = new TData<List<DataDictDetailEntity>>();
            obj.Data = await _dataDictDetailService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<DataDictDetailEntity>> GetEntity(long id)
        {
            var obj = new TData<DataDictDetailEntity>();
            obj.Data = await _dataDictDetailService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(DataDictDetailEntity entity)
        {
            var obj = new TData<string>();
            await _dataDictDetailService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            var obj = new TData();
            await _dataDictDetailService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}
