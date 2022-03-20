using ArtOfTime.Interfaces;
using ArtOfTime.Models.Images;
using System;
using System.Threading.Tasks;

namespace ArtOfTime.Services
{
    public class ImageGeneratorService : IImageGeneratorService
    {
        // TODO: update - not valid at the moment
        // add versions for if debug if prod
        private const string URL = "http://0772-2001-67c-20d0-aac-9d0f-48af-6ed4-ad13.ngrok.io/generate";

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
            // the python script will need at least 30minutes to generate new image
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
