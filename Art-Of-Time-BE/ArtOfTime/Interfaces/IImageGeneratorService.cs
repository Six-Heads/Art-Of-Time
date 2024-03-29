﻿using ArtOfTime.Models.Images;
using System.Threading.Tasks;

namespace ArtOfTime.Interfaces
{
    public interface IImageGeneratorService
    {
        Task GenerateImage(GenerateImageRequestModel requestModel);

        Task<byte[]> GetGeneratedImage(string timestamp);
    }
}
