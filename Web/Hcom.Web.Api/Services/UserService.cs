using Hcom.Web.Api.Core;
using Hcom.Web.Api.Interface;
using Hcom.Web.Api.Models;
using Hcom.Web.Api.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserEP = UserSvcRefence;
using Crtr = ContractorSvcReference;
using Hcom.App.Entities;
using Hcom.App.Entities.Models;

namespace Hcom.Web.Api.Services
{
    public class UserService : IUser
    {
        private readonly IAccountManager _accountManager;
        private readonly ServiceEndpoints _endpointSettings;
        private UserEP.IUserService _userServices;
        private Crtr.IContractorService _crtrServices;
        public const string contractorServiceEndpoint = "ContractorService.svc";
        public const string userServiceEndpoint = "UserService.svc";
        UserManager<ApplicationUser> _userManager;

        public UserService(IOptionsMonitor<ServiceEndpoints> endpointAccessor, IAccountManager accountManager, UserManager<ApplicationUser> userManager)
        {
            _endpointSettings = endpointAccessor.CurrentValue;

            _userServices = new UserEP.UserServiceClient(UserEP.UserServiceClient.EndpointConfiguration.BasicHttpBinding_IUserService,
                _endpointSettings.BaseUrl + userServiceEndpoint);

            _crtrServices = new Crtr.ContractorServiceClient(Crtr.ContractorServiceClient.EndpointConfiguration.BasicHttpBinding_IContractorService,
                _endpointSettings.BaseUrl + contractorServiceEndpoint);

            _accountManager = accountManager;

            _userManager = userManager;
        }

        public Task<string> GetUserFromIdentity(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);

            var userId = claim != null ? claim.Value.ToString() : null;

            var _userName = _userManager.FindByIdAsync(userId).Result?.UserName;

            return Task.FromResult(_userName);
        }


        public async Task<UserEP.User> AuthenticateUserAsync(string username, string password)
        {
            try
            {
                return await _userServices.AuthenticateUserAsync(username, password);
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
            catch (ApplicationException ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<User> GetUserAsync(string username)
        {
            try
            {
                var userfreb = (await _userServices.GetUserAsync(username.ToUpper()));

                if (userfreb == null)
                    return null;
                User frebasUser = new User
                {
                    Email = userfreb.Email,
                    FirstName = userfreb.FullName,
                    Id = userfreb.Id,
                    Role = new Role
                    {
                        id = userfreb.RoleCode,
                        Type = DataMapping.MapRoleByCode(userfreb.RoleCode),
                        Radius = userfreb.InspectionRadius ?? 0
                    },
                    inspectionRadius = (double)userfreb.InspectionRadius,
                    //Role = new App.Entities.Models.Role
                    //{
                    //    id = user.CreatedBy.RoleCode,
                    //    Type = DataMapping.MapRoleByCode(com.CreatedBy.RoleCode)
                    //},  
                };

                if (userfreb.IsActive == false)
                {
                    var user = await _userManager.FindByEmailAsync(username) ?? await _userManager.FindByNameAsync(username);
                    if (user != null)
                    {
                        user.IsEnabled = false;
                        var _result = await _accountManager.UpdateUserAsync(user);
                    }                    
                }


                return frebasUser;
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
            catch (ApplicationException ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<Crtr.Contractor> GetVendorByRepresentativeAsync(string username)
        {
            try
            {
                var contractor = (await _crtrServices.GetVendorByRepresentativeAsync(username));

                return contractor;
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
            catch (ApplicationException ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
