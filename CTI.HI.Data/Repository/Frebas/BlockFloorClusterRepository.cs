
using CTI.IMM.Business.Entities;
using CTI.IMM.Business.Entities.Model;
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
using CTI.HI.Business.Entities;
using System.Data;
using System.Data.SqlClient;

namespace CTI.HI.Data.Repository.Frebas
{
    [Export(typeof(IBlockFloorClusterRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class BlockFloorClusterRepository : DataRepositoryFrebasBase<BlockFloorCluster>, IBlockFloorClusterRepository
    {
        public BlockFloorCluster Get(string code)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    return cntxt.BlockFloorCluster.Where(p => p.Code == code).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IEnumerable<BlockFloorCluster>> GetBlockFloorClusterAsync()
        {
            using (var cntxt = new FrebasContext())
            {
                return await cntxt.BlockFloorCluster.ToListAsync();
            }
        }

        public async Task<IEnumerable<BlockFloorCluster>> GetBlockFloorClusterAsync(Expression<Func<BlockFloorCluster, bool>> expression)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    return await cntxt.BlockFloorCluster.Where(expression).ToListAsync();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IEnumerable<CTI.HI.Business.Entities.Block>> GetBlockFloorClusterAsync(string username, string phaseBuildingCode)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var usr = await cntxt.User.Where(u => u.UserName == username).FirstOrDefaultAsync();

                    if (usr == null)
                        throw new NullReferenceException("User does not exist");

                    var data = new List<CTI.HI.Business.Entities.Block>();

                    if (usr.UserTypeCode == "CONT")
                    {
                        data = await cntxt.VWUnitMilestones.Where(x => x.UserName == username
                                && x.PhaseCode == phaseBuildingCode && x.IsContractor == true).Distinct()
                                .Select(x => new CTI.HI.Business.Entities.Block
                                {
                                    Code = x.BlockCode,
                                    ShortName = x.BlockShortName,
                                    LongName = x.Block,
                                }).Distinct().OrderBy(x => x.LongName).ToListAsync();
                    }
                    else
                    {
                        data = await cntxt.VWUnitMilestones.Where(x => x.UserName == username
                                && x.PhaseCode == phaseBuildingCode && x.IsContractor == false && x.ProjectRoleCode != null).Distinct()
                                .Select(x => new CTI.HI.Business.Entities.Block
                                {
                                    Code = x.BlockCode,
                                    ShortName = x.BlockShortName,
                                    LongName = x.Block,
                                }).Distinct().OrderBy(x => x.LongName).ToListAsync();
                    }

                    if (data != null)
                    {
                        return data;
                    }
                    else
                    {
                        throw new NullReferenceException($"No block cluster found for phase: {phaseBuildingCode} and user: {username}");
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
        }

        public async Task<IEnumerable<InventoryUnit>> GetBlockFloorClusterInventoryUnitAsync(string blockFloorClusterCode)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    return await (from inv in cntxt.InventoryUnit.Where(i => i.BlockFloorClusterCode == blockFloorClusterCode)
                                  join otc in cntxt.InventoryUnitOTC
                                  on inv.ReferenceObject equals otc.ReferenceObject
                                  select inv)
                        .Distinct().OrderBy(p => p.ReferenceObject).ToListAsync();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        protected override BlockFloorCluster AddEntity(FrebasContext entityContext, BlockFloorCluster entity)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<BlockFloorCluster> GetEntities(FrebasContext entityContext)
        {
            throw new NotImplementedException();
        }

        protected override BlockFloorCluster GetEntity(FrebasContext entityContext, int id)
        {
            throw new NotImplementedException();
        }

        protected override BlockFloorCluster UpdateEntity(FrebasContext entityContext, BlockFloorCluster entity)
        {
            throw new NotImplementedException();
        }
    }
}
