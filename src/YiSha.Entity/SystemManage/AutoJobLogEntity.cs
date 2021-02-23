using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;

namespace YiSha.Entity.SystemManage
{
    [Table("SysAutoJobLog")]
    public class AutoJobLogEntity : Entity<long, MasterDbContextLocator>
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string JobGroupName { get; set; }
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string JobName { get; set; }
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public int? LogStatus { get; set; }
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
    }
}
