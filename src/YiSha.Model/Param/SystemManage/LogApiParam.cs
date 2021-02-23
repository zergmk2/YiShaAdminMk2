using System;

namespace YiSha.Model.Param.SystemManage
{
    /// <summary>
    ///     创 建：
    ///     日 期：2020-12-04 12:49
    ///     描 述：Api日志实体查询类
    /// </summary>
    public class LogApiListParam
    {
        /// <summary>
        ///     执行状态(0失败 1成功)
        /// </summary>
        /// <returns></returns>
        public int? LogStatus { get; set; }

        /// <summary>
        ///     接口地址
        /// </summary>
        /// <returns></returns>
        public string ExecuteUrl { get; set; }

        public string UserName { get; set; }
        public string IpAddress { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
