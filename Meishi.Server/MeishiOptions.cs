using System.Text.Json;

namespace Meishi.Server
{
    public class MeishiOptions
    {
        private static JsonSerializerOptions? _githubJsonSerializerOptions;

        public JsonSerializerOptions GithubJsonSerializerOptions
        {
            get {
                if (_githubJsonSerializerOptions == null)
                {
                    _githubJsonSerializerOptions = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                        IgnoreReadOnlyProperties = true,
                        IgnoreReadOnlyFields = true
                    };
                }

                return _githubJsonSerializerOptions;
            }
        }
    }
}
