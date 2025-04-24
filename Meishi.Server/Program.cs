using System.Text.Json;
using Meishi.Server.Services.GithubApi;

namespace Meishi.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateApplicationBuilder(args);
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.MapControllers();

            app.Run();
        }

        public static WebApplicationBuilder CreateApplicationBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services, builder.Configuration);

            return builder;
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            services.Configure<MeishiOptions>(config);

            services.AddHttpClient<IGithubApiService, GithubApiService>(options =>
            {
                options.BaseAddress = new Uri("https://api.github.com/");
                options.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                options.DefaultRequestHeaders.Add("User-Agent", "MeishiApp");
                options.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
            });

            services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                        options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                        options.JsonSerializerOptions.IgnoreReadOnlyFields = true;
                    });
        }
    }
}
