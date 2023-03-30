using System.IO;
using AdminTools.Database;
using AdminTools.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace AdminTools
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
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Data Generator API",
                    Version = "v1",
                    Description =
                        "Utility to generate Product Data for Activate Azure with Microservices Workshop"
                });
                // Set the comments path for the Swagger JSON and UI.
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                //var xmlPath = Path.Combine(basePath, "DataGenerator.xml");
                //c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopping Basket API V1"); });

            app.UseMvc();
        }
    }
}