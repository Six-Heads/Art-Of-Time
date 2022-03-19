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
            this.authToken = GenerateAuthToken(configuration["IPFS:ProjectId"], configuration["IPFS:ProjectSecret"]);
        }

        public async Task<string> Upload(byte[] image, string timestamp, List<string> attributes)
        {
            var uid = Guid.NewGuid().ToString();
            var imageName = uid + ".png";
            var jsonName = uid + ".json";

            var imageUri = await UploadData(image,imageName);

            var id = ""; //SCProvider
            var json = GenerateJsonMetadata(id, imageUri, timestamp, attributes);

            return await UploadData(Encoding.UTF8.GetBytes(json), jsonName);
        }

        private async Task<string> UploadData(byte[] data,string dataName)
        {
            var result = await provider.PostAsyncMultipart<IPFSAddResult>(uploadUri, null, data, dataName, authToken);

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
