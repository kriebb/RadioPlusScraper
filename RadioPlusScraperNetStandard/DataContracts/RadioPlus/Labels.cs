using Newtonsoft.Json;

namespace RadioPlus.Json
{
    public class Labels
    {
        [JsonProperty("interact")] public Interact Interact { get; set; }

        [JsonProperty("submit")] public Submit Submit { get; set; }

        [JsonProperty("sending")] public Sending Sending { get; set; }

        [JsonProperty("succes")] public Succes Succes { get; set; }
    }
}