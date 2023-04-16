using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.TelemetryInitializer
{
    public class ServiceNameTelemetryInitializer : ITelemetryInitializer
    {
        private readonly IConfiguration _configuration;
        
        public ServiceNameTelemetryInitializer(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void Initialize(ITelemetry telemetry)
        {
            // Get service name 
            var serviceName = _configuration["TelemetryNameForService"];

            serviceName = !string.IsNullOrEmpty(serviceName) ? serviceName : "Catalog";

            telemetry.Context.Cloud.RoleName = serviceName;
        }
    }
}
