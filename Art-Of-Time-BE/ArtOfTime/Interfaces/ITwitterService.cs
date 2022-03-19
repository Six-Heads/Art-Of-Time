using ArtOfTime.Models.Twitter;
using System.Threading.Tasks;

namespace ArtOfTime.Interfaces
{
    public interface ITwitterService
    {
        Task<TrendsListModel> GetTrends();
    }
}
