using System;

namespace ArtOfTime.Models.Images
{
    public class GenerateImageRequestModel
    {
        public Guid ImageId { get; set; }

        public string BasedOnText { get; set; }
    }
}
