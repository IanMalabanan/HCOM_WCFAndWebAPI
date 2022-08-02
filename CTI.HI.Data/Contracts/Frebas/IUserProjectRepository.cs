using Core.Common.Contracts;
using CTI.IMM.Business.Entities;
//using CTI.HCOM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Data.Contracts.Frebas
{
    public interface IUserProjectRepository : IDataRepository<Project>
    { 
        Task<string[]> GetProjectAsync(string userName);  
        //Task<IEnumerable<UserProject>> GetProjectAsync(Expression<Func<UserProject, bool>> expression); 
    }
}
