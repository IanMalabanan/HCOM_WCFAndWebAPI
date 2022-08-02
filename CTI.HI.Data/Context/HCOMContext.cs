
using Core.Common.Contracts;
using CTI.HCM.Business.Entities;
//using CTI.HCOM.Business.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks; 

namespace CTI.HI.Data.Context
{
    [Export(typeof(HcomContext))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class HcomContext:DbContext
    {
        public HcomContext():base("HCOMConnection")
        {

            Database.SetInitializer<HcomContext>(null);
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        //public DbSet<UserProject> UserProject { get; set; }
        //public DbSet<InventoryMetadata> InventoryMetadata { get; set; }
       // public DbSet<UserMetadata> UserMetadata { get; set; }
        //public DbSet<InventoryUnitFloorPlan> InventoryUnitFloorPlan { get; set; }
        //public DbSet<Punchlist> Punchlist { get; set; }
        //public DbSet<PunchlistCategory> PunchlistCategory { get; set; }
        //public DbSet<PunchlistSubCategory> PunchlistSubCategory { get; set; }
        //public DbSet<PunchlistNonComplianceTo> NonComplianceTo { get; set; }
        //public DbSet<PunchlistLocation> PunchlistLocation { get; set; }
        //public DbSet<PunchlistCostImpact> CostImpact { get; set; }
        //public DbSet<PunchlistScheduleImpact> ScheduleImpact { get; set; }
        //public DbSet<PunchlistStatus> PunchlistStatus { get; set; }
        //public DbSet<PunchlistComment> PunchlistComment { get; set; }
        //public DbSet<PunchlistCommentAttachment> PunchlistCommentAttachment { get; set; }
        //public DbSet<PunchlistAttachment> PunchlistAttachment { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Ignore<PropertyChangedEventHandler>();
            modelBuilder.Ignore<IIdentifiableEntity>();
            modelBuilder.Ignore<ExtensionDataObject>();
        }
    }
}
