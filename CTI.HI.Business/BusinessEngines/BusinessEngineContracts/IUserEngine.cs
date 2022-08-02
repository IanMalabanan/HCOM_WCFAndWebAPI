using Core.Common.Contracts; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business.BusinessEngines
{
    public interface IUserEngine : IBusinessEngine
    {
        Task<bool> isValidUsernameAsync(string userName); 
        Task<bool> isValidPasswordAsync( string password); 
           
        Task<bool> isValidMobileNumberAsync(string mobileNumber);
        Task<bool> isValidTelephoneNumberAsync(string telephoneNumber);
        Task<bool> isValidEmailAddressAsync(string emailAddress);

        Task<bool> isActiveAsync(string userName);
         
         
    }
}
