using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using System;
using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace order.service
{
    public class Program
    {
        // Gets the root Azure Key Vault endpoint from a machine level environment variable
        // that is populated in the DeployToAzure.ps1 script
        private static string GetKeyVaultEndpoint() => Environment.GetEnvironmentVariable("KEYVAULT_ENDPOINT_MICROTUNES", EnvironmentVariableTarget.Machine);

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 1/16/2020 - lw - added ConfigureAppConfiguration to allow Azure Key Vault
        /// to be included in the configuration builder. This allows us to pull the
        /// key vault secrets directly from key vault without having to put the secret
        /// URL in to User Secrets
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseUrls("http://localhost:8085");
                    webBuilder.UseKestrel();
                    // Open two ports on the Kestrel web server:
                    //     - One for HTTP1 calls (RESTFul)
                    //     - Another for HTTP2 calls (gRPC)
                    //webBuilder.ConfigureKestrel(options =>
                    //{
                    //    options.Listen(IPAddress.Any, 8085,
                    //         listenOptions =>
                    //         {
                    //             listenOptions.Protocols = HttpProtocols.Http1;
                    //         });
                    //    options.Listen(IPAddress.Any, 8087,
                    //        listenOptions =>
                    //        {
                    //            listenOptions.Protocols = HttpProtocols.Http2;
                    //        });
                    //});
                    //// gRPC configuration
                    //// Add logging gRPC https://docs.microsoft.com/en-us/aspnet/core/grpc/diagnostics?view=aspnetcore-3.1
                    //webBuilder.ConfigureLogging(logging =>
                    //{
                    //    logging.AddFilter("Grpc", LogLevel.Debug);
                    //});
                    webBuilder.CaptureStartupErrors(true);
                    // KeyVault configuration
                    webBuilder.ConfigureAppConfiguration((builderContext, config) =>
                    {
                        config.AddEnvironmentVariables();
                        //var keyVaultEndpoint = GetKeyVaultEndpoint();
                        //if (!string.IsNullOrEmpty(keyVaultEndpoint))
                        //{
                        //    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                        //    var keyVaultClient = new KeyVaultClient(
                        //        new KeyVaultClient.AuthenticationCallback(
                        //            azureServiceTokenProvider.KeyVaultTokenCallback));
                        //    config.AddAzureKeyVault(
                        //        keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                        //}
                    });
                });
    }
}