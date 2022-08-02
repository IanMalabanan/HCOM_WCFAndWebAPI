using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
    public class ConstructionMilestone
    {
        public int Id { get; set; }
        public string OTCNumber { get; set; }
        public string ContractorNumber { get; set; }
        public int ManagingContractorID { get; set; }
        public string ReferenceObject { get; set; }
        public string ManagingContractorCode { get; set; }
        public string VendorCode { get; set; }
        public Contractor Contractor { get; set; }
        public string ConstructionMilestoneCode { get; set; }
        public string ConstructionMilestoneDescription { get; set; }
        public decimal Weight { get; set; }
        public int Sequence { get; set; }
        public decimal? PercentageCompletion { get; set; }
        public decimal? PercentageCompletionQA { get; set; }
        public decimal? PercentageCompletionEngineer { get; set; }
        public decimal? PercentageCompletionContractor { get; set; }
        public string PercentageReferenceNumber { get; set; }
        public IEnumerable<ContractorRepresentative> Representatives { get; set; }
        public ContractorRepresentative Representative { get; set; }
        public IEnumerable<Punchlist> Punchlists { get; set; }
        public string PONumber { get; set; }
        public string OTCTypeCode { get; set; }
        public string TradeCode { get; set; }
        public string TradeDescription { get; set; }
        public IEnumerable<MilestoneAttachment> Attachments { get; set; }
        public string LoaContractNumber { get; set; }
    }
}
