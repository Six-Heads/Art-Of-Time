using ArtOfTime.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtOfTime.Services
{
    public class TwitterService
    {
        private readonly IApiProvider apiProvider;

        public TwitterService(IApiProvider apiProvider)
        {
            this.apiProvider = apiProvider ?? throw new ArgumentNullException(nameof(apiProvider));
        }

        public async Task<IAsyncResult> GetTrends()
        {
            string url = "https://api.twitter.com/1.1/trends/place.json?id=1";
            string apiKey = "PsPBffpiyAvN3IWVmkOAiylhc";
            string apiKeySecret = "GShwG86aQ89DUPEziO8ZBNUyBZIaa2SV2dBBy6G2H7uekJGL6X";
            string bearerToken = "AAAAAAAAAAAAAAAAAAAAABB%2FaQEAAAAAOxI9lezK6PrHLE2elqg83PyD62Y%3Daa3EtFVX3BaJ9zVJPKsVSxKQYFyeiOjXbWYmxWZP0lwSJC6Rou";
            string accessToken = "1504860291733020672-LJnenkweY9ahpnUodHA3I7gdGmMnsQ";
            string accessTokenSecret = "y10SCfacOfWtsznPcsLzstsuMm0BRaMT0fHU7i7xbwhJ0";

            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            queryParams.Add("consumer_key", apiKey);
            queryParams.Add("consumer_secret", apiKeySecret);
            queryParams.Add("access_token", accessToken);
            queryParams.Add("token_secret", accessTokenSecret);
            //queryParams.Add("bearer_token", bearerToken);

            //var result = await apiProvider.GetAsync<>(url, null, queryParams, bearerToken);
            return null;
        }


    }
}
