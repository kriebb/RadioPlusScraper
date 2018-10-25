// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using RadioPlus.Json;
//
//    var radioPlusOnDemandData = RadioPlusOnDemandData.FromJson(jsonString);

using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RadioPlusOnDemand.Json
{
    public partial class RadioPlusOnDemandData
    {
        [JsonProperty("id")] public Guid Id { get; set; }

        [JsonProperty("collectionID")] public Guid CollectionId { get; set; }

        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("image")] public string Image { get; set; }

        [JsonProperty("thumb")] public string Thumb { get; set; }

        [JsonProperty("items")] public Item[] Items { get; set; }

        [JsonProperty("numItems")] public long NumItems { get; set; }

        [JsonProperty("timestamp")] public Timestamp Timestamp { get; set; }

        [JsonProperty("isEntry")] public bool IsEntry { get; set; }

        [JsonProperty("duration")] public long Duration { get; set; }

        [JsonProperty("stream")] public string Stream { get; set; }
    }

    public partial class RadioPlusOnDemandData
    {
        public static RadioPlusOnDemandData[] FromJson(string json)
        {
            return JsonConvert.DeserializeObject<RadioPlusOnDemandData[]>(json, Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this RadioPlusOnDemandData[] self)
        {
            return JsonConvert.SerializeObject(self, Converter.Settings);
        }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                ClassNameConverter.Singleton,
                HourRoundConverter.Singleton,
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            }
        };
    }
}