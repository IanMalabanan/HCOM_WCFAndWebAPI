using CTI.HI.Business.Entities; 
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business.Contracts
{
    [ServiceContract]
    public interface IUnitService
    {
        [OperationContract]
        //Task<IEnumerable<Project>> GetProjectByUserAsync(string username, string userRole, double latitude, double longitude);
        Task<IEnumerable<Project>> GetProjectByUserAsync(string username);
        [OperationContract]
        //Task<IEnumerable<PhaseBuilding>> GetPhaseBuildingByProjectAsync(string username, string projectCode, string userRole, double latitude, double longitude);
        Task<IEnumerable<PhaseBuilding>> GetPhaseBuildingByProjectAsync(string username, string projectCode);
        [OperationContract]
        //Task<IEnumerable<Block>> GetBlockByPhaseBuildingAsync(string username, string phaseBuildingCode, string userRole, double latitude, double longitude);
        Task<IEnumerable<Block>> GetBlockByPhaseBuildingAsync(string username, string phaseBuildingCode);
        //[OperationContract]
        //Task<IEnumerable<Unit>> GetUnitByBlockAsync(string username, string blockCode, string userRole, double latitude, double longitude);
        [OperationContract]
        Task<IEnumerable<Unit>> GetUnitByBlockAsync(string username, string blockCode);
        //[OperationContract]
        //Task<IEnumerable<Unit>> GetUnitByProjectAsync(string username, string projectCode, string userRole, double latitude, double longitude);
        [OperationContract]
        Task<IEnumerable<Unit>> GetUnitByUserAsync(string username, string userRole, double latitude, double longitude); 
        [OperationContract]
        Task<Unit> GetUnitAsync(string username, string userRole, double latitude, double longitude, string referenceObject);
        [OperationContract]
        Task<string[]> GetUnitFloorPlan(string referenceObject);
        [OperationContract]
        Task<IEnumerable<Unit>> GetUnitByReferenceObjects(string[] referenceObject);
        [OperationContract]
        Task<IEnumerable<Unit>> GetUnitByRefObject(string username,string referenceObject);



        [OperationContract]
        Task<IEnumerable<Unit>> GetNewUnitByUserAsync(string username);

        [OperationContract]
        Task<IEnumerable<Unit>> GetNewUnitByProjectAsync(string username, string projectCode);

        [OperationContract]
        Task<IEnumerable<Unit>> GetUnitByRefObjectByVendor(string username, string referenceObject, string vendorCode);
    }
}
