using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion;
using Furion.DependencyInjection;
using YiSha.Cache;
using YiSha.Entity;
using YiSha.Enum;
using YiSha.IBusiness.SystemManage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using SqlSugar;

namespace YiSha.WebApi.Handlers
{
    /// <summary>
    ///     验证token和记录日志
    /// </summary>
    public class AuthorizeFilterAttribute : ActionFilterAttribute, ITransient
    {
        /// <summary>
        ///     Api记录操作
        /// </summary>
        public static string LogAllApi = GlobalContext.SystemConfig.LogAllApi;

        private readonly ApiAuthorizeCache _apiAuthorizeCache;
        private readonly IMenuAuthorizeBLL _menuAuthorizeBLL;
        private readonly OperatorCache _operator;

        public AuthorizeFilterAttribute(OperatorCache operatord, IMenuAuthorizeBLL menuAuthorizeBLL,
            ApiAuthorizeCache apiAuthorizeCache)
        {
            _operator = operatord;
            _menuAuthorizeBLL = menuAuthorizeBLL;
            _apiAuthorizeCache = apiAuthorizeCache;
        }

        /// <summary>
        ///     异步接口日志
        /// </summary>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(AllowAnonymousAttribute)); //< -- Here it is

            if (hasAllowAnonymous)
            {
                await next();
            }
            else
            {

                // 类似计时器
                var sw = new Stopwatch();
                sw.Start();

                // 获取用户信息
                var user = await _operator.Current();
                if (user == null)
                {
                    throw new Exception("User not logged in.");
                }

                // 更新用户的权限到缓存中
                if (user.MenuAuthorizes == null || user.MenuAuthorizes.Count() == 0)
                {
                    var objMenuAuthorize = await _menuAuthorizeBLL.GetAuthorizeList(user);
                    user.MenuAuthorizes = objMenuAuthorize.Data;
                    _operator.UpdateOperatorInfo(user);
                }

                // 校验用户权限
                var obj = await CheckAccess(context, user);
                // 没有权限
                if (obj.Tag == 0)
                {
                    obj.Message = "抱歉，您没有权限";
                    context.Result = new JsonResult(obj);
                    return;
                }

                // 执行
                var resultContext = await next();

                #region 保存日志

                    // 如果配置了仅记录报错日志
                    if (LogAllApi.ToUpper() == "ERROR" && resultContext.Exception == null)
                    return;

                    var logApiEntity = new LogApiEntity();
                    logApiEntity.ExecuteUrl = context.HttpContext.Request.Path;
                    logApiEntity.LogStatus = OperateStatusEnum.Success.ParseToInt();

                    #region 获取Post参数

                    switch (context.HttpContext.Request.Method.ToUpper())
                    {
                    case "GET":
                    logApiEntity.ExecuteParam = context.HttpContext.Request.QueryString.Value.ParseToString();
                    break;

                    case "POST":
                    if (context.ActionArguments?.Count > 0)
                    {
                    if (!context.HttpContext.Request.QueryString.HasValue)
                    break;

                    logApiEntity.ExecuteUrl += context.HttpContext.Request.QueryString.Value.ParseToString();
                    logApiEntity.ExecuteParam =
                    TextHelper.GetSubString(JsonConvert.SerializeObject(context.ActionArguments), 4000);
                    }
                    else
                    {
                    logApiEntity.ExecuteParam = context.HttpContext.Request.QueryString.Value.ParseToString();
                    }

                    break;
                    }

                    #endregion

                    if (user != null)
                    logApiEntity.CreatorId = user.UserId;

                    // 异常信息
                    SetExceptionMsg(resultContext, context, logApiEntity);

                    // 计时器结束
                    sw.Stop();

                    logApiEntity.ExecuteTime = sw.ElapsedMilliseconds.ParseToInt();
                    logApiEntity.IpAddress = NetHelper.Ip;

                    // 记录日志
                    await SaveLogAPI(logApiEntity);

                    #endregion
            }
        }

        /// <summary>
        ///     设置异常信息到日志文件和数据库
        /// </summary>
        private void SetExceptionMsg(ActionExecutedContext resultContext, ActionExecutingContext context,
            LogApiEntity logApiEntity)
        {
            if (resultContext.Exception != null)
            {
                #region 异常获取

                var sbException = new StringBuilder();
                var exception = resultContext.Exception;
                sbException.AppendLine(exception.Message);
                while (exception.InnerException != null)
                {
                    sbException.AppendLine(exception.InnerException.Message);
                    exception = exception.InnerException;
                }

                sbException.AppendLine(TextHelper.GetSubString(resultContext.Exception.StackTrace, 8000));

                #endregion

                logApiEntity.ExecuteResult = sbException.ToString();
                logApiEntity.LogStatus = OperateStatusEnum.Fail.ParseToInt();
            }
            else
            {
                var result = context.Result as ObjectResult;
                if (result != null)
                {
                    logApiEntity.ExecuteResult = JsonConvert.SerializeObject(result.Value);
                    logApiEntity.LogStatus = OperateStatusEnum.Success.ParseToInt();
                }
            }
        }

        /// <summary>
        ///     保存日志到数据库
        /// </summary>
        private async Task SaveLogAPI(LogApiEntity logApiEntity)
        {
            try
            {
                logApiEntity.ExecuteParam = TextHelper.GetSubString(logApiEntity.ExecuteParam, 4000);
                logApiEntity.ExecuteResult = TextHelper.GetSubString(logApiEntity.ExecuteResult, 4000);
                logApiEntity.Id = IdGeneratorHelper.Instance.GetId();
                logApiEntity.CreateTime = DateTime.Now;

                // 踩坑：此处使用三方包进行保存，因为当接口采用自动事务管理时，如果此处也使用EF，会被一并回滚，导致无法正常记录
                var logApiDB = App.GetService<ISqlSugarRepository<LogApiEntity>>();
                await logApiDB.InsertAsync(logApiEntity);
            }
            catch (Exception ex)
            {
                LogHelper.Error("日志记录到数据库时发生错误", ex);
            }
        }

        /// <summary>
        ///     校验是否有接口访问权限
        /// </summary>
        private async Task<TData> CheckAccess(ActionExecutingContext context, OperatorInfo user)
        {
            var obj = new TData();
            obj.Tag = 1;

            // 管理员直接跳过
            if (user.IsSystem == 1)
                return obj;

            // 获取当前请求的URL
            var url = context.HttpContext.Request.Path.ToString();
            var authorizeList = await _apiAuthorizeCache.GetAuthorizeByUrl(url);

            // 该接口无关联权限标识
            if (authorizeList.Count() == 0)
                return obj;

            var authorizeInfoList = user.MenuAuthorizes
                .Where(p => authorizeList.Select(a => a.Authorize).Contains(p.Authorize)).ToList();

            if (authorizeInfoList.Any())
            {
                //  新增和修改判断
                if (context.RouteData.Values["Action"].ToString() == "SaveFormJson")
                {
                    var id = context.HttpContext.Request.Form["Id"];
                    if (id.ParseToLong() > 0)
                        if (!authorizeInfoList.Where(p => p.Authorize.Contains("edit")).Any())
                            obj.Tag = 0;
                        else if (!authorizeInfoList.Where(p => p.Authorize.Contains("add")).Any())
                            obj.Tag = 0;
                }
            }
            else
            {
                obj.Tag = 0;
            }

            return obj;
        }
    }
}
