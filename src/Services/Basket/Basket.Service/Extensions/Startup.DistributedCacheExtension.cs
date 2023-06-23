using Basket.Service.Contracts;
using Basket.Service.Infrastructure.Repository;
using Basket.Service.TelemetryInitializer;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Basket.Service.Extensions
{
    public static class DistributedCacheExtension
    {
        public static IServiceCollection RegisterDistrbutedCache(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton(sp =>
            {
                ////var settings = sp.GetRequiredService<IOptions<BasketSettings>>().Value;
                ////var configuration = ConfigurationOptions.Parse(settings.ConnectionString, true);

                var redisconnstr = configuration["redisconnstrsecret"];
                return ConnectionMultiplexer.Connect(redisconnstr);

                //// Most recent
                //var redisconnstr = configuration["redisconnstrsecret"];
                //var options = ConfigurationOptions.Parse(redisconnstr);
                ////options.Ssl = false;
                ////options.SslProtocols = System.Security.Authentication.SslProtocols.Tls12; // "tls12";
                //return ConnectionMultiplexer.Connect(options);
                //// End most recent




                ////var options = new ConfigurationOptions();
                ////options.EndPoints.Add("microv2-redis.redis.cache.windows.net");
                ////options.Ssl = false;
                ////options.Password = "joV2OYDvHIl9W5frSjFPC82b0CgphLo+eb6gImzsKCk=";
                ////return ConnectionMultiplexer.Connect(options);




                //var redisconnstr = configuration["redisconnstrsecret"];
                //ConfigurationOptions stuff = ConfigurationOptions.Parse(redisconnstr);


                //var parsedConnectionString = redisconnstr.Split(';');
                //options.EndPoints.Add(parsedConnectionString[0]);
                //options.Password = parsedConnectionString[1];
                //options.Ssl =  Convert.ToBoolean(parsedConnectionString[2]);
                ////options.EndPoints.Add(redisconnstr);

                ////options.EndPoints.Add("microv2-redis.redis.cache.windows.net");
                ////options.Ssl = false;
                ////options.SslProtocols = SslProtocols.Tls12;
                //////options.Password = "aboR9pLkxU8haxIxb5y+qTbW160mSCFuFZ7dEYxgZqw=";
                //////aboR9pLkxU8haxIxb5y + qTbW160mSCFuFZ7dEYxgZqw =
                ////options.Password = "joV2OYDvHIl9W5frSjFPC82b0CgphLo+eb6gImzsKCk=";
                //return ConnectionMultiplexer.Connect(options);
            });

            services.AddTransient<IDistributedCacheRepository, RedisCacheRepository>();
                      
            return services;
        }
    }
}