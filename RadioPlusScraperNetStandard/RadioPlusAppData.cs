// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using RadioPlus.Json;
//
//    var radioPlusAppData = RadioPlusAppData.FromJson(jsonString);

namespace RadioPlus.Json
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class RadioPlusAppData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("deeplink")]
        public string Deeplink { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("previous")]
        public string Previous { get; set; }

        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("images")]
        public Images Images { get; set; }

        [JsonProperty("labels")]
        public Labels Labels { get; set; }

        [JsonProperty("options")]
        public Options Options { get; set; }

        [JsonProperty("streamUrls")]
        public Dictionary<string, string> StreamUrls { get; set; }

        [JsonProperty("app")]
        public App App { get; set; }

        [JsonProperty("program")]
        public Program Program { get; set; }

        [JsonProperty("song")]
        public Song Song { get; set; }
    }

    public partial class RadioPlusAppData
    {
        public static Dictionary<string, RadioPlusAppData> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, RadioPlusAppData>>(json, RadioPlus.Json.Converter.Settings);
    }
}
