// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using RadioPlus.Json;
//
//    var radioPlusOnDemandData = RadioPlusOnDemandData.FromJson(jsonString);

namespace RadioPlusOnDemand.Json
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class RadioPlusOnDemandData
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("collectionID")]
        public Guid CollectionId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("thumb")]
        public string Thumb { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }

        [JsonProperty("numItems")]
        public long NumItems { get; set; }

        [JsonProperty("timestamp")]
        public Timestamp Timestamp { get; set; }

        [JsonProperty("isEntry")]
        public bool IsEntry { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("stream")]
        public string Stream { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("collectionID")]
        public Guid CollectionId { get; set; }

        [JsonProperty("startTime")]
        public DateTimeOffset StartTime { get; set; }

        [JsonProperty("creationTimestamp")]
        public DateTimeOffset CreationTimestamp { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("stream")]
        public string Stream { get; set; }

        [JsonProperty("stream_aac")]
        public string StreamAac { get; set; }

        [JsonProperty("stream_hls")]
        public string StreamHls { get; set; }

        [JsonProperty("stream_dash")]
        public string StreamDash { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("thumb")]
        public string Thumb { get; set; }

        [JsonProperty("timestamp")]
        public Timestamp Timestamp { get; set; }

        [JsonProperty("isEntry")]
        public bool IsEntry { get; set; }

        [JsonProperty("className")]
        public ClassName ClassName { get; set; }
    }

    public partial class Timestamp
    {
        [JsonProperty("start")]
        public Start Start { get; set; }
    }

    public partial class Start
    {
        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("hourRound")]
        public HourRound HourRound { get; set; }

        [JsonProperty("hour")]
        public string Hour { get; set; }

        [JsonProperty("endHour", NullValueHandling = NullValueHandling.Ignore)]
        public string EndHour { get; set; }

        [JsonProperty("day")]
        public string Day { get; set; }
    }

    public enum ClassName { Entryonly };

    public enum HourRound { The0200, The0600, The0700, The0800, The0900, The1000, The1100, The1200, The1300, The1400, The1500, The1600, The1800, The1900, The2000, The2100, The2200, The2300,
        The0000,
        The0100,
        The0300,
        The0400,
        The0500,
        The2400
    };

    public partial class RadioPlusOnDemandData
    {
        public static RadioPlusOnDemandData[] FromJson(string json) => JsonConvert.DeserializeObject<RadioPlusOnDemandData[]>(json, RadioPlusOnDemand.Json.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this RadioPlusOnDemandData[] self) => JsonConvert.SerializeObject(self, RadioPlusOnDemand.Json.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                ClassNameConverter.Singleton,
                HourRoundConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ClassNameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ClassName) || t == typeof(ClassName?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "entryonly")
            {
                return ClassName.Entryonly;
            }
            throw new Exception("Cannot unmarshal type ClassName:"+value);
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ClassName)untypedValue;
            if (value == ClassName.Entryonly)
            {
                serializer.Serialize(writer, "entryonly");
                return;
            }
            throw new Exception("Cannot marshal type ClassName:"+value);
        }

        public static readonly ClassNameConverter Singleton = new ClassNameConverter();
    }

    internal class HourRoundConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(HourRound) || t == typeof(HourRound?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "00:00":
                    return HourRound.The0000;
                case "01:00":
                    return HourRound.The0100;
                case "02:00":
                    return HourRound.The0200;
                case "03:00":
                    return HourRound.The0300;
                case "04:00":
                    return HourRound.The0400;
                case "05:00":
                    return HourRound.The0500;
                case "06:00":
                    return HourRound.The0600;
                case "07:00":
                    return HourRound.The0700;
                case "08:00":
                    return HourRound.The0800;
                case "09:00":
                    return HourRound.The0900;
                case "10:00":
                    return HourRound.The1000;
                case "11:00":
                    return HourRound.The1100;
                case "12:00":
                    return HourRound.The1200;
                case "13:00":
                    return HourRound.The1300;
                case "14:00":
                    return HourRound.The1400;
                case "15:00":
                    return HourRound.The1500;
                case "16:00":
                    return HourRound.The1600;
                case "18:00":
                    return HourRound.The1800;
                case "19:00":
                    return HourRound.The1900;
                case "20:00":
                    return HourRound.The2000;
                case "21:00":
                    return HourRound.The2100;
                case "22:00":
                    return HourRound.The2200;
                case "23:00":
                    return HourRound.The2300;
                case "24:00":
                    return HourRound.The2400;
            }
            throw new Exception("Cannot unmarshal type HourRound:"+value);
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (HourRound)untypedValue;
            switch (value)
            {
                case HourRound.The0200:
                    serializer.Serialize(writer, "02:00");
                    return;
                case HourRound.The0600:
                    serializer.Serialize(writer, "06:00");
                    return;
                case HourRound.The0700:
                    serializer.Serialize(writer, "07:00");
                    return;
                case HourRound.The0800:
                    serializer.Serialize(writer, "08:00");
                    return;
                case HourRound.The0900:
                    serializer.Serialize(writer, "09:00");
                    return;
                case HourRound.The1000:
                    serializer.Serialize(writer, "10:00");
                    return;
                case HourRound.The1100:
                    serializer.Serialize(writer, "11:00");
                    return;
                case HourRound.The1200:
                    serializer.Serialize(writer, "12:00");
                    return;
                case HourRound.The1300:
                    serializer.Serialize(writer, "13:00");
                    return;
                case HourRound.The1400:
                    serializer.Serialize(writer, "14:00");
                    return;
                case HourRound.The1500:
                    serializer.Serialize(writer, "15:00");
                    return;
                case HourRound.The1600:
                    serializer.Serialize(writer, "16:00");
                    return;
                case HourRound.The1800:
                    serializer.Serialize(writer, "18:00");
                    return;
                case HourRound.The1900:
                    serializer.Serialize(writer, "19:00");
                    return;
                case HourRound.The2000:
                    serializer.Serialize(writer, "20:00");
                    return;
                case HourRound.The2100:
                    serializer.Serialize(writer, "21:00");
                    return;
                case HourRound.The2200:
                    serializer.Serialize(writer, "22:00");
                    return;
                case HourRound.The2300:
                    serializer.Serialize(writer, "23:00");
                    return;
            }
            throw new Exception("Cannot marshal type HourRound:"+value);
        }

        public static readonly HourRoundConverter Singleton = new HourRoundConverter();
    }
}
