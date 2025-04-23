using System.Text.Json;
using Meishi.Server.Services.GithubAuth;

namespace Meishi.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateApplicationBuilder(args);
            var app = builder.Build();

            app.UseDefaultFiles();
            app.MapStaticAssets();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }

        public static WebApplicationBuilder CreateApplicationBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureConfiguration(builder.Configuration);
            ConfigureServices(builder.Services, builder.Configuration);

            return builder;
        }

        public static void ConfigureConfiguration(IConfigurationBuilder config)
        {
            // Add configuration sources.
            config.AddEnvironmentVariables("MEISHI_");
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            // Add services to the container.
            services.Configure<MeishiOptions>(config);

            services.AddHttpClient<IGithubAuthService, GithubAuthService>(options =>
            {
                options.BaseAddress = new Uri("https://github.com/");
                options.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                options.DefaultRequestHeaders.Add("User-Agent", "MeishiApp");
            });

            services.AddHttpClient("GithubApi", options =>
            {
                options.BaseAddress = new Uri("https://api.github.com/");
                options.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                options.DefaultRequestHeaders.Add("User-Agent", "MeishiApp");
                options.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
            });

            services.AddAuthorization();
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
