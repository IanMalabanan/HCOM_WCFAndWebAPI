
using CTI.HI.Data.Context;
using CTI.HI.Data.Contracts.Frebas;
using CTI.IMM.Business.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Data.Repository.Frebas
{
    [Export(typeof(IUserProjectRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class UserProjectRepository : DataRepositoryFrebasBase<Project>, IUserProjectRepository
    {
        public async Task<string[]> GetProjectAsync(string userName)
        {
            using (var cntxt = new FrebasContext())
            {
                //get User
                var _usr = await cntxt.User.Where(u => u.UserName == userName).FirstOrDefaultAsync();

                if (_usr == null)
                    return null;

                try
                {
                    string[] data = null;

                    //if COntractor 
                    if (_usr.UserTypeCode == "CONT")
                    {
                        data = await (from proj in cntxt.VWUnitMilestones.Where(x => x.UserName == userName && x.IsContractor == true)
                                          select proj.ProjectCode)
                                .Distinct().ToArrayAsync();
                    }
                    else
                    {
                        data = await (from proj in cntxt.VWUnitMilestones.Where(x => x.UserName == userName && x.IsContractor == false && x.ProjectRoleCode != null)
                                      select proj.ProjectCode)
                                .Distinct().ToArrayAsync();
                    }

                    if (data != null)
                    {
                        return data;
                    }
                    else
                    {
                        throw new NullReferenceException($"No projects found for user: {userName}");
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
