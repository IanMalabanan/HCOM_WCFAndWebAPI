using Core.Common.Contracts; 
using CTI.IMM.Business.Entities;
using CTI.IMM.Business.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Data.Contracts.Frebas
{
    public interface IBlockFloorClusterRepository : IDataRepository<BlockFloorCluster>
    {
        BlockFloorCluster Get(string code);
        //Task<IEnumerable<BlockFloorCluster>> GetBlockFloorClusterAsync();
        Task<IEnumerable<CTI.HI.Business.Entities.Block>> GetBlockFloorClusterAsync(string username, string phaseBuildingCode);
        Task<IEnumerable<BlockFloorCluster>> GetBlockFloorClusterAsync(Expression<Func<BlockFloorCluster, bool>> expression);
        Task<IEnumerable<InventoryUnit>> GetBlockFloorClusterInventoryUnitAsync(string blockFloorClusterCode); 
    }
}
