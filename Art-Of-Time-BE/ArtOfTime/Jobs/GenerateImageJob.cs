using System.IO;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;

namespace ArtOfTime.Jobs
{
    public class GenerateImageJob
    {

        [AutomaticRetry(Attempts = 0)]
        public async Task Test(PerformContext hangfire)
        {
            string[] lines =
            {
                "First line", "Second line", "Third line"
            };

            await File.WriteAllLinesAsync("WriteLines.txt", lines);
        }
    }
}
