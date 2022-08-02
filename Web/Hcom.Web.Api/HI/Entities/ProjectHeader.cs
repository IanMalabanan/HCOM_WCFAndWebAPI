using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.HI.Entities
{

    [Serializable]
    public class ProjectHeader
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public string ImageUrl { get; set; }

    };
}
