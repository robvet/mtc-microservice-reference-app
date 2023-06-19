using System;
using System.Globalization;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicStore.Extensions;
using MusicStore.Plumbing;
using MusicStore.Properties;
using SharedUtilities.Utilties;

namespace MusicStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // validate configuration data exists
            Guard.ForNullOrEmpty(Configuration["ApiGateway"], "ApiGateway Endpoint Environment Variable not set");
            Guard.ForNullOrEmpty(Configuration["catalogBaseUri"], "CatalogBaseUri Environment Variable not set");
            Guard.ForNullOrEmpty(Configuration["basketBaseUri"], "basketBaseUri Endpoint Environment Variable not set");
            Guard.ForNullOrEmpty(Configuration["orderBaseUri"], "orderBaseUri Endpoint Environment Variable not set");

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/Home/AccessDenied");

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => { builder.WithOrigins("http://example.com"); });
            });

            // Register backing services
            services.RegisterTelemetryCollector(Configuration);
            services.AddLogging();

            //// Register telemetry initializer
            //services.AddSingleton<ITelemetryInitializer, ServiceNameTelemetryInitializer>();

            // Add MVC services to the services container
            services.AddMvc().AddNewtonsoftJson();

            // Add memory cache services
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            // Configure Auth
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(
            //        "ManageStore",
            //        authBuilder => { authBuilder.RequireClaim("ManageStore", "Allowed"); });
            //});
            //services.AddAuthentication();

            services.AddSingleton<IRestClient, RestClient>();
            services.AddSingleton<CookieLogic>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //telemetryConfiguration.InstrumentationKey = Configuration["ApplicationInsights:InstrumentationKey"] ?? throw new ArgumentNullException($"AppInsights key for UI is Null");

            // StatusCode pages to gracefully handle status codes 400-599.
            //app.UseStatusCodePagesWithRedirects("~/Home/StatusCodePage");

            if (env.IsDevelopment())
                // Display custom error page in production when error occurs
                // During development use the ErrorPage middleware to display error information in the browser
                //app.UseStatusCodePagesWithReExecute("/Error/Error/", "? statusCode={0}");
                //app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                //app.UseBrowserLink();

                // redirects to MVC Error View for Home Controller in non-dev environment
                app.UseExceptionHandler("/Error/Error");
            else
                app.UseExceptionHandler("/Home/Error");


            // force the en-US culture, so that the app behaves the same even on machines with different default culture
            var supportedCultures = new[] {new CultureInfo("en-US")};

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseHttpsRedirection();

            // Add static files to the request pipeline
            app.UseStaticFiles();

            app.UseRouting();

            // Add cookie-based authentication to the request pipeline
            app.UseAuthentication();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "areaRoute",
                    "{area:exists}/{controller}/{action}",
                    new {action = "Index"});

                endpoints.MapControllerRoute(
                    "default",
                    "{controller}/{action}/{id?}",
                    new {controller = "Home", action = "Index"});

                endpoints.MapControllerRoute(
                    "api",
                    "{controller}/{id?}");
            });
        }
    }
}