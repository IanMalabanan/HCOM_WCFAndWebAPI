using System.Collections.Generic;

namespace Hcom.Web.Api.Models
{
    public class AuthResult
    {
        public string access_token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}