using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Interface
{
    public interface IFrebasChangeForgotPasswordAPIService
    {
        Task<Tuple<bool, string>> ChangeUserPassword(string userName, string oldPassword, string newPassword);
        Task<Tuple<bool, string>> ForgotPassword(string userName, string newPassword);
        Task<string> GetOTP(string userName);
        Task<Tuple<bool, string>> ValidateOTP(string userName, string otp);
    }
}
