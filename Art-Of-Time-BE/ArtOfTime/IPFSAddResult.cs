using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtOfTime
{
    public class IPFSAddResult : ResultBase
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Hash")]
        public string Hash { get; set; }

        [JsonProperty("Size")]
        public int Size { get; set; }
    }
}
