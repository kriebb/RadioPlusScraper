using Newtonsoft.Json;

namespace RadioPlusOnDemand.Json
{
    public class Timestamp
    {
        [JsonProperty("start")] public Start Start { get; set; }
    }
}