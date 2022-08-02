using Hcom.Web.Api.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Repositories
{
    public class FileUploadRepository : IFileUploadRepository
    {
        private readonly FileUploadDBContext _fileUploadDBContext;
        
        public FileUploadRepository(FileUploadDBContext fileUploadDBContext)
        {
            _fileUploadDBContext = fileUploadDBContext;
        }

        public async Task<string> DeleteMilestoneImageByName(string filename)
        {
            try
            {
                var data = await _fileUploadDBContext.ConstructionMilestoneBinaryImages
                        .Where(x => x.FileName == filename).FirstOrDefaultAsync();

                if (data == null)
                {
                    throw new Exception("Image not found");
                }
                else
                {
                    _fileUploadDBContext.ConstructionMilestoneBinaryImages.Remove(data);
                    
                    await _fileUploadDBContext.SaveChangesAsync();

                    return "Deleted";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> DeletePunchlistImageByName(string filename)
        {
            try
            {
                var data = await _fileUploadDBContext.PunchListBinaryImages
                        .Where(x => x.FileName == filename).FirstOrDefaultAsync();

                if (data == null)
                {
                    throw new Exception("Image not found");
                }
                else
                {
                    _fileUploadDBContext.PunchListBinaryImages.Remove(data);

                    await _fileUploadDBContext.SaveChangesAsync();

                    return "Deleted";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ConstructionMilestoneBinaryImage>> GetMilestoneImage()
        {
            try
            {
                var data = await _fileUploadDBContext.ConstructionMilestoneBinaryImages.ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ConstructionMilestoneBinaryImage> GetMilestoneImageByName(string filename)
        {
            try
            {
                var data = await _fileUploadDBContext.ConstructionMilestoneBinaryImages
                    .Where(x => x.FileName == filename).FirstOrDefaultAsync().ConfigureAwait(false);
                
                    return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PunchListBinaryImage> GetPunchlistImageByName(string filename)
        {
            try
            {
                var data = await _fileUploadDBContext.PunchListBinaryImages
                    .Where(x => x.FileName == filename).FirstOrDefaultAsync();

                if (data == null)
                {
                    return new PunchListBinaryImage();
                }
                else
                {
                    return data;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<PunchListBinaryImage>> GetPunchlistImage()
        {
            try
            {
                var data = await _fileUploadDBContext.PunchListBinaryImages.ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> SaveMilestoneImage(ConstructionMilestoneBinaryImage model)
        {
            try
            {
                var mls = await _fileUploadDBContext.ConstructionMilestoneBinaryImages
                    .Where(x => x.FileName == model.FileName).FirstOrDefaultAsync();

                if (mls == null)
                {
                    ConstructionMilestoneBinaryImage data = new ConstructionMilestoneBinaryImage();

                    data.FileName = model.FileName;

                    data.FileBinary = model.FileBinary;

                    data.CreatedBy = model.CreatedBy;

                    data.DateCreated = DateTime.Now;

                    _fileUploadDBContext.ConstructionMilestoneBinaryImages.Add(data);

                    await _fileUploadDBContext.SaveChangesAsync();

                    return data.FileID;
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> SavePunchlistImage(PunchListBinaryImage model)
        {
            try
            {
                var pun = await _fileUploadDBContext.PunchListBinaryImages
                    .Where(x => x.FileName == model.FileName).FirstOrDefaultAsync();

                if (pun == null)
                {
                    PunchListBinaryImage data = new PunchListBinaryImage();

                    data.FileName = model.FileName;

                    data.FileBinary = model.FileBinary;

                    data.CreatedBy = model.CreatedBy;

                    data.DateCreated = DateTime.Now;

                    _fileUploadDBContext.PunchListBinaryImages.Add(data);

                    await _fileUploadDBContext.SaveChangesAsync();

                    return data.FileID;
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
