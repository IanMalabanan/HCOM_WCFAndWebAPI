using Hcom.App.Entities.DTO;
using Hcom.App.Entities.HCOM;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Interface
{
    public interface IMilestone
    {
        Task<IEnumerable<ActualOnGoingConstructionMilestone>> GetMilestonesByUnitAsync(string username, string _path, string ReferenceObject);
        Task<ActualOnGoingConstructionMilestone> GetMilestoneByIdAsync(int id, string _path, string username);
        Task<IEnumerable<ActualOnGoingConstructionMilestone>> GetMilestoneByProjectIdAsync(string projectcode, string username);
        Task<bool> UpdateMilestonePercentageAsync(MilestonePercentageDto model, string _path, string username);


        Task<string> DeleteMilestoneAttachment(string filename);
        Task<ConstructionMilestoneBinaryImage> GetMilestoneAttachment(string filename);
        Task<string> UploadAllMilestoneImagesFromFTPServerToDatabase(string username);
        Task<string> SaveMilestoneAttachment(IFormFile uploadedFile, string username);

        Task<IEnumerable<ActualOnGoingConstructionMilestone>> GetMilestonesByUnitByVendorAsync(string username, string _path, string ReferenceObject, string vendorCode);
    }
}
