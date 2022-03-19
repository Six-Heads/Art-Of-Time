using ArtOfTime.Interfaces;
using ArtOfTime.Models.Images;
using System;
using System.Threading.Tasks;

namespace ArtOfTime.Services
{
    public class ImageGeneratorService : IImageGeneratorService
    {
        // TODO: update - not valid at the moment
        private const string URL = "http://c428-2001-67c-20d0-aac-19ba-6030-2eeb-d7e3.ngrok.io/generate";

        private readonly IApiProvider apiProvider;

        public ImageGeneratorService(IApiProvider apiProvider)
        {
            this.apiProvider = apiProvider;
        }

        /// <summary>
        /// Send request to the python server to start generating new image
        /// with the given imageId and text
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public async Task GenerateImage(GenerateImageRequestModel requestModel)
        {
            // we dont need the result at the moment
            // the python script will need atleast 30minutes to generate new image
            // TODO: maybe remove await
            await apiProvider.PostAsyncInstantTimeout<GenerateImageRequestModel, object>(URL, null, requestModel);   
        }

        /// <summary>
        /// Get generated image by id
        /// </summary>
        /// <param name="imageId">Id of the image we want</param>
        /// <returns>The image as byte array</returns>
        public async Task<byte[]> GetGeneratedImage(string imageId)
        {
            var response = await apiProvider.GetAsync<GeneratedImageResponseModel>(URL + $"/{imageId}", null, null);

            return !string.IsNullOrWhiteSpace(response.ImageBase64) ? Convert.FromBase64String(response.ImageBase64) : null; 
        }
    }
}
