using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNet.Security.OpenIdConnect.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using AspNet.Security.OpenIdConnect.Server;
using OpenIddict.Core;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using Novell.Directory.Ldap;
using Hcom.Web.Api.Models;
using Hcom.Web.Api.Core;
using Hcom.Web.Api.Utilities.Security;
using Hcom.Web.Api.Utilities;
using UserSvcRefence;
using System.Diagnostics;
using Hcom.Web.Api.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Cti.Auth.Controllers
{

    [ApiExplorerSettings(GroupName = "AUTH")]
    public class AuthorizationController : Controller
    {
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountManager _accountManager;
        protected readonly ILogger<object> _logger;
        private readonly IUser _user;
        private readonly JwtConfig _jwtConfig;
        private readonly ITokenService _tokenService;

        

        public AuthorizationController(
                IOptions<IdentityOptions> identityOptions,
                SignInManager<ApplicationUser> signInManager,
                UserManager<ApplicationUser> userManager,
                IConfiguration config,
                ILogger<AuthorizationController> logger,
                IAccountManager accountManager,
                IUser user,
                IOptionsMonitor<JwtConfig> optionsMonitor,
                ITokenService tokenService)
        {
            _identityOptions = identityOptions;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _accountManager = accountManager;
            _user = user;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenService = tokenService;
            
        }

        private bool ValidateUser(string username, string password)
        {
            string userDn = $"{username}@flidn.filinvestland.com";
            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    connection.Connect("flidn.filinvestland.com", LdapConnection.DEFAULT_PORT);
                    connection.Bind(userDn, password);
                    if (connection.Bound)
                        return true;
                }
            }
            catch (LdapException ex)
            {
                // Log exception
            }
            return false;
        }


        [HttpPost]
        [Route("connect/token")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var existingUser = await _userManager.FindByNameAsync(loginRequest.Username);

                if (existingUser == null)
                {
                    //Check Frebas
                    var frebasAcct = await _user.AuthenticateUserAsync(loginRequest.Username, loginRequest.Password);

                    if (frebasAcct == null)
                        return BadRequest(new TransactionResponse()
                        {
                            Errors = new List<string>() {
                                        "Please check that your email and password is correct"
                                    },
                            Success = false
                        });


                    var roleName = "FREBAS_HCOM";
                    if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole(roleName, roleName);

                        var result1 = await _accountManager.CreateRoleAsync(applicationRole, new string[] { });

                        if (!result1.Item1)
                            throw new Exception($"Error creating role");
                    }

                    var newUser = await _accountManager.CreateUserAsync(new ApplicationUser()
                    {
                        UserName = loginRequest.Username,
                        IsEnabled = true,
                        Email = (frebasAcct.Email == null) ? loginRequest.Username + "@filinvestland.com" : frebasAcct.Email,

                    }, new string[] { roleName }, loginRequest.Password);

                    //check if the creation of user is successful
                    if (newUser.Item1 == false)
                    {
                        var errMsg = newUser.Item2[0].ToString();
                        return BadRequest(new TransactionResponse()
                        {
                            Errors = new List<string>() {
                                        newUser.Item2[0].ToString()
                                    },
                            Success = false
                        });
                    }

                    existingUser = await _userManager.FindByNameAsync(loginRequest.Username);

                    var isCorrect = await _userManager.CheckPasswordAsync(existingUser, loginRequest.Password);

                    if (!isCorrect)
                    {
                        return BadRequest(new TransactionResponse()
                        {
                            Errors = new List<string>() {
                                "Please check that your username and password is correct"
                            },
                            Success = false
                        });
                    }

                }
                else
                {
                    // Ensure the user is enabled.
                    if (!existingUser.IsEnabled)
                    {
                        return BadRequest(new TransactionResponse()
                        {
                            Errors = new List<string>() {
                                        "The specified user account is disabled"
                                    },
                            Success = false
                        });
                    }

                    // Validate the username/password parameters and ensure the account is not locked out.
                    var result = await _signInManager.CheckPasswordSignInAsync(existingUser, loginRequest.Password, true);

                    // Ensure the user is not already locked out.
                    if (result.IsLockedOut)
                    {
                        return BadRequest(new TransactionResponse()
                        {
                            Errors = new List<string>() {
                                        "The specified user account has been suspended"
                                    },
                            Success = false
                        });
                    }

                    // Reject the token request if two-factor authentication has been enabled by the user.
                    if (result.RequiresTwoFactor)
                    {
                        return BadRequest(new TransactionResponse()
                        {
                            Errors = new List<string>() {
                                        "Invalid login procedure"
                                    },
                            Success = false
                        });
                    }

                    // Ensure the user is allowed to sign in.
                    if (result.IsNotAllowed)
                    {
                        return BadRequest(new TransactionResponse()
                        {
                            Errors = new List<string>() {
                                        "The specified user is not allowed to sign in"
                                    },
                            Success = false
                        });
                    }

                    if (!result.Succeeded)
                    {
                        return BadRequest(new TransactionResponse()
                        {
                            Errors = new List<string>() {
                                "Please check that your username and password is correct"
                            },
                            Success = false
                        });
                    }
                }

                var jwtToken = await _tokenService.GenerateJwtToken(existingUser);

                return Ok(new TransactionResponse
                {
                    access_token = jwtToken.access_token,
                    Success = true,
                });

            }
            catch (Exception)
            {
                return BadRequest(new TransactionResponse()
                {
                    Errors = new List<string>() {
                        "Please make sure your account is valid and allowed to access the system"
                    },
                    Success = false
                });
            }
        }
    }
}


