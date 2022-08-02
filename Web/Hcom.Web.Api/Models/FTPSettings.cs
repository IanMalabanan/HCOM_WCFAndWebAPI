using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Models
{
    public class FTPSettings
    {
        public string PostHostName { get; set; }

        public string GetHostName { get; set; }

        public string FtpUsername { get; set; }

        public string FtpPassword { get; set; }
    }
}
