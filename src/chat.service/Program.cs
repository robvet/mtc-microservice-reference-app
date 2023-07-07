//using Cosmos.Chat.GPT.Options;
//using Cosmos.Chat.GPT.Services;
using Cosmos.Chat.GPT.Options;
using Cosmos.Chat.GPT.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.RegisterConfiguration();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

static class ProgramExtensions
{
    public static void RegisterConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<CosmosDb>()
            .Bind(builder.Configuration.GetSection("CosmosDb"));

        // Read key from environment variable or secrets
        builder.Configuration.GetSection("CosmosKey");

        builder.Services.AddOptions<OpenAi>()
            .Bind(builder.Configuration.GetSection("OpenAi"));

        // Read key from environment variable or secrets
        builder.Configuration.GetSection("OpenAiKey");

        //var configuration = new ConfigurationBuilder()
        //    .AddJsonFile("appsettings.json")
        //    .Build();
        //builder.Services.Configure<CosmosOptions>(configuration.GetSection("Cosmos"));
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<CosmosDbService, CosmosDbService>((provider) =>
        {
            var cosmosDbOptions = provider.GetRequiredService<IOptions<CosmosDb>>();
            if (cosmosDbOptions is null)
            {
                throw new ArgumentException($"{nameof(IOptions<CosmosDb>)} was not resolved through dependency injection.");
            }
            else
            {
                return new CosmosDbService(
                    endpoint: cosmosDbOptions.Value?.Endpoint ?? String.Empty,
                    key: cosmosDbOptions.Value?.Key ?? String.Empty,
                    databaseName: cosmosDbOptions.Value?.Database ?? String.Empty,
                    containerName: cosmosDbOptions.Value?.Container ?? String.Empty
                );
            }
        });
        
        services.AddSingleton<OpenAiService, OpenAiService>((provider) =>
        {
            var openAiOptions = provider.GetRequiredService<IOptions<OpenAi>>();
            if (openAiOptions is null)
            {
                throw new ArgumentException($"{nameof(IOptions<OpenAi>)} was not resolved through dependency injection.");
            }
            else
            {
                return new OpenAiService(endpoint: openAiOptions.Value?.Endpoint ?? String.Empty,
                                         key: openAiOptions.Value?.Key ?? String.Empty,
                                         modelName: openAiOptions.Value?.ModelName ?? String.Empty
                );
            }
        });

        services.AddSingleton<ChatService>((provider) =>
        {
            var openAiOptions = provider.GetRequiredService<IOptions<OpenAi>>();

            if (openAiOptions is null)
            {
                throw new ArgumentException($"{nameof(IOptions<OpenAi>)} was not resolved through dependency injection.");
            }
            else
            {
                var cosmosDbService = provider.GetRequiredService<CosmosDbService>();
                var openAiService = provider.GetRequiredService<OpenAiService>();

                return new ChatService(openAiService: openAiService,
                                       cosmosDbService: cosmosDbService,
                                       maxConversationTokens: openAiOptions.Value?.MaxConversationTokens ?? String.Empty
                );
            }   
        });



    }



    //public static void RegisterCosmos(this WebApplicationBuilder builder)
    //{
    //    builder.Services.AddSingleton<CosmosClient>(s =>
    //    {
    //        var options = s.GetService<IOptions<CosmosDb>>();
    //        var cosmosDb = options.Value;
    //        return new CosmosClient(cosmosDb.Endpoint, cosmosDb.Key);
    //    });
    //}



}