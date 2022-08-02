using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CTI.HI.Business.Entities
{
    [Table("hcmVWContractors", Schema = "dbo")]
    public class VWContractors
    {
        [Key]
        [Column("conReferenceObject", Order = 0)]
        public string ReferenceObject { get; set; }
        
        [Key]
        [Column("conVendorCode", Order = 1)]
        public string VendorCode { get; set; }
        
        [Column("conVendorName")]
        public string VendorName { get; set; }

        [Key]
        [Column("conContractorUserName", Order =2)]
        public string ContractorUserName { get; set; }
    }
}
