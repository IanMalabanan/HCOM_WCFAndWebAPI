using Core.Common.Contracts;
using CTI.HCM.Business.Entities;
using CTI.HI.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Data.Contracts.Frebas
{
    public interface IPunchlistRepository : IDataRepository<PunchList>
    {
        Task<Business.Entities.Punchlist> GetPunchlistAsync(int PunchlistId);
        Task<Business.Entities.Punchlist> GetMilestonePunchlistAsync(int PunchlistId);
        Task<IEnumerable<Punchlist>> GetMilestonePunchlistsAsync(int ConstructionMilestoneId);
        Task<IEnumerable<Punchlist>> GetMilestonePunchlistsAsync(Expression<Func<Punchlist, bool>> expression = null);
        Task<IEnumerable<NotificationModel>> GetPunchlistsNotificationAsync(string username,Expression<Func<NotificationModel, bool>> expression = null);
        Task<IEnumerable<NotificationModel>> OpenPunchlistsNotificationAsync(string username, string punchlistStatusCode);
        Task<IEnumerable<NotificationModel>> OpenOverduePunchlistNotificationAsync(string username, string punchlistStatusCode);
        Task<IEnumerable<NotificationModel>> RecentlyClosedPunchlistNotificationAsync(string username, string punchlistStatusCode);

        Task<IEnumerable<Business.Entities.Comment>> GetPunchListCommentsAsync(int PunchlistId);
        Task<IEnumerable<InventoryUnitOTCContractorConstructionMilestonePunchListStatus>> GetMilestonePunchListStatusAsync(Expression<Func<InventoryUnitOTCContractorConstructionMilestonePunchListStatus, bool>> expression);
        Task<IEnumerable<Business.Entities.PunchlistDescription>> GetPunchlistAsync(Expression<Func<Business.Entities.PunchlistDescription, bool>> expression = null);  
        Task<IEnumerable<Business.Entities.PunchlistGroup>> GetPunchlistGroupAsync(Expression<Func<Business.Entities.PunchlistGroup, bool>> expression = null);  
        Task<IEnumerable<PunchListCategory>> GetPunchlistCategoryAsync(Expression<Func<PunchListCategory, bool>> expression = null); 
        Task<IEnumerable<PunchListSubCategory>> GetPunchListSubCategoryAsync(Expression<Func<PunchListSubCategory, bool>> expression = null); 
        Task<IEnumerable<PunchListCategoryNonCompliance>> GetPunchlistNonComplianceAsync(Expression<Func<PunchListCategoryNonCompliance, bool>> expression = null); 
        Task<IEnumerable<PunchListLocation>> GetPunchListLocationAsync(Expression<Func<PunchListLocation, bool>> expression = null); 
        Task<IEnumerable<PunchListCostImpact>> GetPunchlistCostImpactAsync(Expression<Func<PunchListCostImpact, bool>> expression = null); 
        Task<IEnumerable<PunchListScheduleImpact>> GetPunchListScheduleImpactAsync(Expression<Func<PunchListScheduleImpact, bool>> expression = null); 
        Task<IEnumerable<PunchListStatus>> GetPunchListStatusAsync(Expression<Func<PunchListStatus, bool>> expression = null);
        Task<int> SavePunchlistAsync(string username, Business.Entities.Punchlist model);
        Task<IEnumerable<Punchlist>> GetProjectMilestonePunchlist(string username ,string ProjectCode);
        Task<bool> MilestonePunchlistDescriptionAlreadyExistsAsync(int ConstructionMilestoneId, int PunchlistId, string Description);
        Task<bool> NotCloseDescriptionAndLocationAlreadyExistsAsync(int ConstructionMilestoneId, Business.Entities.Punchlist model);



        Task<IEnumerable<Business.Entities.Comment>> GetPunchlistCommentsAsync(int PunchlistID);
        Task<IEnumerable<Business.Entities.Punchlist>> GetMilestonePunchlistsByUnitAsync(string username, string referenceObject);
        Task<IEnumerable<Business.Entities.Punchlist>> GetMilestonePunchlistsByProjectAsync(string username, string projectCode);

        Task<IEnumerable<Business.Entities.Punchlist>> GetMilestonePunchlistsAsync(int[] ConstructionMilestoneId);

        Task<IEnumerable<Comment>> GetPunchListCommentsAsync(int[] PunchlistId);

        Task<IEnumerable<Comment>> GetPunchListCommentsAsync(int[] PunchlistId, string username);

        Task<IEnumerable<PunchlistAttachment>> GetMilestonePunchlistsImagesAsync();
        Task<IEnumerable<Business.Entities.Punchlist>> GetMilestonePunchlistsByUnitByVendorAsync(string username, string referenceObject, string vendorCode);


    }
}

