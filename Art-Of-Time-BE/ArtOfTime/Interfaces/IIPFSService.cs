using System.Threading.Tasks;

namespace ArtOfTime.Interfaces
{
    public interface IIPFSService
    {
        Task<string> UploadData(string data, string dataName);
        Task<string> UploadData(byte[] data, string dataName);
    }
}
