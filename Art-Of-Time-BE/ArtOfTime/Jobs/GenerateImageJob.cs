using ArtOfTime.Interfaces;
using Hangfire;
using Hangfire.Server;
using System.Threading.Tasks;

namespace ArtOfTime.Jobs
{
    public class GenerateImageJob
    {
        private readonly IImageGeneratorService imageGeneratorService;
        private readonly ITwitterService twitterService;
        private readonly IIPFSService iPFSService;
        private readonly IEthereumService ethereumService;

        public GenerateImageJob(
            IImageGeneratorService imageGeneratorService,
            ITwitterService twitterService,
            IIPFSService iPFSService,
            IEthereumService ethereumService)
        {
            this.imageGeneratorService = imageGeneratorService;
            this.twitterService = twitterService;
            this.iPFSService = iPFSService;
            this.ethereumService = ethereumService;
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task Work(PerformContext hangfire)
        {
            // TODO:
            // Fetch not fetched images
            // Upload image to blockchain

            // TODO:
            // Fetch trends

            // TODO:
            // Request a new image to be generated

            string[] lines =
            {
                "First line", "Second line", "Third line"
            };
        }
    }
}
