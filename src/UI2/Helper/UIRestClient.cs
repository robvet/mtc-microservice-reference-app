using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MusicStore.Helper
{
    public class UIRestClient : IRestClient
    {
        //// Best pracitce: Make HttpClient static and reuse.
        //// Creating a new instance for each request is an antipattern that can
        //// result in socket exhaustion.
        private static readonly HttpClient _client;

        private readonly string _apiGateway;
        //private readonly HttpClient _client = new HttpClient();

        // Create a TimeSpan of 4 minutes so that HTTP Calls do not timeout when debugging
        // Do not do this in production!!!
        private readonly TimeSpan _httpTimeOut = new TimeSpan(0, 4, 0);
        private readonly ILogger<UIRestClient> _logger;

        static UIRestClient()
        {
            _client = new HttpClient();
        }

        public UIRestClient(IConfiguration config, ILogger<UIRestClient> logger)
        {
            _apiGateway = config["ApiGateway"];
            _client.Timeout = _httpTimeOut;
            _logger = logger;
        }

        public async Task<UIRestResponse<TReturnMessage>> GetAsync<TReturnMessage>(string path)
            where TReturnMessage : class, new()
        {
            HttpResponseMessage response = null;

            // robvet, 6-28-18, removed "http" prefix constant as we now get this
            // directly from the configuration file
            //var uri = new Uri($"http://{_apiGateway}/{path}");
            var uri = new Uri($"{_apiGateway}/{path}");

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                response = await _client.GetAsync(uri);
            }
            catch (HttpRequestException ex)
            {
                // Looking for race condition where backend services have not started yet
                if (ex.Message == "No connection could be made because the target machine actively refused it.")
                {
                    _logger.LogInformation($"UIRestClient in UI: Services unavailable: {ex.Message}");

                    throw new Exception(
                        "Backend services are not available - wait several seconds and refresh browser");
                }

                _logger.LogInformation($"UIRestClient in UI: HttpRequestExceptionServices: {ex.Message}");

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"UIRestClient in UI: Generic Exception: {ex.Message}");

                throw;
            }

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();

                // if status code 400 or greater, thrown exception so that
                // exception handler takes care of it
                if ((int) response.StatusCode >= 400)
                {
                    var error = $"{response.StatusCode}:{message}";
                    throw new HttpRequestException(error);
                }

                return new UIRestResponse<TReturnMessage>(response, null, string.Empty);
            }

            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<TReturnMessage>(result);
            return new UIRestResponse<TReturnMessage>(response, data, string.Empty);
        }

        public async Task<UIRestResponse<TReturnMessage>> PostAsync<TReturnMessage>(string path, object dataObject = null)
            where TReturnMessage : class, new()

        {
            HttpResponseMessage response = null;

            var uri = new Uri($"{_apiGateway}/{path}");

            var content = dataObject == null ? "{}" : JsonConvert.SerializeObject(dataObject);

            try
            {
                response = await _client.PostAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"UIRestClient in UI: Generic Exception: {ex.Message}");
                throw;
            }
            
            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();

                // if status code 400 or greater, thrown exception so that
                // exception handler takes care of it
                if ((int)response.StatusCode >= 400)
                {
                    var error = $"{response.StatusCode}:{message}";
                    throw new HttpRequestException(error);
                }

                return new UIRestResponse<TReturnMessage>(response, null, string.Empty);

                //var ex = new HttpRequestException(
                //    $"Error: StatusCode: {response.StatusCode} - Message: {response.ReasonPhrase}");
                //ex.Data.Add("StatusCode", response.StatusCode);
                //ex.Data.Add("ReasonPhrase", response.ReasonPhrase);
                //throw new Exception(ex.Message);
            }

            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<TReturnMessage>(result);
            return new UIRestResponse<TReturnMessage>(response, data, string.Empty);
        }

        public async Task<UIRestResponse<TReturnMessage>> PutAsync<TReturnMessage>(string path, object dataObject = null)
            where TReturnMessage : class, new()
        {
            HttpResponseMessage response;

            var uri = new Uri($"{_apiGateway}/{path}");

            var content = dataObject != null ? JsonConvert.SerializeObject(dataObject) : "{}";

            try
            {
                response = await _client.PutAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"UIRestClient in UI: Generic Exception: {ex.Message}");
                throw;
            }

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();

                // if status code 400 or greater, thrown exception so that
                // exception handler takes care of it
                if ((int)response.StatusCode >= 400)
                {
                    var error = $"{response.StatusCode}:{message}";
                    throw new HttpRequestException(error);
                }

                return new UIRestResponse<TReturnMessage>(response, null, string.Empty);

                //var ex = new HttpRequestException(
                //    $"Error: StatusCode: {response.StatusCode} - Message: {response.ReasonPhrase}");
                //ex.Data.Add("StatusCode", response.StatusCode);
                //ex.Data.Add("ReasonPhrase", response.ReasonPhrase);
                //throw new Exception(ex.Message);
            }

            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<TReturnMessage>(result);
            return new UIRestResponse<TReturnMessage>(response, data, string.Empty);
        }

        public async Task<UIRestResponse<bool>> DeleteAsync(string path)
        {
            HttpResponseMessage response;
            
            var uri = new Uri($"{_apiGateway}/{path}");

            try
            {
                response = await _client.DeleteAsync(uri);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"UIRestClient in UI: Generic Exception: {ex.Message}");
                throw;
            }



            //if (!response.IsSuccessStatusCode)
            //{
            //    var ex = new HttpRequestException(
            //        $"Error: StatusCode: {response.StatusCode} - Message: {response.ReasonPhrase}");
            //    ex.Data.Add("StatusCode", response.StatusCode);
            //    ex.Data.Add("ReasonPhrase", response.ReasonPhrase);
            //    throw new Exception(ex.Message);
            //}

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();

                // if status code 400 or greater, thrown exception so that
                // exception handler takes care of it
                if ((int)response.StatusCode >= 400)
                {
                    var error = $"{response.StatusCode}:{message}";
                    throw new HttpRequestException(error);
                }

                //var ex = new HttpRequestException(
                //    $"Error: StatusCode: {response.StatusCode} - Message: {response.ReasonPhrase}");
                //ex.Data.Add("StatusCode", response.StatusCode);
                //ex.Data.Add("ReasonPhrase", response.ReasonPhrase);
                //throw new Exception(ex.Message);
            }

            return new UIRestResponse<bool>(response, response.IsSuccessStatusCode, string.Empty);
        }
    }
}