namespace ArtOfTime.Interfaces
{
    using System.Threading.Tasks;

    public interface IEthereumService
    {
        Task CreateToken(string tokenURI);

        Task<int> GetCollectionSize();
    }
}
