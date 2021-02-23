using System.Collections.Generic;
using YiSha.Util.Helper;
using Newtonsoft.Json;

namespace YiSha.Util.Model
{
    public class OperatorInfo
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public long? UserId { get; set; }

        public int? UserStatus { get; set; }

        public int? IsOnline { get; set; }

        public string UserName { get; set; }

        public string RealName { get; set; }

        public string ApiToken { get; set; }

        public int? IsSystem { get; set; }

        public string Portrait { get; set; }

        [JsonConverter(typeof(StringJsonConverter))]
        public long? DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        /// <summary>
        ///     岗位Id
        /// </summary>
        public string PositionIds { get; set; }

        /// <summary>
        ///     角色Id
        /// </summary>
        public string RoleIds { get; set; }


        public List<MenuAuthorizeInfo> MenuAuthorizes { get; set; }

        public string JwtToken { get; set; }
    }

    public class RoleInfo
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public long RoleId { get; set; }
    }
}
