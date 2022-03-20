using ArtOfTime.Data.Entities;
using ArtOfTime.Data.Repositories.Contracts;
using ArtOfTime.Helpers;
using ArtOfTime.Interfaces;
using ArtOfTime.Models.Images;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GenerateImageJob> logger;

        public GenerateImageJob(
            IImageRepository imageRepository,
            IImageGeneratorService imageGeneratorService,
            ITwitterService twitterService,
            IIPFSService iPFSService,
            IEthereumService ethereumService,
            ILogger<GenerateImageJob> logger)
        {
            this.imageRepository = imageRepository;
            this.imageGeneratorService = imageGeneratorService;
            this.twitterService = twitterService;
            this.iPFSService = iPFSService;
            this.ethereumService = ethereumService;
            this.logger = logger;
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task Work(PerformContext hangfire)
        {
            await RequestImageGeneration();
            await FetchGeneratedImages();
            await UploadImages();
            await UploadJsonMetadata();
            await MintNFTs();
        }

        private async Task RequestImageGeneration()
        {
            // Get new trends and request image generation
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
                    IsUploadedImage = false,
                    IsUploadedJson = false,
                    IsMinted = false,
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
                logger.LogError(ex.Message);
            }
        }


        private async Task FetchGeneratedImages()
        {
            // Fetch not fetched images
            var notFetchedImages = await imageRepository.GetNotFetchedImages();

            foreach (var image in notFetchedImages)
            {
                try
                {
                    // fetch image from python api
                    var result = await imageGeneratorService.GetGeneratedImage(image.TimeStamp);

                    if (result != null)
                    {
                        image.IsFetched = true;
                        image.ImageBase64 = result;

                        // update db
                        await imageRepository.UpdateImage(image);
                    }
                    else
                    {
                        logger.LogInformation($"Failed fetching image from Generator: {image.TimeStamp}");
                    }

                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                }
            }
        }

        private async Task UploadImages()
        {
            // Upload not uploaded images
            var notUploadedImages = await imageRepository.GetNotUploadedImages();

            foreach (var image in notUploadedImages)
            {
                try
                {
                    // upload image to ipfs server
                    var imageUrl = await iPFSService.UploadData(image.ImageBase64, image.UidFilename + ".png");

                    if (imageUrl != null)
                    {
                        image.ImageBase64 = null;
                        image.IsUploadedImage = true;
                        image.ImageUrl = imageUrl;

                        // update db
                        await imageRepository.UpdateImage(image);
                    }
                    else
                    {
                        logger.LogInformation($"Failed uploading image to IPFS: {image.TimeStamp}");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                }
            }
        }

        private async Task UploadJsonMetadata()
        {
            // Upload not uploaded jsons
            var notUploadedImages = await imageRepository.GetNotUploadedJson();

            foreach (var image in notUploadedImages)
            {
                try
                {
                    // Request current collection size to determine the next NFT id
                    var id = await ethereumService.GetCollectionSize() + 1;

                    var jsonMetadata = MetadataHelper.GenerateJsonMetadata(
                            id.ToString(),
                            image.ImageUrl,
                            image.TimeStamp,
                            image.BasedOnText.Split(", ").ToList());

                    // upload json to ipfs server
                    var jsonUrl = await iPFSService.UploadData(jsonMetadata, image.UidFilename + ".json");

                    if (jsonUrl != null)
                    {
                        image.JsonUrl = jsonUrl;
                        image.IsUploadedJson = true;

                        // update db
                        await imageRepository.UpdateImage(image);
                    }
                    else
                    {
                        logger.LogInformation($"Failed uploading json to IPFS: {image.TimeStamp}");
                    }

                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                }
            }
        }


        private async Task MintNFTs()
        {
            var notMintedNFTs = await imageRepository.GetNotMinted();

            foreach (var nft in notMintedNFTs)
            {
                try
                {
                    //write nft to the blockchain
                    await ethereumService.CreateToken(nft.JsonUrl);

                    nft.IsMinted = true;
                    await imageRepository.UpdateImage(nft);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                }
            }
        }
    }
}
