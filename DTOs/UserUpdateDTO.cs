using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthKalumManagement.DTOs
{
    public class UserUpdateDTO
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [JsonPropertyName("normalizedusername")]
        public string NormalizedUserName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}