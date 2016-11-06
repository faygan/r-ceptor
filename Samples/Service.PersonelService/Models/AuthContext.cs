using Newtonsoft.Json;

namespace Service.PersonelService.Models
{
    public class AuthContext
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("pass")]
        public string Pass { get; set; }
    }
}