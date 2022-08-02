using Hcom.Web.Api.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IFileUploadRepository _fileUploadRepository;

        public FileUploadService(IFileUploadRepository fileUploadRepository)
        {
            _fileUploadRepository = fileUploadRepository;
        }

        public Task<string> DeleteMilestoneImageByName(string filename)
        {
            try
            {
                return _fileUploadRepository.DeleteMilestoneImageByName(filename);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public Task<string> DeletePunchlistImageByName(string filename)
        {
            try
            {
                return _fileUploadRepository.DeletePunchlistImageByName(filename);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IEnumerable<ConstructionMilestoneBinaryImage>> GetMilestoneImage()
        {
            try
            {
                return await _fileUploadRepository.GetMilestoneImage();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<ConstructionMilestoneBinaryImage> GetMilestoneImageByName(string filename)
        {
            try
            {
                return await _fileUploadRepository.GetMilestoneImageByName(filename);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IEnumerable<PunchListBinaryImage>> GetPunchlistImage()
        {
            try
            {
                return await _fileUploadRepository.GetPunchlistImage();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<PunchListBinaryImage> GetPunchlistImageByName(string filename)
        {
            try
            {
                return await _fileUploadRepository.GetPunchlistImageByName(filename);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<int> SaveMilestoneImage(ConstructionMilestoneBinaryImage model)
        {
            try
            {
                return await _fileUploadRepository.SaveMilestoneImage(model);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<int> SavePunchlistImage(PunchListBinaryImage model)
        {
            try
            {
                return await _fileUploadRepository.SavePunchlistImage(model);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
