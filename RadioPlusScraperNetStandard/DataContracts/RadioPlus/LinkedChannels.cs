﻿using Newtonsoft.Json;

namespace RadioPlus.Json
{
    public partial class LinkedChannels
    {
        [JsonProperty("current")]
        public string Current { get; set; }

        [JsonProperty("channels")]
        [JsonConverter(typeof(DecodeArrayConverter))]
        public long[] Channels { get; set; }
    }
}