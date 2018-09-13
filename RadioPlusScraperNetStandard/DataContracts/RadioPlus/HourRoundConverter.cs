using System;
using Newtonsoft.Json;

namespace RadioPlusOnDemand.Json
{
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
                case "17:00":
                    return HourRound.The1700;
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