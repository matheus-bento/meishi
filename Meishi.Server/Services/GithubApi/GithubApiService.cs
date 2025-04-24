using System.Text.Json;
using Meishi.Server.Services.GithubApi.Models;

namespace Meishi.Server.Services.GithubApi
{
    /// <summary>
    ///     Typed HTTP Client to the GitHub API and get information about a
    ///     specified user and their repositories
    /// </summary>
    public class GithubApiService : IGithubApiService
    {
        private readonly HttpClient _httpClient;

        public GithubApiService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<GithubUserResponse> GetUserInfoAsync(string user)
        {
            var res = await this._httpClient.GetAsync($"/users/{user}");

            res.EnsureSuccessStatusCode();

            var githubUser =
                await res.Content.ReadFromJsonAsync<GithubUserResponse>(
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                    }
                );

            if (githubUser is null)
            {
                throw new Exception("Could not get user information");
            }

            return githubUser;
        }

        public async Task<IEnumerable<GithubRepoResponse>> GetUserRepositoriesAsync(string user)
        {
            var res = await this._httpClient.GetAsync($"/users/{user}/repos");

            res.EnsureSuccessStatusCode();

            var repos =
                await res.Content.ReadFromJsonAsync<IEnumerable<GithubRepoResponse>>(
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                    }
                );

            if (repos == null)
            {
                throw new Exception("Could not get user repositories");
            }

            return repos;
        }
    }
}
