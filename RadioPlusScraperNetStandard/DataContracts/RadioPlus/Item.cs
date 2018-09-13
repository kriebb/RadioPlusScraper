using System;
using Newtonsoft.Json;

namespace RadioPlusOnDemand.Json
{
    public partial class Item
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("collectionID")]
        public Guid CollectionId { get; set; }

        [JsonProperty("startTime")]
        public DateTimeOffset StartTime { get; set; }

        [JsonProperty("creationTimestamp")]
        public DateTimeOffset CreationTimestamp { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("stream")]
        public string Stream { get; set; }

        [JsonProperty("stream_aac")]
        public string StreamAac { get; set; }

        [JsonProperty("stream_hls")]
        public string StreamHls { get; set; }

        [JsonProperty("stream_dash")]
        public string StreamDash { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("thumb")]
        public string Thumb { get; set; }

        [JsonProperty("timestamp")]
        public Timestamp Timestamp { get; set; }

        [JsonProperty("isEntry")]
        public bool IsEntry { get; set; }

        [JsonProperty("className")]
        public ClassName ClassName { get; set; }
    }
}