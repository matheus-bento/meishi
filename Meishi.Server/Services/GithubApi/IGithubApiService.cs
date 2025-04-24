using Meishi.Server.Services.GithubApi.Models;

namespace Meishi.Server.Services.GithubApi
{
    public interface IGithubApiService
    {
        /// <summary>
        ///       Gets information about the authenticated github user
        /// </summary>
        Task<GithubUserResponse> GetUserInfoAsync(string user);

        /// <summary>
        ///      Gets the repositories owned by the authenticated github user
        /// </summary>
        Task<IEnumerable<GithubRepoResponse>> GetUserRepositoriesAsync(string user);
    }
}
