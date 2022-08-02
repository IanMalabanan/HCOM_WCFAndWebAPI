using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
    public class Contractor
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public IEnumerable<ContractorRepresentative> Representative { get; set; }
    }
}
