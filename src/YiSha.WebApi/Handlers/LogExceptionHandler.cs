using System.Threading.Tasks;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc.Filters;
using YiSha.Util.Helper;

namespace YiSha.WebApi.Handlers
{
    /// <summary>
    ///     异常日志操作
    /// </summary>
    public class LogExceptionHandler : IGlobalExceptionHandler, ISingleton
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            // 写日志文件
            LogHelper.Error(context.Exception);
            return Task.CompletedTask;
        }
    }
}
