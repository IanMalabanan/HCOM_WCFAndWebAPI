using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Interface
{
    public interface IFileUploadService
    {
        Task<PunchListBinaryImage> GetPunchlistImageByName(string filename);
        Task<string> DeletePunchlistImageByName(string filename);
        Task<int> SavePunchlistImage(PunchListBinaryImage model);
        Task<IEnumerable<PunchListBinaryImage>> GetPunchlistImage();
        Task<IEnumerable<ConstructionMilestoneBinaryImage>> GetMilestoneImage();
        Task<ConstructionMilestoneBinaryImage> GetMilestoneImageByName(string filename);
        Task<string> DeleteMilestoneImageByName(string filename);
        Task<int> SaveMilestoneImage(ConstructionMilestoneBinaryImage model);
    }
}
