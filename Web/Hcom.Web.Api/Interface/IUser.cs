using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserEP = UserSvcRefence;
using Crtr = ContractorSvcReference;

namespace Hcom.Web.Api.Interface
{
    public interface IUser
    {
        Task<UserEP.User> AuthenticateUserAsync(string username, string password);
        Task<App.Entities.User> GetUserAsync(string username);
        Task<Crtr.Contractor> GetVendorByRepresentativeAsync(string username);
        Task<string> GetUserFromIdentity(ClaimsPrincipal principal);
    }
}
