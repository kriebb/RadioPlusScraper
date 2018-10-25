using Newtonsoft.Json;

namespace RadioPlus.Json
{
    public class Song
    {
        [JsonProperty("artist")] public string Artist { get; set; }

        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("full")] public string Full { get; set; }

        [JsonProperty("image")] public string Image { get; set; }

        [JsonProperty("thumb")] public string Thumb { get; set; }
    }
}