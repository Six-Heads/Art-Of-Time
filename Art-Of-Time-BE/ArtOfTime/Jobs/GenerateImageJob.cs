using System;
using System.IO;
using System.Threading.Tasks;

namespace ArtOfTime.Jobs
{
    public class GenerateImageJob
    {

        public async Task Test()
        {
            string[] lines =
            {
                "First line", "Second line", "Third line"
            };

            await File.WriteAllLinesAsync("WriteLines.txt", lines);
        }
    }
}
