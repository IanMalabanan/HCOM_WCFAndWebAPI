using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
    public class Block
    {
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
