using System;
using Newtonsoft.Json;

namespace RadioPlusOnDemand.Json
{
    internal class ClassNameConverter : JsonConverter
    {
        public static readonly ClassNameConverter Singleton = new ClassNameConverter();

        public override bool CanConvert(Type t)
        {
            return t == typeof(ClassName) || t == typeof(ClassName?);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "entryonly") return ClassName.Entryonly;
            throw new Exception("Cannot unmarshal type ClassName:" + value);
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            var value = (ClassName) untypedValue;
            if (value == ClassName.Entryonly)
            {
                serializer.Serialize(writer, "entryonly");
                return;
            }

            throw new Exception("Cannot marshal type ClassName:" + value);
        }
    }
}