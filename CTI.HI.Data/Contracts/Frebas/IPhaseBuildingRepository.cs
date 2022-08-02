using Core.Common.Contracts; 
using CTI.IMM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks; 

namespace CTI.HI.Data.Contracts.Frebas
{
    public interface IPhaseBuildingRepository : IDataRepository<PhaseBuilding>
    {
        PhaseBuilding Get(string code);
        Task<IEnumerable<PhaseBuilding>> GetAllPhaseBuildingAsync();
        //Task<IEnumerable<PhaseBuilding>> GetAllPhaseBuildingAsync(Expression<Func<PhaseBuilding, bool>> expression);
        Task<IEnumerable<CTI.HI.Business.Entities.PhaseBuilding>> GetAllPhaseBuildingAsync(string username, string projectcode);
        Task<IEnumerable<BlockFloorCluster>> GetPhaseBuildingBlockFloorClusterAsync(string phaseBuildingCode);

    }
 
}
