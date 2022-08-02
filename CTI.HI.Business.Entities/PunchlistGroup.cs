using Core.Common.Contracts;
using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CTI.HI.Business.Entities
{ 
    public class PunchlistGroup 
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string CategoryCode { get; set; }
    }
}
