using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.ServiceModel;
using System.Threading.Tasks;
using CTI.HI.Business.Entities;
using CTI.HI.Business.Entities.Notification;
using Hcom.App.Core.Enums;
using Hcom.App.Entities;
using Hcom.App.Entities.HCOM;
using Hcom.App.Entities.Models;
using Hcom.Web.Api.Core;
using Hcom.Web.Api.Interface;
using Hcom.Web.Api.Models;
using Hcom.Web.Api.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotificationSvcReference;
using OpenIddict.Validation;
using PunchlistSvcReference;
using RestSharp;
using UserSvcRefence;
using Coordinates = PunchlistSvcReference.Coordinates;
using DeviceInfoModel = PunchlistSvcReference.DeviceInfoModel;

namespace Hcom.Web.Api.HI.Controllers
{

    [ApiExplorerSettings(GroupName = "HCOM")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/hi")]
    public class PunchlistController : Controller //CoreController
    {
        private readonly IPunchlist _punchlist;
        private readonly IUser _user;
        private readonly ILogger _logger;

        public PunchlistController(IPunchlist punchlist, IUser user, ILogger<HouseController> logger)
        {
            _punchlist = punchlist;
            _user = user;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("GetPunchlistAttachment")]
        public async Task<IActionResult> GetImageFile(string filename)
        {
            try
            {
                var data = await _punchlist.GetPunchlistAttachment(filename);

                if (data == null)
                {
                    throw new Exception("Image not found");
                }
                else
                {
                    byte[] response = (byte[])data.FileBinary;
                    return File(response, "image/jpeg");
                }
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
        [HttpDelete("DeletePunchlistAttachment")]
        public async Task<IActionResult> DeleteImageFile(string filename)
        {
            try
            {
                var data = await _punchlist.DeletePunchlistAttachment(filename);

                return Ok(data);
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
        [HttpPost("UploadAllPunchlistImagesFromFTPServerToDatabase")]
        public async Task<IActionResult> PunchlistAttachments()
        {
            try
            {
                var res = await _punchlist.UploadAllPunchlistImagesFromFTPServerToDatabase(User.Identity.Name);

                return Ok(res);
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

        [HttpPost("SavePunchlistAttachment")]
        public async Task<IActionResult> SaveFile(IFormFile uploadedFile)
        {
            try
            {
                var res = await _punchlist.SavePunchlistAttachment(uploadedFile, User.Identity.Name);

                return Ok(res);
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


        [HttpGet("Punchlists")]
        public async Task<IActionResult> GetPunchlist(int ConstructionMilestoneId)
        {
            try
            {
                var punchlist = await _punchlist.GetPunchlistAsync(ConstructionMilestoneId, Fileurl());
                return Ok(punchlist);
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

        [HttpGet("PunchlistDetails")]
        public async Task<IActionResult> GetPunchlistDetails(int punchlistid)
        {
            try
            {
                var punchlist = await _punchlist.GetPunchlistDetailsAsync(punchlistid, Fileurl());
                return Ok(punchlist);
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


        [HttpGet("PunchlistCategory")]
        public async Task<IActionResult> GetPunchlistCategory()
        {
            try
            {
                var punchlistcategory = await _punchlist.GetPunchlistCategoryAsync();
                return Ok(punchlistcategory);
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


        [HttpGet("PunchlistSubCategory")]
        public async Task<IActionResult> GetPunchlistSubCategory()
        {
            try
            {

                var punchlistsubcategory = await _punchlist.GetPunchlistSubCategoryAsync();
                return Ok(punchlistsubcategory);
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


        [HttpGet("NonCompliantTo")]
        public async Task<IActionResult> GetNonComplianceTo()
        {
            try
            {

                var complianceto = await _punchlist.GetNonCompliancesToAsync();
                return Ok(complianceto);
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



        [HttpGet("PunchlistLocation")]
        public async Task<IActionResult> GetPunchlistLocation()
        {
            try
            {

                var punchlistlocation = await _punchlist.GetPunchlistLocationAsync();
                return Ok(punchlistlocation);
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


        [HttpGet("CostImpact")]
        public async Task<IActionResult> GetCostImpact()
        {
            try
            {

                var costimpact = await _punchlist.GetCostImpactAsync();
                return Ok(costimpact);
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


        [HttpGet("ScheduledImpact")]
        public async Task<IActionResult> GetScheduledImpact()
        {
            try
            {

                var scheduledimpact = await _punchlist.GetScheduledImpactAsync();
                return Ok(scheduledimpact);
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


        [HttpGet("PunchlistStatus")]
        public async Task<IActionResult> GetPunchlistStatus()
        {
            try
            {

                var punchliststatus = await _punchlist.GetPunchlistStatusAsync();
                return Ok(punchliststatus);
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

        [HttpGet("PunchlistDescription")]
        public async Task<IActionResult> GetPunchlistDescription()
        {
            try
            {

                var punchlistdesc = await _punchlist.GetPunchlistDescriptions();
                return Ok(punchlistdesc);
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

        [HttpGet("PunchlistStatusbyRole")]
        public async Task<IActionResult> GetPunchlistStatusbyRole(UserRoleType x)
        {
            try
            {
                var statusrole = await _punchlist.GetPunchlistStatusbyRoleAsync(x, User.Identity.Name);
                return Ok(statusrole);
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

        [HttpGet("PunchlistGroup")]
        public async Task<IActionResult> GetPunchlistGroups()
        {
            try
            {
                var punchlistgroup = await _punchlist.GetPunchlistGroupAsync();
                return Ok(punchlistgroup);
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


        [HttpGet("PunchlistbyProject")]
        public async Task<IActionResult> GetPunchlistbyProject(string projectcode)
        {
            try
            {
                var punchlistbyproject = await _punchlist.GetPunchlistbyProjectAsync(projectcode, _user.GetUserFromIdentity(HttpContext.User).Result, Fileurl());
                return Ok(punchlistbyproject); 
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

        [HttpGet("PunchlistbyUnit")]
        public async Task<IActionResult> GetPunchlistbyUnit(string referenceObject)
        {
            try
            {
                var punchlistbyreferenceObject = await _punchlist.GetPunchlistbyUnitAsync(referenceObject, User.Identity.Name, Fileurl());
                return Ok(punchlistbyreferenceObject);
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

        [HttpGet("PunchlistbyUnit/{referenceObject}/{vendorCode}")]
        public async Task<IActionResult> GetPunchlistbyUnitByVendor(string referenceObject, string vendorCode)
        {
            try
            {
                var punchlistbyreferenceObjectandVendor = await _punchlist.GetPunchlistbyUnitByVendorAsync(referenceObject, User.Identity.Name, Fileurl(),vendorCode);
                return Ok(punchlistbyreferenceObjectandVendor);
                var punchlistbyproject = await _punchlist.GetPunchlistbyUnitAsync(referenceObject, _user.GetUserFromIdentity(HttpContext.User).Result, Fileurl());
                return Ok(punchlistbyproject);
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


        [HttpPost("PunchlistSave")]
        public async Task<IActionResult> SavePunchlist([FromBody] App.Entities.HCOM.Punchlist punch)
        {
            try
            {

                var x = await _punchlist.PunchlistSaveAsync(punch, _user.GetUserFromIdentity(HttpContext.User).Result, Fileurl());
                return Ok(x);
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



        //TODO: Move to basecontroller
        string Fileurl(string file)
        {
            string Path = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}/api/util/Attachment?filename=" + file;
            return Path;
        }

        string Fileurl()
        {
            string Path = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}/api/util/Attachment?filename=";
            return Path;
        }

        
    }
}