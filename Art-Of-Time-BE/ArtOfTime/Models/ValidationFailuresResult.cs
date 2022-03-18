using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ArtOfTime.Models
{
    public class ValidationFailuresResult
    {
        [JsonProperty("errors")]
        public IDictionary<string, string[]> Failures { get; set; }

        [JsonProperty("type")]
        public Uri Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
