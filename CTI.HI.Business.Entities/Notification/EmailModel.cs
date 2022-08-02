using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities.Notification
{
    public class EmailModel
    {
        public string Id { get; set; }
        public string From { get; set; } 
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Format { get; set; }
        public IEnumerable<EmailAttachment> Attachments { get; set; }
    }
}
