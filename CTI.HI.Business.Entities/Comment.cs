using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
   public class Comment 
    {
        public int CommentId { get; set; }
        public int PunchlistId { get; set; }
        public List<string> ImageUrl { get; set; }
        public List<string> AttachmentFileName { get; set; }
        public string Message { get; set; }
        public string CreatedByUsername { get; set; }
        public DateTime CreatedDate { get; set; }
        public User CreatedBy { get; set; }
        public PunchlistStatus PunchlistStatus { get; set; }
        public DateTime DueDate { get; set; }
    }
}
