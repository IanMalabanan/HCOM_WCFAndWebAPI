using System;

namespace CTI.HI.Business.Entities
{
    public class Project
    {
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string ImageUrl { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
    }
}
