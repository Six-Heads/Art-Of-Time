using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtOfTime.Interfaces
{
    public interface ITwitterService
    {
        Task<List<string>> GetTrends();
    }
}
