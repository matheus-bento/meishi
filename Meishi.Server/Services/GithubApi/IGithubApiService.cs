namespace Meishi.Server.Services.GithubApi
{
    public interface IGithubApiService
    {
        /// <summary>
        ///       Gets information about the authenticated github user
        /// </summary>
        Task GetUserInfoAsync();

        /// <summary>
        ///      Gets the repositories owned by the authenticated github user
        /// </summary>
        Task GetUserRepositoriesAsync();
    }
}
