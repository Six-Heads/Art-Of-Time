using ArtOfTime.Interfaces;
using ArtOfTime.Models.Images;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtOfTime.Services
{
    public class ImageGeneratorService : IImageGeneratorService
    {
        // TODO: update - not valid at the moment
        private const string URL = "http://localhost:5000/";

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
            await apiProvider.PostAsync<GenerateImageRequestModel, object>(URL, null, requestModel);   
        }

        /// <summary>
        /// Get generated image by id
        /// </summary>
        /// <param name="imageId">Id of the image we want</param>
        /// <returns>The image as byte array</returns>
        public async Task<byte[]> GetGeneratedImage(string imageId)
        {
            return await apiProvider.GetAsync<byte[]>(URL, new object[] { imageId }, null);
        }
    }
}
