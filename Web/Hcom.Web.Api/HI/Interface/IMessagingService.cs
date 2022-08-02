using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.HI.Managers
{
   public interface IMessagingService
    { 
        //SmsValidation IsValid(string value);
        bool ValidateNumber(int Number);
        int getAvailableCredits();
    }
}
