using System.Text.Json;
using Meishi.Server.Services.GithubAuth.Models;
using Microsoft.Extensions.Options;

namespace Meishi.Server.Services.GithubAuth
{
    public class GithubAuthService : IGithubAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GithubAuthService> _logger;
        private readonly IOptions<MeishiOptions> _options;

        public GithubAuthService(HttpClient httpClient, ILogger<GithubAuthService> logger, IOptions<MeishiOptions> options)
        {
            _httpClient = httpClient;
            _logger = logger;
            _options = options;
        }

        public async Task<GithubAccessTokenResponse> GetAccessTokenAsync(string code)
        {
            var queryParams = QueryString.Create(
                new Dictionary<string, string?>
                {
                    { "client_id", this._options.Value.ClientId },
                    { "client_secret", this._options.Value.ClientSecret },
                    { "code", code }
                }
            );

            this._logger.LogInformation("Requesting access token\nRequest content: {0}", queryParams);

            var res = 
                await _httpClient
                    .PostAsync("/login/oauth/access_token" + queryParams.ToUriComponent(), null);

            this._logger.LogInformation("Access token response: {0}", res);

            res.EnsureSuccessStatusCode();

            var resContent =
                await res.Content.ReadFromJsonAsync<GithubAccessTokenResponse>(
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                    }
                );

            if (resContent == null)
            {
                throw new Exception("Failed to get access token from GitHub");
            }

            return resContent;
        }

        public async Task<GithubAccessTokenResponse> RefreshTokenAsync(string refreshToken)
        {
            var queryParams = QueryString.Create(
                new Dictionary<string, string?>
                {
                    { "client_id", this._options.Value.ClientId },
                    { "client_secret", this._options.Value.ClientSecret },
                    { "grant_type", "refresh_token" },
                    { "refresh_token", refreshToken }
                }
            );

            this._logger.LogInformation("Refresh access token request\nRequest content: {0}", queryParams);

            var res =
                await _httpClient
                    .PostAsync("/login/oauth/access_token" + queryParams.ToUriComponent(), null);

            this._logger.LogInformation("Refresh access token response: {0}", res);

            res.EnsureSuccessStatusCode();

            var resContent =
                await res.Content.ReadFromJsonAsync<GithubAccessTokenResponse>(
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                    }
                );

            if (resContent == null)
            {
                throw new Exception("Failed to get access token from GitHub");
            }

            return resContent;
        }
    }
}
