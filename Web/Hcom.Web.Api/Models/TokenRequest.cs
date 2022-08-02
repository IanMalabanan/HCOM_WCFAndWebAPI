using System.ComponentModel.DataAnnotations;

namespace Hcom.Web.Api.Models
{
    public class TokenRequest
    {
        [Required]
        public string Token { get; set; }

         [Required]
        public string RefreshToken { get; set; }
    }
}