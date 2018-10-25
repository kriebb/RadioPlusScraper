using Newtonsoft.Json;

namespace RadioPlus.Json
{
    public class App
    {
        [JsonProperty("ios_button")] public string IosButton { get; set; }

        [JsonProperty("ios_smart_banner")] public string IosSmartBanner { get; set; }

        [JsonProperty("android")] public string Android { get; set; }
    }
}