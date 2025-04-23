namespace Meishi.Server.Services.GithubAuth.Models
{
    public class GithubAccessTokenResponse
    {
        private readonly DateTime _createdAt;

        public GithubAccessTokenResponse()
        {
            this._createdAt = DateTime.Now;
        }

        public string AccessToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public DateTime ExpiresInDate
        {
            get => this._createdAt.AddSeconds(this.ExpiresIn);
        }

        public string RefreshToken { get; set; } = string.Empty;
        public int RefreshTokenExpiresIn { get; set; }
        public DateTime RefreshTokenExpiresInDate
        {
            get => this._createdAt.AddSeconds(this.RefreshTokenExpiresIn);
        }

        public string Scope { get; set; } = string.Empty;
        public string TokenType { get; set; } = string.Empty;
    }
}
