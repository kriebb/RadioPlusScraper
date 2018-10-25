using Newtonsoft.Json;

namespace RadioPlus.Json
{
    public class Program
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("image")] public string Image { get; set; }

        [JsonProperty("thumb")] public string Thumb { get; set; }
    }
}