using ArtOfTime.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfTime.Services
{
    public class IPFSService : IIPFSService
    {
        private const string uploadUri = "https://ipfs.infura.io:5001/api/v0/add";
        private const string getUri = "https://ipfs.infura.io:5001/api/v0/cat?arg={0}";
        private string authToken;

        private readonly IApiProvider provider;

        public IPFSService(IApiProvider provider, IConfiguration configuration /*, ISCProvider*/)
        {
            this.provider = provider;
            this.authToken = GenerateAuthToken(configuration["IPFS:ProjectId"], configuration["IPFS:Project:ProjectSecret"]);
        }

        public async Task<string> Upload(byte[] image, string timestamp, List<string> attributes)
        {
            var imageUri = await UploadData(image);

            var id = ""; //SCProvider

            var json = GenerateJsonMetadata(id, imageUri, timestamp, attributes);
            var jsonBytes = Encoding.UTF8.GetBytes(json);

            return await UploadData(jsonBytes);
        }

        private async Task<string> UploadData(byte[] data)
        {
            var result = await provider.PostAsync<byte[], IPFSAddResult>(uploadUri,
                                                                         null,
                                                                         data,
                                                                         authToken);

            return string.Format(getUri, result.Hash);

        }

        private string GenerateJsonMetadata(string id, string imageUri, string timestamp, List<string> twitterAttributes)
        {
            return
$@"{{
    ""name"": ""ArtOfTime"",
    ""description"": ""ArtOfTime#{id}"",
    ""image"": ""{imageUri}""
    ""attributes"": [
        {{
                ""value"": ""{ twitterAttributes[0]}""
        }},
        {{
                ""value"": ""{ twitterAttributes[1]}""
        }},
        {{
                ""value"": ""{ twitterAttributes[2]}""
        }},
        {{
                ""value"": ""{ twitterAttributes[3]}""
        }},
        {{
                ""value"": ""{ twitterAttributes[4]}""
        }},
        {{
            ""display_type"": ""date"",
            ""trait_type"": ""birthday"", 
            ""value"": {timestamp}
        }}
    ]
}}";

        }

        private string GenerateAuthToken(string id, string secret)
        {
            return $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{id}:{secret}"))}";
        }
    }
}
