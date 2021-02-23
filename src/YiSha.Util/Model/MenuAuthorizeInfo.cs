namespace YiSha.Util.Model
{
    public class MenuAuthorizeInfo
    {
        public long? MenuId { get; set; }

        /// <summary>
        ///     用户Id或者角色Id
        /// </summary>
        public long? AuthorizeId { get; set; }

        /// <summary>
        ///     用户或者角色
        /// </summary>
        public int? AuthorizeType { get; set; }

        /// <summary>
        ///     权限标识
        /// </summary>
        public string Authorize { get; set; }
    }
}
