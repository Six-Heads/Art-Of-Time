using Newtonsoft.Json;

namespace ArtOfTime.Models.Twitter
{
    public class TrendLocationModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("woeid")]
        public int Woeid { get; set; }
    }
}
