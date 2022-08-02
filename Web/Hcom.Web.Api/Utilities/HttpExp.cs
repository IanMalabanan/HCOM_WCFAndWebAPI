using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Utilities
{
    public class HttpExp : HttpRequestException
    {
        public HttpStatusCode HttpCode { get; }

        public HttpExp(HttpStatusCode code) : this(code, null, null)
        {
        }

        public HttpExp(HttpStatusCode code, string message) : this(code, message, null)
        {
        }

        public HttpExp(HttpStatusCode code, string message, Exception inner) : base(message, inner)
        {
            HttpCode = code;
        }
    }
}
