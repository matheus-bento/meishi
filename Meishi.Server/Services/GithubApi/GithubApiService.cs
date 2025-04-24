using System.Text.Json;
using Meishi.Server.Services.GithubApi.Models;
using Microsoft.Extensions.Options;

namespace Meishi.Server.Services.GithubApi
{
    /// <summary>
    ///     Typed HTTP Client to the GitHub API and get information about a
    ///     specified user and their repositories
    /// </summary>
    public class GithubApiService : IGithubApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GithubApiService> _logger;
        private readonly IOptions<MeishiOptions> _options;

        public GithubApiService(
            HttpClient httpClient,
            ILogger<GithubApiService> logger,
            IOptions<MeishiOptions> options)
        {
            this._httpClient = httpClient;
            this._logger = logger;
            this._options = options;
        }

        public async Task<GithubUserResponse> GetUserInfoAsync(string user)
        {
            string reqUrl = "/users/" + user;

            var res = await this._httpClient.GetAsync(reqUrl);

            res.EnsureSuccessStatusCode();

            var githubUser =
                await res.Content.ReadFromJsonAsync<GithubUserResponse>(
                    this._options.Value.GithubJsonSerializerOptions);

            this._logger.LogInformation(
                "GET {} response: {}",
                reqUrl,
                JsonSerializer.Serialize(
                    githubUser, this._options.Value.GithubJsonSerializerOptions)
            );

            if (githubUser is null)
            {
                throw new Exception("Could not get user information");
            }

            return githubUser;
        }

        public async Task<IEnumerable<GithubRepoResponse>> GetUserRepositoriesAsync(string user)
        {
            string reqUrl = "/users/" + user + "/repos";

            var res = await this._httpClient.GetAsync(reqUrl);

            res.EnsureSuccessStatusCode();

            var repos =
                await res.Content.ReadFromJsonAsync<IEnumerable<GithubRepoResponse>>(
                    this._options.Value.GithubJsonSerializerOptions);

            this._logger.LogInformation(
                "GET {} response: {}",
                reqUrl,
                JsonSerializer.Serialize(repos, this._options.Value.GithubJsonSerializerOptions)
            );

            if (repos == null)
            {
                throw new Exception("Could not get the user repositories");
            }

            return repos;
        }
    }
}
