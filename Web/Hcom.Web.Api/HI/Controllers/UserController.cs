using Hcom.App.Entities;
using Hcom.Web.Api.Core;
using Hcom.Web.Api.Models;
using Hcom.Web.Api.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserSvcRefence;
using Microsoft.Extensions.Logging;
using Hcom.Web.Api.Interface;
using System.ServiceModel;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Hcom.Web.Api.HI.Controllers
{

    [ApiExplorerSettings(GroupName = "HCOM")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/hi")]
    public class UserController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUser _user;
        UserManager<ApplicationUser> _userManager;

        public UserController(ILogger<UserController> logger, IUser user, UserManager<ApplicationUser> userManager)
        {
            _user = user;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet("User")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _user.GetUserAsync(_user.GetUserFromIdentity(HttpContext.User).Result);
                return Ok(users);
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
        [HttpGet("frebasuser/{username}")]
        public async Task<IActionResult> GetFrebasUserByUserName(string username)
        {
            try
            {
                var usr = new App.Entities.User();
                var users = await _user.GetUserAsync(username);

                if (users == null)
                    return BadRequest(usr);

                return Ok(users);
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
        [HttpGet("hiuser/{username}")]
        public async Task<IActionResult> GetHIUserByUserName(string username)
        {
            try
            {
                var usr = new ApplicationUser();
                var user = await _userManager.FindByEmailAsync(username) ?? await _userManager.FindByNameAsync(username);

                if (user == null)
                    return BadRequest(usr);
                
                return Ok(user);
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
        [HttpGet("vendor/{username}")]
        public async Task<IActionResult> GetVendorByRepresentativeAsync(string username)
        {
            try
            {
                var user = await _user.GetVendorByRepresentativeAsync(username);

                return Ok(user);
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


    }
}
