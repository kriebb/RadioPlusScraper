using System;
using Newtonsoft.Json;

namespace RadioPlus.Json
{
    internal class SuccesConverter : JsonConverter
    {
        public static readonly SuccesConverter Singleton = new SuccesConverter();

        public override bool CanConvert(Type t)
        {
            return t == typeof(Succes) || t == typeof(Succes?);
        }

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

            var value = (Succes) untypedValue;
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
    }
}