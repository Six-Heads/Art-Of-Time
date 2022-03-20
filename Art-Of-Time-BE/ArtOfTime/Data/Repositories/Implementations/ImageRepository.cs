using ArtOfTime.Data.Entities;
using ArtOfTime.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtOfTime.Data.Repositories.Implementations
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ImageRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateImage(Image image)
        {
            await dbContext.AddAsync(image);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Image> GetImageByTimeStamp(string timeStamp)
        {
            return await dbContext
                .Images
                .FindAsync(timeStamp);
        }

        public async Task<List<Image>> GetNotFetchedImages()
        {
            return await dbContext
                .Images
                .Where(x => !x.IsFetched)
                .ToListAsync();
        }

        public async Task<List<Image>> GetNotUploadedImages()
        {
            return await dbContext
                .Images
                .Where(x => x.IsFetched && !x.IsUploadedImage)
                .ToListAsync();
        }


        public async Task<List<Image>> GetNotUploadedJson()
        {
            return await dbContext
               .Images
               .Where(x => x.IsFetched && x.IsUploadedImage && !x.IsUploadedJson)
               .ToListAsync();
        }

        public async Task<List<Image>> GetNotMinted()
        {
            return await dbContext
               .Images
               .Where(x => x.IsFetched && x.IsUploadedImage && x.IsUploadedJson && !x.IsMinted)
               .ToListAsync();
        }

        public async Task UpdateImage(Image image)
        {
            dbContext.Update(image);
            await dbContext.SaveChangesAsync();
        }
    }
}
