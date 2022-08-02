using Hcom.App.Entities.HCOM;
using Hcom.Web.Api.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Hcom.Web.Api.Models;
using Hcom.Web.Api.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using NotificationSvcReference;
using OpenIddict.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;


namespace Hcom.Web.Api.HI.Controllers
{

    [ApiExplorerSettings(GroupName = "HCOM")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/hi")]
    public class NotificationController : Controller
    {
        private readonly INotification _notification;
        private readonly ILogger _logger;
        private readonly IUser _user;

        public NotificationController(INotification notification, IUser user, ILogger<HouseController> logger)
        {
            _notification = notification;
            _logger = logger;
            _user = user;
        }

        [HttpGet("OpenPunchlists")]
        public async Task<IActionResult> GetOpenPunchlist()
        { 
            try
            {
                //time start
                var _timeStartSaveCache = DateTime.Now;
                var openpunchlist = await _notification.GetOpenPunchlistAsync(_user.GetUserFromIdentity(HttpContext.User).Result);
                //time end
                var _timeEndSaveCache = DateTime.Now;
                Debug.WriteLine($"{"Total time of GetClosedMilestoneAsync : " + (_timeEndSaveCache - _timeStartSaveCache).TotalSeconds }");
                return Ok(openpunchlist);
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

        [HttpGet("OpenOverduePunchlists")]
        public async Task<IActionResult> GetOpenOverduePunchlists()
        { 
            try
            {
                //time start
                var _timeStartSaveCache = DateTime.Now;
                var openoverdue = await _notification.GetOpenOverduePunchlistsAsync(_user.GetUserFromIdentity(HttpContext.User).Result);
                //time end
                var _timeEndSaveCache = DateTime.Now;
                Debug.WriteLine($"{"Total time of GetClosedMilestoneAsync : " + (_timeEndSaveCache - _timeStartSaveCache).TotalSeconds }");
                return Ok(openoverdue);
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

        [HttpGet("RecentlyClosedPunchlists")]
        public async Task<IActionResult> GetRecentlyClosedPunchlists()
        { 
            try
            {
                //time start
                var _timeStartSaveCache = DateTime.Now;
                var recentlyclosed = await _notification.GetRecentlyClosedPunchlistsAsync(_user.GetUserFromIdentity(HttpContext.User).Result);
                //time end
                var _timeEndSaveCache = DateTime.Now;
                Debug.WriteLine($"{"Total time of GetClosedMilestoneAsync : " + (_timeEndSaveCache - _timeStartSaveCache).TotalSeconds }");
                return Ok(recentlyclosed);
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

        [HttpGet("DelayedMilestones")]
        public async Task<IActionResult>GetDelayedMilestones()
        { 
            try
            {
                //time start
                var _timeStartSaveCache = DateTime.Now;
                var delayedmilestone = await _notification.GetDelayedMilestonesAsync(_user.GetUserFromIdentity(HttpContext.User).Result);
                //time end
                var _timeEndSaveCache = DateTime.Now;
                Debug.WriteLine($"{"Total time of GetClosedMilestoneAsync : " + (_timeEndSaveCache - _timeStartSaveCache).TotalSeconds }");
                return Ok(delayedmilestone);
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

        [HttpGet("RecentlyClosedMilestone")]
        public async Task<IActionResult> GetRecentlyClosedMilestone() 
        {
            try
            {
                //time start
                var _timeStartSaveCache = DateTime.Now;
                var recentlyclosed = await _notification.GetRecentlyClosedMilestoneAsync(_user.GetUserFromIdentity(HttpContext.User).Result);
                //time end
                var _timeEndSaveCache = DateTime.Now;
                Debug.WriteLine($"{"Total time of GetClosedMilestoneAsync : " + (_timeEndSaveCache - _timeStartSaveCache).TotalSeconds }");
                return Ok(recentlyclosed);
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
