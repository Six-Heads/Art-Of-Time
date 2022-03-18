using Newtonsoft.Json;
using System;

namespace ArtOfTime.Models
{
    public class ServerErrorResult
    {
        [JsonProperty("type")]
        public Uri Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }
    }
}
