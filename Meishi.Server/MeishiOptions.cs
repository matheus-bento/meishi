namespace Meishi.Server
{
    public class MeishiOptions
    {
        [ConfigurationKeyName("CLIENT_ID")]
        public string ClientId { get; set; } = string.Empty;

        [ConfigurationKeyName("CLIENT_SECRET")]
        public string ClientSecret { get; set; } = string.Empty;
    }
}
