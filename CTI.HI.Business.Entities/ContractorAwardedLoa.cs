using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
    public class ContractorAwardedLoa
    {
        public string OTCNumber { get; set; }
        public string NTPReferenceNumber { get; set; }
        public string NTPNumber { get; set; }
        public DateTime? NTPDate { get; set; }
        public DateTime? NTPTagDate { get; set; }
        public string TagBy { get; set; }
        public string LoaReferenceNumber { get; set; }
        public string LoaContractNumber { get; set; }
        public DateTime? LoaDate  { get; set; }
    }
}
