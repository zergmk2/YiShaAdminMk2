namespace YiSha.Model.Param.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-19 08:59
    ///     描 述：数据字典值实体查询类
    /// </summary>
    public class DataDictDetailListParam
    {
        /// <summary>
        ///     字典类型(外键)
        /// </summary>
        /// <returns></returns>
        public string DictType { get; set; }

        /// <summary>
        ///     字典键(一般从1开始)
        /// </summary>
        /// <returns></returns>
        public int? DictKey { get; set; }

        /// <summary>
        ///     字典值
        /// </summary>
        /// <returns></returns>
        public string DictValue { get; set; }
    }
}
