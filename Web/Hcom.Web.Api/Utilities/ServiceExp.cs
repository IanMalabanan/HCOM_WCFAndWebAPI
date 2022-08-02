using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Utilities
{
    public class ServiceExp : Exception
    {
        public HttpStatusCode HttpCode { get; }
        public string Content { get; set; }
        public ServiceExp(string content)
        {
            Content = content;
        }
        public ServiceExp(string content, HttpStatusCode code)
        {
            Content = content;
            HttpCode = code;
        }
    }
}
