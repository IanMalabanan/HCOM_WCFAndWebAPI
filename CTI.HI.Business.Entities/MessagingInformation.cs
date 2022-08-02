using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
    public class MessagingInformation
    {
        public Contractor Contractor { get; set; }
        public ContractorRepresentative Representative { get; set; }
        public Unit Unit { get; set; }
        public string PunchlistReferenceNumber { get; set; }
        public string PunchlistDescription { get; set; }
    }
}
