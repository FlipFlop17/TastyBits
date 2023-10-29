using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Application.ReturnModels
{
    public class EmojiApiResultModel
    {
        [JsonProperty("slug")]
        public string Slug { get; set; }
        [JsonProperty("character")]
        public string Character { get; set; }
        [JsonProperty("unicodeName")]
        public string UnicodeName { get; set; }
        [JsonProperty("codePoint")]
        public string CodePoint { get; set; }
        [JsonProperty("group")]
        public string Group { get; set; }
        [JsonProperty("subGroup")]
        public string SubGroup { get; set; }
    }
}
