using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities.Notification
{
   public class EmailAttachment
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
}
