using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtOfTime.Interfaces
{
    public interface IApiProvider
    {
        Task<TResult> GetAsync<TResult>(string uri, object[] uriParams, Dictionary<string, object> queryParams, string token = "");

        Task<TResult> PostAsyncInstantTimeout<TRequest, TResult>(string uri, object[] uriParams, TRequest data, string token = "", string header = "");

        Task<TResult> PostAsyncMultipart<TResult>(string uri, object[] uriParams, byte[] data, string dataName, string token = "", string header = "");
    }
}