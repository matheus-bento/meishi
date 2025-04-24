namespace Meishi.Server.Services.GithubApi.Models
{
    public class GithubUserResponse
    {
        public int Id { get; set; }
        public string Login { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;
        public string Company { get; set; } = String.Empty;
        public string NotificationEmail { get; set; } = String.Empty;
        public string Location { get; set; } = String.Empty;
        public string Bio { get; set; } = String.Empty;
        public int Followers { get; set; }
    }
}
