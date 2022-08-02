using CTI.HI.Business.Entities.Notification;
using Hcom.App.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Interface
{
    public interface IPunchlist
    {
        Task<bool> PunchlistSaveAsync(App.Entities.HCOM.Punchlist punch, string username, string _path);
        Task<IEnumerable<App.Entities.HCOM.PunchlistStatus>> GetPunchlistStatusbyRoleAsync(UserRoleType userRole, string username);
        Task<App.Entities.HCOM.Punchlist> GetPunchlistDetailsAsync(int Punchlistid, string _path);
        Task<IEnumerable<App.Entities.HCOM.Punchlist>> GetPunchlistAsync(int ConstructionMilestoneId, string _path);
        Task<IEnumerable<App.Entities.HCOM.PunchlistCategory>> GetPunchlistCategoryAsync();
        Task<IEnumerable<App.Entities.HCOM.PunchlistSubCategory>> GetPunchlistSubCategoryAsync();
        Task<IEnumerable<App.Entities.HCOM.NonComplianceTo>> GetNonCompliancesToAsync();
        Task<IEnumerable<App.Entities.HCOM.PunchlistLocation>> GetPunchlistLocationAsync();
        Task<IEnumerable<App.Entities.HCOM.CostImpact>> GetCostImpactAsync();
        Task<IEnumerable<App.Entities.HCOM.ScheduleImpact>> GetScheduledImpactAsync();
        Task<IEnumerable<App.Entities.HCOM.PunchlistStatus>> GetPunchlistStatusAsync();
        Task<List<App.Entities.HCOM.PunchlistDescription>> GetPunchlistDescriptions();
        Task<List<App.Entities.HCOM.PunchlistGroup>> GetPunchlistGroupAsync();
        Task<IEnumerable<App.Entities.HCOM.Punchlist>> GetPunchlistbyUnitAsync(string referenceObject, string username, string _path);
        Task<IEnumerable<App.Entities.HCOM.Punchlist>> GetPunchlistbyProjectAsync(string projectcode, string username, string _path);

        Task<IEnumerable<App.Entities.HCOM.Punchlist>> GetPunchlistbyUnitByVendorAsync(string referenceObject, string username, string _path, string vendorCode);

        bool SendSmsByInfobip(string msg, string recipient);
        bool SendEmailbyInfobip(EmailModel email);

        Task<string> SavePunchlistAttachment(IFormFile uploadedFile, string username);
        Task<string> UploadAllPunchlistImagesFromFTPServerToDatabase(string username);
        Task<PunchListBinaryImage> GetPunchlistAttachment(string filename);
        Task<string> DeletePunchlistAttachment(string filename);
    }
}
