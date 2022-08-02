using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Core
{
    public class CtrlBase : ControllerBase,IAsyncActionFilter
    {
        protected readonly IAccountManager _accountManager;

        public CtrlBase(IAccountManager accountManager)
        {

        }
        protected string ClientInfo = "";
        protected string  CurrentUser = "";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            CurrentUser = context.HttpContext.User.Identity.Name;
            StringValues _clientInfo;
            if (context.HttpContext.Request.Headers.TryGetValue("ClientInfo", out _clientInfo))
                      ClientInfo = _clientInfo;
            await next();
        }

       public  string Fileurl(string file)
        {
            string Path = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}/api/util/Attachment?filename=" + file;
            return Path;
        }

    }

    public static class HttpRequestExtension
    {
        public static string GetHeader(this HttpRequest request, string key)
        {
            return request.Headers.FirstOrDefault(x => x.Key == key).Value.FirstOrDefault();
        }
    }
     
  

}
