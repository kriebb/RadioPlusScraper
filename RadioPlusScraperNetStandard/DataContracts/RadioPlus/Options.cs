using Newtonsoft.Json;

namespace RadioPlus.Json
{
    public class Options
    {
        [JsonProperty("adheseId")] public string AdheseId { get; set; }

        [JsonProperty("hasPlaylist", NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasPlaylist { get; set; }

        [JsonProperty("hasPrograms")] public bool HasPrograms { get; set; }

        [JsonProperty("hasOndemand")] public bool HasOndemand { get; set; }

        [JsonProperty("hasSchedules")] public bool HasSchedules { get; set; }

        [JsonProperty("hasBuyAction", NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasBuyAction { get; set; }

        [JsonProperty("hasWebcam", NullValueHandling = NullValueHandling.Ignore)]
        public HasWebcam HasWebcam { get; set; }
    }
}