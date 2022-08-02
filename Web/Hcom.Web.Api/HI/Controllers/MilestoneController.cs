using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
using CTI.HI.Business.Entities;
using Hcom.App.Entities.DTO;
using Hcom.App.Entities.HCOM;
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
using MilestoneSvcReference;
using OpenIddict.Validation;
using UserSvcRefence;
using MilestoneAttachment = MilestoneSvcReference.MilestoneAttachment;

namespace Hcom.Web.Api.HI.Controllers
{
    [ApiExplorerSettings(GroupName = "HCOM")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/hi")]
    public class MilestoneController : Controller
    {
        private readonly IMilestone _milestoneService;
        private readonly IUser _user;
        private readonly ILogger _logger;

        public MilestoneController(IMilestone milestoneService, IUser user, ILogger<HouseController> logger)
        {
            _milestoneService = milestoneService;
            _user = user;
            _logger = logger;
        }

        [HttpGet("GetMilestoneAttachment")]
        public async Task<IActionResult> GetImageFile(string filename)
        {
            try
            {
                var data = await _milestoneService.GetMilestoneAttachment(filename);

                if (data == null)
                {
                    throw new Exception("Image not found");
                }
                else
                {
                    byte[] response = (byte[])data.FileBinary;
                    var ass = File(response, "image/jpeg");

                    return ass;
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

        [HttpDelete("DeleteMilestoneAttachment")]
        public async Task<IActionResult> DeleteImageFile(string filename)
        {
            try
            {
                var data = await _milestoneService.DeleteMilestoneAttachment(filename);

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
        [HttpPost("UploadAllMilestoneImagesFromFTPServerToDatabase")]
        public async Task<IActionResult> MilestoneAttachments()
        {
            try
            {
                var res = await _milestoneService.UploadAllMilestoneImagesFromFTPServerToDatabase(User.Identity.Name);

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


        [HttpPost("SaveMilestoneAttachment")]
        public async Task<IActionResult> SaveFile(IFormFile uploadedFile)
        {
            try
            {
                var res = await _milestoneService.SaveMilestoneAttachment(uploadedFile, User.Identity.Name);

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



        [HttpGet("Milestone/{Id}")]
        public async Task<IActionResult> GetMilestonebyId(int Id)
        {
            try
            {
                var xmilestoneid = await _milestoneService.GetMilestoneByIdAsync(Id, Fileurl(), _user.GetUserFromIdentity(HttpContext.User).Result);
                return Ok(xmilestoneid);
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

        [HttpGet("UnitMilestone")]
        public async Task<IActionResult> GetMilestonesByUnit(string referencecode)
        {
            try
            {
                var unitmilestone = await _milestoneService.GetMilestonesByUnitAsync(_user.GetUserFromIdentity(HttpContext.User).Result, Fileurl(), referencecode);
                return Ok(unitmilestone);
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

        [HttpGet("UnitMilestone/{referencecode}/{vendorCode}")]
        public async Task<IActionResult> GetMilestonesByUnitByVendor(string referencecode, string vendorCode)
        {
            try
            {
                var unitmilestone = await _milestoneService.GetMilestonesByUnitByVendorAsync(User.Identity.Name, Fileurl(), referencecode, vendorCode);
                return Ok(unitmilestone);
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

        [HttpGet("MilestonebyProject")]
        public async Task<IActionResult> GetMilestonebyProject(string projectcode)
        {
            try
            {
                var milestoneproject = await _milestoneService.GetMilestoneByProjectIdAsync(projectcode, _user.GetUserFromIdentity(HttpContext.User).Result);

                return Ok(milestoneproject);
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
        /// <summary>
        /// Coming soon
        /// </summary>
        [HttpPut("UpdatePercentageCompletion")]
        public async Task<IActionResult> UpdatePercentageCompletion([FromBody] MilestonePercentageDto model)
        {
            try
            {
                var updatepercentage = await _milestoneService.UpdateMilestonePercentageAsync(model, Fileurl(), _user.GetUserFromIdentity(HttpContext.User).Result);
                return Ok(updatepercentage);
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


        #region methods

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

        

        #endregion

    }
}


