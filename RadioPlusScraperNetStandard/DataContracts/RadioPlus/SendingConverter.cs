using System;
using Newtonsoft.Json;

namespace RadioPlus.Json
{
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
}