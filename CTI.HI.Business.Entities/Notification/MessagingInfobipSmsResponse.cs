using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CTI.HI.Business.Entities.Notification
{
    public class MessagingInfobipSmsResponse 
    {

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; } 
    }
}
