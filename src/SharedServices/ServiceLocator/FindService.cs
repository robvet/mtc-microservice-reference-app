using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ServiceLocator
{
    public class FindService : IFindService
    {
        private readonly IConfiguration _configuration;
        private ILogger _logger;

        public FindService(IConfiguration config, ILogger<FindService> logger)
        {
            _configuration = config;
            _logger = logger;
        }

        public string GetServiceUri(ServiceEnum serviceName)
        {
            // Transform from Enum to string
            var key = serviceName.ToString();

            _logger.LogInformation($"Locating Service URL with Key {key}");

            // Retrieve URL from Configuration
            var uri = _configuration[key];

            _logger.LogInformation($"Target Service URL is {uri}");

            if (uri == null)
                throw new NullReferenceException(
                    $"Uri key for {key} has not been configured. Add to it UserSecrets or appsettings.json");

            return uri;
        }
    }
}