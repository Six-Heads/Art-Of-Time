namespace ArtOfTime.Interfaces
{
    using System.Threading.Tasks;

    using Nethereum.Contracts;

    public interface IEthereumService
    {
        Task CreateToken(string tokenURI);

        Task<int> GetCollectionSize();
    }
}
