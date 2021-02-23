using System.Collections.Generic;
using YiSha.Util.Model;

namespace YiSha.Model.Result.SystemManage
{
    public class UserAuthorizeInfo
    {
        public int? IsSystem { get; set; }
        public List<MenuAuthorizeInfo> MenuAuthorize { get; set; }
    }
}
