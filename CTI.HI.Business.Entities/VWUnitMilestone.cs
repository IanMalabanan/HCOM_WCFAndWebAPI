using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CTI.HI.Business.Entities
{
    [Table("hcmVWUnitMilestones", Schema = "dbo")]
    public class VWUnitMilestone
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

        [Column("vumInventoryUnitNumber")]
        public string InventoryUnitNumber { get; set; }

        [Column("vumLongitude")]
        public decimal? Longitude { get; set; }

        [Column("vumLatitude")]
        public decimal? Latitude { get; set; }

        [Column("vumUnitLongitude")]
        public decimal? UnitLongitude { get; set; }

        [Column("vumUnitLatitude")]
        public decimal? UnitLatitude { get; set; }


        [Column("vumProjectRoleCode")]
        public string ProjectRoleCode { get; set; }

        [Column("vumProjectRoleDescription")]
        public string ProjectRoleDescription { get; set; }

        [Column("vumUserNoAllowed")]
        public string UserNoAllowed { get; set; }

        [Column("vumUserRadius")]
        public decimal? UserRadius { get; set; }

        [Column("vumMilestoneCode")]
        public string MilestoneCode { get; set; }

        [Column("vumMilestoneDescription")]
        public string MilestoneDescription { get; set; }

        [Column("vumPercentageCompletion")]
        public decimal? PercentageCompletion { get; set; }

        [Column("vumImageURL")]
        public string ImageURL { get; set; }

        [Column("vumIsContractor")]
        public Boolean IsContractor { get; set; }

        [Column("vumOTCNumber")]
        public string OTCNumber { get; set; }

        [Column("vumVendorCode")]
        public string VendorCode { get; set; }

        [Column("vumVendorName")]
        public string VendorName { get; set; }

        [Column("vumContractorUserName")]
        public string ContractorUserName { get; set; }

    }
}
