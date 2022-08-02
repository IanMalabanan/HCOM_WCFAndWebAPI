using Core.Common.Contracts;
using CTI.HCM.Business.Entities;
using CTI.HI.Business.Entities;
using CTI.IMM.Business.Entities;
using CTI.IMM.Business.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Data.Contracts.Frebas
{
    public interface IInventoryUnitRepository : IDataRepository<InventoryUnit>
    { 
        Task<IEnumerable<InventoryUnit>> GetInventoryUnitAsync(Expression<Func<InventoryUnit,bool>> expression);
        Task<UnitModel> GetUnitAsync(string referenceObject);
        Task<IEnumerable<UnitModel>> GetUnitAsync(Expression<Func<UnitModel, bool>> expression);

        Task<IEnumerable<Unit>> GetUnitByBlockAsync(string username, string block);

        Task<IEnumerable<InventoryUnitCoordinates>> GetUnitCoordinatesAsync(Expression<Func<InventoryUnitCoordinates, bool>> expression);
        Task<IEnumerable<HouseModelLayout>> GetInventoryFloorPlanAsync(string ReferenceObject);
        Task<IEnumerable<HouseModelLayout>> GetInventoryFloorPlanAsync(Expression<Func<HouseModelLayout, bool>> expression);
        Task<IEnumerable<UnitModel>> GetNearbyUnitAsync(string RoleCode, double inspectionRadius, double latitude, double longitude, string[] referenceObjectArray);
        Task<IEnumerable<AssignInventoryUnitPhysicalCondition>> GetUnitPhysicalConditionAsync(Expression<Func<AssignInventoryUnitPhysicalCondition, bool>> expression);
        Task<IEnumerable<UnitContractor>> GetUnitContractorAsync(string[] ReferenceObjects);
        Task<IEnumerable<UnitContractor>> GetUnitContractorDetailsAsync(string username,string ReferenceObjects);

        Task<IEnumerable<MilestonePercentage>> GetUnitMilestonePercentageAsync(string[] ReferenceObjects);
        Task<MilestonePercentage> GetNewUnitMilestonePercentageAsync(string username,string ReferenceObjects);


        Task<IEnumerable<Unit>> GetUnitByRefObject(string username, string ReferenceObject);

        Task<string[]> FilterInternalPTGReferenceObjectsAsync(string[] referenceObjectArray);
        Task<UnitModel> GetUnitByOTCNumberAsync(string otcNumber);


        Task<IEnumerable<Unit>> GetNewUnitByUser(string username);
        Task<IEnumerable<Unit>> GetNewUnitByProject(string username, string projectCode);

        Task<IEnumerable<UnitContractor>> GetUnitContractorDetailsAsync(string username, string[] ReferenceObjects);

        Task<IEnumerable<MilestonePercentage>> GetNewUnitMilestonePercentageAsync(string username, string[] ReferenceObjects);
        Task<IEnumerable<HouseModelLayout>> GetInventoryFloorPlanAsync(string[] ReferenceObject);

        Task<IEnumerable<Unit>> GetUnitByRefObjectByVendor(string username, string ReferenceObject, string vendorCode);
    }
}
