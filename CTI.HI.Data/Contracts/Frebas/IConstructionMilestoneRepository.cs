using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks; 
using CTI.HCM.Business.Entities;
using CTI.HCM.Business.Entities.Models;
using CTI.HI.Business.Entities;

namespace CTI.HI.Data.Contracts.Frebas
{
    public interface IConstructionMilestoneRepository:IDataRepository<InventoryUnitOTCContractorConstructionMilestone>
    {
        Task<IEnumerable<ConstructionMilestoneModel>> GetConstructionMilestoneAsync(string ReferenceObject);
        Task<IEnumerable<ConstructionMilestoneModel>> GetConstructionMilestoneAsync(Func<ConstructionMilestoneModel, bool> expression);
        Task<IEnumerable<MilestoneAttachment>> GetConstructionMilestoneAttachmentAsync(Expression<Func<MilestoneAttachment, bool>> expression);
        Task<IEnumerable<MilestoneAttachment>> GetConstructionMilestoneAttachmentAsync(int id);

        Task<bool> UpdateMilestonePercentageAsync(string userName,
                                                            int ConstructionMilestoneId,
                                                            string otcNumber,
                                                            string contractorNumber,
                                                            int managingContractorId,
                                                            decimal newPercentage, 
                                                            string milestoneCode,
                                                            IEnumerable<MilestoneAttachment> attachments,
                                                            DateTime? dateVisited = null);

        Task<IEnumerable<InventoryUnitOTCContractorConstructionMilestone>> GetConstructionMilestonesAsync(Expression<Func<InventoryUnitOTCContractorConstructionMilestone, bool>> expression);
        Task<IEnumerable<NotificationModel>> GetUserConstructionMilestoneNotificationAsync(string userName);
        Task<IEnumerable<ConstructionMilestoneModel>> GetUserConstructionMilestoneAsync(string userName, Func<ConstructionMilestoneModel, bool> expression);
        Task<string> GetMilestoneReferenceObjectAsync(int ConstructionMilestoneId);
        Task<IEnumerable<ContractorAwardedLoa>> GetLOADetails(Expression<Func<ContractorAwardedLoa, bool>> expression);
        Task<InventoryUnitOTCContractorConstructionMilestonePercentage> GetMilestonePOCAsync(Expression<Func<InventoryUnitOTCContractorConstructionMilestonePercentage, bool>> expression);
        Task<IEnumerable<InventoryUnitOTCContractorConstructionMilestonePercentage>> GetMilestonesPOCAsync(Expression<Func<InventoryUnitOTCContractorConstructionMilestonePercentage, bool>> expression);
        Task<IEnumerable<InventoryUnitOTCContractorConstructionMilestonePercentage>> GetMilestonesPOCAsync(int id);

        Task<IEnumerable<Business.Entities.ConstructionMilestone>> GetConstructionMilestoneByID(string username, int id);
        Task<IEnumerable<Business.Entities.ConstructionMilestone>> GetConstructionMilestoneByUnit(string username,string referenceObject);
        Task<IEnumerable<Business.Entities.ConstructionMilestone>> GetConstructionMilestoneByProject(string username, string projectcode);

        Task<IEnumerable<MilestoneAttachment>> GetConstructionMilestoneAttachmentAsync(int[] id);

        Task<IEnumerable<Business.Entities.ConstructionMilestone>> GetConstructionMilestoneByUnitByVendor(string username, string referenceObject, string vendorCode);

        Task<IEnumerable<MilestoneAttachment>> GetMilestoneImagesAsync();

    }
}
