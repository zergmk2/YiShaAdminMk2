using System.ComponentModel.DataAnnotations.Schema;
using Furion.DatabaseAccessor;
using YiSha.Util.Helper;
using Newtonsoft.Json;

namespace YiSha.Entity.SystemManage
{
    [Table("SysMenu")]
    public class MenuEntity : IEntity<MasterDbContextLocator>
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public long? Id { get; set; }

        [JsonConverter(typeof(StringJsonConverter))]
        public long? ParentId { get; set; }

        public string MenuName { get; set; }

        public string MenuIcon { get; set; }

        public string MenuUrl { get; set; }

        public string MenuTarget { get; set; }

        public int MenuSort { get; set; }

        public int MenuType { get; set; }

        public int MenuStatus { get; set; }

        public string Authorize { get; set; }

        public string Remark { get; set; }
    }
}
