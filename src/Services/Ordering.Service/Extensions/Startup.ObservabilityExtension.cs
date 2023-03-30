using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.API.TelemetryInitializer;

namespace Ordering.API.Extensions
{
    public static class ObservabilityExtension
    {
        public static IServiceCollection RegisterTelemetryCollector(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(x =>
            {
                x.InstrumentationKey = configuration["aiinstrumkeysecret"] ??
                                       throw new ArgumentNullException("aiinstrumkeysecret",
                                           "AppInsights instrumentation key for Basket missing");
                x.DeveloperMode = true;
            });

            // Add Application Insights TelemetryClient object
            // https://briancaos.wordpress.com/2020/05/07/c-azure-telemetryclient-will-leak-memory-if-not-implemented-as-a-singleton/
            //https://github.com/Azure/azure-functions-host/issues/4820
            // https://github.com/microsoft/ApplicationInsights-dotnet/issues/1152
            services.AddSingleton(x =>
            {
                var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
                //telemetryConfiguration.InstrumentationKey = configuration["aiinstrumkeysecret"];

                var serviceName = configuration["TelemetryNameForService"];
                serviceName = !string.IsNullOrEmpty(serviceName) ? serviceName : "Ordering";

                var telemetryClient = new TelemetryClient(telemetryConfiguration);
                
                // Need to explicitly add rolename to telemetryClient instance
                telemetryClient.Context.Cloud.RoleName = serviceName;
                telemetryClient.InstrumentationKey = configuration["aiinstrumkeysecret"];
                
                return telemetryClient;
            });

            // Register telemetry initializer
            services.AddSingleton<ITelemetryInitializer, ServiceNameTelemetryInitializer>();

            return services;
        }
    }
}