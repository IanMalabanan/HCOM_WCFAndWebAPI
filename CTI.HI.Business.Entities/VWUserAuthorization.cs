using Core.Common.Contracts;
using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace CTI.HI.Business.Entities
{

    public class VWUserAuthorization
    {
        public string ReferenceObject { get; set; }

        public string UserName { get; set; }

    }

}
