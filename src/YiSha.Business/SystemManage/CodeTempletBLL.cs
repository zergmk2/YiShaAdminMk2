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
    ///     日 期：2020-12-04 12:28
    ///     描 述：代码模板业务类
    /// </summary>
    public class CodeTempletBLL : ICodeTempletBLL, ITransient
    {
        private readonly ICodeTempletService _codeTempletService;

        public CodeTempletBLL(ICodeTempletService codeTempletService)
        {
            _codeTempletService = codeTempletService;
        }

        #region 获取数据

        public async Task<TData<List<CodeTempletEntity>>> GetList(CodeTempletListParam param)
        {
            var obj = new TData<List<CodeTempletEntity>>();
            obj.Data = await _codeTempletService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<CodeTempletEntity>>> GetPageList(CodeTempletListParam param, Pagination pagination)
        {
            var obj = new TData<List<CodeTempletEntity>>();
            obj.Data = await _codeTempletService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<CodeTempletEntity>> GetEntity(long id)
        {
            var obj = new TData<CodeTempletEntity>();
            obj.Data = await _codeTempletService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(CodeTempletEntity entity)
        {
            var obj = new TData<string>();
            await _codeTempletService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            var obj = new TData();
            await _codeTempletService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}
