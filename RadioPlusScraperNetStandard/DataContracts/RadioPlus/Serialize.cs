using System.Collections.Generic;
using Newtonsoft.Json;

namespace RadioPlus.Json
{
    public static class Serialize
    {
        public static string ToJson(this Dictionary<string, RadioPlusAppData> self)
        {
            return JsonConvert.SerializeObject(self, Converter.Settings);
        }
    }
}