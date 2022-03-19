using ArtOfTime;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Converters;
using ArtOfTime.Models;
using ArtOfTime.Interfaces;

namespace ArtOfTime.Services
{
    /// <summary>
    /// Core Api Requests Provider
    /// </summary>
    public class ApiProvider : IApiProvider
    {
        private readonly JsonSerializerSettings serializerSettings;

        /// <summary>
        /// Core Api Request Provider
        /// </summary>
        public ApiProvider()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                NullValueHandling = NullValueHandling.Ignore
            };
            serializerSettings.Converters.Add(new StringEnumConverter());
        }

        /// <summary>
        /// Perform asynchronous Get request
        /// </summary>
        /// <typeparam name="TResult">Type of result</typeparam>
        /// <param name="uri">endpoint</param>
        /// <param name="uriParams">uri injected params</param>
        /// <param name="queryParams">query paams</param>
        /// <param name="token">bearer token</param>
        /// <returns></returns>
        public async Task<TResult> GetAsync<TResult>(string uri, object[] uriParams, Dictionary<string, object> queryParams, string token = "")
        {
            try
            {
                using (HttpClient httpClient = CreateHttpClient(token))
                {
                    var urlWithQueryParams = PrepareEndpoint(uri, uriParams, queryParams);

                    var request = new HttpRequestMessage(HttpMethod.Get, urlWithQueryParams);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await httpClient.SendAsync(request);
                    await HandleResponse(response);
                    string serialized = await response.Content.ReadAsStringAsync();

                    TResult result = JsonConvert.DeserializeObject<TResult>(serialized, serializerSettings);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Prepare URL
        /// </summary>
        /// <param name="uri">endpoint</param>
        /// <param name="uriParams">uri injected params</param>
        /// <param name="queryParams">query params</param>
        /// <returns></returns>
        private string PrepareEndpoint(string uri, object[] uriParams, Dictionary<string, object> queryParams)
        {
            if (uri.Contains("{0}") && (uriParams == null || uriParams.Length == 0))
                throw new ArgumentException("uri expects uriParams to be set");

            StringBuilder uriBuilder = new StringBuilder();

            //Do we have any formatting in endpoint
            if (uri.Contains("{0}"))
            {
                uriBuilder.Append(string.Format(uri, uriParams));
            }
            else
            {
                uriBuilder.Append(uri);
            }

            if (queryParams != null)
            {
                string delim = "?";
                foreach (var key in queryParams.Keys)
                {
                    if (!uriBuilder.ToString().Contains("directions") && !uriBuilder.ToString().Contains("distancematrix"))
                    {
                        uriBuilder.Append($"{delim}{key}={HttpUtility.UrlEncode(queryParams[key].ToString())}");
                    }
                    else
                    {
                        uriBuilder.Append($"{delim}{key}={queryParams[key].ToString()}");
                    }
                    //uriBuilder.Append($"{delim}{key}={HttpUtility.UrlEncode(queryParams[key].ToString())}");
                    delim = "&";
                }
            }

            return uriBuilder.ToString();
        }

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
        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, object[] uriParams, TRequest data, string token = "", string header = "")
        {
            try
            {
                using (HttpClient httpClient = CreateHttpClient(token))
                {
                    var urlWithUriParams = PrepareEndpoint(uri, uriParams, null);

                    var request = new HttpRequestMessage(HttpMethod.Post, urlWithUriParams);

                    if (!string.IsNullOrEmpty(header))
                    {
                        AddHeaderParameter(httpClient, header);
                    }

                    var jsonData = JsonConvert.SerializeObject(data, serializerSettings);
                    request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    await HandleResponse(response);

                    string serialized = await response.Content.ReadAsStringAsync();

                    TResult result = JsonConvert.DeserializeObject<TResult>(serialized, serializerSettings);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Perfor asyncrhonous Post request
        /// </summary>
        /// <typeparam name="TRequest">Request Model Type</typeparam>
        /// <typeparam name="TResult">Result Model Type</typeparam>
        /// <param name="uri">Endpoint Uri</param>
        /// <param name="data">Request Data</param>
        /// <param name="token">Access Bearer Token</param>
        /// <param name="header">Additional Header Parameter With Unique GUID</param>
        /// <returns>Result Data</returns>
        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "", string header = "")

        {
            try
            {
                using (HttpClient httpClient = CreateHttpClient(token))
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, uri);

                    if (!string.IsNullOrEmpty(header))
                    {
                        AddCustomHeader(httpClient, header);
                        // This is line commented because it is adding guid to every header that is created
                        //AddHeaderParameter(httpClient, header);
                    }

                    var jsonData = JsonConvert.SerializeObject(data, serializerSettings);
                    request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    await HandleResponse(response);

                    string serialized = await response.Content.ReadAsStringAsync();

                    TResult result = JsonConvert.DeserializeObject<TResult>(serialized, serializerSettings);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Perfor asyncrhonous Post request
        /// </summary>
        /// <typeparam name="TResult">Result Model Type</typeparam>
        /// <param name="uri">Endpoint Uri</param>
        /// <param name="uriParams">uri injected params</param>
        /// <param name="token">Access Bearer Token</param>
        /// <param name="header">Additional Header Parameter With Unique GUID</param>
        /// <returns>Result Data</returns>
        public async Task<TResult> PostAsync<TResult>(string uri, object[] uriParams, string token = "", string header = "")
        {
            try
            {
                using (HttpClient httpClient = CreateHttpClient(token))
                {
                    var urlWithUriParams = PrepareEndpoint(uri, uriParams, null);

                    var request = new HttpRequestMessage(HttpMethod.Post, urlWithUriParams);

                    if (!string.IsNullOrEmpty(header))
                    {
                        AddHeaderParameter(httpClient, header);
                    }

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    await HandleResponse(response);

                    string serialized = await response.Content.ReadAsStringAsync();

                    TResult result = JsonConvert.DeserializeObject<TResult>(serialized, serializerSettings);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

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
        public async Task<TResult> PostAsync<TResult>(string uri, object[] uriParams, Dictionary<string, object> queryParams, string token = "", string header = "")
        {
            try
            {
                using (HttpClient httpClient = CreateHttpClient(token))
                {
                    var urlWithUriParams = PrepareEndpoint(uri, uriParams, queryParams);

                    var request = new HttpRequestMessage(HttpMethod.Post, urlWithUriParams);

                    if (!string.IsNullOrEmpty(header))
                    {
                        AddHeaderParameter(httpClient, header);
                    }

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    await HandleResponse(response);

                    string serialized = await response.Content.ReadAsStringAsync();

                    TResult result = JsonConvert.DeserializeObject<TResult>(serialized, serializerSettings);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Perfor asyncrhonous Post request
        /// </summary>
        /// <typeparam name="TResult">Result Model Type</typeparam>
        /// <param name="uri">Endpoint Uri</param>
        /// <param name="token">Access Bearer Token</param>
        /// <param name="header">Additional Header Parameter With Unique GUID</param>
        /// <returns>Result Data</returns>
        public async Task<TResult> PostAsync<TResult>(string uri, string token = "", string header = "")
        {
            try
            {
                using (HttpClient httpClient = CreateHttpClient(token))
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, uri);

                    if (!string.IsNullOrEmpty(header))
                    {
                        AddHeaderParameter(httpClient, header);
                    }

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    await HandleResponse(response);

                    string serialized = await response.Content.ReadAsStringAsync();

                    TResult result = JsonConvert.DeserializeObject<TResult>(serialized, serializerSettings);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

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
        public async Task<TResult> PutAsync<TRequest, TResult>(string uri, object[] uriParams, TRequest data, string token = "", string header = "")
        {
            try
            {
                using (HttpClient httpClient = CreateHttpClient(token))
                {
                    var urlWithUriParams = PrepareEndpoint(uri, uriParams, null);

                    var request = new HttpRequestMessage(HttpMethod.Put, urlWithUriParams);

                    if (!string.IsNullOrEmpty(header))
                    {
                        AddHeaderParameter(httpClient, header);
                    }

                    var jsonData = JsonConvert.SerializeObject(data, serializerSettings);
                    request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    await HandleResponse(response);

                    string serialized = await response.Content.ReadAsStringAsync();

                    TResult result = JsonConvert.DeserializeObject<TResult>(serialized, serializerSettings);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Perform asynchronous Put Request
        /// </summary>
        /// <typeparam name="TRequest">Request Model Type</typeparam>
        /// <typeparam name="TResult">Result Model Type</typeparam>
        /// <param name="uri">Endpoint Uri</param>
        /// <param name="data">Data to persist</param>
        /// <param name="token">Access Bearer Token</param>
        /// <param name="header">Additional Header Parameter With Unique GUID</param>
        /// <returns>Result Data</returns>
        public async Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data, string token = "", string header = "")
        {
            try
            {
                using (HttpClient httpClient = CreateHttpClient(token))
                {
                    var request = new HttpRequestMessage(HttpMethod.Put, uri);

                    if (!string.IsNullOrEmpty(header))
                    {
                        AddHeaderParameter(httpClient, header);
                    }

                    var jsonData = JsonConvert.SerializeObject(data, serializerSettings);
                    request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    await HandleResponse(response);

                    string serialized = await response.Content.ReadAsStringAsync();

                    TResult result = JsonConvert.DeserializeObject<TResult>(serialized, serializerSettings);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Instantiate HttpClient
        /// </summary>
        /// <param name="token">Access Bearere Token</param>
        /// <returns>Created Instance</returns>
        private HttpClient CreateHttpClient(string token = "")
        {
            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return httpClient;
        }

        /// <summary>
        /// Add Unique Header Parameter With Unique GUID 
        /// </summary>
        /// <param name="httpClient">HttpClient object</param>
        /// <param name="parameter">Header Parameter Name</param>
        private void AddHeaderParameter(HttpClient httpClient, string parameter)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrEmpty(parameter))
                return;

            httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
        }

        private void AddCustomHeader(HttpClient httpClient, string parameter)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrEmpty(parameter))
                return;

            var arr = parameter.Split(": ", StringSplitOptions.RemoveEmptyEntries).ToArray();
            httpClient.DefaultRequestHeaders.Add(arr[0], arr[1]);
        }

        /// <summary>
        /// Handle Response Success or Failiure
        /// </summary>
        /// <param name="response">Response to handle</param>
        /// <returns></returns>
        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception("No access");
                }

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var validationData = JsonConvert.DeserializeObject<ValidationFailuresResult>(content, serializerSettings);
                    throw new Exception("Bad Request");
                }

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var unsuccessfulData = JsonConvert.DeserializeObject<ServerErrorResult>(content, serializerSettings);
                    throw new Exception(unsuccessfulData.Title);
                }

                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}

