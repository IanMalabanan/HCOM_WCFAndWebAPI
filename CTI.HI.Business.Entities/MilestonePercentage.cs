using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
    public class MilestonePercentage
    {
        public string ReferenceObject { get; set; }
        public decimal? PercentageCompletion { get; set; }
        public decimal? TargetPercentageCompletion { get; set; }
        public string Variance { get; set; }
        public string OngoingMilestoneActivity { get; set; }
        public string Contractors { get; set; }
        public decimal? BillingPercentage { get; set; }
        public int PunchlistPendingCount { get; set; }
        public int PunchlistOverdueCount { get; set; }
        public int PunchlistCount { get; set; }
    }
}
