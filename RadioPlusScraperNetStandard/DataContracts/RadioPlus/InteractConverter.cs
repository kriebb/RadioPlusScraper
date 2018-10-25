using System;
using Newtonsoft.Json;

namespace RadioPlus.Json
{
    internal class InteractConverter : JsonConverter
    {
        public static readonly InteractConverter Singleton = new InteractConverter();

        public override bool CanConvert(Type t)
        {
            return t == typeof(Interact) || t == typeof(Interact?);
        }

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

            var value = (Interact) untypedValue;
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
    }
}