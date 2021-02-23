namespace YiSha.Model.Param.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 11:25
    ///     描 述：菜单实体查询类
    /// </summary>
    public class MenuListParam
    {
        public string MenuName { get; set; }
        public int? MenuStatus { get; set; }
        public long? AuthorizeId { get; set; }
        public int? AuthorizeType { get; set; }
        public string AuthorizeIds { get; set; }
    }
}
