using CTI.HI.Business.Entities;
using CTI.HI.Business.Entities.Notification;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business.Contracts
{
    [ServiceContract]
    public interface INotificationService
    {
        [OperationContract]
        Task<IEnumerable<NotificationModel>> GetOpenPunchlistsAsync(string userName);
        [OperationContract]
        Task<IEnumerable<NotificationModel>> GetOpenOverduePunchlistsAsync(string userName);
        [OperationContract]
        Task<IEnumerable<NotificationModel>> GetRecentlyClosedPunchlistsAsync(string userName);
        [OperationContract]
        Task<IEnumerable<NotificationModel>> GetDelayedMilestonesAsync(string userName);
        [OperationContract]
        Task<IEnumerable<NotificationModel>> GetRecentlyClosedMilestonesAsync(string userName);
        [OperationContract]
        Task<MessagingInformation> GetMessagingInformation(int punchlistId);
        [OperationContract]
        Task SendPunchlistNotificationAsync(int punchlistId);
        [OperationContract]
        Task SendTextTest(string number,string text);
        Task SendCompletePercentageNotification(HCM.Business.Entities.Models.ConstructionMilestoneModel milestone, IMM.Business.Entities.Model.UnitModel unit, ContractorAwardedLoa loa);
    }
}
