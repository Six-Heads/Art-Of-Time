using ArtOfTime.Interfaces;
using ArtOfTime.Models.Twitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtOfTime.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly IApiProvider apiProvider;

        public TwitterService(IApiProvider apiProvider)
        {
            this.apiProvider = apiProvider ?? throw new ArgumentNullException(nameof(apiProvider));
        }

        public async Task<TrendsListModel> GetTrends()
        {
            string url = "https://api.twitter.com/1.1/trends/place.json";
            string apiKey = "PsPBffpiyAvN3IWVmkOAiylhc";
            string apiKeySecret = "GShwG86aQ89DUPEziO8ZBNUyBZIaa2SV2dBBy6G2H7uekJGL6X";
            string bearerToken = "AAAAAAAAAAAAAAAAAAAAABB%2FaQEAAAAAaX92XCqT7U0uhALKrc75jHFm4mk%3D1el8kPiOkQvXjUtRwqBnYE7FYNFDx5qGSxzgeR2PaBQ5wqd20T";
            string accessToken = "1504860291733020672-LJnenkweY9ahpnUodHA3I7gdGmMnsQ";
            string accessTokenSecret = "y10SCfacOfWtsznPcsLzstsuMm0BRaMT0fHU7i7xbwhJ0";

            object[] uriParams = new object[]
            {
                apiKey,
                apiKeySecret,
                accessToken,
                accessTokenSecret
            };

            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            queryParams.Add("id", 1);

            try
            {
            var result = await apiProvider.GetAsync<TrendsListModel>(url, uriParams, queryParams, bearerToken);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }


    }
}
