using ArtOfTime.Data.Entities;
using ArtOfTime.Data.Repositories.Contracts;
using ArtOfTime.Helpers;
using ArtOfTime.Interfaces;
using ArtOfTime.Models.Images;
using Hangfire;
using Hangfire.Server;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ArtOfTime.Jobs
{
    public class GenerateImageJob
    {
        private readonly IImageRepository imageRepository;
        private readonly IImageGeneratorService imageGeneratorService;
        private readonly ITwitterService twitterService;
        private readonly IIPFSService iPFSService;
        private readonly IEthereumService ethereumService;

        public GenerateImageJob(
            IImageRepository imageRepository,
            IImageGeneratorService imageGeneratorService,
            ITwitterService twitterService,
            IIPFSService iPFSService,
            IEthereumService ethereumService)
        {
            this.imageRepository = imageRepository;
            this.imageGeneratorService = imageGeneratorService;
            this.twitterService = twitterService;
            this.iPFSService = iPFSService;
            this.ethereumService = ethereumService;
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task Work(PerformContext hangfire)
        {
            // Fetch not fetched images
            var notFetchedImages = await imageRepository.GetNotFetchedImages();

            foreach (var image in notFetchedImages)
            {
                try
                {
                    var result = await imageGeneratorService.GetGeneratedImage(image.TimeStamp);

                    if (result != null)
                    {
                        var jsonUrl = await iPFSService.Upload(result, image.TimeStamp, image.BasedOnText.Split(", ").ToList());
                        await ethereumService.CreateToken(jsonUrl);
                        image.IsFetched = true;
                        await imageRepository.UpdateImage(image);
                    }
                }
                catch (Exception ex)
                {
                    // TODO: Handle
                }
            }

            try
            {
                // Get trends
                var twitterTrends = await twitterService.GetTrends();

                // Create new image entity
                var newImage = new Image
                {
                    TimeStamp = CommonHelper.GetCurrentTimestamp().ToString(),
                    BasedOnText = string.Join(", ", twitterTrends),
                    IsFetched = false,
                };

                // Save new image entity to db
                await imageRepository.CreateImage(newImage);

                // Send request to generate new image
                await imageGeneratorService.GenerateImage(new GenerateImageRequestModel
                {
                    ImageId = newImage.TimeStamp,
                    BasedOnText = newImage.BasedOnText
                });
            }
            catch (Exception ex)
            {
                // TODO: handle
            }

        }
    }
}
