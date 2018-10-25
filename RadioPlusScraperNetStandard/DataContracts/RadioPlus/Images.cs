using Newtonsoft.Json;

namespace RadioPlus.Json
{
    public class Images
    {
        [JsonProperty("icon")] public string Icon { get; set; }

        [JsonProperty("thumb")] public string Thumb { get; set; }

        [JsonProperty("facebook")] public string Facebook { get; set; }

        [JsonProperty("app")] public string App { get; set; }

        [JsonProperty("logo", NullValueHandling = NullValueHandling.Ignore)]
        public string Logo { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public string Image { get; set; }
    }
}