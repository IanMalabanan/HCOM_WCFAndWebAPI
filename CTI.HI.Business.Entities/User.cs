 
using System;
using System.Collections.Generic;
using System.Text;

namespace CTI.HI.Business.Entities
{
    public class User
    {
        public User()
        {
            InspectionRadius = 0;
        }
        public string Id { get; set; }
        public string FullName { get; set; } 
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string RoleCode { get; set; } 
        public decimal? InspectionRadius { get; set; }
    }
}
