using CTI.IMM.Business.Entities;
using CTI.HI.Data.Context;
using CTI.HI.Data.Contracts.Frebas;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CTI.HCM.Business.Entities;
using CTI.HI.Business.Entities;

namespace CTI.HI.Data.Repository.Frebas
{
    [Export(typeof(IMilestoneRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MilestoneRepository : DataRepositoryFrebasBase<Milestone>, IMilestoneRepository
    {
        public async Task<IEnumerable<Milestone>> GetAllMilestoneAsync()
        {
            using (var cntxt = new FrebasContext())
            {
                return await cntxt.Milestone.ToListAsync();
            }
        }

        public async Task<IEnumerable<Milestone>> GetAllMilestoneAsync(Expression<Func<Milestone, bool>> expression)
        {
            using (var cntxt = new FrebasContext())
            {
                return await cntxt.Milestone.Where(expression).ToListAsync();
            }
        }


        public async Task<Milestone> GetMilestoneAsync(string milCode)
        {
            using (var cntxt = new FrebasContext())
            {
                return await cntxt.Milestone.Where(t => t.Code == milCode).FirstOrDefaultAsync();
            }
        }

        


        //private IQueryable<UnitModel> GetUnitModelQueryable(FrebasContext cntxt)
        //{
        //    return (from imm in cntxt.InventoryUnit
        //            join pro in cntxt.Project
        //            on imm.ProjectCode equals pro.Code
        //            join phb in cntxt.PhaseBuilding
        //            on imm.PhaseBuildingCode equals phb.Code
        //            join bfc in cntxt.BlockFloorCluster
        //            on imm.BlockFloorClusterCode equals bfc.Code
        //            select new UnitModel
        //            {
        //                ReferenceObject = imm.ReferenceObject,
        //                Project = pro,
        //                PhaseBuilding = phb,
        //                BlockFloor = bfc,
        //                LotUnitShareNumber = imm.LotUnitShareNumber,
        //                InventoryUnitNumber = imm.InventoryUnitNumber
        //            });
        //}


        //private IQueryable<Business.Entities.Notification.PercentageComplete> GetMilestonePercentageComplete(FrebasContext cntxt)
        //{
        //    return cntxt.ViewOngoingConstructionMilestone.Select(x => new Business.Entities.Notification.PercentageComplete
        //    {
        //        BillingType = x.BILLINGTYPE,
        //        ContractNumber = x.CONTRACTNUMBER,
        //        NTPNumber = x.NTPNUMBER,
        //        PhaseBlkLot = x.PHASEBLKLOT,
        //        ReferenceObject = x.ReferenceObject,   
        //        MilestonePercentage = x.Milestone,
        //        Amount = x.Amount,
        //        ForBilling = x.ForBilling,
        //        Retention = x.Retention

        //    }); 
        //} 

        #region "others"

        protected override Milestone AddEntity(FrebasContext entityContext, Milestone entity)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Milestone> GetEntities(FrebasContext entityContext)
        {
            throw new NotImplementedException();
        }

        protected override Milestone GetEntity(FrebasContext entityContext, int id)
        {
            throw new NotImplementedException();
        }

        protected override Milestone UpdateEntity(FrebasContext entityContext, Milestone entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

//return (from imm in cntxt.InventoryUnitOTC
//        join pro in cntxt.InventoryUnitOTCManagingContractor
//        on imm.OTCNumber equals pro.OTCNumber
//        join otc in cntxt.InventoryUnitOTCContractorAwarded
//         on new { OTCNumber = pro.OTCNumber, ManagingContractor = pro.ManagingContractor, ManagingContractorId = pro.ID  }
//         equals new { OTCNumber = otc.OTCNumber, ManagingContractor = otc.ManagingContractor, ManagingContractorId = otc.ManagingContractorID}
//          join 
//        join phb in cntxt.PhaseBuilding
//        on imm.PhaseBuildingCode equals phb.Code
//        join bfc in cntxt.BlockFloorCluster
//        on imm.BlockFloorClusterCode equals bfc.Code

//        select new Business.Entities.Notification.PercentageComplete
//        {
//             BillingType = "",
//             ContractNumber = 0,
//             Date = System.DateTime.Now,
//             From = "",
//             PhaseBlkLot = "",
//        });
