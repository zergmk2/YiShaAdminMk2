using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.LinqBuilder;
using YiSha.Entity;
using Microsoft.EntityFrameworkCore;
using YiSha.IService.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Service.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 12:28
    ///     描 述：代码模板服务类
    /// </summary>
    public class CodeTempletService : ICodeTempletService, ITransient
    {
        private readonly IRepository<CodeTempletEntity> _codeTempletEntityDB;

        public CodeTempletService(IRepository<CodeTempletEntity> codeTempletEntityDB)
        {
            _codeTempletEntityDB = codeTempletEntityDB;
        }

        #region 获取数据

        /// <summary>
        ///     带条件查询所有
        /// </summary>
        public async Task<List<CodeTempletEntity>> GetList(CodeTempletListParam param)
        {
            #region 查询条件

            var query = _codeTempletEntityDB.AsQueryable();
            /*

            */

            #endregion

            var data = await query.ToListAsync();
            return data;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        public async Task<List<CodeTempletEntity>> GetPageList(CodeTempletListParam param, Pagination pagination)
        {
            #region 查询条件

            var query = _codeTempletEntityDB.AsQueryable();
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
        public async Task<CodeTempletEntity> GetEntity(long id)
        {
            var list = await _codeTempletEntityDB.AsAsyncEnumerable(p => p.Id == id);
            return list.FirstOrDefault();
        }


        /// <summary>
        ///     查询多个ID主键数据
        /// </summary>
        public async Task<List<CodeTempletEntity>> GetListByIds(string ids)
        {
            if (ids.IsNullOrEmpty())
                throw new Exception("参数不合法！");

            var idArr = TextHelper.SplitToArray<long>(ids, ',').ToList();
            var data = await _codeTempletEntityDB.AsAsyncEnumerable(a => idArr.Contains(a.Id.GetValueOrDefault()));

            return data;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(CodeTempletEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                // 默认赋值
                entity.Id = IdGeneratorHelper.Instance.GetId();
                ;
                await _codeTempletEntityDB.InsertNowAsync(entity);
            }
            else
            {
                // 默认赋值
                await _codeTempletEntityDB.UpdateNowAsync(entity, ignoreNullValues: true);
            }
        }

        public async Task DeleteForm(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                throw new Exception("参数不合法！");

            var _ids = ids.Split(",");
            await _codeTempletEntityDB.BatchDeleteAsync(_ids);
        }

        #endregion

        #region 私有方法

        #endregion
    }
}
