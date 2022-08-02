using Hcom.App.Entities;
using Hcom.App.Entities.DTO;
using Hcom.App.Entities.HCOM;
using Hcom.App.Entities.Models;
using Hcom.Web.Api.Interface;
using Hcom.Web.Api.Models;
using Hcom.Web.Api.Utilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserEP = UserSvcRefence;
using WCF = MilestoneSvcReference;
using MilestoneAttachment = MilestoneSvcReference.MilestoneAttachment;
using System.IO;
using System.Net;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace Hcom.Web.Api.Services
{
    public class MilestoneService : IMilestone
    {
        private readonly ServiceEndpoints _endpointSettings;
        private readonly FTPSettings _fTPSettings;
        private readonly IFileUploadService _fileUploadService;
        private WCF.IMilestoneService _wcfServices;
        private UserEP.IUserService _userServices;
        public const string mileStoneServiceEndpoint = "Milestone.svc";
        public const string punchlistServiceEndpoint = "PunchlistService.svc";
        public const string userServiceEndpoint = "UserService.svc";

        public MilestoneService(IOptionsMonitor<ServiceEndpoints> endpointAccessor, IOptionsMonitor<FTPSettings> ftpsettings, IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;

            _endpointSettings = endpointAccessor.CurrentValue;

            _fTPSettings = ftpsettings.CurrentValue;

            _wcfServices = new WCF.MilestoneServiceClient(WCF.MilestoneServiceClient.EndpointConfiguration.BasicHttpBinding_IMilestoneService,
                 _endpointSettings.BaseUrl + mileStoneServiceEndpoint);

            _userServices = new UserEP.UserServiceClient(UserEP.UserServiceClient.EndpointConfiguration.BasicHttpBinding_IUserService,
                _endpointSettings.BaseUrl + userServiceEndpoint);
        }

        public async Task<IEnumerable<ActualOnGoingConstructionMilestone>> GetMilestonesByUnitAsync(string username, string _path, string ReferenceObject)
        {
            try
            {
                var unitmilestone = (await _wcfServices.GetMilestonesByUnitAsync(username, ReferenceObject));
                var ongoingmilestone = unitmilestone.Select(item => new ActualOnGoingConstructionMilestone()
                {
                    Id = item.Id,
                    OTCNumber = item.OTCNumber,
                    ContractorNumber = item.ContractorNumber,
                    ManagingContractorID = item.ManagingContractorID,
                    ConstructionMilestoneCode = item.ConstructionMilestoneCode,
                    ConstructionMilestoneDescription = item.ConstructionMilestoneDescription,
                    Weight = item.Weight,
                    Sequence = item.Sequence,
                    PONumber = item.PONumber,
                    Trade = item.TradeCode,
                    OTCTypeCode = item.OTCTypeCode,
                    PercentageCompletion = item.PercentageCompletion,
                    PercentageCompletionQA = item.PercentageCompletionQA,
                    PercentageCompletionEngineer = item.PercentageCompletionEngineer,
                    PercentageCompletionContractor = item.PercentageCompletionContractor,
                    PercentageReferenceNumber = item.PercentageReferenceNumber,
                    MilestoneImages = (item.Attachments != null ? item.Attachments.Select(f => _path + f.FileName).ToList() : null),
                    ManagingContractor = DataMapping.MapManagingContractorByCode(item.ManagingContractorCode),
                    Punchlists = (item.Punchlists != null ? item.Punchlists.Select(param => new Punchlist()
                    {
                        PunchListID = param.PunchListID,
                        ConstructionMilestoneId = param.ConstructionMilestoneId,
                        OTCNumber = param.OTCNumber,
                        ContractorNumber = param.ContractorNumber,
                        ManagingContractorID = param.ManagingContractorID,
                        ConstructionMilestoneCode = param.MilestoneCode,
                        PunchListCategory = param.PunchListCategory,
                        PunchListSubCategory = param.PunchListSubCategory,
                        NonCompliantTo = param.NonCompliantTo,
                        ReferenceNumber = param.ReferenceNumber,
                        ReferenceSheet = param.ReferenceSheet,
                        PunchListDescription = param.PunchListDescription,
                        PunchListLocation = param.PunchListLocation,
                        CostImpact = param.CostImpact,
                        ScheduleImpact = param.ScheduleImpact,
                        AssignedTo = param.AssignedTo,
                        PunchListStatus = param.PunchListStatus,
                        DueDate = param.DueDate ?? DateTime.Now,
                        AttachmentFileNames = param.AttachmentFileNames?.Select(x => x.FileName).ToList(),
                        Comments = (param.Comments != null ? param.Comments?.Select(com => new Comment()
                        {
                            CommentId = com.CommentId,
                            AttachmentFileName = com.AttachmentFileName?.ToList(),
                            Message = com.Message,
                            CreatedBy = (com.CreatedBy != null ? new User
                            {
                                Id = com.CreatedBy.Id,
                                FirstName = com.CreatedBy.FullName,
                                LastName = "",
                                //Role = new Role() { Type= App.Core.Enums.UserRoleType.Contractor }
                                RoleId = com.CreatedBy.RoleCode,
                                Email = com.CreatedBy.Email,
                                Role = new Role
                                {
                                    id = com.CreatedBy.Id,
                                    Type = DataMapping.MapRoleByCode(com.CreatedBy.RoleCode)
                                }
                            } : new User())
                        }).ToList() : new List<Comment>()),
                        Message = param.Message,
                        PunchListStatusDetail = new PunchlistStatus
                        {
                            Code = param.PunchListStatusDetail.Code,
                            Name = param.PunchListStatusDetail.Name,
                            IsOpen = DataMapping.MapPunchlistStatusByCode(param.PunchListStatusDetail.Code).IsOpen,
                            IsClosed = DataMapping.MapPunchlistStatusByCode(param.PunchListStatusDetail.Code).IsClosed
                        }
                    }).ToList() : new List<Punchlist>()),
                    Representative = (item.Representatives != null ? item.Representatives.Select(rep => new Representative
                    {
                        Id = rep.Id,
                        ContactNumber = rep.ContactNumber,
                        Email = rep.Email,
                        Name = rep.Name
                    }).ToList() : null),
                    LoaContractNumber = item.LoaContractNumber,
                    UnitReferenceObject = item.ReferenceObject,
                    Contractor = (item.Contractor != null ? new Contractor
                    {
                        Id = item.Contractor.Code,
                        Name = item.Contractor.Name
                    } : null),
                    ContractorCode = item.Contractor?.Code,
                }).ToList();
                
                return ongoingmilestone;
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
            catch (ApplicationException ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IEnumerable<ActualOnGoingConstructionMilestone>> GetMilestonesByUnitByVendorAsync(string username, string _path, string ReferenceObject, string vendorCode)
        {
            try
            {
                var unitmilestone = (await _wcfServices.GetMilestonesByUnitByVendorAsync(username, ReferenceObject,vendorCode));
                var ongoingmilestone = unitmilestone.Select(item => new ActualOnGoingConstructionMilestone()
                {
                    Id = item.Id,
                    OTCNumber = item.OTCNumber,
                    ContractorNumber = item.ContractorNumber,
                    ManagingContractorID = item.ManagingContractorID,
                    ConstructionMilestoneCode = item.ConstructionMilestoneCode,
                    ConstructionMilestoneDescription = item.ConstructionMilestoneDescription,
                    Weight = item.Weight,
                    Sequence = item.Sequence,
                    PONumber = item.PONumber,
                    Trade = item.TradeCode,
                    OTCTypeCode = item.OTCTypeCode,
                    PercentageCompletion = item.PercentageCompletion,
                    PercentageCompletionQA = item.PercentageCompletionQA,
                    PercentageCompletionEngineer = item.PercentageCompletionEngineer,
                    PercentageCompletionContractor = item.PercentageCompletionContractor,
                    PercentageReferenceNumber = item.PercentageReferenceNumber,
                    MilestoneImages = (item.Attachments != null ? item.Attachments.Select(f => _path + f.FileName).ToList() : null),
                    ManagingContractor = DataMapping.MapManagingContractorByCode(item.ManagingContractorCode),
                    Punchlists = (item.Punchlists != null ? item.Punchlists.Select(param => new Punchlist()
                    {
                        PunchListID = param.PunchListID,
                        ConstructionMilestoneId = param.ConstructionMilestoneId,
                        OTCNumber = param.OTCNumber,
                        ContractorNumber = param.ContractorNumber,
                        ManagingContractorID = param.ManagingContractorID,
                        ConstructionMilestoneCode = param.MilestoneCode,
                        PunchListCategory = param.PunchListCategory,
                        PunchListSubCategory = param.PunchListSubCategory,
                        NonCompliantTo = param.NonCompliantTo,
                        ReferenceNumber = param.ReferenceNumber,
                        ReferenceSheet = param.ReferenceSheet,
                        PunchListDescription = param.PunchListDescription,
                        PunchListLocation = param.PunchListLocation,
                        CostImpact = param.CostImpact,
                        ScheduleImpact = param.ScheduleImpact,
                        AssignedTo = param.AssignedTo,
                        PunchListStatus = param.PunchListStatus,
                        DueDate = param.DueDate ?? DateTime.Now,
                        AttachmentFileNames = param.AttachmentFileNames?.Select(x => x.FileName).ToList(),
                        Comments = (param.Comments != null ? param.Comments?.Select(com => new Comment()
                        {
                            CommentId = com.CommentId,
                            AttachmentFileName = com.AttachmentFileName?.ToList(),
                            Message = com.Message,
                            CreatedBy = (com.CreatedBy != null ? new User
                            {
                                Id = com.CreatedBy.Id,
                                FirstName = com.CreatedBy.FullName,
                                LastName = "",
                                //Role = new Role() { Type= App.Core.Enums.UserRoleType.Contractor }
                                RoleId = com.CreatedBy.RoleCode,
                                Email = com.CreatedBy.Email,
                                Role = new Role
                                {
                                    id = com.CreatedBy.Id,
                                    Type = DataMapping.MapRoleByCode(com.CreatedBy.RoleCode)
                                }
                            } : new User())
                        }).ToList() : new List<Comment>()),
                        Message = param.Message,
                        PunchListStatusDetail = new PunchlistStatus
                        {
                            Code = param.PunchListStatusDetail.Code,
                            Name = param.PunchListStatusDetail.Name,
                            IsOpen = DataMapping.MapPunchlistStatusByCode(param.PunchListStatusDetail.Code).IsOpen,
                            IsClosed = DataMapping.MapPunchlistStatusByCode(param.PunchListStatusDetail.Code).IsClosed
                        }
                    }).ToList() : new List<Punchlist>()),
                    Representative = (item.Representatives != null ? item.Representatives.Select(rep => new Representative
                    {
                        Id = rep.Id,
                        ContactNumber = rep.ContactNumber,
                        Email = rep.Email,
                        Name = rep.Name
                    }).ToList() : null),
                    LoaContractNumber = item.LoaContractNumber,
                    UnitReferenceObject = item.ReferenceObject,
                    Contractor = (item.Contractor != null ? new Contractor
                    {
                        Id = item.Contractor.Code,
                        Name = item.Contractor.Name
                    } : null),
                    ContractorCode = item.Contractor?.Code,
                }).ToList();

                return ongoingmilestone;
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
            catch (ApplicationException ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<ActualOnGoingConstructionMilestone> GetMilestoneByIdAsync(int id, string _path, string username)
        {
            try
            {
                var milestoneid = (await _wcfServices.GetMilestoneByIdAsync(username, id));
                var ongoing = new ActualOnGoingConstructionMilestone()
                {
                    OTCNumber = milestoneid.OTCNumber,
                    OTCTypeCode = milestoneid.OTCTypeCode,
                    Id = milestoneid.Id,
                    ContractorNumber = milestoneid.ContractorNumber,
                    ManagingContractorID = milestoneid.ManagingContractorID,
                    ConstructionMilestoneCode = milestoneid.ConstructionMilestoneCode,
                    Weight = milestoneid.Weight,
                    Sequence = milestoneid.Sequence,
                    PercentageCompletion = milestoneid.PercentageCompletion,
                    PercentageCompletionQA = milestoneid.PercentageCompletionQA,
                    PercentageCompletionEngineer = milestoneid.PercentageCompletionEngineer,
                    PercentageCompletionContractor = milestoneid.PercentageCompletionContractor,
                    PercentageReferenceNumber = milestoneid.PercentageReferenceNumber,
                    ConstructionMilestoneDescription = milestoneid.ConstructionMilestoneDescription,
                    Trade = milestoneid.TradeDescription,
                    MilestoneImages = milestoneid.Attachments.Select(f => _path + f.FileName).ToList(),
                    Contractor = new Contractor
                    {
                        Name = milestoneid.Contractor.Name,
                        FirstName = milestoneid.Contractor.Name,
                        Id = milestoneid.Contractor.Code
                    },
                    ManagingContractor = DataMapping.MapManagingContractorByCode(milestoneid.ManagingContractorCode),
                    PONumber = milestoneid.PONumber,
                    ContractorCode = milestoneid.Contractor.Code,
                    Punchlists = (milestoneid.Punchlists != null ? milestoneid.Punchlists.Select(param => new Punchlist()
                    {
                        PunchListID = param.PunchListID,
                        ConstructionMilestoneId = param.ConstructionMilestoneId,
                        OTCNumber = param.OTCNumber,
                        ContractorNumber = param.ContractorNumber,
                        ManagingContractorID = param.ManagingContractorID,

                        ConstructionMilestoneCode = param.MilestoneCode,
                        PunchListCategory = param.PunchListCategory,
                        PunchListSubCategory = param.PunchListSubCategory,
                        NonCompliantTo = param.NonCompliantTo,
                        ReferenceNumber = param.ReferenceNumber,
                        ReferenceSheet = param.ReferenceSheet,
                        PunchListDescription = param.PunchListDescription,
                        PunchlistDescriptionDetail = new PunchlistDescription
                        {
                            Code = param.PunchListDescriptionDetails.Code,
                            Name = param.PunchListDescriptionDetails.Name
                        },
                        PunchListLocation = param.PunchListLocation,
                        CostImpact = param.CostImpact,
                        ScheduleImpact = param.ScheduleImpact,
                        AssignedTo = param.AssignedTo,
                        PunchListStatus = param.PunchListStatus,
                        DueDate = param.DueDate ?? DateTime.Now,
                        AttachmentFileNames = param.AttachmentFileNames?.Select(x => x.FileName).ToList(),
                        Comments = (param.Comments != null ? param.Comments.Select(com => new Comment()
                        {
                            CommentId = com.CommentId,
                            AttachmentFileName = com.AttachmentFileName?.ToList(),
                            Message = com.Message,
                            CreatedBy = (com.CreatedBy != null ? new User
                            {
                                Id = com.CreatedBy.Id,
                                FirstName = com.CreatedBy.FullName,
                                LastName = "",
                                //Role = new Role() { Type= App.Core.Enums.UserRoleType.Contractor }
                                RoleId = com.CreatedBy.RoleCode,
                                Email = com.CreatedBy.Email,
                                Role = new Role
                                {
                                    id = com.CreatedBy.Id,
                                    Type = DataMapping.MapRoleByCode(com.CreatedBy.RoleCode)
                                }
                            } : new User())
                        }).ToList() : new List<Comment>()),
                        Message = param.Message,
                        PunchListStatusDetail = new PunchlistStatus
                        {
                            Code = param.PunchListStatusDetail.Code,
                            Name = param.PunchListStatusDetail.Name,
                            IsOpen = DataMapping.MapPunchlistStatusByCode(param.PunchListStatusDetail.Code).IsOpen, //param.PunchListStatusDetail.IsOpen,
                            IsClosed = DataMapping.MapPunchlistStatusByCode(param.PunchListStatusDetail.Code).IsClosed //param.PunchListStatusDetail.IsClosed
                        }

                    }).ToList() : new List<Punchlist>()),
                    Representative = milestoneid.Representatives.Select(rep => new Representative
                    {
                        Id = rep.Id,
                        ContactNumber = rep.ContactNumber,
                        Email = rep.Email,
                        Name = rep.Name
                    }).ToList(),
                    LoaContractNumber = milestoneid.LoaContractNumber
                };
                return ongoing;
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
            catch (ApplicationException ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IEnumerable<ActualOnGoingConstructionMilestone>> GetMilestoneByProjectIdAsync(string projectcode, string username)
        {
            try
            {
                var usr = await _userServices.GetUserAsync(username);
                if (usr == null)
                    throw new ApplicationException("User does not exists");
                if (usr.RoleCode == null)
                    throw new ApplicationException("No defined Role for this User.");

                var milestoneproject = await (_wcfServices.GetMilestoneByProjectAsync(username, projectcode));
                List<ActualOnGoingConstructionMilestone> ongoingmilestone = new List<ActualOnGoingConstructionMilestone>();
                foreach (var item in milestoneproject)
                {
                    ongoingmilestone.Add(new ActualOnGoingConstructionMilestone
                    {
                        Id = item.Id,
                        OTCNumber = item.OTCNumber,
                        OTCTypeCode = item.OTCTypeCode,
                        ContractorNumber = item.ContractorNumber,
                        ManagingContractorID = item.ManagingContractorID,
                        ManagingContractor = DataMapping.MapManagingContractorByCode(item.ManagingContractorCode),
                        ConstructionMilestoneCode = item.ConstructionMilestoneCode,
                        ConstructionMilestoneDescription = item.ConstructionMilestoneDescription,
                        Weight = item.Weight,
                        Sequence = item.Sequence,
                        PercentageCompletion = item.PercentageCompletion,
                        PercentageReferenceNumber = item.PercentageReferenceNumber,
                        PONumber = item.PONumber,
                        Trade = item.TradeDescription,
                        MilestoneImages = item.Attachments?.Select(a => a.FileName).ToList(),
                        Contractor = new Contractor
                        {
                            FirstName = item.Contractor.Name,
                            Name = item.Contractor.Name,
                            Id = item.Contractor.Code
                        },
                        ContractorCode = item.Contractor.Code,
                        LoaContractNumber = item.LoaContractNumber,
                        UnitReferenceObject = item.ReferenceObject,
                        Representative = item.Representatives.Select(x => new Representative
                        {
                            ContactNumber = x.ContactNumber,
                            Email = x.Email,
                            Id = x.Id,
                            Name = x.Name
                        }).ToList()
                    });
                }

                return ongoingmilestone;
            }
            catch(ApplicationException ex)
            {
                throw new ApplicationException(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

        }

        public async Task<bool> UpdateMilestonePercentageAsync(MilestonePercentageDto model, string _path, string username)
        {
            try
            {
                var update = await _wcfServices.UpdateMilestonePercentageAsync(username,
                    model.Id,
                    model.OTCNumber,
                    model.ContractorNumber,
                    model.ManagingContractorID,
                    model.NewPercentage,
                    model.ConstructionMilestoneCode,
                    model.MilestoneImages.Select(i => new MilestoneAttachment
                    {
                        ConstructionMilestoneId = model.Id,
                        FileName = i,
                        FilePath = _path + i
                    }).ToArray()
                    , model.DateVisited);
                return update;
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
            catch (ApplicationException ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        public async Task<string> DeleteMilestoneAttachment(string filename)
        {
            try
            {
                return await _fileUploadService.DeleteMilestoneImageByName(filename);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<ConstructionMilestoneBinaryImage> GetMilestoneAttachment(string filename)
        {
            try
            {
                return await _fileUploadService.GetMilestoneImageByName(filename);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        public async Task<string> SaveMilestoneAttachment(IFormFile uploadedFile, string username)
        {
            try
            {
                if (uploadedFile == null)
                    return null;

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/Attachment");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var fileNameExt = Path.GetExtension(uploadedFile.FileName);

                var compfileName = "HI" + DateTime.Now.ToString("MMddyyyyHHmmssss") + fileNameExt;

                string fileNameWithPath = Path.Combine(path, compfileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    uploadedFile.CopyTo(stream);
                }

                byte[] filebytes;
                using (var fs = new FileStream(
                    fileNameWithPath, FileMode.Open, FileAccess.Read))
                {
                    filebytes = new byte[fs.Length];
                    fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
                    fs.Flush();
                    fs.Dispose();
                }

                ConstructionMilestoneBinaryImage milestoneBinaryImage = new ConstructionMilestoneBinaryImage
                {
                    FileBinary = filebytes,

                    FileName = compfileName,

                    CreatedBy = username
                };

                await _fileUploadService.SaveMilestoneImage(milestoneBinaryImage);

                if ((System.IO.File.Exists(fileNameWithPath)))
                {
                    System.IO.File.Delete(fileNameWithPath);
                }

                return compfileName;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        public async Task<string> UploadAllMilestoneImagesFromFTPServerToDatabase(string username)
        {
            var _timeStart = DateTime.Now;
            Debug.WriteLine($"Starting to upload milestone images to DB");
            
            int count = 0;
            string flname = string.Empty;

            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/Attachment");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var result = new List<string>();
                var lstfrebas = Task.Run(async () =>(await _wcfServices.GetMilestoneImagesAsync()).Select(x => x.FileName).Distinct().ToList());
                var lstHI = Task.Run(async() => (await _fileUploadService.GetMilestoneImage()).Select(x => x.FileName).ToList());

                await Task.WhenAll(lstfrebas,lstHI);

                foreach (var item in lstfrebas.Result)
                {
                    var wHLists = lstHI.Result.Find(
                        delegate (string p)
                        {
                            return p == item;
                        }
                        );

                    if (wHLists == null)
                    {
                        result.Add(item);
                    }
                }

                if (result.Count > 0)
                {
                    //result = result.Take(300).ToList();

                    foreach (var filename in result)
                    {
                        var newData = await _fileUploadService.GetMilestoneImageByName(filename);

                        if (IsExists(_fTPSettings.PostHostName, _fTPSettings.FtpUsername, _fTPSettings.FtpPassword, string.Empty, filename))
                        {
                            if (newData == null)
                            {
                                Debug.WriteLine($"Saving Milestone Image : {filename}");

                                flname = filename;

                                DownloadFileFromFTPServer(_fTPSettings.PostHostName, _fTPSettings.FtpUsername, _fTPSettings.FtpPassword, string.Empty, filename, path);

                                await SaveFileToDatabase(filename, username);
                                
                                count++;

                                Debug.WriteLine($"{filename} has been successfully inserted.");
                            }
                        }
                    }

                    //time end
                    var _timeEndCall = DateTime.Now;
                    TimeSpan t = TimeSpan.FromSeconds((_timeEndCall - _timeStart).TotalSeconds);
                    string _time = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds,
                                    t.Milliseconds);

                    Debug.WriteLine($"{"Total time of uploading images from FTP Server : " + _time}");
                    var total = count;
                    count = 0;
                    return $"Successfully inserted {total} milestone images to database. Total time of uploading milestone images from FTP Server : {_time}";
                }
                else
                {
                    count = 0;
                    return $"All images from FTP already uploaded to DB";
                }
            }
            catch (Exception ex)
            {
                if (count > 0)
                {
                    Debug.WriteLine($"Error encountered. Uploaded only {count} images from FTP Server.");
                }

                //time end
                var _timeEndCall = DateTime.Now;
                TimeSpan t = TimeSpan.FromSeconds((_timeEndCall - _timeStart).TotalSeconds);
                string _time = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                t.Hours,
                                t.Minutes,
                                t.Seconds,
                                t.Milliseconds);

                Debug.WriteLine($"{"Total time of uploading images from FTP Server : " + _time}");
                Debug.WriteLine($"{"Error Message : " + ErrorMessageUtil.GetFullExceptionMessage(ex)}");
                var total = count;
                count = 0;
                throw new Exception($"Error encountered upon uploading {flname}. Total time of uploading images from FTP Server : {_time}. Uploaded only {total} images from FTP Server. See details : {ErrorMessageUtil.GetFullExceptionMessage(ex)}");
            }
        }

        #region Method
        public async Task VerifyAndSave(string filename, string username, string path)
        {
            try
            {
                var data = await _fileUploadService.GetMilestoneImageByName(filename);

                if (IsExists(_fTPSettings.PostHostName, _fTPSettings.FtpUsername, _fTPSettings.FtpPassword, string.Empty, filename))
                {
                    if (data == null)
                    {
                        Debug.WriteLine($"Saving Milestone Image : {filename}");

                        DownloadFileFromFTPServer(_fTPSettings.PostHostName, _fTPSettings.FtpUsername, _fTPSettings.FtpPassword, string.Empty, filename, path);

                        await SaveFileToDatabase(filename, username);

                        Debug.WriteLine($"{filename} has been successfully inserted.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error on saving {filename}. See message details: {ex.Message}");
                throw new Exception($"Error on saving {filename}. See message details: {ex.Message}");
            }
        }

        public void DownloadFileFromFTPServer(string ftpURL, string UserName, string Password, string ftpDirectory, string FileName, string LocalDirectory)
        {
            try
            {
                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(ftpURL + "/" + ftpDirectory + "/" + FileName);

                requestFileDownload.Credentials = new NetworkCredential(UserName, Password);

                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();

                Stream responseStream = responseFileDownload.GetResponseStream();

                FileStream writeStream = new FileStream(LocalDirectory + "/" + FileName, FileMode.Create);

                int length = 1024;

                Byte[] buffer = new Byte[length];

                int bytesRead = responseStream.Read(buffer, 0, length);

                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, length);
                }

                responseStream.Close();

                writeStream.Close();

                requestFileDownload = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task SaveFileToDatabase(string file, string username)
        {
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/Attachment");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileName = Path.Combine(path, file);

                byte[] filebytes;
                using (var fs = new FileStream(
                    fileName, FileMode.Open, FileAccess.Read))
                {
                    filebytes = new byte[fs.Length];
                    fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));

                    fs.Flush();
                    fs.Dispose();
                }

                ConstructionMilestoneBinaryImage milestoneBinaryImage = new ConstructionMilestoneBinaryImage
                {
                    FileBinary = filebytes,

                    FileName = file,

                    CreatedBy = string.IsNullOrEmpty(username) ? "Undefined User" : username
                };

                await _fileUploadService.SaveMilestoneImage(milestoneBinaryImage);

                if ((System.IO.File.Exists(fileName)))
                {
                    System.IO.File.Delete(fileName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public bool IsExists(string ftpURL, string UserName, string Password, string ftpDirectory, string FileName)
        {
            FtpWebRequest ftpRequest = null;
            FtpWebResponse ftpResponse = null;
            bool IsExists = true;
            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(ftpURL + "/" + ftpDirectory + "/" + FileName);
                ftpRequest.Credentials = new NetworkCredential(UserName, Password);
                ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpResponse.Close();
                ftpRequest = null;


            }
            catch (Exception ex)
            {
                IsExists = false;
            }

            return IsExists;
        }
        #endregion

    }
}
