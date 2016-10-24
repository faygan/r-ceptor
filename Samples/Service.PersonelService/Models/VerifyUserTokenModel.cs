using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Service.PersonelService.Models
{
    public class VerifyUserTokenModel
    {

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("access_token")]
        public string Token { get; set; }

    }
}