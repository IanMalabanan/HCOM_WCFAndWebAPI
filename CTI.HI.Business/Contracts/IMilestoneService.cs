using CTI.HI.Business.Entities;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business.Contracts
{
    [ServiceContract]
    public interface IMilestoneService
    {
        [OperationContract]
        Task<IEnumerable<ConstructionMilestone>> GetMilestonesByUnitAsync(string username,string ReferenceObject);
        [OperationContract]
        Task<IEnumerable<ConstructionMilestone>> GetMilestonesAsync(string otcNumber, string contractorNumber, int ManagingContractorID);
        [OperationContract]
        Task<ConstructionMilestone> GetMilestoneAsync(string otcNumber, string contractorNumber, int ManagingContractorID, string milestoneCode);
        [OperationContract]
        Task<ConstructionMilestone> GetMilestoneByIdAsync(string username,int constructionMilestoneId);
        [OperationContract]
        Task<bool> UpdateMilestonePercentageAsync(string userName,
                                                            int ConstructionMilestoneId,
                                                            string otcNumber,
                                                            string contractorNumber,
                                                            int managingContractorId,
                                                            decimal newPercentage,
                                                            string milestoneCode,
                                                            IEnumerable<MilestoneAttachment> attachments,
                                                            DateTime? dateVisited = null);

        [OperationContract]
        Task<IEnumerable<ConstructionMilestone>> GetMilestoneByProjectAsync(string username, string projectCode);

        [OperationContract]
        Task<IEnumerable<Business.Entities.ConstructionMilestone>> GetMilestonesByUnitByVendorAsync(string username, string ReferenceObject, string vendorCode);

        [OperationContract]
        Task<IEnumerable<MilestoneAttachment>> GetMilestoneImagesAsync();



    }
}
