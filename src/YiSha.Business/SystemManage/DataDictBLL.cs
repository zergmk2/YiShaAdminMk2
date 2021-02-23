using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using YiSha.Entity;
using YiSha.IBusiness.SystemManage;
using YiSha.IService.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Model.Result.SystemManage;
using YiSha.Business.Cache;

namespace YiSha.Business.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-18 16:04
    ///     描 述：数据字典业务类
    /// </summary>
    public class DataDictBLL : IDataDictBLL, ITransient
    {
        private readonly IDataDictService _dataDictService;
        private DataDictCache _dataDictCache;
        private DataDictDetailCache _dataDictDetailCache;
        public DataDictBLL(IDataDictService dataDictService, DataDictCache dataDictCache,
            DataDictDetailCache dataDictDetailCache)
        {
            _dataDictService = dataDictService;
            _dataDictCache = dataDictCache;
            _dataDictDetailCache = dataDictDetailCache;
        }

        #region 获取数据

        public async Task<TData<List<DataDictEntity>>> GetList(DataDictListParam param)
        {
            var obj = new TData<List<DataDictEntity>>();
            obj.Data = await _dataDictService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<DataDictEntity>>> GetPageList(DataDictListParam param, Pagination pagination)
        {
            var obj = new TData<List<DataDictEntity>>();
            obj.Data = await _dataDictService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<DataDictEntity>> GetEntity(long id)
        {
            var obj = new TData<DataDictEntity>();
            obj.Data = await _dataDictService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(DataDictEntity entity)
        {
            var obj = new TData<string>();
            await _dataDictService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            var obj = new TData();
            await _dataDictService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<DataDictInfo>>> GetDataDictList()
        {
            TData<List<DataDictInfo>> obj = new TData<List<DataDictInfo>>();
            List<DataDictEntity> dataDictList = await _dataDictCache.GetList();
            List<DataDictDetailEntity> dataDictDetailList = await _dataDictDetailCache.GetList();
            List<DataDictInfo> dataDictInfoList = new List<DataDictInfo>();
            foreach (DataDictEntity dataDict in dataDictList)
            {
                List<DataDictDetailInfo> detailList = dataDictDetailList.Where(p => p.DictType == dataDict.DictType).OrderBy(p => p.DictSort).Select(p => new DataDictDetailInfo
                {
                    DictKey = p.DictKey,
                    DictValue = p.DictValue,
                    ListClass = p.ListClass,
                    IsDefault = p.IsDefault,
                    DictStatus = p.DictStatus,
                    Remark = p.Remark
                }).ToList();
                dataDictInfoList.Add(new DataDictInfo
                {
                    DictType = dataDict.DictType,
                    Detail = detailList
                });
            }
            obj.Data = dataDictInfoList;
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}
