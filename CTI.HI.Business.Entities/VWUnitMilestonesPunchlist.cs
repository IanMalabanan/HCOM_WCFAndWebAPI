using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CTI.HI.Business.Entities
{
    [Table("hcmVWUnitMilestonesPunchlist", Schema = "dbo")]
    public class VWUnitMilestonesPunchlist
    {
        [Key]
        [Column("pumUserName", Order = 2)]
        public string UserName { get; set; }

        [Column("pumOTCTypeCode")]
        public string OTCTypeCode { get; set; }

        [Key]
        [Column("pumReferenceObject", Order = 1)]
        public string ReferenceObject { get; set; }

        [Column("pumProjectName")]
        public string ProjectName { get; set; }

        [Column("pumPhase")]
        public string Phase { get; set; }

        [Column("pumBlock")]
        public string Block { get; set; }

        [Column("pumLot")]
        public string Lot { get; set; }

        [Key]
        [Column("pumMilestoneCode", Order = 3)]
        public string MilestoneCode { get; set; }

        [Column("pumMilestoneDescription")]
        public string MilestoneDescription { get; set; }

        [Column("pumPercentageCompletion")]
        public decimal? PercentageCompletion { get; set; }

        [Column("pumPunchListCode")]
        public string PunchListCode { get; set; }

        [Column("pumPunchListDescription")]
        public string PunchListDescription { get; set; }

        [Column("pumPunchListStatus")]
        public string PunchListStatus { get; set; }

        [Column("pumPunchlistDueDate")]
        public DateTime? PunchlistDueDate { get; set; }

        [Column("pumPunchlistStatusCode")]
        public string PunchlistStatusCode { get; set; }

        [Column("pumProjectCode")]
        public string ProjectCode { get; set; }

        [Column("pumPhaseCode")]
        public string PhaseCode { get; set; }

        [Column("pumBlockCode")]
        public string BlockCode { get; set; }

        [Column("pumConstructionMilestoneID")]
        public int ConstructionMilestoneID { get; set; }

        [Key]
        [Column("pumPunchlistID", Order = 4)]
        public int PunchlistID { get; set; }

        [Column("pumDateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("pumDateModified")]
        public DateTime? DateModified { get; set; }

        [Column("pumAuthorizedPersonnelID")]
        public int AuthorizedPersonnelID { get; set; }

        [Column("pumPunchListScheduleImpactCode")]
        public string PunchListScheduleImpactCode { get; set; }

        [Column("pumPunchListCostImpactCode")]
        public string PunchListCostImpactCode { get; set; }

        [Column("pumPunchListLocationCode")]
        public string PunchListLocationCode { get; set; }

        [Column("pumPunchlistGroupCode")]
        public string PunchlistGroupCode { get; set; }

        [Column("pumReferenceSheet")]
        public string ReferenceSheet { get; set; }

        [Column("pumPunchListCategoryNonComplianceCode")]
        public string PunchListCategoryNonComplianceCode { get; set; }

        [Column("pumPunchListSubCategoryCode")]
        public string PunchListSubCategoryCode { get; set; }

        [Column("pumPunchListCategoryCode")]
        public string PunchListCategoryCode { get; set; }

        [Column("pumConstructionMilestoneContractorNumber")]
        public string ConstructionMilestoneContractorNumber { get; set; }

        [Column("pumConstructionMilestoneOTCNumber")]
        public string ConstructionMilestoneOTCNumber { get; set; }

        [Column("pumReferenceNumber")]
        public string ReferenceNumber { get; set; }


        [Column("pumConstructionMilestoneManagingContractorID")]
        public int ConstructionMilestoneManagingContractorID { get; set; }


        [Column("pumPunchlistStatDateModified")]
        public DateTime? PunchlistStatDateModified { get; set; }

        [Column("pumPunchlistStatDateCreated")]
        public DateTime? PunchlistStatDateCreated { get; set; }

        [Column("pumInventoryUnitNumber")]
        public string InventoryUnitNumber { get; set; }


        [Column("pumPunchlistStatDueDate")]
        public DateTime? PunchlistStatDueDate { get; set; }


        [Column("pumPhaseShortName")]
        public string PhaseShortName { get; set; }


        [Column("pumBlockShortName")]
        public string BlockShortName { get; set; }

        [Column("pumProjectShortName")]
        public string ProjectShortName { get; set; }

        [Column("pumVendorCode")]
        public string VendorCode { get; set; }

        [Column("pumVendorName")]
        public string VendorName { get; set; }

        [Column("pumContractorUserName")]
        public string ContractorUserName { get; set; }

    }
}
