
using System.Net.Http.Headers;
using Meishi.Server.Services.GithubAuth;
using Meishi.Server.Services.GithubAuth.Models;

namespace Meishi.Server.Services.GithubApi
{
    public class GithubApiService : IGithubApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IGithubAuthService _githubAuthService;

        private readonly string _code;

        private string _accessToken;
        private DateTime _expiresIn;

        private string _refreshToken;
        private DateTime _refreshTokenExpiresIn;

        /// <summary>
        ///     Accesses GitHub API to get information about the user and their repositories
        /// </summary>
        /// <param name="httpClientFactory">HttpClient factory configured to access GitHub's API</param>
        /// <param name="githubAuthService">Typed HttpClient to get and refresh the access token for the API</param>
        /// <param name="code">The code returned by GitHub after the user authorizes the application</param>
        public GithubApiService(
            IHttpClientFactory httpClientFactory,
            IGithubAuthService githubAuthService,
            string code)
        {
            this._httpClientFactory = httpClientFactory;
            this._githubAuthService = githubAuthService;

            this._code = code;

            this._accessToken = string.Empty;
            this._expiresIn = DateTime.MinValue;

            this._refreshToken = string.Empty;
            this._refreshTokenExpiresIn = DateTime.MinValue;
        }

        /// <summary>
        ///       Verifies the access token and refreshes it if necessary
        /// </summary>
        private async Task VerifyToken()
        {
            if (string.IsNullOrEmpty(this._accessToken))
            {
                GithubAccessTokenResponse accessTokenResponse =
                    await this._githubAuthService.GetAccessTokenAsync(this._code);

                this._accessToken = accessTokenResponse.AccessToken;
                this._expiresIn = accessTokenResponse.ExpiresInDate;

                this._refreshToken = accessTokenResponse.RefreshToken;
                this._refreshTokenExpiresIn = accessTokenResponse.RefreshTokenExpiresInDate;
            }

            if (DateTime.Now >= this._expiresIn)
            {
                if (DateTime.Now >= this._refreshTokenExpiresIn)
                {
                    throw new Exception("Could not refresh the access token because the refresh token is also expired");
                }

                GithubAccessTokenResponse refreshedTokenResponse =
                    await this._githubAuthService.RefreshTokenAsync(this._refreshToken);

                this._accessToken = refreshedTokenResponse.AccessToken;
                this._expiresIn = refreshedTokenResponse.ExpiresInDate;

                this._refreshToken = refreshedTokenResponse.RefreshToken;
                this._refreshTokenExpiresIn = refreshedTokenResponse.RefreshTokenExpiresInDate;
            }
        }

        public async Task GetUserInfoAsync()
        {
            await this.VerifyToken();

            var httpClient = this._httpClientFactory.CreateClient("GithubApi");

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", this._accessToken);

            // TODO: Map the response to an object and return it
            var res = await httpClient.GetAsync("/user");

            res.EnsureSuccessStatusCode();
        }

        public async Task GetUserRepositoriesAsync(string user)
        {
            await this.VerifyToken();

            var httpClient = this._httpClientFactory.CreateClient("GithubApi");

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", this._accessToken);

            // TODO: Map the response to an object and return it
            var res = await httpClient.GetAsync($"/users/{user}/repos");

            res.EnsureSuccessStatusCode();
        }
    }
}
