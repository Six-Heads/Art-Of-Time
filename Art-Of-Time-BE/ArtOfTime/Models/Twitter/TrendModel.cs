using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtOfTime.Models.Twitter
{
    public class TrendModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string PromotaedContent { get; set; }

        public string Query { get; set; }

        public int? TweetVolume { get; set; }
    }
}
