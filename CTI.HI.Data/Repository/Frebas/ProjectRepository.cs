
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
using System.Data.SqlClient;
using System.Data;

namespace CTI.HI.Data.Repository.Frebas
{
    [Export(typeof(IProjectRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ProjectRepository : DataRepositoryFrebasBase<Project>, IProjectRepository
    {
        public Project Get(string code)
        {
            using (var cntxt = new FrebasContext())
            {
                return cntxt.Project.Where(p => p.Code == code).FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Project>> GetAllProjectAsync()
        {
            using (var cntxt = new FrebasContext())
            {
                return await cntxt.Project.OrderBy(x => x.LongName).ToListAsync();
            }
        }

        public async Task<IEnumerable<Project>> GetProjectAsync(Expression<Func<Project, bool>> expression)
        {
            using (var cntxt = new FrebasContext())
            {
                return await cntxt.Project.Where(expression).OrderBy(x => x.LongName).ToListAsync();
            }
        }

        public async Task<IEnumerable<CTI.HI.Business.Entities.Project>> GetProjectByUserAsync(string username)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var usr = await cntxt.User.Where(u => u.UserName == username).FirstOrDefaultAsync();

                    if (usr == null)
                        throw new NullReferenceException("User does not exist");

                    var data = new List<CTI.HI.Business.Entities.Project>();


                    //if COntractor 
                    if (usr.UserTypeCode == "CONT")
                    {
                        data = await cntxt.VWUnitMilestones
                        .Where(x => x.UserName == username && x.IsContractor == true
                        && x.Latitude != null && x.Latitude > 0 && x.Longitude != null && x.Longitude > 0).Distinct()
                        .Select(x => new CTI.HI.Business.Entities.Project
                        {
                            Code = x.ProjectCode,
                            ShortName = x.ProjectShortName,
                            LongName = x.ProjectName,
                            Longitude = x.Longitude ?? 0,
                            Latitude = x.Latitude ?? 0,
                            ImageUrl = x.ImageURL ?? "",
                        }).Distinct().OrderBy(x => x.LongName)
                        .ToListAsync();
                    }
                    else
                    {
                        data = await cntxt.VWUnitMilestones
                        .Where(x => x.UserName == username && x.IsContractor == false && x.ProjectRoleCode != null
                        && x.Latitude != null && x.Latitude > 0 && x.Longitude != null && x.Longitude > 0).Distinct()
                        .Select(x => new CTI.HI.Business.Entities.Project
                        {
                            Code = x.ProjectCode,
                            ShortName = x.ProjectShortName,
                            LongName = x.ProjectName,
                            Longitude = x.Longitude ?? 0,
                            Latitude = x.Latitude ?? 0,
                            ImageUrl = x.ImageURL ?? "",
                        }).Distinct().OrderBy(x => x.LongName)
                        .ToListAsync();
                    }

                    if (data.Count > 0)
                    {
                        return data;
                    }
                    else
                    {
                        throw new NullReferenceException($"No projects found for user: {username}");
                    }
                }
                catch(NullReferenceException ex)
                {
                    throw new NullReferenceException(ex.Message, ex.InnerException);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }
        }

        public async Task<IEnumerable<ProjectCoordinates>> GetProjectCoordinatesAsync(Expression<Func<ProjectCoordinates, bool>> expression)
        {
            using (var cntxt = new FrebasContext())
            {
                return await cntxt.ProjectCoordinates.Where(expression).ToListAsync();
            }
        }

        public async Task<IEnumerable<ProjectCoordinates>> GetProjectCoordinatesByProjectAsync(string projectcode)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    return await cntxt.ProjectCoordinates.Where(x => x.ProjectCode == projectcode).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<PhaseBuilding>> GetProjectPhaseBuildingAsync(string projectCode)
        {
            using (var cntxt = new FrebasContext())
            {
                return await (from inv in cntxt.InventoryUnit.Where(i => i.ProjectCode == projectCode)
                              join phs in cntxt.PhaseBuilding
                              on inv.PhaseBuildingCode equals phs.Code
                              join otc in cntxt.InventoryUnitOTC
                              on inv.ReferenceObject equals otc.ReferenceObject
                              select phs).Distinct().OrderBy(p => p.LongName).ToListAsync();

                //return x.GroupBy(p => p.Code)
                //  .Select(g => g.First())
                //  .ToList(); 
            }
        }

        protected override Project AddEntity(FrebasContext entityContext, Project entity)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Project> GetEntities(FrebasContext entityContext)
        {
            throw new NotImplementedException();
        }

        protected override Project GetEntity(FrebasContext entityContext, int id)
        {
            throw new NotImplementedException();
        }

        protected override Project UpdateEntity(FrebasContext entityContext, Project entity)
        {
            throw new NotImplementedException();
        }
    }
}
