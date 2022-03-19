using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtOfTime.Models.Twitter
{
    public class TrendsListModel
    {
        [JsonProperty("trends")]
        public List<TrendModel> Trends { get; set; }

        [JsonProperty("as_of")]
        public DateTime ReceivedAt { get; set; }

        [JsonProperty()]
        public DateTime CreatedOn { get; set; }

        [JsonProperty()]
        public List<TrendLocationModel> TrendLocations { get; set; }
    }
}
