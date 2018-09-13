using Newtonsoft.Json;

namespace RadioPlusOnDemand.Json
{
    public partial class Timestamp
    {
        [JsonProperty("start")]
        public Start Start { get; set; }
    }
}