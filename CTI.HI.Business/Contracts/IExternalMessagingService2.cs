using CTI.HI.Business.Entities.Notification;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CTI.HI.Business.Managers
{
    public interface IExternalMessagingService2
    {
        [OperationContract]
        Task<Tuple<bool, string[]>> SendSMSAsync(SMSModel email);

         [OperationContract]
        Task<Tuple<bool, string[]>> SendEmailAsync(EmailModel email);     
    }
}