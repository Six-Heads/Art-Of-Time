using ArtOfTime.Helpers;
using ArtOfTime.Interfaces;
using ArtOfTime.Models.Twitter;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtOfTime.Services
{
    public class TwitterService : ITwitterService
    {
        private readonly IApiProvider apiProvider;
        private readonly IConfiguration configuration;

        public TwitterService(IApiProvider apiProvider, IConfiguration configuration)
        {
            this.apiProvider = apiProvider;
            this.configuration = configuration;
        }

        public async Task<List<string>> GetTrends()
        {
            string url = this.configuration["TwitterApi:GetTrendsUrl"];
            string apiKey = this.configuration["TwitterApi:ApiKey"];
            string apiKeySecret = this.configuration["TwitterApi:ApiKeySecret"];
            string bearerToken = this.configuration["TwitterApi:BearerToken"];
            string accessToken = this.configuration["TwitterApi:AccessToken"];
            string accessTokenSecret = this.configuration["TwitterApi:AccessTokenSecret"];
            string id = this.configuration["TwitterApi:CountryWoeid"];

            object[] uriParams = new object[]
            {
                apiKey,
                apiKeySecret,
                accessToken,
                accessTokenSecret
            };

            var queryParams = new Dictionary<string, object>
            {
                { "id", id }
            };

            try
            {
                var result = await apiProvider.GetAsync<List<TrendsListModel>>(url, uriParams, queryParams, bearerToken);
                var helper = new TwitterDataProcessingHelper();

                return await helper.ExtractText(result[0].Trends);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
