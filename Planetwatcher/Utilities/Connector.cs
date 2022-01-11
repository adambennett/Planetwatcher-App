using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Planetwatcher.Utilities
{
    public static class Connector
    {
        private static CookieContainer   _cookies;
        private static HttpClientHandler _handler;
        private static HttpClient        _client;

        /// <summary>
        /// Send a GET request to the Planetwatcher API.
        /// </summary>
        /// <param name="endpoint">Reference a variable from the Endpoints utility class to match your desired route.</param>
        /// <param name="timeout">Set this to force the connection to timeout and continue execution after a certain amount of time.</param>
        public static async Task<HttpResponseMessage> Get(string endpoint, TimeSpan? timeout = null)
        {
            try
            {
                if (timeout == null)
                {
                    return await GetClient().GetAsync(endpoint);
                }
                var cts = new CancellationTokenSource();
                cts.CancelAfter(timeout.Value);
                var response = GetClient().GetAsync(endpoint, cts.Token);
                while (!response.IsCompleted)
                {
                    cts.Token.ThrowIfCancellationRequested();
                }
                return await response.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception sending GET request!" + ex);
            }
            return null;
        }

        
        /// <summary>
        /// Send a POST request to the Planetwatcher API.
        /// </summary>
        /// <param name="endpoint">Reference a variable from the Endpoints utility class to match your desired route.</param>
        /// <param name="body">An object to send in the body of the POST. <br/> Will be automatically serialized into JSON.</param>
        /// <param name="timeout">Set this to force the connection to timeout and continue execution after a certain amount of time.</param>
        public static async Task<HttpResponseMessage> Post(string endpoint, object body, TimeSpan? timeout = null)
        {
            try
            {
                var json = JsonConvert.SerializeObject(body);
                var formattedBody = new StringContent(json, Encoding.UTF8,"application/json");
                if (timeout == null)
                {
                    return await GetClient().PostAsync(endpoint, formattedBody);
                }
                var cts = new CancellationTokenSource();
                cts.CancelAfter(timeout.Value);
                var response = GetClient().PostAsync(endpoint, formattedBody, cts.Token);
                while (!response.IsCompleted)
                {
                    cts.Token.ThrowIfCancellationRequested();
                }
                return await response.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception sending POST request!" + ex);
            }
            return null;
        }
        
        /// <summary>
        /// Send a PUT request to the Planetwatcher API.
        /// </summary>
        /// <param name="endpoint">Reference a variable from the Endpoints utility class to match your desired route.</param>
        /// <param name="body">An object to send in the body of the PUT. <br/> Will be automatically serialized into JSON.</param>
        /// <param name="timeout">Set this to force the connection to timeout and continue execution after a certain amount of time.</param>
        public static async Task<HttpResponseMessage> Put(string endpoint, object body, TimeSpan? timeout = null)
        {
            try
            {
                var json = JsonConvert.SerializeObject(body);
                var formattedBody = new StringContent(json, Encoding.UTF8,"application/json");
                if (timeout == null)
                {
                    return await GetClient().PutAsync(endpoint, formattedBody);
                }
                var cts = new CancellationTokenSource();
                cts.CancelAfter(timeout.Value);
                var response = GetClient().PutAsync(endpoint, formattedBody, cts.Token);
                while (!response.IsCompleted)
                {
                    cts.Token.ThrowIfCancellationRequested();
                }
                return await response.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception sending PUT request!" + ex);
            }
            return null;
        }
        
        /// <summary>
        /// Send a DELETE request to the Planetwatcher API.
        /// </summary>
        /// <param name="endpoint">Reference a variable from the Endpoints utility class to match your desired route.</param>
        /// /// <param name="body">An object to send in the body of the DELETE. <br/> Will be automatically serialized into JSON.</param>
        /// <param name="timeout">Set this to force the connection to timeout and continue execution after a certain amount of time.</param>
        public static async Task<HttpResponseMessage> Delete(string endpoint, object body, TimeSpan? timeout = null)
        {
            try
            {
                if (timeout == null)
                {
                    return await GetClient().DeleteAsJsonAsync(endpoint, body);
                }
                var cts = new CancellationTokenSource();
                cts.CancelAfter(timeout.Value);
                var response = GetClient().DeleteAsJsonAsync(endpoint, body, cts.Token);
                while (!response.IsCompleted)
                {
                    cts.Token.ThrowIfCancellationRequested();
                }
                return await response.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception sending DELETE request!" + ex);
            }
            return null;
        }
        
        /// <summary>
        /// Send a GET request to the Planetwatcher API. <br/>
        /// Parses the response and returns a string automatically using HttpClient. <br/>
        /// Cannot be authenticated, as the returned value is only the string content without a status code. <br/>
        /// Also cannot make use of cancellation tokens, so no timeout options can be set. Use at your own risk :)
        /// </summary>
        /// <param name="endpoint">Reference a variable from the Endpoints utility class to match your desired route.</param>
        public static async Task<string> GetString(string endpoint)
        {
            try
            {
                var response = await GetClient().GetStringAsync(endpoint);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception sending GET request!" + ex);
            }
            return null;
        }
        
        private static HttpClient GetClient()
        {
            if (_client != null) return _client;
            _cookies = new CookieContainer();
            _handler = new HttpClientHandler {CookieContainer = _cookies};
            _client = new HttpClient(_handler);
            _client.DefaultRequestHeaders.Add("User-Agent", "Planetwatcher Mobile Application - " +  Xamarin.Essentials.DeviceInfo.Platform);
            return _client;
        }
    }
    
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T data)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) });

        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T data, CancellationToken cancellationToken)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) }, cancellationToken);

        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, Uri requestUri, T data)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) });

        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, Uri requestUri, T data, CancellationToken cancellationToken)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) }, cancellationToken);

        private static HttpContent Serialize(object data) => new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
    }
}