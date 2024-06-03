using Newtonsoft.Json;

namespace Core.DTOs.User.Response
{
    public class FacebookAuthenticationPayload
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty("last_name")]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}
