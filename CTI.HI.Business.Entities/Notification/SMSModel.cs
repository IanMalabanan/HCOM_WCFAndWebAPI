using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities.Notification
{
   public class SMSModel
    {
        public string From { get; set; }
        public string Text { get; set; }
        public string[] To { get; set; }
    }
}
