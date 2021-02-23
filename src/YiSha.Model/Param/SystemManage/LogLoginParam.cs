using System;

namespace YiSha.Model.Param.SystemManage
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-04 12:55
    ///     描 述：登陆日志实体查询类
    /// </summary>
    public class LogLoginListParam
    {
        /// <summary>
        ///     ip地址
        /// </summary>
        /// <returns></returns>
        public string IpAddress { get; set; }

        /// <summary>
        ///     浏览器
        /// </summary>
        /// <returns></returns>
        public string Browser { get; set; }

        /// <summary>
        ///     操作系统
        /// </summary>
        /// <returns></returns>
        public string OS { get; set; }

        public string UserName { get; set; }
        public int? LogStatus { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
