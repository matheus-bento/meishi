namespace Meishi.Server.Services.GithubApi.Models
{
    public class GithubRepoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string HtmlUrl { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public bool Private { get; set; } = false;
        public int StargazersCount { get; set; }
        public int WatchersCount { get; set; }
    }
}
