using Newtonsoft.Json;

namespace MvcRicardoNogales.Models
{
    public class LoginDTO
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
