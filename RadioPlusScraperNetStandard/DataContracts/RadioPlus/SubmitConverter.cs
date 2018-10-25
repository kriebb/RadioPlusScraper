using System;
using Newtonsoft.Json;

namespace RadioPlus.Json
{
    internal class SubmitConverter : JsonConverter
    {
        public static readonly SubmitConverter Singleton = new SubmitConverter();

        public override bool CanConvert(Type t)
        {
            return t == typeof(Submit) || t == typeof(Submit?);
        }

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

            var value = (Submit) untypedValue;
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
    }
}