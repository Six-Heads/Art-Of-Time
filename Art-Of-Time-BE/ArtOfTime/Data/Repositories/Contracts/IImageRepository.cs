using ArtOfTime.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtOfTime.Data.Repositories.Contracts
{
    public interface IImageRepository
    {
        Task<Image> GetImageByTimeStamp(string timeStamp); 
        Task<List<Image>> GetNotFetchedImages();
        Task CreateImage(Image image);
        Task UpdateImage(Image image);
    }
}
