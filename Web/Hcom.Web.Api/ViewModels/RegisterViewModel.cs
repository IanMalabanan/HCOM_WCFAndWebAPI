using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string ContactNo { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
