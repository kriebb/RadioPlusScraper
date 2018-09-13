using System.Collections.Generic;
using Newtonsoft.Json;

namespace RadioPlus.Json
{
    public static class Serialize
    {
        public static string ToJson(this Dictionary<string, RadioPlusAppData> self) => JsonConvert.SerializeObject(self, RadioPlus.Json.Converter.Settings);
    }
}