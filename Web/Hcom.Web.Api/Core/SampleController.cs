using Hcom.Web.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Core
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SampleController : CtrlBase
    {

        public SampleController(IAccountManager accountManager) : base(accountManager)
        {
          
        }

        [HttpGet("clientInfoo ")]
        public async Task<IActionResult> GetClientInfo()
        {

            ApplicationUser user = await _accountManager.GetUserByUserNameAsync(CurrentUser);
            
             await Task.Delay(1);     
            return Ok(ClientInfo);
        }
    }
}
