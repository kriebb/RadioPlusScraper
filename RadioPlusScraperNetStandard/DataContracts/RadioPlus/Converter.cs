using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RadioPlus.Json
{
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                InteractConverter.Singleton,
                SendingConverter.Singleton,
                SubmitConverter.Singleton,
                SuccesConverter.Singleton,
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            }
        };
    }
}