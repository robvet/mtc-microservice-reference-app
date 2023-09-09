using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedUtilities.Utilties;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace MusicStore.Plumbing
{
    public class RestClient : IRestClient
    {
        //// Best pracitce: Make HttpClient static and reuse.
        //// Creating a new instance for each request is an antipattern that can
        //// result in socket exhaustion.
        private static readonly HttpClient _client = new HttpClient();
        //private readonly HttpClient _client = new HttpClient();
        private readonly string _apiGateway;

        // apikey
        private readonly string _apikey;

        // Create a TimeSpan of 4 minutes so that HTTP Calls do not timeout when debugging
        // Do not do this in production!!!
        private readonly TimeSpan _httpTimeOut = new TimeSpan(0, 4, 0);
        private readonly ILogger<RestClient> _logger;
        
        static RestClient()
        {
            // Set Rest Client as static so that it is only created once
            _client = new HttpClient();
        }

        public RestClient(IConfiguration config, ILogger<RestClient> logger)
        {
            Guard.ForNullOrEmpty(config["ApiGateway"], "ApiGateway not set");
            _apiGateway = config["ApiGateway"];
            _client.Timeout = _httpTimeOut;
            _logger = logger;
            Guard.ForNullOrEmpty(config["apikey"], "ApiKey not set");
            _apikey = config["apikey"];
        }

        public async Task<RestResponse<TReturnMessage>> GetAsync<TReturnMessage>(string path)
            where TReturnMessage : class, new()
        {
            HttpResponseMessage response;

            // robvet, 6-28-18, removed "http" prefix constant as we now get this
            // directly from the configuration file
            //var uri = new Uri($"http://{_apiGateway}/{path}");
            ////var uri = new Uri($"{_apiGateway}/{path}");
            var uri = new Uri($"{_apiGateway}/{path}");

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Pay Attention: Restclient is static, so we need to clear the default request header each time
            // the method is called as the previously-called api keys remain in the header
            _client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");

            _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apikey);

            try
            {
                response = await _client.GetAsync(uri);
            }
            catch (HttpRequestException ex)
            {
                // Looking for race condition where backend services have not started yet
                if (ex.Message == "No connection could be made because the target machine actively refused it.")
                {
                    _logger.LogError($"UIRestClient in UI: Services unavailable: {ex.Message}");

                    throw new Exception(
                        "Backend services are not available - wait several seconds and refresh browser");
                }

                _logger.LogError($"UIRestClient in UI: HttpRequestExceptionServices: {ex.Message}");

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"UIRestClient in UI: Generic Exception: {ex.Message}");

                throw;
            }

            if (!response.IsSuccessStatusCode)
            {

                var message = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Bad Status Code returned in UI RestClient: {message}");

                // if status code 400 or greater, thrown exception so that
                // exception handler takes care of it
                if ((int) response.StatusCode == 400)
                {
                    var error = $"Status Code of 400 returned in UI RestClient: {message}";
                    _logger.LogError(error);
                    throw new HttpRequestException(error);
                }

                if ((int)response.StatusCode == 404)
                {
                    _logger.LogError($"Status Code of 404 returned in UI RestClient: {message}");
                    //var error = $"{response.StatusCode}:{message}";
                    //throw new HttpRequestException(error);
                }

                if ((int)response.StatusCode >= 500)
                {
                    var errorMessage = $"Status Code of 500 returned in UI RestClient Get(): {message}";
                    _logger.LogError(errorMessage);
                    throw new HttpRequestException(errorMessage);
                }

                return new RestResponse<TReturnMessage>(response, null, string.Empty);
            }

            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<TReturnMessage>(result);
            return new RestResponse<TReturnMessage>(response, data, string.Empty);
        }

        public async Task<RestResponse<TReturnMessage>> PostAsync<TReturnMessage>(string path, object dataObject = null)
            where TReturnMessage : class, new()

        {
            HttpResponseMessage response = null;

            var uri = new Uri($"{_apiGateway}/{path}");

            // Pay Attention: Restclient is static, so we need to clear the default request header each time
            // the method is called as the previously-called api keys remain in the header
            _client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");

            _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apikey);

            // TODO remove
            //_client.DefaultRequestHeaders.Remove("x-correlationToken");
            //_client.DefaultRequestHeaders.Add("x-correlationToken", Guid.NewGuid().ToString());

            var content = dataObject == null ? "{}" : JsonConvert.SerializeObject(dataObject);
            try
            {

                response = await _client.PostAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));

                //if (content == "{}")
                //{
                //    response = await _client.PostAsync(uri, null);
                //}
                //    //response = await _client.PostAsync(uri, null);
                //else
                //{ 
                //    response = await _client.PostAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));
                //}
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
                if ((int)response.StatusCode == 400)
                {
                    var error = $"Status Code of 400 returned in UI RestClient: {message}";
                    _logger.LogError(error);
                    throw new HttpRequestException(error);
                }

                if ((int)response.StatusCode == 404)
                {
                    _logger.LogError($"Status Code of 404 returned in UI RestClient: {message}");
                    //var error = $"{response.StatusCode}:{message}";
                    //throw new HttpRequestException(error);
                }

                if ((int)response.StatusCode >= 500)
                {
                    var errorMessage = $"Status Code of 500 returned in UI RestClient Post(): {message}";
                    _logger.LogError(errorMessage);
                    throw new HttpRequestException(errorMessage);
                }

                //var ex = new HttpRequestException(
                //    $"Error: StatusCode: {response.StatusCode} - Message: {response.ReasonPhrase}");
                //ex.Data.Add("StatusCode", response.StatusCode);
                //ex.Data.Add("ReasonPhrase", response.ReasonPhrase);
                //throw new Exception(ex.Message);
            }

            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<TReturnMessage>(result);
            return new RestResponse<TReturnMessage>(response, data, string.Empty);
        }

        public async Task<RestResponse<TReturnMessage>> PutAsync<TReturnMessage>(string path, object dataObject = null)
            where TReturnMessage : class, new()
        {
            HttpResponseMessage response;

            var uri = new Uri($"{_apiGateway}/{path}");

            // Pay Attention: Restclient is static, so we need to clear the default request header each time
            // the method is called as the previously-called api keys remain in the header
            _client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");

            _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apikey);

            //_client.DefaultRequestHeaders.Remove("x-correlationToken");
            //_client.DefaultRequestHeaders.Add("x-correlationToken", Guid.NewGuid().ToString());

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
                if ((int)response.StatusCode == 400)
                {
                    var error = $"Status Code of 400 returned in UI RestClient: {message}";
                    _logger.LogError(error);
                    throw new HttpRequestException(error);
                }

                if ((int)response.StatusCode == 404)
                {
                    _logger.LogError($"Status Code of 404 returned in UI RestClient: {message}");
                    //var error = $"{response.StatusCode}:{message}";
                    //throw new HttpRequestException(error);
                }

                if ((int)response.StatusCode >= 500)
                {
                    var errorMessage = $"Status Code of 500 returned in UI RestClient Put(): {message}";
                    _logger.LogError(errorMessage);
                    throw new HttpRequestException(errorMessage);
                }

                //var ex = new HttpRequestException(
                //    $"Error: StatusCode: {response.StatusCode} - Message: {response.ReasonPhrase}");
                //ex.Data.Add("StatusCode", response.StatusCode);
                //ex.Data.Add("ReasonPhrase", response.ReasonPhrase);
                //throw new Exception(ex.Message);
            }

            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<TReturnMessage>(result);
            return new RestResponse<TReturnMessage>(response, data, string.Empty);
        }

        public async Task<RestResponse<bool>> DeleteAsync(string path)
        {
            HttpResponseMessage response;
            
            var uri = new Uri($"{_apiGateway}/{path}");

            // Pay Attention: Restclient is static, so we need to clear the default request header each time
            // the method is called as the previously-called api keys remain in the header
            _client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");

            _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apikey);

            //_client.DefaultRequestHeaders.Remove("x-correlationToken");
            //_client.DefaultRequestHeaders.Add("x-correlationToken", Guid.NewGuid().ToString());

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
                if ((int)response.StatusCode == 400)
                {
                    var error = $"Status Code of 400 returned in UI RestClient: {message}";
                    _logger.LogError(error);
                    throw new HttpRequestException(error);
                }

                if ((int)response.StatusCode >= 500)
                {
                    _logger.LogError($"Status Code of 500 returned in UI RestClient: {message}");
                    //var error = $"{response.StatusCode}:{message}";
                    //throw new HttpRequestException(error);
                }

                //var ex = new HttpRequestException(
                //    $"Error: StatusCode: {response.StatusCode} - Message: {response.ReasonPhrase}");
                //ex.Data.Add("StatusCode", response.StatusCode);
                //ex.Data.Add("ReasonPhrase", response.ReasonPhrase);
                //throw new Exception(ex.Message);
            }

            return new RestResponse<bool>(response, response.IsSuccessStatusCode, string.Empty);
        }

        //private Uri PrepareCall(string path)
        //{
        //    // robvet, 6-28-18, removed "http" prefix constant as we now get this
        //    // directly from the configuration file
        //    //var uri = new Uri($"http://{_apiGateway}/{path}");
        //    ////var uri = new Uri($"{_apiGateway}/{path}");
        //    var uri = new Uri($"{_apiGateway}/{path}");

        //    _client.DefaultRequestHeaders.Accept.Clear();
        //    _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    // Pay Attention: Restclient is static, so we need to clear the default request header each time
        //    // the method is called as the previously-called api keys remain in the header
        //    _client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");

        //    _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apikey);

        //    // TODO remove
        //    _client.DefaultRequestHeaders.Remove("x-correlationToken");
        //    _client.DefaultRequestHeaders.Add("x-correlationToken", Guid.NewGuid().ToString());

        //    return uri;
        //}
    }
}