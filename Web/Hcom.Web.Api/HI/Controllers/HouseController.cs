using System;
using System.Linq;
using System.Threading.Tasks;
using Hcom.Web.Api.Core;
using Hcom.Web.Api.Interface;
using Hcom.Web.Api.Models;
using Hcom.Web.Api.Utilities;
using Hcom.Web.Api.Utilities.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Validation;

namespace Hcom.Web.Api.HI.Controllers
{
    [ApiExplorerSettings(GroupName = "HCOM")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/hi")]
    public class HouseController : Controller // CtrlBase
    {
        #region Constructor
        private readonly IUnit _unit;
        private readonly IUser _user;
        private readonly ILogger _logger;
        private string curlong = "0";
        private string curlat = "0";
        private string deviceid = "0";


        public HouseController(IUnit unit
            , ILogger<HouseController> logger
            , IUser user
           )
        {
            _unit = unit;
            _logger = logger;
            _user = user;
        }
        #endregion

        #region Resources

        private void GetLocation(string loc)
        {
            if (loc != null && loc.Split('|').Count() > 1)
            {
                curlat = loc.Split('|')[0];
                curlong = loc.Split('|')[1];
                deviceid = loc.Split('|')[2];
            }
        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetProjects([FromHeader(Name = "clientInfo")] string loc, string keyword = "")
        {
            try
            {
                var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ","");

                GetLocation(loc);
                var projects = await _unit.GetProjectByUserAsync(_user.GetUserFromIdentity(HttpContext.User).Result,keyword);
                return Ok(projects);
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

        [HttpGet("phases")]
        public async Task<IActionResult> GetPhase([FromHeader(Name = "clientInfo")] string loc, string projectCode, string keyword = "")
        {
            try
            {
                GetLocation(loc);
                var phases = await _unit.GetPhaseByProjectAsync(projectCode, _user.GetUserFromIdentity(HttpContext.User).Result, keyword);
                return Ok(phases);
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

        [HttpGet("blocks")]
        public async Task<IActionResult> GetBlock([FromHeader(Name = "clientInfo")] string loc, string phaseCode, string keyword = "")
        {
            try
            {
                GetLocation(loc);
                var blocks = await _unit.GetBlockByPhaseAsync(phaseCode, _user.GetUserFromIdentity(HttpContext.User).Result, keyword);
                return Ok(blocks);
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

        [HttpGet("units")]
        public async Task<IActionResult> GetUnits([FromHeader(Name = "clientInfo")] string loc, string blockCode, string keyword = "")
        {
            try
            {
                GetLocation(loc);
                var x = await _unit.GetUnitByBlockAsync(blockCode, _user.GetUserFromIdentity(HttpContext.User).Result, keyword);
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

        [HttpGet("unit/{id}")]
        public async Task<IActionResult> GetUnits([FromHeader(Name = "clientInfo")] string loc, string id)
        {
            try
            {
                GetLocation(loc);
                var x = await _unit.GetUnitAsync(id, _user.GetUserFromIdentity(HttpContext.User).Result);
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

        [HttpGet("unit/{referenceObject}/{vendorCode}")]
        public async Task<IActionResult> GetUnitsByVendor([FromHeader(Name = "clientInfo")] string loc, string referenceObject, string vendorCode)
        {
            try
            {
                _logger.LogError("asd");
                GetLocation(loc);
                var x = await _unit.GetUnitByVendorAsync(referenceObject, User.Identity.Name, vendorCode);
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


        [HttpGet("userunits")]
        public async Task<IActionResult> GetUserUnits([FromHeader(Name = "clientInfo")] string loc, string keyword = "")
        {
            try
            {
                GetLocation(loc);
                var projects = await _unit.GetUnitsByUserAsync(_user.GetUserFromIdentity(HttpContext.User).Result, keyword);
                return Ok(projects);
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


        [HttpGet("userunitsbyproject")]
        public async Task<IActionResult> GetUserUnitsbyProject(string projectcode)
        {
            try
            {
                var unitsproj = await _unit.GetUserunitsbyProjectAsync(projectcode, _user.GetUserFromIdentity(HttpContext.User).Result);
                return Ok(unitsproj);
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


        [HttpPost("unit/qrcode")]
        public async Task<IActionResult> ReadUnitQRCode([FromHeader(Name = "clientInfo")] string loc, [FromBody] string text)
        {
            try
            {
                var _encryptionProvider = RijndaelEncryptionProvider.GetInstance();

                var refObject = _encryptionProvider.Decrypt(text);

                GetLocation(loc);
                var x = await _unit.GetUnitAsync(refObject, _user.GetUserFromIdentity(HttpContext.User).Result);
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
                return NotFound();
            }
        }


        #endregion

    }


}