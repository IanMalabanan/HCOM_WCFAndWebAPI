using CTI.HI.Business.Entities;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business.Contracts
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        Task<User> AuthenticateUserAsync(string username,string password);

        [OperationContract]
        Task<User> GetUserAsync(string username);
    }
}
