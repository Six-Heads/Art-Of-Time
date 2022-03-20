using ArtOfTime.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ArtOfTime.Services
{
    public class ApiProvider : IApiProvider
    {
        private readonly JsonSerializerSettings serializerSettings;

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


        public async Task<TResult> PostAsyncInstantTimeout<TRequest,TResult>(string uri, object[] uriParams, TRequest data, string token = "", string header = "")
        {
            try
            {
                using (HttpClient httpClient = CreateHttpClient(token))
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(1);
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

                    HandleResponse(response);

                    string serialized = await response.Content.ReadAsStringAsync();

                    TResult result = JsonConvert.DeserializeObject<TResult>(serialized, serializerSettings);

                    return result;
                }
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException)
                {
                    return default;
                }

                throw new Exception(ex.Message);
            }
        }

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
                    HandleResponse(response);
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

        private string PrepareEndpoint(string uri, object[] uriParams, Dictionary<string, object> queryParams)
        {
            if (uri.Contains("{0}") && (uriParams == null || uriParams.Length == 0))
                throw new ArgumentException("uri expects uriParams to be set");

            StringBuilder uriBuilder = new StringBuilder();

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

                    delim = "&";
                }
            }

            return uriBuilder.ToString();
        }

        public async Task<TResult> PostAsyncMultipart<TResult>(string uri, object[] uriParams, byte[] data, string dataName, string token = "", string header = "")
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

                    var multipartContent = new MultipartFormDataContent();
                    var byteData = new ByteArrayContent(data); 
                    multipartContent.Add(byteData, dataName, dataName);
                    request.Content = multipartContent;

                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    HandleResponse(response);

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

        private HttpClient CreateHttpClient(string token = "")
        {
            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                if (token.StartsWith("Basic"))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token.Split()[1]);
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return httpClient;
        }

        private void AddHeaderParameter(HttpClient httpClient, string parameter)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrEmpty(parameter))
                return;

            httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
        }

        private void HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}

