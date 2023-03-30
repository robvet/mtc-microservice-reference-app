using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Tools.Database;
using Tools.Filters;

namespace Tools
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register DataContext           
            services.ConfigureDataContext(Configuration);
            // Register Azure Table Storage - used to pre-populate Catalog Read Table
            services.ConfigureAzureTableStorage(Configuration);

            services.AddScoped<IMusicRepository, MusicRepository>();

            services.AddMvc(config =>
                {
                    // Attach Customer Exception Filter
                    config.Filters.Add(typeof(DataGeneratorCustomExceptionFilter));
                }
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Tools API",
                    Version = "v1",
                    Description =
                        "Utilities for the Architecting Microservices Workshop"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) 
                app.UseDeveloperExceptionPage();
            
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopping Basket API V1"); });
        }
    }
}