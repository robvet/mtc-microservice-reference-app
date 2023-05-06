using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using SharedUtilities.Utilties;

namespace RestCommunicator
{
    public static class HttpClientFactoryExtension
    {
        private static int _retryCount;
        private static int _tripCircuit;
        private static int _timeout;

        // Extension method to register http services
        public static IServiceCollection AddHttpServices<T>(this IServiceCollection services,
            IConfiguration configuration)
            where T : class, IRestClient
        {
            // Creating long TimeSpan to avoid timeouts when debugging - do not do in production
            _timeout = Convert.ToInt32(configuration["HttpClientTimeout"] ?? "30");

            if (Convert.ToBoolean(configuration["HttpClientEnableResiliency"] ?? "true"))
            {
                _retryCount = Convert.ToInt32(configuration["HttpClientRetryCount"] ?? "3");
                _tripCircuit = Convert.ToInt32(configuration["HttpClientExceptionsAllowedBeforeBreaking"] ?? "5");
            }
            else
            {
                _retryCount = Convert.ToInt32("0");
                _tripCircuit = Convert.ToInt32("1000");
            }

            // We need instance of LoggerFactory, which requires ServiceProvider instance
            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetService<LoggerFactory>();
            var logger = loggerFactory.CreateLogger<T>();

            services.AddHttpClient<IRestClient, T>(client =>
                {
                    client.Timeout = new TimeSpan(0, 0, _timeout);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                })
                .AddPolicyHandler(GetRetryPolicy<RestClient>(configuration, logger))
                .AddPolicyHandler(GetCircuitBreakerPolicy<RestClient>(configuration, logger));
            //.AddHttpMessageHandler<AuthorizationHandler>();
            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy<T>(IConfiguration configuration, ILogger logger)
        {
            var retries = _retryCount;
            var backOffDuration = configuration["BackOffDuration"].ParseInt(2);

            return HttpPolicyExtensions
                .HandleTransientHttpError() // Handles HttpRequestException, 5xx and 408 (request timeout)
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound) // Handles 404
                                                                            //.Or<NullReferenceException>()
                .OrResult(msg =>
                    msg.StatusCode == HttpStatusCode.TooManyRequests) // Handles 429 - server busy-too many requests
                .WaitAndRetryAsync(retries,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(backOffDuration, retryAttempt)),
                    (result, timespan, retryCount, context) =>
                    {
                        if (logger == null)
                            return;

                        logger.LogError(
                            "A non-success code {StatusCode} was received on retry {RetryAttempt} for {PolicyKey}",
                            (int)result.Result.StatusCode, retryCount, context.PolicyKey);
                    });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy<T>(IConfiguration configuration,
            ILogger logger)
        {
            var exceptionsAllowed = _tripCircuit;
            var durationOfBreak = configuration["DurationOfBreak"].ParseInt(30);

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(exceptionsAllowed,
                    TimeSpan.FromSeconds(durationOfBreak),
                    (result, timeSpan, context) =>
                    {
                        if (logger == null)
                            return;

                        logger.LogError($"CircuitBreaker onBreak for {timeSpan.TotalSeconds} seconds");
                    },
                    context =>
                    {
                        if (logger == null)
                            return;

                        logger.LogError("CircuitBreaker onRest");
                    });
        }
    }
}