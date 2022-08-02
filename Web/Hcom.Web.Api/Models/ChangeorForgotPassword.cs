using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Models
{
    public class ChangeorForgotPassword
    {
        public string confirmPassword { get; set; }
        public string deviceID { get; set; }
        public string deviceUsed { get; set; }
        public string environment { get; set; }
        public string generatedOTP { get; set; }
        public Boolean isResend { get; set; }
        public string location { get; set; }
        public string newPassword { get; set; }
        public string oldPassword { get; set; }
        public string sessionID { get; set; }
        public string userName { get; set; }

    }
}
