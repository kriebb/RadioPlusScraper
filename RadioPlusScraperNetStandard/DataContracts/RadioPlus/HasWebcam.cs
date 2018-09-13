using Newtonsoft.Json;

namespace RadioPlus.Json
{
    public partial class HasWebcam
    {
        [JsonProperty("webcamId")]
        public string WebcamId { get; set; }
    }
}