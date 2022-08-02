using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CTI.HI.Business.Entities
{
    [Table("hcmVWUnitMilestonesWithLoaAndTradeType", Schema = "dbo")]
    public class VWUnitMilestonesWithLoaAndTradeType
    {
        [Key]
        [Column("vumUserName", Order = 2)]
        public string UserName { get; set; }

        [Column("vumOTCTypeCode")]
        public string OTCTypeCode { get; set; }

        [Key]
        [Column("vumReferenceObject", Order = 1)]
        public string ReferenceObject { get; set; }

        [Column("vumProjectCode")]
        public string ProjectCode { get; set; }

        [Column("vumProjectName")]
        public string ProjectName { get; set; }

        [Column("vumProjectShortName")]
        public string ProjectShortName { get; set; }

        [Column("vumPhaseCode")]
        public string PhaseCode { get; set; }

        [Column("vumPhase")]
        public string Phase { get; set; }

        [Column("vumPhaseShortName")]
        public string PhaseShortName { get; set; }

        [Column("vumBlockCode")]
        public string BlockCode { get; set; }

        [Column("vumBlock")]
        public string Block { get; set; }

        [Column("vumBlockShortName")]
        public string BlockShortName { get; set; }

        [Column("vumLot")]
        public string Lot { get; set; }

        [Column("vumInventoryUnitReferenceObject")]
        public string InventoryUnitReferenceObject { get; set; }

        [Column("vumInventoryUnitNumber")]
        public string InventoryUnitNumber { get; set; }

        [Column("vumLongitude")]
        public decimal? Longitude { get; set; }

        [Column("vumLatitude")]
        public decimal? vumLatitude { get; set; }

        [Column("vumProjectRoleCode")]
        public string ProjectRoleCode { get; set; }

        [Column("vumProjectRoleDescription")]
        public string ProjectRoleDescription { get; set; }

        [Column("vumUserNoAllowed")]
        public int UserNoAllowed { get; set; }

        [Column("vumUserRadius")]
        public decimal? UserRadius { get; set; }

        [Column("vumMilestoneCode")]
        public string MilestoneCode { get; set; }

        [Column("vumMilestoneDescription")]
        public string MilestoneDescription { get; set; }

        [Column("vumConstructionMilestoneCode")]
        public string ConstructionMilestoneCode { get; set; }

        [Column("vumConstructionMilestonePercentageCompletion")]
        public decimal? ConstructionMilestonePercentageCompletion { get; set; }

        [Column("vumConstructionMilestoneID")]
        public int ConstructionMilestoneID { get; set; }

        [Column("vumConstructionMilestoneOTCNumber")]
        public string ConstructionMilestoneOTCNumber { get; set; }

        [Column("vumConstructionMilestoneContractorNumber")]
        public string ConstructionMilestoneContractorNumber { get; set; }

        [Column("vumConstructionMilestoneManagingContractorID")]
        public int ConstructionMilestoneManagingContractorID { get; set; }

        [Column("vumConstructionMilestoneMilestonePercentageReferenceNumber")]
        public string ConstructionMilestoneMilestonePercentageReferenceNumber { get; set; }

        [Column("vumConstructionMilestoneMilestonePONumber")]
        public string ConstructionMilestoneMilestonePONumber { get; set; }

        [Column("vumConstructionMilestoneMilestoneSequence")]
        public int ConstructionMilestoneMilestoneSequence { get; set; }

        [Column("vumConstructionMilestoneMilestoneWeight")]
        public decimal? ConstructionMilestoneMilestoneWeight { get; set; }

        [Column("vumConstructionMilestoneMilestonePercentageTagDate")]
        public DateTime? ConstructionMilestoneMilestonePercentageTagDate { get; set; }

        [Column("vumManagingContractorCode")]
        public string ManagingContractorCode { get; set; }

        [Column("vumLOAContractNumber")]
        public string LOAContractNumber { get; set; }

        [Column("vumTradeTypeCode")]
        public string TradeTypeCode { get; set; }

        [Column("vumTradeType")]
        public string TradeType { get; set; }

        [Column("vumVendorCode")]
        public string VendorCode { get; set; }

        [Column("vumVendorName")]
        public string VendorName { get; set; }

        [Column("vumContractorCode")]
        public string ContractorCode { get; set; }

        [Column("vumContractorName")]
        public string ContractorName { get; set; }

        [Column("vumContractorMobileNumber")]
        public string ContractorMobileNumber { get; set; }

        [Column("vumContractorEmail")]
        public string ContractorEmail { get; set; }

        [Column("vumContractorID")]
        public int? ContractorID { get; set; }

        [Column("vumIsContractor")]
        public Boolean IsContractor { get; set; }

        [Column("vumPercentageRemarks")]
        public string PercentageRemarks { get; set; }

        [Column("vumPercentageTagBy")]
        public string PercentageTagBy { get; set; }

        [Column("vumContractorUserName")]
        public string ContractorUserName { get; set; }

        [Column("vumOTCNumber")]
        public string OTCNumber { get; set; }
    }
}
