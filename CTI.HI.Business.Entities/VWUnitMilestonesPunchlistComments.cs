using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CTI.HI.Business.Entities
{
    [Table("hcmVWUnitMilestonesPunchlistComments", Schema = "dbo")]
    public class VWUnitMilestonesPunchlistComments
    {
        [Key]
        [Column("pciUserName", Order = 0)]
        public string UserName { get; set; }

        [Column("pciOTCTypeCode")]
        public string OTCTypeCode { get; set; }

        
        [Column("pciReferenceObject")]
        public string ReferenceObject { get; set; }

        [Column("pciProjectCode")]
        public string ProjectCode { get; set; }

        [Column("pciProjectName")]
        public string ProjectName { get; set; }

        [Column("pciProjectShortName")]
        public string ProjectShortName { get; set; }

        [Column("pciPhaseCode")]
        public string PhaseCode { get; set; }

        [Column("pciPhase")]
        public string Phase { get; set; }

        [Column("pciPhaseShortName")]
        public string PhaseShortName { get; set; }

        [Column("pciBlockCode")]
        public string BlockCode { get; set; }

        [Column("pciBlock")]
        public string Block { get; set; }

        [Column("pciBlockShortName")]
        public string BlockShortName { get; set; }

        [Column("pciLot")]
        public string Lot { get; set; }

        [Column("pciInventoryUnitNumber")]
        public string InventoryUnitNumber { get; set; }

        [Column("pciMilestoneCode")]
        public string MilestoneCode { get; set; }

        [Column("pciMilestoneDescription")]
        public string MilestoneDescription { get; set; }

        [Column("pciPunchListCode")]
        public string pciPunchListCode { get; set; }

        [Column("pciPunchListStatus")]
        public string PunchListStatus { get; set; }

        [Column("pciPunchlistComments")]
        public string PunchlistComments { get; set; }

        [Column("pciPunchListCommentDueDate")]
        public DateTime? PunchListCommentDueDate { get; set; }

        [Column("pciPunchlistStatDateModified")]
        public DateTime? PunchlistStatDateModified { get; set; }

        [Column("pciPunchlistStatDateCreated")]
        public DateTime? PunchlistStatDateCreated { get; set; }

        [Column("pciPunchlistCommentCreatedBy")]
        public string PunchlistCommentCreatedBy { get; set; }

        [Key]
        [Column("pciPunchlistID", Order = 1)]
        public int PunchlistID { get; set; }

        [Column("pciPunchlistMilestoneRecordNumber")]
        public int PunchlistMilestoneRecordNumber { get; set; }

        [Column("pciPunchlistStatDueDate")]
        public DateTime? PunchlistStatDueDate { get; set; }

        [Column("pciPunchlistDescription")]
        public string PunchlistDescription { get; set; }

        [Column("pciPunchlistStatusCode")]
        public string PunchlistStatusCode { get; set; }

        [Key]
        [Column("pciCommentID", Order= 2)]
        public int CommentID { get; set; }

        [Column("pciCommentCreatedBy")]
        public string CommentCreatedBy { get; set; }

        [Column("pciCommentDateCreated")]
        public DateTime? CommentDateCreated { get; set; }

        [Column("pciCommentTagBy")]
        public string CommentTagBy { get; set; }

    }
}
