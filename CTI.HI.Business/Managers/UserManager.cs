using Core.Common.Contracts;
using CTI.HI.Business.Contracts;
using CTI.HI.Business.Entities;
using CTI.HI.Data.Contracts.Frebas;
using CTI.HI.Data.Repository.Frebas;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business.Managers
{
    class UserManager : ManagerBase, IUserService
    {
        #region Constructors
        private IUserRepository _UserRepo;
        public UserManager()
        {
            _UserRepo = new UserRepository();
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        public UserManager(IDataRepositoryFactory dataRepositoryFactory) : this()
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;

        public UserManager(IBusinessEngineFactory businessEngineFactory) : this()
        {
            _BusinessEngineFactory = businessEngineFactory;
        }

        public UserManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory) : this()
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
        }
        #endregion
        
        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            Log.Information("AuthenticateUserAsync for {user}", username);
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    return await _UserRepo.AuthenticateUserAsync(username, password);
                }
                catch(Exception ex)
                {
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
                }
            });
        }

        public async Task<User> GetUserAsync(string username)
        {
            Log.Information("GetUserAsync by {user} ", username);
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    return await _UserRepo.GetUserInfoAsync(username);
                }
                catch (Exception ex)
                {
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
                }
            });


        }
    }
}
