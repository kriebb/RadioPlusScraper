// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using RadioPlus.Json;
//
//    var radioPlusAppData = RadioPlusAppData.FromJson(jsonString);

namespace RadioPlus.Json
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

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

    public partial class App
    {
        [JsonProperty("ios_button")]
        public string IosButton { get; set; }

        [JsonProperty("ios_smart_banner")]
        public string IosSmartBanner { get; set; }

        [JsonProperty("android")]
        public string Android { get; set; }
    }

    public partial class Images
    {
        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("thumb")]
        public string Thumb { get; set; }

        [JsonProperty("facebook")]
        public string Facebook { get; set; }

        [JsonProperty("app")]
        public string App { get; set; }

        [JsonProperty("logo", NullValueHandling = NullValueHandling.Ignore)]
        public string Logo { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public string Image { get; set; }
    }

    public partial class Info
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("twitter")]
        public string Twitter { get; set; }

        [JsonProperty("sms", NullValueHandling = NullValueHandling.Ignore)]
        public long? Sms { get; set; }

        [JsonProperty("linkedChannels", NullValueHandling = NullValueHandling.Ignore)]
        public LinkedChannels LinkedChannels { get; set; }
    }

    public partial class LinkedChannels
    {
        [JsonProperty("current")]
        public string Current { get; set; }

        [JsonProperty("channels")]
        [JsonConverter(typeof(DecodeArrayConverter))]
        public long[] Channels { get; set; }
    }

    public partial class Labels
    {
        [JsonProperty("interact")]
        public Interact Interact { get; set; }

        [JsonProperty("submit")]
        public Submit Submit { get; set; }

        [JsonProperty("sending")]
        public Sending Sending { get; set; }

        [JsonProperty("succes")]
        public Succes Succes { get; set; }
    }

    public partial class Options
    {
        [JsonProperty("adheseId")]
        public string AdheseId { get; set; }

        [JsonProperty("hasPlaylist", NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasPlaylist { get; set; }

        [JsonProperty("hasPrograms")]
        public bool HasPrograms { get; set; }

        [JsonProperty("hasOndemand")]
        public bool HasOndemand { get; set; }

        [JsonProperty("hasSchedules")]
        public bool HasSchedules { get; set; }

        [JsonProperty("hasBuyAction", NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasBuyAction { get; set; }

        [JsonProperty("hasWebcam", NullValueHandling = NullValueHandling.Ignore)]
        public HasWebcam HasWebcam { get; set; }
    }

    public partial class HasWebcam
    {
        [JsonProperty("webcamId")]
        public string WebcamId { get; set; }
    }

    public partial class Program
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("thumb")]
        public string Thumb { get; set; }
    }

    public partial class Song
    {
        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("full")]
        public string Full { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("thumb")]
        public string Thumb { get; set; }
    }

    public enum Interact { LaatVanJeHoren, LaatVanUHoren };

    public enum Sending { JeReactieWordtNuVerstuurd, UwReactieWordtNuVerstuurd };

    public enum Submit { StuurOnsJeReactie, StuurOnsUwReactie };

    public enum Succes { BedanktVoorJeReactie, BedanktVoorUwReactie };

    public partial class RadioPlusAppData
    {
        public static Dictionary<string, RadioPlusAppData> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, RadioPlusAppData>>(json, RadioPlus.Json.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Dictionary<string, RadioPlusAppData> self) => JsonConvert.SerializeObject(self, RadioPlus.Json.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                InteractConverter.Singleton,
                SendingConverter.Singleton,
                SubmitConverter.Singleton,
                SuccesConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class DecodeArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long[]);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            reader.Read();
            var value = new List<long>();
            while (reader.TokenType != JsonToken.EndArray)
            {
                var converter = ParseStringConverter.Singleton;
                var arrayItem = (long)converter.ReadJson(reader, typeof(long), null, serializer);
                value.Add(arrayItem);
                reader.Read();
            }
            return value.ToArray();
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (long[])untypedValue;
            writer.WriteStartArray();
            foreach (var arrayItem in value)
            {
                var converter = ParseStringConverter.Singleton;
                converter.WriteJson(writer, arrayItem, serializer);
            }
            writer.WriteEndArray();
            return;
        }

        public static readonly DecodeArrayConverter Singleton = new DecodeArrayConverter();
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class InteractConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Interact) || t == typeof(Interact?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Laat van je horen!":
                    return Interact.LaatVanJeHoren;
                case "Laat van u horen!":
                    return Interact.LaatVanUHoren;
            }
            throw new Exception("Cannot unmarshal type Interact");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Interact)untypedValue;
            switch (value)
            {
                case Interact.LaatVanJeHoren:
                    serializer.Serialize(writer, "Laat van je horen!");
                    return;
                case Interact.LaatVanUHoren:
                    serializer.Serialize(writer, "Laat van u horen!");
                    return;
            }
            throw new Exception("Cannot marshal type Interact");
        }

        public static readonly InteractConverter Singleton = new InteractConverter();
    }

    internal class SendingConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Sending) || t == typeof(Sending?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Je reactie wordt nu verstuurd":
                    return Sending.JeReactieWordtNuVerstuurd;
                case "Uw reactie wordt nu verstuurd":
                    return Sending.UwReactieWordtNuVerstuurd;
            }
            throw new Exception("Cannot unmarshal type Sending");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Sending)untypedValue;
            switch (value)
            {
                case Sending.JeReactieWordtNuVerstuurd:
                    serializer.Serialize(writer, "Je reactie wordt nu verstuurd");
                    return;
                case Sending.UwReactieWordtNuVerstuurd:
                    serializer.Serialize(writer, "Uw reactie wordt nu verstuurd");
                    return;
            }
            throw new Exception("Cannot marshal type Sending");
        }

        public static readonly SendingConverter Singleton = new SendingConverter();
    }

    internal class SubmitConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Submit) || t == typeof(Submit?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Stuur ons je reactie":
                    return Submit.StuurOnsJeReactie;
                case "Stuur ons uw reactie":
                    return Submit.StuurOnsUwReactie;
            }
            throw new Exception("Cannot unmarshal type Submit");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Submit)untypedValue;
            switch (value)
            {
                case Submit.StuurOnsJeReactie:
                    serializer.Serialize(writer, "Stuur ons je reactie");
                    return;
                case Submit.StuurOnsUwReactie:
                    serializer.Serialize(writer, "Stuur ons uw reactie");
                    return;
            }
            throw new Exception("Cannot marshal type Submit");
        }

        public static readonly SubmitConverter Singleton = new SubmitConverter();
    }

    internal class SuccesConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Succes) || t == typeof(Succes?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Bedankt voor je reactie!":
                    return Succes.BedanktVoorJeReactie;
                case "Bedankt voor uw reactie!":
                    return Succes.BedanktVoorUwReactie;
            }
            throw new Exception("Cannot unmarshal type Succes");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Succes)untypedValue;
            switch (value)
            {
                case Succes.BedanktVoorJeReactie:
                    serializer.Serialize(writer, "Bedankt voor je reactie!");
                    return;
                case Succes.BedanktVoorUwReactie:
                    serializer.Serialize(writer, "Bedankt voor uw reactie!");
                    return;
            }
            throw new Exception("Cannot marshal type Succes");
        }

        public static readonly SuccesConverter Singleton = new SuccesConverter();
    }
}
