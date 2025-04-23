namespace Meishi.Server.Services.GithubApi
{
    public interface IGithubApiService
    {
        /// <summary>
        ///       Gets information about the authenticated github user
        /// </summary>
        Task GetUserInfoAsync();

        /// <summary>
        ///      Gets the repositories of the specified github user
        /// </summary>
        /// <param name="user">The username of the github user that will have their repositories listed</param>
        Task GetUserRepositoriesAsync(string user);
    }
}
