using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Web;
using System.Net.Mail;
using Hcom.Web.Api.Core;
using Hcom.Web.Api.Models;
using Hcom.Web.Api.ViewModels;
using Hcom.Web.Api.Utilities.Security;
using AutoMapper;
using Hcom.Web.Api.Interface;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Polly;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using Hcom.Web.Api.Utilities;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace Hcom.Web.Api.Controllers
{

    [ApiExplorerSettings(GroupName = "AUTH")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly IFrebasChangeForgotPasswordAPIService _frebasChangeForgotPasswordAPIService;

        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private const string GetUserByIdActionName = "GetUserById";
        private const string GetRoleByIdActionName = "GetRoleById";
        protected readonly ILogger<object> _logger;
        private readonly IUser _user;
        
        
        UserManager<ApplicationUser> _userManager;

        public AccountController(IAccountManager accountManager,
                           IAuthorizationService authorizationService,
                           ILogger<AccountController> logger,
                           UserManager<ApplicationUser> userManager,
                           IFrebasChangeForgotPasswordAPIService frebasChangeForgotPasswordAPIService,
                           IMapper mapper,
                           IUser user)
        {
            _accountManager = accountManager;
            _authorizationService = authorizationService;
            _frebasChangeForgotPasswordAPIService = frebasChangeForgotPasswordAPIService;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _user = user;
        }

        #region API Route
        
        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAccount([FromBody] RegisterViewModel model)
        {
            try
            {
                var user = _mapper.Map<RegisterViewModel, ApplicationUser>(model);

                user.UserName = model.UserName;
                user.Email = model.Email;


                //Create Role(if not yet created)

                var roleName = model.Role;
                if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
                {
                    ApplicationRole applicationRole = new ApplicationRole(roleName, roleName);

                    var result = await this._accountManager.CreateRoleAsync(applicationRole, new string[] { });

                    if (!result.Item1)
                        throw new Exception($"Error creating role");
                }



                //Enable User
                user.IsEnabled = true;

                var _result = await _accountManager.CreateUserAsync(user, new string[] { roleName }, model.NewPassword);

                if (_result.Item1)
                {
                    return Ok(model);
                }
                else
                {
                    foreach (var error in _result.Item2)
                    {
                        ModelState.AddModelError("Registration", error);
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
                return BadRequest(ErrorMessageUtil.GetFullExceptionMessage(e));
            }
        }

        [Route("enablesuperuser")]
        [HttpGet]
        public async Task<IActionResult> EnableSuperUser()
        {
            try
            {
                const string adminRoleName = "administrator";

                await EnsureRoleAsync(adminRoleName, "Global Administrator", ApplicationPermissions.GetAllPermissionValues());
                await CreateUserAsync("admin", "!P@ssw0rd123cti4lyf", "Inbuilt Administrator", "appsdev@filinvestland.com", "+1 (123) 000-0000", new string[] { adminRoleName
                });
                return Ok("Use it wisely");
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Null Reference Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (ApplicationException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Application Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (Exception ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Function Error: {errMsg}");
                return BadRequest(errMsg);
            }
        }


        [AllowAnonymous]
        [Route("passwordreset")]
        [Authorize(Policies.ManageAllUsersPolicy)]
        [HttpGet]
        public async Task<IActionResult> ResetPassword([FromQuery] string Username, [FromQuery] string NewPassword)
        {
            try
            {
                var CurrentUser = User.Claims;
                var user = await _userManager.FindByEmailAsync(Username) ?? await _userManager.FindByNameAsync(Username);
                if (user == null)
                {
                    var frebasUser = await _user.GetUserAsync(Username);
                    if (frebasUser == null)
                        throw new ApplicationException("User does not exists");

                    var roleName = "FREBAS_HCOM";
                    if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole(roleName, roleName);

                        var result1 = await this._accountManager.CreateRoleAsync(applicationRole, new string[] { });

                        if (!result1.Item1)
                            throw new ApplicationException($"Error creating role");
                    }

                    var x = await _accountManager.CreateUserAsync(new ApplicationUser()
                    {
                        UserName = Username,
                        IsEnabled = true,
                        Email = (frebasUser.Email == null) ? Username + "@filinvestland.com" : frebasUser.Email,

                    }, new string[] { roleName }, "HcomP@ssw0rd123");

                    if (x.Item1 == false)
                    {
                        var errMsg = x.Item2[0].ToString();
                        throw new ApplicationException(errMsg);
                    }

                    user = await _userManager.FindByEmailAsync(Username) ?? await _userManager.FindByNameAsync(Username);
                }

                var _result = await _accountManager.ResetPasswordAsync(user, NewPassword);

                if (!_result.Item1)
                    throw new ApplicationException(string.Join(",", _result.Item2));

                if (_result.Item1)
                {
                    DateTime myDateTime = DateTime.Now;
                    string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");

                    Claim claim = new Claim("PasswordLastUpdateDate", sqlFormattedDate, ClaimValueTypes.String);

                    await _userManager.AddClaimAsync(user, claim);

                    return Ok("Reset Successful!");
                }
                else
                {
                    foreach (var error in _result.Item2)
                    {
                        ModelState.AddModelError("Password Reset", error);
                    }
                }
                return BadRequest(ModelState);
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Null Reference Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (ApplicationException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Application Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (Exception ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Function Error: {errMsg}");
                return BadRequest(errMsg);
            }


        }


        [AllowAnonymous]
        //[Authorize(Policies.ManageAllUsersPolicy)]
        [Route("passwordreset")]
        [HttpPost]
        public async Task<IActionResult> PasswordReset([FromQuery] string Username, [FromQuery] string NewPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(Username) ?? await _userManager.FindByNameAsync(Username);
                if (user == null)
                {
                    var frebasUser = await _user.GetUserAsync(Username);
                    if (frebasUser == null)
                        throw new ApplicationException("User does not exists");

                    var roleName = "FREBAS_HCOM";
                    if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole(roleName, roleName);

                        var result1 = await this._accountManager.CreateRoleAsync(applicationRole, new string[] { });

                        if (!result1.Item1)
                            throw new ApplicationException($"Error creating role");
                    }

                    var x = await _accountManager.CreateUserAsync(new ApplicationUser()
                    {
                        UserName = Username,
                        IsEnabled = true,
                        Email = (frebasUser.Email == null) ? Username + "@filinvestland.com" : frebasUser.Email,

                    }, new string[] { roleName }, "HcomP@ssw0rd123");

                    if (x.Item1 == false)
                    {
                        var errMsg = x.Item2[0].ToString();
                        throw new ApplicationException(errMsg);
                    }

                    user = await _userManager.FindByEmailAsync(Username) ?? await _userManager.FindByNameAsync(Username);
                }

                var _result = await _accountManager.ResetPasswordAsync(user, NewPassword);

                if (!_result.Item1)
                    throw new ApplicationException(string.Join(",", _result.Item2));

                var _frebasResult = await _frebasChangeForgotPasswordAPIService.ForgotPassword(Username, NewPassword);

                if (!_frebasResult.Item1)
                    throw new ApplicationException(string.Join(",", _frebasResult.Item2));

                DateTime myDateTime = DateTime.Now;
                string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");

                Claim claim = new Claim("PasswordLastUpdateDate", sqlFormattedDate, ClaimValueTypes.String);

                await _userManager.AddClaimAsync(user, claim);

                return Ok("Reset Successful!");
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Null Reference Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (ApplicationException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Application Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (Exception ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Function Error: {errMsg}");
                return BadRequest(errMsg);
            }
        }

        

        [AllowAnonymous]
        [Authorize(Policies.ManageAllUsersPolicy)]
        [Route("passwordchange")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromQuery] string Username, [FromQuery] string OldPassword, [FromQuery] string NewPassword)
        {
            try
            {   
                var user = await _userManager.FindByEmailAsync(Username) ?? await _userManager.FindByNameAsync(Username);
                if (user == null)
                {
                    var frebasUser = await _user.GetUserAsync(Username);
                    if (frebasUser == null)
                        throw new ApplicationException("User does not exists");

                    var roleName = "FREBAS_HCOM";
                    if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole(roleName, roleName);

                        var result1 = await this._accountManager.CreateRoleAsync(applicationRole, new string[] { });

                        if (!result1.Item1)
                            throw new ApplicationException($"Error creating role");
                    }

                    var x = await _accountManager.CreateUserAsync(new ApplicationUser()
                    {
                        UserName = Username,
                        IsEnabled = true,
                        Email = (frebasUser.Email == null) ? Username + "@filinvestland.com" : frebasUser.Email,

                    }, new string[] { roleName }, "HcomP@ssw0rd123");

                    if (x.Item1 == false)
                    {
                        var errMsg = x.Item2[0].ToString();
                        throw new ApplicationException(errMsg);
                    }

                    user = await _userManager.FindByEmailAsync(Username) ?? await _userManager.FindByNameAsync(Username);
                }

                var _result = await _accountManager.ResetPasswordAsync(user, NewPassword);

                if (!_result.Item1)
                    throw new ApplicationException(string.Join(",", _result.Item2));

                var _frebasResult = await _frebasChangeForgotPasswordAPIService.ChangeUserPassword(Username, OldPassword, NewPassword);
                if (!_frebasResult.Item1)
                    throw new ApplicationException(string.Join(",", _frebasResult.Item2));

                DateTime myDateTime = DateTime.Now;
                string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");

                Claim claim = new Claim("PasswordLastUpdateDate", sqlFormattedDate, ClaimValueTypes.String);

                await _userManager.AddClaimAsync(user, claim);

                return Ok("Change Password Successful!");
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Null Reference Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (ApplicationException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Application Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (Exception ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Function Error: {errMsg}");
                return BadRequest(errMsg);
            }
        }

        [AllowAnonymous]
        [Authorize(Policies.ManageAllUsersPolicy)]
        [Route("verifyuser")]
        [HttpGet]
        public async Task<IActionResult> VerifyUsernameIfExist([FromQuery] string Username)
        {
            try
            {
                ApplicationUser user = new ApplicationUser();

                var userdata = await _userManager.FindByEmailAsync(Username) ?? await _userManager.FindByNameAsync(Username);

                if (userdata == null)
                {
                    var frebasUser = await _user.GetUserAsync(Username);
                    if (frebasUser == null)
                        throw new ApplicationException("User does not exists");

                    var roleName = "FREBAS_HCOM";
                    if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole(roleName, roleName);

                        var result1 = await this._accountManager.CreateRoleAsync(applicationRole, new string[] { });

                        if (!result1.Item1)
                            throw new ApplicationException($"Error creating role");
                    }

                    var x = await _accountManager.CreateUserAsync(new ApplicationUser()
                    {
                        UserName = Username,
                        IsEnabled = true,
                        Email = (frebasUser.Email == null) ? Username + "@filinvestland.com" : frebasUser.Email,

                    }, new string[] { roleName }, "HcomP@ssw0rd123");

                    if (x.Item1 == false)
                    {
                        var errMsg = x.Item2[0].ToString();
                        throw new ApplicationException(errMsg);
                    }

                    userdata = await _userManager.FindByEmailAsync(Username) ?? await _userManager.FindByNameAsync(Username);
                }

                var _result = await _frebasChangeForgotPasswordAPIService.GetOTP(Username);

                if (_result != "Success")
                    throw new ApplicationException(_result);

                return Ok("One - Time Password (OTP) has been sent to your registered email address.");
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Null Reference Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (ApplicationException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Application Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (Exception ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Function Error: {errMsg}");
                return BadRequest(errMsg);
            }
        }

        [AllowAnonymous]
        [Authorize(Policies.ManageAllUsersPolicy)]
        [Route("validateotp")]
        [HttpGet]
        public async Task<IActionResult> ValidateOTPCode([FromQuery] string Username, [FromQuery] string OTP)
        {
            try
            {
                var _result = await _frebasChangeForgotPasswordAPIService.ValidateOTP(Username,OTP);
                if (!_result.Item1)
                    throw new Exception("Invalid OTP Code");

                return Ok("Valid OTP Code");
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Null Reference Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (ApplicationException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Application Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (Exception ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Function Error: {errMsg}");
                return BadRequest(errMsg);
            }
        }
       
        [Route("getlastpassworddatechange")]
        [HttpGet]
        public async Task<IActionResult> GetUserClaims()
        {
            try
            {
               var userclaim = new IdentityUserClaim<string>();

                var Id = _userManager.GetUserId(HttpContext.User);

                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (!string.IsNullOrEmpty(Id))
                {
                    var data = _userManager.GetClaimsAsync(user).Result.ToList().Where(x => x.Type == "PasswordLastUpdateDate").LastOrDefault();

                    if (data != null)
                    {
                        userclaim.UserId = Id;

                        userclaim.ClaimType = data.Type;

                        userclaim.ClaimValue = data.Value;
                    }
                    else
                    {
                        userclaim = new IdentityUserClaim<string>();
                    }

                }
                else
                {
                    userclaim = new IdentityUserClaim<string>();
                }

                return Ok(userclaim);
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Null Reference Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (ApplicationException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Application Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (Exception ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Function Error: {errMsg}");
                return BadRequest(errMsg);
            }
        }




        //[Authorize(Policies.ManageAllUsersPolicy)]
        //[Route("resetpassword")]
        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(string Username, string NewPassword)
        //{
        //    try
        //    {
        //        var CurrentUser = User.Claims;
        //        var user = await _userManager.FindByEmailAsync(Username) ?? await _userManager.FindByNameAsync(Username);
        //        if (user == null)
        //            return StatusCode(222, "User does not exists!");

        //        var _result = await _accountManager.ResetPasswordAsync(user, NewPassword);

        //        if (_result.Item1)
        //        {
        //            return Ok("Reset Successful!");
        //        }
        //        else
        //        {
        //            foreach (var error in _result.Item2)
        //            {
        //                ModelState.AddModelError("Password Reset", error);
        //            }
        //        }
        //        return BadRequest(ModelState);
        //    }
        //    catch (Exception e)
        //    {

        //        _logger.LogError(e.StackTrace);
        //        return BadRequest("Internal Error found");
        //    }


        //}

        //[Authorize(Policies.ManageAllUsersPolicy)]
        [AllowAnonymous]
        [Route("activateuser")]
        [HttpGet]
        public async Task<IActionResult> Enableuser([FromQuery] string Username)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(Username) ?? await _userManager.FindByNameAsync(Username);
                if (user == null)
                    return StatusCode(222, "User does not exists!");
                user.IsEnabled = true;
                var _result = await _accountManager.UpdateUserAsync(user);


                if (_result.Item1)
                {
                    return Ok("Reset Successful!");
                }
                else
                {
                    foreach (var error in _result.Item2)
                    {
                        ModelState.AddModelError("User activated", error);
                    }
                }
                return BadRequest(ModelState);
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Null Reference Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (ApplicationException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Application Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (Exception ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Function Error: {errMsg}");
                return BadRequest(errMsg);
            }
        }

        [Authorize(Policies.ManageAllUsersPolicy)]
        [Route("deactivateuser")]
        [HttpPost]
        public async Task<IActionResult> Deactivateuser(string Username)
        {
            try
            {

                var user = await _userManager.FindByEmailAsync(Username) ?? await _userManager.FindByNameAsync(Username);
                if (user == null)
                    return StatusCode(222, "User does not exists!");
                user.IsEnabled = false;
                var _result = await _accountManager.UpdateUserAsync(user);



                if (_result.Item1)
                {
                    return Ok("Reset Successful!");
                }
                else
                {
                    foreach (var error in _result.Item2)
                    {
                        ModelState.AddModelError("User deactivated", error);
                    }
                }
                return BadRequest(ModelState);
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Null Reference Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (ApplicationException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Application Error: {errMsg}");
                return BadRequest(errMsg);
            }
            catch (Exception ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                _logger.LogError($"Function Error: {errMsg}");
                return BadRequest(errMsg);
            }


        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _accountManager.GetRolesLoadRelatedAsync(-1, 1);
            return Ok(_mapper.Map<List<RoleViewModel>>(roles));
        }
        
        [HttpGet("users/{pageNumber:int}/{pageSize:int}")]
        [ProducesResponseType(200, Type = typeof(List<UserViewModel>))]
        public async Task<IActionResult> GetUsers(int pageNumber, int pageSize)
        {
            var usersAndRoles = await _accountManager.GetUsersAndRolesAsync(pageNumber, pageSize);

            List<UserViewModel> usersVM = new List<UserViewModel>();

            foreach (var item in usersAndRoles)
            {
                var userVM = _mapper.Map<UserViewModel>(item.Item1);
                userVM.Roles = item.Item2;

                usersVM.Add(userVM);
            }

            return Ok(usersVM);
        }

        #endregion


        #region Methods

        private void AddErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
            {
                ApplicationRole applicationRole = new ApplicationRole(roleName, description);

                var result = await this._accountManager.CreateRoleAsync(applicationRole, claims);

                if (!result.Item1)
                    throw new Exception($"Creating \"{description}\" role failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(string userName, string password, string fullName, string email, string phoneNumber, string[] roles)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,
                IsEnabled = true,
                CreatedBy = "Initializer"
            };

            var result = await _accountManager.CreateUserAsync(applicationUser, roles, password);

            if (!result.Item1)
                throw new Exception($"Creating \"{userName}\" user failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");


            return applicationUser;
        }

        #endregion
    }
}

