using System.Collections.Generic;

namespace YiSha.Model.Param.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 16:22
    ///     描 述：接口权限实体查询类
    /// </summary>
    public class ApiAuthorizeListParam
    {
        /// <summary>
        ///     请求接口
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     菜单权限标识
        /// </summary>
        public string Authorize { get; set; }
    }

    public class ApiAuthorizeSaveParam
    {
        /// <summary>
        ///     请求接口
        /// </summary>
        public List<string> Urls { get; set; }

        /// <summary>
        ///     菜单权限标识
        /// </summary>
        public string Authorize { get; set; }
    }
}
