using Newtonsoft.Json;

namespace RadioPlus.Json
{
    public class HasWebcam
    {
        [JsonProperty("webcamId")] public string WebcamId { get; set; }
    }
}