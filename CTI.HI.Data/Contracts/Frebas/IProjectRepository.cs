using Core.Common.Contracts;
using CTI.HCM.Business.Entities;
using CTI.IMM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Data.Contracts.Frebas
{
    public interface IProjectRepository : IDataRepository<Project>
    {
        Project Get(string code);
        Task<IEnumerable<Project>> GetAllProjectAsync();  
        Task<IEnumerable<Project>> GetProjectAsync(Expression<Func<Project,bool>> expression);
        Task<IEnumerable<PhaseBuilding>> GetProjectPhaseBuildingAsync(string projectCode);
        Task<IEnumerable<ProjectCoordinates>> GetProjectCoordinatesAsync(Expression<Func<ProjectCoordinates, bool>> expression);
        Task<IEnumerable<ProjectCoordinates>> GetProjectCoordinatesByProjectAsync(string projectcode);
        Task<IEnumerable<CTI.HI.Business.Entities.Project>> GetProjectByUserAsync(string username);
    }
}
