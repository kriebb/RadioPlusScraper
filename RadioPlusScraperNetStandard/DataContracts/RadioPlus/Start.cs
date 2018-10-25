using System;
using Newtonsoft.Json;

namespace RadioPlusOnDemand.Json
{
    public class Start
    {
        [JsonProperty("date")] public DateTimeOffset Date { get; set; }

        [JsonProperty("hourRound")] public HourRound HourRound { get; set; }

        [JsonProperty("hour")] public string Hour { get; set; }

        [JsonProperty("endHour", NullValueHandling = NullValueHandling.Ignore)]
        public string EndHour { get; set; }

        [JsonProperty("day")] public string Day { get; set; }
    }
}