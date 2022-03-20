using ArtOfTime.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfTime.Services
{
    public class IPFSService : IIPFSService
    {

        private string authToken;
        private string uploadUri;
        private string getUri;


        private readonly IApiProvider provider;

        public IPFSService(IApiProvider provider, IConfiguration configuration)
        {
            this.provider = provider;
            this.authToken = GenerateAuthToken(configuration["IPFS:ProjectId"], configuration["IPFS:ProjectSecret"]);
            this.getUri = configuration["IPFS:UploadEndpoint"];
            this.uploadUri = configuration["IPFS:DownloadEndpoint"];

        }

        public async Task<string> UploadData(string data, string dataName)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return null;
            }

            var byteData = Encoding.UTF8.GetBytes(data);
            return await UploadData(byteData, dataName);
        }

        public async Task<string> UploadData(byte[] data, string dataName)
        {
            if (data.Length == 0 || string.IsNullOrWhiteSpace(dataName))
            {
                return null;
            }

            try
            {
                var result = await provider.PostAsyncMultipart<IPFSAddResult>(uploadUri, null, data, dataName, authToken);

                return result != null ? string.Format(getUri, result.Hash) : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GenerateAuthToken(string id, string secret)
        {
            return $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{id}:{secret}"))}";
        }
    }
}
