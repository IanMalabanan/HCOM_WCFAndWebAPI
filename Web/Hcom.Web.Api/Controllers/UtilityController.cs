using Hcom.Web.Api.Interface;
using Hcom.Web.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Validation;
using PunchlistSvcReference;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Controllers
{

    [ApiExplorerSettings(GroupName = "UTILITY")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/util")]
    public class UtilityController : Controller
    {
        private readonly FTPSettings configuration;
        private readonly IWebHostEnvironment _env;

        public UtilityController(IOptionsMonitor<FTPSettings> config, IWebHostEnvironment env)
        {
            configuration = config.CurrentValue;
            _env = env;
            
        }

         
        [HttpGet("Attachment")]
        public IActionResult ImageFile(string filename)
        {
            //Byte[] b = System.IO.File.ReadAllBytes(Path.Combine(_env.ContentRootPath + @"\Attachment\", filename));
            // return File(b, "image/jpeg");
            //Byte[] b = Encoding.ASCII.GetBytes(Path.Combine(GetHostName, filename));
            try
            {
                var url = String.Format("{0}{1}", configuration.GetHostName, filename);
                byte[] response = new System.Net.WebClient().DownloadData(url);
                return File(response, "image/jpeg");
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            
        }


        [HttpPost("Attachment")]
        public async Task<string> SaveFile(IFormFile uploadedFile)
        { 
            if (uploadedFile == null)
                return null;

            var fileNameExt = Path.GetExtension(uploadedFile.FileName);
            var compfileName = "HI" + DateTime.Now.ToString("MMddyyyyHHmmssss") + fileNameExt;
              
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(configuration.PostHostName + compfileName);
            request.Credentials = new NetworkCredential(configuration.FtpUsername, configuration.FtpPassword);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            using (Stream ftpStream = request.GetRequestStream())
            {
                uploadedFile.CopyTo(ftpStream);
            }

            return await Task.FromResult(compfileName);
         


        }

    } 
}
 