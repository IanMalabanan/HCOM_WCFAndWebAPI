using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Interface
{
    public interface IUnit
    {
        Task<IEnumerable<App.Entities.Project>> GetProjectByUserAsync(string username, string keyword = "");
        Task<IEnumerable<App.Entities.PhaseBuilding>> GetPhaseByProjectAsync(string projectCode, string username, string keyword = "");
        Task<IEnumerable<App.Entities.BlockFloorCluster>> GetBlockByPhaseAsync(string phaseCode, string username, string keyword = "");
        Task<IEnumerable<App.Entities.Unit>> GetUnitByBlockAsync(string blockCode, string username, string keyword = "");
        Task<App.Entities.Unit> GetUnitAsync(string referenceObject, string username);
        Task<IEnumerable<App.Entities.Unit>> GetUnitsByUserAsync(string username, string keyword = "");
        Task<IEnumerable<App.Entities.Unit>> GetUserunitsbyProjectAsync(string projectcode, string username);
        Task<App.Entities.Unit> GetUnitByVendorAsync(string referenceObject, string username, string vendorCode);
    }
}
