using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthKalumManagement.DTOs
{
    public class UserAddRoleDTO
    {
        [JsonPropertyName("idUser")]
        public string IdUser { get; set; }
        [JsonPropertyName("role")]
        public string Role { get; set; }
    }
}