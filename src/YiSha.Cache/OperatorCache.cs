using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using YiSha.Enum;
using YiSha.IService.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Cache
{
    /// <summary>
    ///     用户信息缓存
    /// </summary>
    public class OperatorCache : ITransient
    {
        private readonly ICache _cache;
        private readonly IUserService _userService;
        private readonly string _userCacheKeyFix = CacheKeys.UserCacheFix.ParseToString();

        public OperatorCache(Func<string, ISingleton, object> resolveNamed, IUserService userService)
        {
            _cache = resolveNamed(GlobalContext.SystemConfig.CacheService, default) as ICache;
            _userService = userService;
        }

        public async Task AddCurrent(string token)
        {
            var user = await _userService.GetUserByToken(token);
            if (user != null) _cache.Set(_userCacheKeyFix + token, user, DateTime.Now.AddDays(7));
        }


        public async Task<OperatorInfo> Current(string token = "")
        {
            OperatorInfo user = null;

            // 如果没传token，就拿请求中的token
            if (token.IsEmpty())
                token = GetToken();

            if (string.IsNullOrEmpty(token))
                return user;

            user = _cache.Get<OperatorInfo>(_userCacheKeyFix + token);

            if (user == null)
                user = await _userService.GetUserByToken(token);

            return user;
        }

        public void UpdateOperatorInfo(OperatorInfo user)
        {
            _cache.Set(GetToken(), user, DateTime.Now.AddDays(7));
        }

        /// <summary>
        ///     获取token
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            var token = NetHelper.HttpContext.User.FindFirstValue("ApiToken").ParseToString();
            return token;
        }
    }
}
