using Hcom.Web.Api.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Models
{
    public class ServiceEndpoints
    {
        public string BaseUrl { get; set; }

        //public string ContractorServiceEndpoint { get; set; } = "ContractorService.svc"

        public string FrebasBaseUrl { get; set; }

        public string Environment { get; set; }
    }
}
