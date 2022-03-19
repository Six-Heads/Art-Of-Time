using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtOfTime.Interfaces
{
    public interface IApiProvider
    {
        /// <summary>
        /// Perform asynchronous Get request
        /// </summary>
        /// <typeparam name="TResult">Result Model Type</typeparam>
        /// <param name="uri">>Universal Resource Identifier</param>
        /// <param name="uriParams">URI Parameters</param>
        /// <param name="queryParams">Query String Parameters</param>
        /// <param name="token">Access Bearer Token</param>
        /// <returns></returns>
        Task<TResult> GetAsync<TResult>(string uri, object[] uriParams, Dictionary<string, object> queryParams, string token = "");
       
        Task<TResult> PostAsyncInstantTimeout<TRequest, TResult>(string uri, object[] uriParams, TRequest data, string token = "", string header = "")


        /// <summary>
        /// Perfor asyncrhonous Post request
        /// </summary>
        /// <typeparam name="TRequest">Request Model Type</typeparam>
        /// <typeparam name="TResult">Result Model Type</typeparam>
        /// <param name="uri">Endpoint Uri</param>
        /// <param name="uriParams">uri injected params</param>
        /// <param name="data">Request Data</param>
        /// <param name="token">Access Bearer Token</param>
        /// <param name="header">Additional Header Parameter With Unique GUID</param>
        /// <returns>Result Data</returns>
        Task<TResult> PostAsync<TRequest, TResult>(string uri, object[] uriParams, TRequest data, string token = "", string header = "");


        /// <summary>
        /// Perfor asyncrhonous Post request
        /// </summary>
        /// <typeparam name="TRequest">Request Model Type</typeparam>
        /// <typeparam name="TResult">Result Model Type</typeparam>
        /// <param name="uri">Universal Resource Identifier</param>
        /// <param name="data">Request Data</param>
        /// <param name="token">Access Bearer Token</param>
        /// <param name="header">Additional Header Parameter With Unique GUID</param>
        /// <returns>Result Data</returns>
        Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "", string header = "");


        /// <summary>
        /// Perfor asyncrhonous Post request
        /// </summary>
        /// <typeparam name="TResult">Result Model Type</typeparam>
        /// <param name="uri">Endpoint Uri</param>
        /// <param name="uriParams">uri injected params</param>
        /// <param name="token">Access Bearer Token</param>
        /// <param name="header">Additional Header Parameter With Unique GUID</param>
        /// <returns>Result Data</returns>
        Task<TResult> PostAsync<TResult>(string uri, object[] uriParams, string token = "", string header = "");


        /// <summary>
        /// Perfor asyncrhonous Post request
        /// </summary>
        /// <typeparam name="TResult">Result Model Type</typeparam>
        /// <param name="uri">Endpoint Uri</param>
        /// <param name="uriParams">uri injected params</param>
        /// <param name="queryParams">query params</param>
        /// <param name="token">Access Bearer Token</param>
        /// <param name="header">Additional Header Parameter With Unique GUID</param>
        /// <returns>Result Data</returns>
        Task<TResult> PostAsync<TResult>(string uri, object[] uriParams, Dictionary<string, object> queryParams, string token = "", string header = "");


        Task<TResult> PostAsyncMultipart<TResult>(string uri, object[] uriParams, byte[] data, string dataName, string token = "", string header = "");

        /// <summary>
        /// Perfor asyncrhonous Post request
        /// </summary>
        /// <typeparam name="TResult">Result Model Type</typeparam>
        /// <param name="uri">Universal Resource Identifier</param>
        /// <param name="token">Access Bearer Token</param>
        /// <param name="header">Additional Header Parameter With Unique GUID</param>
        /// <returns>Result Data</returns>
        Task<TResult> PostAsync<TResult>(string uri, string token = "", string header = "");
          

        /// <summary>
        /// Perform asynchronous Put Request
        /// </summary>
        /// <typeparam name="TRequest">Request Model Type</typeparam>
        /// <typeparam name="TResult">Result Model Type</typeparam>
        /// <param name="uri">Endpoint Uri</param>
        /// <param name="uriParams">uri injected params</param>
        /// <param name="data">Data to persist</param>
        /// <param name="token">Access Bearer Token</param>
        /// <param name="header">Additional Header Parameter With Unique GUID</param>
        /// <returns>Result Data</returns>
        Task<TResult> PutAsync<TRequest, TResult>(string uri, object[] uriParams, TRequest data, string token = "", string header = "");
            
    }
}