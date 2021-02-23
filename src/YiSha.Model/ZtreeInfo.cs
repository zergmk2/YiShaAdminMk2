using YiSha.Util.Helper;
using Newtonsoft.Json;

namespace YiSha.Model
{
    public class ZtreeInfo
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public long? id { get; set; }

        [JsonConverter(typeof(StringJsonConverter))]
        public long? pId { get; set; }

        public string name { get; set; }

        public bool open => true;

        public bool @checked { set; get; }

        public string title => name;

        public bool disabled { get; set; }
    }
}
