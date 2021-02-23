using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Furion;
using Furion.Authorization;
using Furion.DataEncryption;
using Furion.DynamicApiController;
using YiSha.Cache;
using YiSha.Entity;
using YiSha.Enum;
using YiSha.IBusiness.OrganizationManage;
using YiSha.IBusiness.SystemManage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace YiSha.WebApi.Areas.HomeManage
{
    /// <summary>
    ///     登陆登出控制器
    /// </summary>
    [Route("HomeManage/[controller]")]
    [ApiDescriptionSettings(SplitCamelCase = false)]
    [AllowAnonymous]
    public class LoginController : IDynamicApiController
    {
        private readonly ILogLoginBLL _logLoginBLL;
        private readonly OperatorCache _operatorCache;
        private readonly IUserBLL _userBLL;

        public LoginController(IUserBLL userBLL, ILogLoginBLL logLoginBLL, OperatorCache operatorCache)
        {
            _userBLL = userBLL;
            _logLoginBLL = logLoginBLL;
            _operatorCache = operatorCache;
        }

        /// <summary>
        ///     用户登陆
        /// </summary>
        [HttpPost]
        public async Task<TData<OperatorInfo>> Login([FromForm] string userName, [FromForm] string password)
        {
            var obj = new TData<OperatorInfo>();
            var userObj = await _userBLL.CheckLogin(userName, password);
            if (userObj.Tag == 1)
            {
                await _userBLL.UpdateLoginInfo(userObj.Data);
                await _operatorCache.AddCurrent(userObj.Data.ApiToken);
                obj.Data = await _operatorCache.Current(userObj.Data.ApiToken);
            }

            obj.Message = userObj.Message;

            var ip = NetHelper.Ip;
            var browser = NetHelper.Browser;
            var os = NetHelper.GetOSVersion();
            var userAgent = NetHelper.UserAgent;

            var logLoginEntity = new LogLoginEntity
            {
                LogStatus = userObj.Tag == 1
                    ? OperateStatusEnum.Success.ParseToInt()
                    : OperateStatusEnum.Fail.ParseToInt(),
                Remark = userObj.Message,
                IpAddress = ip,
                IpLocation = IpLocationHelper.GetIpLocation(ip),
                Browser = browser,
                OS = os,
                ExtraRemark = userAgent,
                CreatorId = userObj.Data == null ? 0 : userObj.Data.Id,
                CreateTime = DateTime.Now
            };

            await _logLoginBLL.SaveForm(logLoginEntity);

            if (userObj.Tag == 0)
                return obj;

            // 生成前端的token
            // 生成 token
            var jwtSettings = App.GetOptions<JWTSettingsOptions>();
            var datetimeOffset = DateTimeOffset.UtcNow;

            var accessToken = JWTEncryption.Encrypt(jwtSettings.IssuerSigningKey, new Dictionary<string, object>
            {
                {"UserId", userObj.Data.Id.ToString()}, // 存储Id
                {"Account", userObj.Data.UserName}, // 存储用户名
                {"ApiToken", userObj.Data.ApiToken}, // ApiToken
                {JwtRegisteredClaimNames.Iat, datetimeOffset.ToUnixTimeSeconds()},
                {JwtRegisteredClaimNames.Nbf, datetimeOffset.ToUnixTimeSeconds()},
                {
                    JwtRegisteredClaimNames.Exp,
                    DateTimeOffset.UtcNow.AddSeconds(jwtSettings.ExpiredTime.Value * 60).ToUnixTimeSeconds()
                },
                {JwtRegisteredClaimNames.Iss, jwtSettings.ValidIssuer},
                {JwtRegisteredClaimNames.Aud, jwtSettings.ValidAudience}
            });

            // 覆盖apitoken，因为前端需要的是jwt生成的token，而缓存使用的是数据库的apitoken字段
            obj.Data.JwtToken = accessToken;

            obj.Tag = userObj.Tag;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userObj.Data.UserName),
                new Claim("ApiToken", userObj.Data.ApiToken),
            };
            
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            var authProperties = new AuthenticationProperties();
            await NetHelper.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return obj;
        }
    }
}
