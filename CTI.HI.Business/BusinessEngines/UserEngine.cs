using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using CTI.HI.Business.BusinessEngines;
using CTI.HI.Business.Entities; 

namespace CTI.HI.Business.BusinessEngines
{
    [Export(typeof(IUserEngine))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class UserEngine : IUserEngine
    {

        [ImportingConstructor]
        public UserEngine(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        IDataRepositoryFactory _DataRepositoryFactory;

     
         


        public Task<bool> isValidUsernameAsync(string userName)
        {
            return Task.Run(() => { return true; });
        }
        public  Task<bool> isValidPasswordAsync(string password)
        {
            return Task.Run(() => { return true; });
        }

        public Task<bool> isValidMobileNumberAsync(string mobileNumber)
        {
            return Task.Run(() => { return true; });
        }

        public Task<bool> isValidTelephoneNumberAsync(string telephoneNumber)
        {
            return Task.Run(() => { return true; });
        }

        public Task<bool> isValidEmailAddressAsync(string emailAddress)
        {
            return Task.Run(() => { return true; });
        }

        public Task<bool> isActiveAsync(string userName)
        {
            throw new Exception();
            
        }

        //public async Task<IEnumerable<ApplicationException>> validateAllAsync(User user)
        //{
        //    List<ApplicationException> result = new List<ApplicationException>();

        //    if (!(await isValidUsernameAsync(user.UserName)))
        //        result.Add(new ApplicationException("Username is invalid"));
        //    if (!(await isValidPasswordAsync(user.Password)))
        //        result.Add(new ApplicationException("Password is invalid"));

        //    return result;
        //}
    }
}
