using Core.Common.Contracts;
using CTI.HI.Business.Entities;
using CTI.IMM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Data.Contracts.Frebas
{
    public interface IMilestoneRepository : IDataRepository<Milestone>
    {
        Task<IEnumerable<Milestone>> GetAllMilestoneAsync();
        Task<Milestone> GetMilestoneAsync(string milCode);
        Task<IEnumerable<Milestone>> GetAllMilestoneAsync(Expression<Func<Milestone, bool>> expression);

        
    }
}
