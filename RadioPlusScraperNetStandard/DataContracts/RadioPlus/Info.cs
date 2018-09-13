using Newtonsoft.Json;

namespace RadioPlus.Json
{
    public partial class Info
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("sms", NullValueHandling = NullValueHandling.Ignore)]
        public long? Sms { get; set; }

        [JsonProperty("linkedChannels", NullValueHandling = NullValueHandling.Ignore)]
        public LinkedChannels LinkedChannels { get; set; }
    }
}