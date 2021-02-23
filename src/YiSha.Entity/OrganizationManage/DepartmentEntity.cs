using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;
using YiSha.Util.Helper;
using Newtonsoft.Json;

namespace YiSha.Entity
{
    /// <summary>
    ///     创 建：song
    ///     日 期：2020-12-06 14:12
    ///     描 述：部门信息实体类
    /// </summary>
    [Table("SysDepartment")]
    public class DepartmentEntity : IEntity<MasterDbContextLocator>
    {
        /// <summary>
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? CreatorId { get; set; }

        /// <summary>
        ///     父部门Id(0表示是根部门)
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? ParentId { get; set; }

        /// <summary>
        ///     部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        ///     部门电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        ///     部门传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        ///     部门Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     部门负责人Id
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? PrincipalId { get; set; }

        /// <summary>
        ///     部门排序
        /// </summary>
        public int? DepartmentSort { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///     是否启用
        /// </summary>
        public bool? Enable { get; set; }

        /// <summary>
        ///     负责人电话
        /// </summary>
        public string PrincipalPhone { get; set; }

        /// <summary>
        ///     部门编号
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        ///     多个部门Id
        /// </summary>
        [NotMapped]
        public string Ids { get; set; }

        /// <summary>
        ///     负责人名称
        /// </summary>
        [NotMapped]
        public string PrincipalName { get; set; }

        /// <summary>
        ///     子集内容
        /// </summary>
        [NotMapped]
        public List<DepartmentEntity> children { set; get; }
    }
}
