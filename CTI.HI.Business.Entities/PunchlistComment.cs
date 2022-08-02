using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
    public class PunchlistComment
    {
        public int Id { get; set; }
        public int PunchlistId { get; set; }
        public string Message { get; set; }
        public string CreatedBy { get; set; }
    }
}
