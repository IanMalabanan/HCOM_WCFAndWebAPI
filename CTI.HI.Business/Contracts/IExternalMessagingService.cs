using CTI.HI.Business.Entities.Notification;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CTI.HI.Business.Managers
{
    public interface IExternalMessagingService
    { 
        [OperationContract]
        Task<Tuple<bool, string[]>> SendSmsAsync(string msg, string recipient); //SMSModel sms

         [OperationContract]
        Task<Tuple<bool, string[]>> SendEmailAsync(EmailModel email);     
    }
}