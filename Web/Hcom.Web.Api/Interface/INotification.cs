using Hcom.App.Entities.HCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Interface
{
    public interface INotification
    {
        Task<IEnumerable<TransactionNotificationModel>> GetOpenPunchlistAsync(string username);
        Task<IEnumerable<TransactionNotificationModel>> GetOpenOverduePunchlistsAsync(string username);
        Task<IEnumerable<TransactionNotificationModel>> GetRecentlyClosedPunchlistsAsync(string username);
        Task<IEnumerable<TransactionNotificationModel>> GetDelayedMilestonesAsync(string username);
        Task<IEnumerable<TransactionNotificationModel>> GetRecentlyClosedMilestoneAsync(string username);

    }
}
