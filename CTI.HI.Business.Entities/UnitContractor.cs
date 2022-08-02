using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
    public class UnitContractor
    {
        public string ReferenceObject { get; set; }
        public IEnumerable<Contractor> Contractors { get; set; }
    }
}
