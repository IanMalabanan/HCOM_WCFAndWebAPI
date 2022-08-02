using CTI.HI.Business.Entities;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business.Contracts
{
    [ServiceContract]
    public interface IPunchlistService
    {
        [OperationContract]
        Task<IEnumerable<Punchlist>> GetPunchlistsAsync(int ConstructionMilestoneId);

        [OperationContract]
        Task<IEnumerable<Punchlist>> GetPunchlistsByUnitAsync(string username,string ReferenceObject);

        [OperationContract]
        Task<Punchlist> GetPunchlistAsync(int PunchlistId);

        [OperationContract]
        Task<IEnumerable<PunchlistDescription>> GetPunchlistDescriptionsAsync();

        [OperationContract]
        Task<IEnumerable<PunchlistGroup>> GetPunchlistGroupAsync();

        [OperationContract]
        Task<IEnumerable<PunchlistCategory>> GetPunchlistCategoryAsync();

        [OperationContract]
        Task<IEnumerable<PunchlistSubCategory>> GetPunchlistSubCategoryAsync();

        [OperationContract]
        Task<IEnumerable<NonComplianceTo>> GetNonComplianceToAsync();

        [OperationContract]
        Task<IEnumerable<PunchlistLocation>> GetPunchlistLocationAync();

        [OperationContract]
        Task<IEnumerable<CostImpact>> GetCostImpactAsync();

        [OperationContract]
        Task<IEnumerable<ScheduleImpact>> GetScheduleImpactAsync();

        [OperationContract]
        Task<IEnumerable<PunchlistStatus>> GetPunchlistStatusAsync();

        [OperationContract]
        Task<IEnumerable<PunchlistStatus>> GetPunchlistStatusByRoleAsync(string roleCode);

        [OperationContract]
        Task<IEnumerable<Punchlist>> GetPunchlistbyProject(string username, string projectCode);

        ///////////////////////////////////////////////////

        [OperationContract]
        Task<IEnumerable<PunchlistComment>> GetPunchlistCommentAsync(int punchlistid);

        [OperationContract]
        Task<int> SavePunchlistAsync(string username, Business.Entities.Punchlist model);

        [OperationContract]
        Task<IEnumerable<Punchlist>> GetPunchlistsByUnitByVendorAsync(string username, string ReferenceObject, string vendorCode);
        [OperationContract]
        Task<IEnumerable<PunchlistAttachment>> GetMilestonePunchlistsImagesAsync();

        //[OperationContract]
        //Task<IEnumerable<Business.Entities.Comment>> GetPunchlistCommentsAsync(int PunchlistID);
    }
}
