using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using CTI.HI.Business.Entities.Notification;

namespace CTI.HI.Business.Managers.External
{
    [Export(typeof(IExternalMessagingService2))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PromoTexterManager : IExternalMessagingService2
    {
        public Task<Tuple<bool, string[]>> SendEmailAsync(SMSModel email)
        {
             throw new NotImplementedException();   
        }

        public Task<Tuple<bool, string[]>> SendEmailAsync(EmailModel email)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string[]>> SendSmsAsync(SMSModel sms)
        {
            throw new NotImplementedException();
            //  string apiURL = "https://api.zerobounce.net/v1/validate?apikey=" + apiKey + "&email=" + HttpUtility.UrlEncode(email);
        }

        public Task<Tuple<bool, string[]>> SendSmsAsync(string msg, string recipient)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<bool, string[]>> SendSMSAsync(SMSModel email)
        {
            throw new NotImplementedException();
        }
    }
}
