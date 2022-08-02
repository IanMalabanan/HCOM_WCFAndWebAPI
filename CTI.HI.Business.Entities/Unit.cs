using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
    public class Unit
    {
         
        public string ReferenceObject { get; set; } 
        public Project Project { get; set; } 
        public PhaseBuilding PhaseBuilding { get; set; } 
        public Block BlockFloor { get; set; }
        public string LotUnitShareNumber { get; set; }
        public string InventoryUnitNumber { get; set; }
        public MilestonePercentage MilestonePercentage { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string[] FloorPlan { get; set; }
        public string  VendorCode { get; set; }
    }
}
