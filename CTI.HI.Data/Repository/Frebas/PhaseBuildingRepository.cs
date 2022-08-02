 
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
using System.Data.SqlClient;
using System.Data;

namespace CTI.HI.Data.Repository.Frebas
{
    [Export(typeof(IPhaseBuildingRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PhaseBuildingRepository : DataRepositoryFrebasBase<PhaseBuilding>, IPhaseBuildingRepository
    {
        public PhaseBuilding Get(string code)
        {
            using (var cntxt = new FrebasContext())
            {
                return cntxt.PhaseBuilding.Where(p => p.Code == code).FirstOrDefault();
            }
        }

        public async Task<IEnumerable<PhaseBuilding>> GetAllPhaseBuildingAsync()
        {
            using (var cntxt = new FrebasContext())
            {
                return await cntxt.PhaseBuilding.ToListAsync();
            }
        }

        //public async Task<IEnumerable<PhaseBuilding>> GetAllPhaseBuildingAsync(Expression<Func<PhaseBuilding, bool>> expression)
        //{
        //    using (var cntxt = new FrebasContext())
        //    {
        //        return await cntxt.PhaseBuilding.Where(expression).ToListAsync();
        //    }
        //}

        public async Task<IEnumerable<CTI.HI.Business.Entities.PhaseBuilding>> GetAllPhaseBuildingAsync(string username, string projectcode)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    var usr = await cntxt.User.Where(u => u.UserName == username).FirstOrDefaultAsync();

                    if (usr == null)
                        throw new NullReferenceException("User does not exist");

                    var data = new List<CTI.HI.Business.Entities.PhaseBuilding>();

                    if (usr.UserTypeCode == "CONT")
                    {
                        data = await cntxt.VWUnitMilestones.Where(x => x.UserName == username && x.ProjectCode == projectcode && x.IsContractor == true).Distinct()
                              .Select(x => new CTI.HI.Business.Entities.PhaseBuilding
                              {
                                  Code = x.PhaseCode,
                                  ShortName = x.PhaseShortName,
                                  LongName = x.Phase
                              }).Distinct().OrderBy(x => x.LongName).ToListAsync();
                    }
                    else
                    {
                        data = await cntxt.VWUnitMilestones.Where(x => x.UserName == username && x.ProjectCode == projectcode 
                        && x.IsContractor == false && x.ProjectRoleCode != null).Distinct()
                              .Select(x => new CTI.HI.Business.Entities.PhaseBuilding
                              {
                                  Code = x.PhaseCode,
                                  ShortName = x.PhaseShortName,
                                  LongName = x.Phase
                              }).Distinct().OrderBy(x => x.LongName).ToListAsync();
                    }


                    if (data != null)
                    {
                        return data;
                    }
                    else
                    {
                        throw new NullReferenceException($"No phase found for project: {projectcode} and user: {username}");
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IEnumerable<BlockFloorCluster>> GetPhaseBuildingBlockFloorClusterAsync(string phaseBuildingCode)
        {
            using (var cntxt = new FrebasContext())
            {
                return await(from inv in cntxt.InventoryUnit.Where(i => i.PhaseBuildingCode == phaseBuildingCode)
                             join bfc in cntxt.BlockFloorCluster 
                             on inv.BlockFloorClusterCode equals bfc.Code
                             join otc in cntxt.InventoryUnitOTC
                             on inv.ReferenceObject equals otc.ReferenceObject
                             select bfc).Distinct().OrderBy(p => p.Code).ToListAsync();
            }
        }

        protected override PhaseBuilding AddEntity(FrebasContext entityContext, PhaseBuilding entity)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<PhaseBuilding> GetEntities(FrebasContext entityContext)
        {
            throw new NotImplementedException();
        }

        protected override PhaseBuilding GetEntity(FrebasContext entityContext, int id)
        {
            throw new NotImplementedException();
        }

        protected override PhaseBuilding UpdateEntity(FrebasContext entityContext, PhaseBuilding entity)
        {
            throw new NotImplementedException();
        }
    }
}
