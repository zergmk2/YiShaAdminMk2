using System.Threading.Tasks;
using Furion;
using Furion.Authorization;
using Furion.DataEncryption;
using YiSha.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using YiSha.Util;

namespace YiSha.WebApi.Handlers
{
    /// <summary>
    ///     JWT 授权自定义处理程序
    /// </summary>
    public class JwtHandler : AppAuthorizeHandler
    {
        /// <summary>
        ///     请求管道
        /// </summary>
        public override async Task<bool> PipelineAsync(AuthorizationHandlerContext context,
            DefaultHttpContext httpContext)
        {
            return await CheckLogin(httpContext);
        }

        /// <summary>
        ///     登录校验
        /// </summary>
        private static async Task<bool> CheckLogin(DefaultHttpContext httpContext)
        {
            var str = httpContext.Request.Headers["Authorization"].ToString();

            if (str.IsEmpty())
                return false;

            var token = str[7..];

            var result = JWTEncryption.Validate(token);

            // 校验不通过
            if (result.IsValid == false)
                return false;

            // 获取用户
            var userId = result.Token.GetPayloadValue<string>("UserId").ParseToLong();
            var apiToken = result.Token.GetPayloadValue<string>("ApiToken").ParseToString();
            if (userId.IsEmpty() || apiToken.IsEmpty())
                return false;

            var _operator = App.GetService<OperatorCache>();
            var user = await _operator.Current();
            if (user == null)
                return false;

            return true;
        }
    }
}
