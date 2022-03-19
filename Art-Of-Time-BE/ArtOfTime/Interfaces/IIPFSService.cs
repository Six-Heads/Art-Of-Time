using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtOfTime.Interfaces
{
    public interface IIPFSService
    {
        public Task<string> Upload(byte[] image, string timestamp, List<string> attributes);
    }
}
