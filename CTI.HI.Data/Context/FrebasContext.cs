
using Core.Common.Contracts;
using CTI.ARM.Business.Entities;
using CTI.HCM.Business.Entities;
using CTI.IMM.Business.Entities;
using CTI.SSM.Business.Entities;
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
    [Export(typeof(FrebasContext))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class FrebasContext : DbContext
    {
        public FrebasContext() : base("FrebasConnection")
        {

            Database.SetInitializer<FrebasContext>(null);
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Database.CommandTimeout = 300;
            Database.Connection.Open();
            Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
            Database.Log = (x) => System.Diagnostics.Debug.WriteLine(x);
        }

        public DbSet<Project> Project { get; set; }
        public DbSet<PhaseBuilding> PhaseBuilding { get; set; }
        public DbSet<BlockFloorCluster> BlockFloorCluster { get; set; }
        public DbSet<InventoryUnit> InventoryUnit { get; set; }
        public DbSet<Milestone> Milestone { get; set; }
        public DbSet<ConstructionMilestone> ConstructionMilestone { get; set; }
        public DbSet<InventoryUnitOTCContractorConstructionMilestone> InventoryUnitOTCContractorConstructionMilestone { get; set; }
        public DbSet<InventoryUnitOTC> InventoryUnitOTC { get; set; }
        public DbSet<InventoryUnitOTCContractor> InventoryUnitOTCContractor { get; set; }
        public DbSet<InventoryUnitOTCContractorAwarded> InventoryUnitOTCContractorAwarded { get; set; }
        public DbSet<InventoryUnitOTCContractorAwardedLOA> InventoryUnitOTCContractorAwardedLOA { get; set; }
        public DbSet<InventoryUnitOTCContractorConstructionMilestoneMovement> InventoryUnitOTCContractorConstructionMilestoneMovement { get; set; }
        public DbSet<InventoryUnitOTCManagingContractor> InventoryUnitOTCManagingContractor { get; set; }
        public DbSet<InventoryUnitOTCQualified> InventoryUnitOTCQualified { get; set; }
        public DbSet<InventoryUnitConstructionTarget> InventoryUnitConstructionTarget { get; set; }
        public DbSet<OTCType> OTCType { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserType> UserType { get; set; }
        public DbSet<ContractorPersonnel> ContractorPersonnel { get; set; }
        public DbSet<ContractorPersonnelAuthorized> ContractorPersonnelAuthorized { get; set; }
        public DbSet<VendorMasterData> Vendor { get; set; }
        public DbSet<ProjectDirectory> ProjectDirectory { get; set; }
        public DbSet<ProjectRole> ProjectRole { get; set; }
        public DbSet<UserProjectRole> UserProjectRole { get; set; }
        public DbSet<ProjectCoordinates> ProjectCoordinates { get; set; }
        public DbSet<InventoryUnitCoordinates> InventoryUnitCoordinates { get; set; }
        public DbSet<HouseModelLayout> HouseModelLayout { get; set; }


        public DbSet<InventoryUnitOTCContractorConstructionMilestonePunchlist> MilestonePunchlist { get; set; }
        public DbSet<InventoryUnitOTCContractorConstructionMilestonePunchlistChanged> MilestonePunchlistChanged { get; set; }
        public DbSet<InventoryUnitOTCContractorConstructionMilestoneImage> MilestoneImage { get; set; }
        public DbSet<InventoryUnitOTCContractorConstructionMilestonePunchListStatus> MilestonePunchListStatus { get; set; }
        public DbSet<InventoryUnitOTCContractorConstructionMilestonePunchListComments> MilestonePunchListComments { get; set; }
        public DbSet<PunchListProjectSequence> PunchListProjectSequence { get; set; }
        public DbSet<PunchListScheduleImpact> PunchListScheduleImpact { get; set; }
        public DbSet<PunchListCostImpact> PunchListCostImpact { get; set; }
        public DbSet<PunchListSubCategory> PunchListSubCategory { get; set; }
        public DbSet<PunchListLocation> PunchListLocation { get; set; }
        public DbSet<PunchListCategoryNonCompliance> PunchListCategoryNonCompliance { get; set; }
        public DbSet<PunchList> PunchListSubject { get; set; }
        public DbSet<PunchListCategory> PunchListCategory { get; set; }
        public DbSet<InventoryUnitOTCContractorConstructionMilestonePunchListImage> PunchListImage { get; set; }
        public DbSet<PunchListStatus> PunchListStatus { get; set; }
        public DbSet<AssignInventoryUnitPhysicalCondition> AssignInventoryUnitPhysicalCondition { get; set; }
        public DbSet<ConstructionTradeType> MilestoneTradeTypes { get; set; }
        public DbSet<PunchListGroup> PunchListSubjectGroup { get; set; }
        public DbSet<InventoryUnitPreConstruct> InventoryUnitPreConstruct { get; set; }
        public DbSet<InventoryUnitOTCContractorConstructionMilestonePercentage> InventoryUnitOTCContractorConstructionMilestonePercentage { get; set; }

        public DbSet<InventoryUnitPercentage> InventoryUnitPercentage { get; set;}
        public DbSet<VWUserAuthorization> VWUserAuthorization { get; set;}
        public DbSet<ReferenceInventoryUnits> ReferenceInventoryUnits { get; set; }
        
        //public DbSet<PunchListBinaryImage> PunchlistBinaryImages { get; set; }
        //public DbSet<ConstructionMilestoneBinaryImage> ConstructionMilestoneBinaryImages { get; set; }
        


        public DbSet<Business.Entities.VWUnitMilestone> VWUnitMilestones { get; set; }

        public DbSet<Business.Entities.VWUnitMilestonesPunchlist> VWUnitMilestonesPunchlists { get; set; }

        public DbSet<Business.Entities.VWUnitMilestonesPunchlistComments> VWUnitMilestonesPunchlistComments { get; set; }

        public DbSet<Business.Entities.VWUnitMilestonesWithLoaAndTradeType> VWUnitMilestonesWithLoaAndTradeTypes { get; set; }

        public DbSet<Business.Entities.VWContractors> VWContractors { get; set; }




        //public DbSet<Business.Entities.MilestoneEntity.InventoryUnitBatchNumber> InventoryUnitBatchNumber {get; set;}
        //public DbSet<Business.Entities.MilestoneEntity.InventoryUnitOTCContractorPercentageDetails> InventoryUnitOTCContractorPercentageDetails { get; set;}
        //public DbSet<VendorMasterData> VendorMasterData { get; set;}
        //public DbSet<Business.Entities.MilestoneEntity.ViewOngoingConstructionMilestone> ViewOngoingConstructionMilestone { get; set; }









        //added
        //vwReferenceInventoryUnits  missing
        //armVendorMasterData        missing done
        //InventoryUnitOTCContractorPercentageDetails missing done
        //ConstructionMilestone missing done existing
        //InventoryUnitBatchNumber missing  done
        //BlockFloorClusterPriorityNumber missing done


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Ignore<PropertyChangedEventHandler>();
            modelBuilder.Ignore<IIdentifiableEntity>();
            modelBuilder.Ignore<ExtensionDataObject>();

            //modelBuilder.Entity<Business.Entities.VWUnitMilestonesPunchlistComments>()
            //    .HasKey(t => new { t.UserName, t.PunchlistID, t.CommentID });
        }


    }
}
