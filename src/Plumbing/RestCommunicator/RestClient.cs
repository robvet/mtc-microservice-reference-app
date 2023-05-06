using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceLocator;
using SharedUtilities.Utilties;

namespace RestCommunicator
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RestClient> _logger;

        private readonly IFindService _serviceLocator;
        //private readonly ServiceMapper _serviceMapper;

        public RestClient(HttpClient httpClient,
            ILogger<RestClient> logger,
            //IOptions<ServiceMapper> serviceMapper,
            IFindService serviceLocator)
        {
            _httpClient = httpClient;
            // _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer [token]");
            _logger = logger;
            //_serviceMapper = serviceMapper.Value;
            _serviceLocator = serviceLocator;
        }

        public async Task<RestResponse<TReturnMessage>> GetAsync<TReturnMessage>(ServiceEnum serviceName,
            string path, string correlationToken)
            where TReturnMessage : class, new()
        {
            HttpResponseMessage response = null;

            //**************************************************************************           
            // Temp Debugging code
            if (string.IsNullOrEmpty(path)) throw new Exception("Path is empty in RestClient in services layer");
            //**************************************************************************           

            // Parse URI
            var uri = new Uri($"{_serviceLocator.GetServiceUri(serviceName)}/{path}");

            //**************************************************************************           
            // Temp Debugging Code to address transient connectivity errors
            if (uri.IsNullOrDbNull()) throw new Exception("URI is empty in RestClient in services layer");
            //**************************************************************************  

            // Load headers
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer [token]");

            // Pack correlationToken in the custom http header
            _httpClient.DefaultRequestHeaders.Add("x-correlationToken", correlationToken);

            // Fire the call
            try
            {
                response = await _httpClient.GetAsync(uri);
            }
            catch (HttpRequestException ex)
            {
                // Looking for race condition where backend services have not started yet
                if (ex.Message == "No connection could be made because the target machine actively refused it.")
                {
                    _logger.LogError($"RestClient for Shared Service: '{serviceName}' unavailable for {uri}: {ex.Message}");

                    throw new Exception($"RestClient in Service: Service '{serviceName}' unavailable for {uri}: {ex.Message}");
                }

                _logger.LogError($"RestClient in UI: HttpRequestExceptionServices: {ex.Message}");

                throw;
            }

            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error in RestClient for Shared Services: Call to {uri} returned Exception:{ex.Message} for CorrelationId:{correlationToken}");

                throw;
            }

            // Capture response
            var result = await response.Content.ReadAsStringAsync();

            // Build custom response to return to caller
            // We return three values, or a Tuple type:
            //   value 1: payload
            //   vaule 2: optional error text message
            //   vaule 3: http status code for response
            if (!response.IsSuccessStatusCode)
                // If unsuccessful, return null payload, error message, and http status code
                return new RestResponse<TReturnMessage>(null,
                    result += $" with correlationId:{correlationToken}",
                    response.StatusCode);

            // If successful, return payload and http status code - no message required
            return new RestResponse<TReturnMessage>(JsonConvert.DeserializeObject<TReturnMessage>(result),
                string.Empty,
                response.StatusCode);
        }

        public async Task<RestResponse<TReturnMessage>> PostAsync<TReturnMessage>(
            ServiceEnum serviceName,
            string path,
            string correlationToken,
            object dataObject = null) where TReturnMessage : class, new()

        {
            var uri = new Uri($"{_serviceLocator.GetServiceUri(serviceName)}/{path}");

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer [token]");

            // Pack the correlationToken in the Http header
            _httpClient.DefaultRequestHeaders.Add("x-correlationToken", correlationToken);

            var newContent = dataObject != null ? JsonConvert.SerializeObject(dataObject) : "{}";
            var response =
                await _httpClient.PostAsync(uri, new StringContent(newContent, Encoding.UTF8, "application/json"));

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                // If unsuccessful, return null payload, error message, and http status code
                return new RestResponse<TReturnMessage>(null,
                    result += $"{response.ReasonPhrase} with correlationId:{correlationToken}",
                    response.StatusCode);

            // If successful, return payload and http status code - no message required
            return new RestResponse<TReturnMessage>(JsonConvert.DeserializeObject<TReturnMessage>(result),
                string.Empty,
                response.StatusCode);
        }

        public async Task<RestResponse<TReturnMessage>> PutAsync<TReturnMessage>(ServiceEnum serviceName, string path,
            string correlationToken,
            object dataObject = null)
            where TReturnMessage : class, new()
        {
            var uri = new Uri($"{_serviceLocator.GetServiceUri(serviceName)}/{path}");

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer [token]");

            // Pack the correlationToken in the Http header
            _httpClient.DefaultRequestHeaders.Add("x-correlationToken", correlationToken);

            var updatedContent = dataObject != null ? JsonConvert.SerializeObject(dataObject) : "{}";
            var response = await _httpClient.PutAsync(uri,
                new StringContent(updatedContent, Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                // If unsuccessful, return null payload, error message, and http status code
                return new RestResponse<TReturnMessage>(null,
                    result += $" with correlationId:{correlationToken}",
                    response.StatusCode);

            // If successful, return payload and http status code - no message required
            return new RestResponse<TReturnMessage>(JsonConvert.DeserializeObject<TReturnMessage>(result),
                string.Empty,
                response.StatusCode);
        }

        public async Task<bool> DeleteAsync(ServiceEnum serviceName, string path, string correlationToken)
        {
            var uri = new Uri($"{_serviceLocator.GetServiceUri(serviceName)}/{path}");

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer [token]");

            // Pack the correlationToken in the Http header
            _httpClient.DefaultRequestHeaders.Add("x-correlationToken", correlationToken);

            var response = await _httpClient.DeleteAsync(uri);

            return response.IsSuccessStatusCode;
        }
    }
}