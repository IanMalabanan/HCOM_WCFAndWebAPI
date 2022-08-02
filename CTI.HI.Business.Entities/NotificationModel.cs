using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
    public class NotificationModel
    {
        public Unit Unit { get; set; }
        public DateTime DatePosted { get; set; }//
        public string Subject { get; set; }
        public string MessageBy { get; set; }//
        public string Message { get; set; }//
        public DateTime? DueDate { get; set; }//
        public int ConstructionMilestoneId { get; set; }//
        public int PunchlistId { get; set; }// 
        public string PunchlistStatusCode { get; set; }
    }
}
