using Core.Common.Contracts;
using CTI.HI.Business.Contracts;
using CTI.HI.Business.Entities.Notification;
using CTI.HI.Business.Managers.External;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
    ConcurrencyMode = ConcurrencyMode.Multiple)]
    class MessagingManager : IMessagingService, IExternalMessagingService
    {  
        //private readonly IRepository _repo;
        [Import]
        private readonly IExternalMessagingService2 _smsService;

        private readonly IExternalMessagingService2 _emailService;


        //[Import]
        //IExternalMessagingService _smsService;

        public MessagingManager()
        {
            _smsService = new InfoBipManager();
            _emailService = new InfoBipManager();
        }
        
        public MessagingManager(IExternalMessagingService2 externalMessagingService) 
        {
            _smsService = externalMessagingService;
        }
        public MessagingManager(IExternalMessagingService2 smsService, IExternalMessagingService2 emailService )
        {
            _smsService = smsService;
            _emailService = new InfoBipManager(); //emailService
           // _repo = repo; 
        }

        //public MessagingManager(IExternalMessagingService smsService)
        //{
        //    _smsService = smsService;
        //}
        //public MessagingManager()
        //{
        // _smsService = new InfoBipManager();
        //  _emailService = new PromoTexterManagerr();
        //}

        public async Task<Tuple<bool, string[]>> SendSmsAsync(string msg, string recipient)
        {
            try
            {
                Log.Information("Messaging : SendSmsAsync");
                // var _Repo =   //_DataRepositoryFactory.<IExternalMessagingService>();
                var sms = new SMSModel
                {
                    Text = msg,
                    To = new string[] { recipient }
                };
                return await _smsService.SendSMSAsync(sms);
            }
            catch(Exception ex)
            {
                Log.Error("Exception detail {ex}", ex);
                throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
            }
        }

        public async Task<Tuple<bool, string[]>> SendEmailAsync(EmailModel sms)
        {
            try
            {
                Log.Information("Messaging : SendEmailAsync");
                return await _emailService.SendEmailAsync(sms);
            }
            catch (Exception ex)
            {
                Log.Error("Exception detail {ex}", ex);
                throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
            } 
        }

    }
}
