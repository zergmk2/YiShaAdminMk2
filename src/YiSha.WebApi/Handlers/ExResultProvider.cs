using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using Furion.UnifyResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.WebApi.Handlers
{
    /// <summary>
    ///     异常返回值操作
    /// </summary>
    [SkipScan]
    [UnifyModel(typeof(RESTfulResult<>))]
    public class ExResultProvider : IUnifyResultProvider
    {
        /// <summary>
        ///     异常返回值
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IActionResult OnException(ExceptionContext context)
        {
            // 解析异常信息
            var (ErrorCode, ErrorObject) = UnifyContext.GetExceptionMetadata(context);

            return new JsonResult(new TData<object>
            {
                Message = ErrorObject.ToString()
            });
        }

        /// <summary>
        ///     成功返回值
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IActionResult OnSucceeded(ActionExecutedContext context)
        {
            object data;
            // 处理内容结果
            if (context.Result is ContentResult contentResult)
                data = contentResult.Content;
            // 处理对象结果
            else if (context.Result is ObjectResult objectResult)
                data = objectResult.Value;
            else
                return null;

            return new JsonResult(data);
        }

        /// <summary>
        ///     验证失败返回值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="modelStates"></param>
        /// <param name="validationResults"></param>
        /// <param name="validateFaildMessage"></param>
        /// <returns></returns>
        public IActionResult OnValidateFailed(ActionExecutingContext context, ModelStateDictionary modelStates,
            Dictionary<string, IEnumerable<string>> validationResults, string validateFaildMessage)
        {
            var msgs = new List<string>();
            foreach (var item in validationResults)
                if (item.Value != null && item.Value.Count() > 0)
                {
                    var msg = $"参数校验失败：{string.Join(" | ", item.Value)}";
                    msgs.Add(msg);
                    LogHelper.Error($"请求URL为：{context.HttpContext.Request.Path} {msg}");
                }

#if DEBUG
            return new JsonResult(new TData
            {
                Message = string.Join("\r\n", msgs)
            });
#else
            return new JsonResult(new TData
            {
                Message = "系统遇到问题，请联系我们"
            });
#endif
        }

        /// <summary>
        ///     处理输出状态码
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public async Task OnResponseStatusCodes(HttpContext context, int statusCode)
        {
            /* 不处理
             switch (statusCode)
             {
                 // 处理 401 状态码
                 case StatusCodes.Status401Unauthorized:
                     await context.Response.WriteAsJsonAsync(new
                     {
                         code = StatusCodes.Status401Unauthorized,
                         msg = "未授权"
                     }) ;
                     break;

                 default:
                     break;
             }*/
        }
    }
}
