using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using YiSha.Entity;
using YiSha.IBusiness.SystemManage;
using YiSha.IService.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 16:22
    ///     描 述：接口权限业务类
    /// </summary>
    public class ApiAuthorizeBLL : IApiAuthorizeBLL, ITransient
    {
        private readonly IApiAuthorizeService _apiAuthorizeService;

        public ApiAuthorizeBLL(IApiAuthorizeService apiAuthorizeService)
        {
            _apiAuthorizeService = apiAuthorizeService;
        }

        #region 获取数据

        public async Task<TData<List<ApiAuthorizeEntity>>> GetList(ApiAuthorizeListParam param)
        {
            var obj = new TData<List<ApiAuthorizeEntity>>();
            obj.Data = await _apiAuthorizeService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ApiAuthorizeEntity>>> GetPageList(ApiAuthorizeListParam param,
            Pagination pagination)
        {
            var obj = new TData<List<ApiAuthorizeEntity>>();
            obj.Data = await _apiAuthorizeService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<ApiAuthorizeEntity>> GetEntity(long id)
        {
            var obj = new TData<ApiAuthorizeEntity>();
            obj.Data = await _apiAuthorizeService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(ApiAuthorizeEntity entity)
        {
            var obj = new TData<string>();
            await _apiAuthorizeService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            var obj = new TData();
            await _apiAuthorizeService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        ///     保存权限
        /// </summary>
        /// <param name="authorize">权限标识</param>
        /// <param name="urls">url地址</param>
        /// <returns></returns>
        public async Task<TData> SaveAccess(string authorize, List<string> urls)
        {
            var obj = new TData();

            // 删除该权限标识对应的所有url
            await _apiAuthorizeService.DeleteByAuthorize(authorize);

            if (urls != null && urls.Count() > 0)
            {
                var apiAuthorizes = new List<ApiAuthorizeEntity>();

                foreach (var url in urls)
                {
                    var entity = new ApiAuthorizeEntity
                    {
                        Id = IdGeneratorHelper.Instance.GetId(),
                        Authorize = authorize,
                        Url = url
                    };
                    apiAuthorizes.Add(entity);
                }

                // 批量新增数据
                await _apiAuthorizeService.AddAccess(apiAuthorizes);
            }

            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}
