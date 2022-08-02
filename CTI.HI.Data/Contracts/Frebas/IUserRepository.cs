using Core.Common.Contracts;
using CTI.SSM.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Data.Contracts.Frebas
{
    public interface IUserRepository : IDataRepository<User>
    {
        Task<CTI.HI.Business.Entities.User> AuthenticateUserAsync(string userName,string password);
        Task<CTI.HI.Business.Entities.User> GetUserInfoAsync(string userName);
        Task<CTI.HI.Business.Entities.User> GetUserInfoBaseOnDateAsync(string userName,DateTime ReferenceDate);
        Task<double> GetUserInspectionRadius(string RoleCode);
    }
}
