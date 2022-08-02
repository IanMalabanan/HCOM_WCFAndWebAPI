 
using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
    public class Punchlist 
    {
        public int PunchListID { get; set; }
        public int ConstructionMilestoneId { get; set; }
        public string OTCNumber { get; set; }
        public string ContractorNumber { get; set; }
        public int ManagingContractorID { get; set; }
        public string MilestoneCode { get; set; }
        public string PunchListCategory { get; set; }
        public string PunchListSubCategory { get; set; }
        public string NonCompliantTo { get; set; }
        public string ReferenceSheet { get; set; }
        public string PunchListDescription { get; set; }
        public string PunchListGroup { get; set; }
        public PunchlistDescription PunchListDescriptionDetails { get; set; }
        public string PunchListLocation { get; set; }
        public string CostImpact { get; set; }
        public string ScheduleImpact { get; set; }
        public int AssignedTo { get; set; } 
        public string PunchListStatus { get; set; } 
        public string ReferenceNumber { get; set; }
        public DateTime? DueDate { get; set; }
        public List<Comment> Comments { get; set; }
        public Comment Comment { get; set; }
        public List<PunchlistCommentAttachment> AttachmentFileNames { get; set; }
        public string Message { get; set; }
        public PunchlistStatus PunchListStatusDetail { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DeviceInfoModel DeviceInformation { get; set; }
        public Coordinates Coordinates { get; set; }

        public string CreatedBy { get; set; }
    }
}
