using Hcom.Web.Api.Interface;
using Hcom.Web.Api.Models;
using Hcom.Web.Api.Utilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserEP = UserSvcRefence;
using NotifEP = NotificationSvcReference;
using WCF = PunchlistSvcReference;
using RestSharp;
using CTI.HI.Business.Entities.Notification;
using System.Net.Http;
using System.Net.Http.Headers;
using Hcom.App.Core.Enums;
using System.Diagnostics;
using Hcom.App.Entities;
using Hcom.App.Entities.HCOM;
using Hcom.App.Entities.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Hcom.Web.Api.Services
{
    public class PunchListService : IPunchlist
    {
        private readonly ServiceEndpoints _endpointSettings;
        private readonly ConfigSettings _configSettings;
        private readonly FTPSettings _fTPSettings;
        private readonly IFileUploadService _fileUploadService;
        private readonly UserInfoProvider _userInfoProvider;
        private WCF.IPunchlistService _wcfServices;
        private NotifEP.INotificationService _notificationService;
        private UserEP.IUserService _userServices;
        private Dictionary<string, User> _usersInfo;
        public const string punchlistServiceEndpoint = "PunchListService.svc";
        public const string notificationServiceEndpoint = "NotificationService.svc";
        public const string userServiceEndpoint = "UserService.svc";

        public PunchListService(IOptionsMonitor<ServiceEndpoints> endpointAccessor, IOptionsMonitor<FTPSettings> ftpsettings, IFileUploadService fileUploadService, IOptionsMonitor<ConfigSettings> configSettings, UserInfoProvider userInfoProvider)
        {
            _endpointSettings = endpointAccessor.CurrentValue;

            _configSettings = configSettings.CurrentValue;

            _fileUploadService = fileUploadService;

            _fTPSettings = ftpsettings.CurrentValue;

            _wcfServices = new WCF.PunchlistServiceClient(WCF.PunchlistServiceClient.EndpointConfiguration.BasicHttpBinding_IPunchlistService,
                 _endpointSettings.BaseUrl + punchlistServiceEndpoint);

            _userServices = new UserEP.UserServiceClient(UserEP.UserServiceClient.EndpointConfiguration.BasicHttpBinding_IUserService,
                _endpointSettings.BaseUrl + userServiceEndpoint);

            _notificationService = new NotifEP.NotificationServiceClient(NotifEP.NotificationServiceClient.EndpointConfiguration.BasicHttpBinding_INotificationService,
                 _endpointSettings.BaseUrl + notificationServiceEndpoint);
            
            _userInfoProvider = userInfoProvider;
        }

        public async Task<bool> PunchlistSaveAsync(Punchlist punch, string username, string _path)
        {
            try
            {
                var pnchList = new WCF.Punchlist()
                {
                    PunchListID = punch.PunchListID,
                    OTCNumber = punch.OTCNumber,
                    ContractorNumber = punch.ContractorNumber,
                    ManagingContractorID = punch.ManagingContractorID,
                    ConstructionMilestoneId = punch.ConstructionMilestoneId,
                    MilestoneCode = punch.ConstructionMilestoneCode,
                    PunchListCategory = punch.PunchListCategory,
                    PunchListSubCategory = punch.PunchListSubCategory,
                    NonCompliantTo = punch.NonCompliantTo,
                    ReferenceSheet = punch.ReferenceSheet,
                    PunchListDescription = punch.PunchListDescription,
                    PunchListLocation = punch.PunchListLocation,
                    CostImpact = punch.CostImpact,
                    ScheduleImpact = punch.ScheduleImpact,
                    AssignedTo = punch.AssignedTo,
                    PunchListStatus = punch.PunchListStatus,
                    ReferenceNumber = punch.ReferenceNumber,
                    PunchListDescriptionDetails = new WCF.PunchlistDescription
                    {
                        Code = punch.PunchlistDescriptionDetail.Code,
                        Name = punch.PunchlistDescriptionDetail.Name,
                        GroupCode = punch.PunchlistDescriptionDetail.GroupCode
                    },
                    DueDate = punch.DueDate,
                    Message = punch.Message,
                    AttachmentFileNames = punch.AttachmentFileNames.Select(a => new PunchlistSvcReference.PunchlistCommentAttachment
                    {
                        FileName = a,
                        FilePath = _path + a
                    }).ToArray(),
                    DateCreated = punch.created_at,
                    Coordinates = new WCF.Coordinates
                    {
                        Latitude = punch.Coordinates.Latitude,
                        Longitude = punch.Coordinates.Longitude
                    },
                    DeviceInformation = new WCF.DeviceInfoModel
                    {
                        Name = punch.DeviceInformation.Name,
                        Idiom = punch.DeviceInformation.Idiom,
                        Manufacturer = punch.DeviceInformation.Manufacturer,
                        Model = punch.DeviceInformation.Model,
                        Platform = punch.DeviceInformation.Platform,
                        VersionString = punch.DeviceInformation.VersionString
                    }

                };

                int createdPunchlistId = await _wcfServices.SavePunchlistAsync(username, pnchList);

                //check if created
                if (punch.PunchListID <= 0)
                {
                    if (_configSettings.AllowSMSSending == true)
                    {
                        await _notificationService.SendPunchlistNotificationAsync(createdPunchlistId);
                    }

                };
                return true;
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

        public bool SendSmsByInfobip(string msg, string recipient)
        {

            string username = "FILINVEST04";
            string password = "!P@ssw0rd123";

            byte[] concatenated = System.Text.ASCIIEncoding.ASCII.GetBytes(username + ":" + password);
            string header = System.Convert.ToBase64String(concatenated);

            var client = new RestClient("https://vnz2m.api.infobip.com/sms/2/text/single");

            var request = new RestRequest(Method.POST);
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Basic " + header);
            request.AddParameter("application/json", "{\"from\":\"HCOM\", \"to\":\"" + recipient + "\",\"text\":\"" + msg + ".\"}", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return true;
        }

        public bool SendEmailbyInfobip(EmailModel email)
        {
            string username = "FILINVEST04";
            string password = "!P@ssw0rd123";

            byte[] concatenated = System.Text.ASCIIEncoding.ASCII.GetBytes(username + ":" + password);
            string header = System.Convert.ToBase64String(concatenated);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.infobip.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", header);
            var request = new MultipartFormDataContent();
            request.Add(new StringContent(email.From), "from");
            request.Add(new StringContent(email.To), "to");
            request.Add(new StringContent(email.Subject), "subject");
            request.Add(new StringContent(email.Body), "html");


            var response = client.PostAsync("email/1/send", request).Result;
            return true;
        }

        public async Task<IEnumerable<PunchlistStatus>> GetPunchlistStatusbyRoleAsync(UserRoleType userRole, string username)
        {
            try
            {
                var usr = await _userServices.GetUserAsync(username);
                if (usr == null)
                    throw new ApplicationException("User does not exists");
                if (usr.RoleCode == null)
                    throw new ApplicationException("No defined Role for this User.");

                var status = await _wcfServices.GetPunchlistStatusByRoleAsync(usr.RoleCode);

                List<PunchlistStatus> punchliststatus = new List<PunchlistStatus>();

                foreach (var item in status)
                {
                    punchliststatus.Add(new PunchlistStatus()
                    {
                        Code = item.Code,
                        Name = item.Name,
                        CurrentStatus = item.CurrentStatus,
                        IsOpen = DataMapping.MapPunchlistStatusByCode(item.Code).IsOpen,
                        IsClosed = DataMapping.MapPunchlistStatusByCode(item.Code).IsClosed
                    });
                }

                return punchliststatus;
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

        public async Task<Punchlist> GetPunchlistDetailsAsync(int Punchlistid, string _path)
        {
            try
            {
                var details = await _wcfServices.GetPunchlistAsync(Punchlistid);

                List<Punchlist> punchlist = new List<Punchlist>();

                return new Punchlist()
                {
                    PunchListID = details.PunchListID,
                    OTCNumber = details.OTCNumber,
                    ContractorNumber = details.ContractorNumber,
                    ManagingContractorID = details.ManagingContractorID,
                    ConstructionMilestoneId = details.ConstructionMilestoneId,
                    ConstructionMilestoneCode = details.MilestoneCode,
                    PunchListCategory = details.PunchListCategory,
                    PunchListSubCategory = details.PunchListSubCategory,
                    NonCompliantTo = details.NonCompliantTo,
                    ReferenceSheet = details.ReferenceSheet,
                    PunchListDescription = details.PunchListDescription,
                    PunchlistDescriptionDetail = new PunchlistDescription
                    {
                        Code = details.PunchListDescriptionDetails?.Code,
                        Name = details.PunchListDescriptionDetails?.Name,
                        GroupCode = details.PunchListDescriptionDetails.GroupCode
                    },
                    PunchListGroup = details.PunchListDescriptionDetails?.GroupCode,
                    PunchListLocation = details.PunchListLocation,
                    CostImpact = details.CostImpact,
                    ScheduleImpact = details.ScheduleImpact,
                    AssignedTo = details.AssignedTo,
                    PunchListStatus = details.PunchListStatus,
                    DueDate = details.DueDate ?? DateTime.Now,
                    Comments = details.Comments.Select(param => new Comment
                    {
                        CommentId = param.CommentId,
                        AttachmentFileName = param.AttachmentFileName.Select(f => _path + f).ToList(),
                        created_at = param.CreatedDate,
                        CreatedBy = new User
                        {
                            Id = param.CreatedBy.Id,
                            FirstName = param.CreatedBy.FullName,
                            LastName = "",
                            RoleId = param.CreatedBy.RoleCode,
                            Role = new Role
                            {
                                id = param.CreatedBy.RoleCode,
                                Type = DataMapping.MapRoleByCode(param.CreatedBy.RoleCode)
                            },
                            Email = param.CreatedBy.Email
                        },
                        Message = param.Message,
                        DueDate = param.DueDate,
                        PunchlistStatus = new PunchlistStatus
                        {
                            Code = param.PunchlistStatus?.Code,
                            Name = param.PunchlistStatus?.Name,
                            IsOpen = DataMapping.MapPunchlistStatusByCode(param.PunchlistStatus?.Code).IsOpen,
                            IsClosed = DataMapping.MapPunchlistStatusByCode(param.PunchlistStatus?.Code).IsClosed
                        }
                    }).ToList() ?? new List<Comment>(),
                    Message = details.Message,
                    PunchListStatusDetail = new PunchlistStatus
                    {
                        Code = details.PunchListStatusDetail?.Code,
                        Name = details.PunchListStatusDetail?.Name,
                        IsOpen = DataMapping.MapPunchlistStatusByCode(details.PunchListStatusDetail?.Code).IsOpen,
                        IsClosed = DataMapping.MapPunchlistStatusByCode(details.PunchListStatusDetail?.Code).IsClosed
                    },
                };
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

        public async Task<IEnumerable<Punchlist>> GetPunchlistAsync(int ConstructionMilestoneId, string _path)
        {
            try
            {
                var punchlist = (await _wcfServices.GetPunchlistsAsync(ConstructionMilestoneId)).ToList();

                var _usernames = punchlist.SelectMany(x => x.Comments)
                    .Select(x => x.CreatedByUsername).Distinct().ToArray();

                _usersInfo = (await _userInfoProvider.GetUsersAsync(_usernames))
                    .ToDictionary(x => x.Id);

                List<Punchlist> punch = new List<Punchlist>();

                punchlist.ForEach(item =>
                        punch.Add(new Punchlist()
                        {
                            PunchListID = item.PunchListID,
                            OTCNumber = item.OTCNumber,
                            ContractorNumber = item.ContractorNumber,
                            ManagingContractorID = item.ManagingContractorID,
                            ConstructionMilestoneId = item.ConstructionMilestoneId,
                            ConstructionMilestoneCode = item.MilestoneCode,
                            PunchListCategory = item.PunchListCategory,
                            PunchListSubCategory = item.PunchListSubCategory,
                            NonCompliantTo = item.NonCompliantTo,
                            ReferenceSheet = item.ReferenceSheet,
                            PunchListDescription = item.PunchListDescription,
                            PunchlistDescriptionDetail = new PunchlistDescription
                            {
                                Code = item.PunchListDescriptionDetails?.Code,
                                Name = item.PunchListDescriptionDetails?.Name,
                                GroupCode = item.PunchListDescriptionDetails.GroupCode
                            },
                            Comments = item.Comments.Select(x =>
                            {
                                var _createdUser = GetFromDic(x.CreatedByUsername);
                                var _comment = new Comment
                                {
                                    CommentId = x.CommentId,
                                    Message = x.Message,
                                    CreatedBy = new App.Entities.User
                                    {
                                        Id = _createdUser?.Id,
                                        RoleId = _createdUser?.Role?.id,
                                        Email = _createdUser?.Email,
                                        FirstName = _createdUser?.FullName,
                                        LastName = ""
                                    },
                                    AttachmentFileName = x.AttachmentFileName.Select(f => _path + f).ToList(),
                                    DueDate = x.DueDate,
                                    PunchlistStatus = new PunchlistStatus
                                    {
                                        Code = x.PunchlistStatus?.Code,
                                        Name = x.PunchlistStatus?.Name,
                                        IsOpen = DataMapping.MapPunchlistStatusByCode(x.PunchlistStatus?.Code).IsOpen,
                                        IsClosed = DataMapping.MapPunchlistStatusByCode(x.PunchlistStatus?.Code).IsClosed
                                    }
                                };
                                return _comment;
                            }).ToList() ?? new List<Comment>(),

                            PunchListGroup = item.PunchListGroup,
                            PunchListLocation = item.PunchListLocation,
                            CostImpact = item.CostImpact,
                            ScheduleImpact = item.ScheduleImpact,
                            AssignedTo = item.AssignedTo,
                            PunchListStatus = item.PunchListStatus,
                            DueDate = item.DueDate ?? DateTime.Now,
                            Message = item.Message ?? "",
                            PunchListStatusDetail = new PunchlistStatus
                            {
                                Code = item.PunchListStatusDetail.Code,
                                Name = item.PunchListStatusDetail.Name,
                                IsOpen = DataMapping.MapPunchlistStatusByCode(item.PunchListStatusDetail.Code).IsOpen,
                                IsClosed = DataMapping.MapPunchlistStatusByCode(item.PunchListStatusDetail.Code).IsClosed
                            }
                        })
                );

                return punch;
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

        private User GetFromDic(string username)
        {
            User _out;
            if (_usersInfo.TryGetValue(username.ToUpper(), out _out))
                return _out;

            return null;
        }

        public async Task<IEnumerable<PunchlistCategory>> GetPunchlistCategoryAsync()
        {
            try
            {
                var punchlistcategory = (await _wcfServices.GetPunchlistCategoryAsync());
                List<PunchlistCategory> category = new List<PunchlistCategory>();
                foreach (var item in punchlistcategory)
                {
                    category.Add(new PunchlistCategory()
                    {
                        Code = item.Code,
                        Name = item.Name,
                        IsNonCompliance = item.isCompliance
                    });
                }

                return category;
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

        public async Task<IEnumerable<PunchlistSubCategory>> GetPunchlistSubCategoryAsync()
        {
            try
            {
                var subcat = await _wcfServices.GetPunchlistSubCategoryAsync();
                List<PunchlistSubCategory> subcategory = new List<PunchlistSubCategory>();
                foreach (var item in subcat)
                {
                    subcategory.Add(new PunchlistSubCategory()
                    {
                        Code = item.Code,
                        Name = item.Name,
                        OfficeDays = item.OfficeDays
                    });
                }

                return subcategory;
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

        public async Task<IEnumerable<NonComplianceTo>> GetNonCompliancesToAsync()
        {
            try
            {
                var noncomp = await _wcfServices.GetNonComplianceToAsync();
                List<NonComplianceTo> noncomplianceto = new List<NonComplianceTo>();
                foreach (var item in noncomp)
                {
                    noncomplianceto.Add(new NonComplianceTo()
                    {
                        Code = item.Code,
                        Name = item.Name
                    });
                }

                return noncomplianceto;
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

        public async Task<IEnumerable<PunchlistLocation>> GetPunchlistLocationAsync()
        {
            try
            {
                var punchlistloc = await _wcfServices.GetPunchlistLocationAyncAsync();
                List<PunchlistLocation> loc = new List<PunchlistLocation>();
                foreach (var item in punchlistloc)
                {
                    loc.Add(new PunchlistLocation()
                    {
                        Code = item.Code,
                        Name = item.Name
                    });
                }
                return loc;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }

        public async Task<IEnumerable<CostImpact>> GetCostImpactAsync()
        {
            try
            {
                var costimpact = await _wcfServices.GetCostImpactAsync();
                List<CostImpact> cstimpct = new List<CostImpact>();
                foreach (var item in costimpact)
                {
                    cstimpct.Add(new CostImpact()
                    {
                        Code = item.Code,
                        Name = item.Name
                    });
                }
                return cstimpct;
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

        public async Task<IEnumerable<ScheduleImpact>> GetScheduledImpactAsync()
        {
            try
            {
                var schedimpact = await _wcfServices.GetScheduleImpactAsync();
                List<ScheduleImpact> scheduleimpact = new List<ScheduleImpact>();
                foreach (var item in schedimpact)
                {
                    scheduleimpact.Add(new ScheduleImpact()
                    {
                        Code = item.Code,
                        Name = item.Name
                    });
                }
                return scheduleimpact;
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

        public async Task<IEnumerable<PunchlistStatus>> GetPunchlistStatusAsync()
        {
            try
            {
                var status = await _wcfServices.GetPunchlistStatusAsync();
                List<PunchlistStatus> punchliststatus = new List<PunchlistStatus>();
                foreach (var item in status)
                {
                    punchliststatus.Add(new PunchlistStatus()
                    {
                        Code = item.Code,
                        Name = item.Name,
                        IsOpen = DataMapping.MapPunchlistStatusByCode(item.Code).IsOpen,
                        IsClosed = DataMapping.MapPunchlistStatusByCode(item.Code).IsClosed
                    });
                }
                return punchliststatus;
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

        public async Task<List<PunchlistDescription>> GetPunchlistDescriptions()
        {
            try
            {
                var desc = await _wcfServices.GetPunchlistDescriptionsAsync();
                List<PunchlistDescription> description = new List<PunchlistDescription>();
                //   var vs = desc.Select(x => x.Name).ToList();
                foreach (var item in desc)
                {
                    description.Add(new PunchlistDescription
                    {
                        Code = item.Code,
                        Name = item.Name,
                        GroupCode = item.GroupCode
                    });
                }
                return description;
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

        public async Task<List<PunchlistGroup>> GetPunchlistGroupAsync()
        {
            try
            {
                var group = await _wcfServices.GetPunchlistGroupAsync();
                List<PunchlistGroup> punchlistgroup = new List<PunchlistGroup>();
                foreach (var item in group)
                {
                    punchlistgroup.Add(new PunchlistGroup
                    {
                        Code = item.Code,
                        CategoryCode = item.CategoryCode,
                        Name = item.Name
                    });
                }
                return punchlistgroup;
                //throw new NotImplementedException();
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

        public async Task<IEnumerable<Punchlist>> GetPunchlistbyUnitAsync(string referenceObject, string username, string _path)
        {
            try
            {
                //time start
                var _timeStartCallMilestoneByUnitInWCF = DateTime.Now;
                Debug.WriteLine($"Starting to call Punchlist By Unit in WCF ");
                var punchlist = (await _wcfServices.GetPunchlistsByUnitAsync(username, referenceObject)).ToList();
                //time end
                var _timeEndCallMilestoneByUnitInWCF = DateTime.Now;
                Debug.WriteLine($"{"Total time of calling Punchlist By Unit in WCF  : " + (_timeEndCallMilestoneByUnitInWCF - _timeStartCallMilestoneByUnitInWCF).TotalSeconds }");

                var _usernames = punchlist.SelectMany(x => x.Comments)
                .Select(x => x.CreatedByUsername).Distinct().ToArray();

                _usersInfo = (await _userInfoProvider.GetUsersAsync(_usernames))
                    .ToDictionary(x => x.Id);

                List<Punchlist> punch = new List<Punchlist>();

                //time start
                var _timeStartCallMilestoneByUnitToTransfer = DateTime.Now;
                Debug.WriteLine($"Starting to transfer Punchlist By Unit To List ");
                punchlist.ForEach(item =>

                        punch.Add(new Punchlist
                        {
                            PunchListID = item.PunchListID,
                            OTCNumber = item.OTCNumber,
                            ContractorNumber = item.ContractorNumber,
                            ManagingContractorID = item.ManagingContractorID,
                            ConstructionMilestoneId = item.ConstructionMilestoneId,
                            ConstructionMilestoneCode = item.MilestoneCode,
                            PunchListCategory = item.PunchListCategory,
                            PunchListSubCategory = item.PunchListSubCategory,
                            NonCompliantTo = item.NonCompliantTo,
                            ReferenceSheet = item.ReferenceSheet,
                            PunchListDescription = item.PunchListDescription,
                            PunchlistDescriptionDetail = new PunchlistDescription
                            {
                                Code = item.PunchListDescriptionDetails.Code,
                                Name = item.PunchListDescriptionDetails.Name,
                                GroupCode = item.PunchListDescriptionDetails.GroupCode
                            },
                            PunchListGroup = item.PunchListGroup,
                            PunchListLocation = item.PunchListLocation,
                            CostImpact = item.CostImpact,
                            ScheduleImpact = item.ScheduleImpact,
                            AssignedTo = item.AssignedTo,
                            PunchListStatus = item.PunchListStatus,
                            DueDate = item.DueDate ?? DateTime.Now,
                            Comments = item.Comments.Select(x => {
                                var _createdUser = GetFromDic(x.CreatedByUsername);
                                var _comment = new Comment
                                {
                                    CommentId = x.CommentId,
                                    Message = x.Message,
                                    CreatedBy = new App.Entities.User
                                    {
                                        Id = _createdUser?.Id,
                                        RoleId = _createdUser?.Role?.id,
                                        Email = _createdUser?.Email,
                                        FirstName = _createdUser?.FullName,
                                        LastName = ""
                                    },
                                    AttachmentFileName = x.AttachmentFileName.Select(f => _path + f).ToList(),
                                    DueDate = x.DueDate,
                                    PunchlistStatus = new PunchlistStatus
                                    {
                                        Code = x.PunchlistStatus?.Code,
                                        Name = x.PunchlistStatus?.Name,
                                        IsOpen = DataMapping.MapPunchlistStatusByCode(x.PunchlistStatus?.Code).IsOpen,
                                        IsClosed = DataMapping.MapPunchlistStatusByCode(x.PunchlistStatus?.Code).IsClosed
                                    }
                                };
                                return _comment;
                            }).ToList() ?? new List<Comment>(),
                            Message = item.Message ?? "",
                            PunchListStatusDetail = new PunchlistStatus
                            {
                                Code = item.PunchListStatusDetail.Code,
                                Name = item.PunchListStatusDetail.Name,
                                IsOpen = DataMapping.MapPunchlistStatusByCode(item.PunchListStatusDetail.Code).IsOpen,
                                IsClosed = DataMapping.MapPunchlistStatusByCode(item.PunchListStatusDetail.Code).IsClosed
                            },
                        })
                );
                //time end
                var _timeEndCallMilestoneByUnitToTransfer = DateTime.Now;
                Debug.WriteLine($"{"Total time of transferring Punchlist By Unit  : " + (_timeEndCallMilestoneByUnitToTransfer - _timeStartCallMilestoneByUnitToTransfer).TotalSeconds }");

                return punch;
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

        public async Task<IEnumerable<Punchlist>> GetPunchlistbyUnitByVendorAsync(string referenceObject, string username, string _path,string vendorCode)
        {
            try
            {
                //time start
                var _timeStartCallMilestoneByUnitInWCF = DateTime.Now;
                Debug.WriteLine($"Starting to call Punchlist By Unit in WCF ");
                var punchlist = (await _wcfServices.GetPunchlistsByUnitByVendorAsync(username, referenceObject,vendorCode)).ToList();
                //time end
                var _timeEndCallMilestoneByUnitInWCF = DateTime.Now;
                Debug.WriteLine($"{"Total time of calling Punchlist By Unit in WCF  : " + (_timeEndCallMilestoneByUnitInWCF - _timeStartCallMilestoneByUnitInWCF).TotalSeconds }");

                var _usernames = punchlist.SelectMany(x => x.Comments)
                .Select(x => x.CreatedByUsername).Distinct().ToArray();

                _usersInfo = (await _userInfoProvider.GetUsersAsync(_usernames))
                    .ToDictionary(x => x.Id);

                List<Punchlist> punch = new List<Punchlist>();

                //time start
                var _timeStartCallMilestoneByUnitToTransfer = DateTime.Now;
                Debug.WriteLine($"Starting to transfer Punchlist By Unit To List ");
                punchlist.ForEach(item =>

                        punch.Add(new Punchlist
                        {
                            PunchListID = item.PunchListID,
                            OTCNumber = item.OTCNumber,
                            ContractorNumber = item.ContractorNumber,
                            ManagingContractorID = item.ManagingContractorID,
                            ConstructionMilestoneId = item.ConstructionMilestoneId,
                            ConstructionMilestoneCode = item.MilestoneCode,
                            PunchListCategory = item.PunchListCategory,
                            PunchListSubCategory = item.PunchListSubCategory,
                            NonCompliantTo = item.NonCompliantTo,
                            ReferenceSheet = item.ReferenceSheet,
                            PunchListDescription = item.PunchListDescription,
                            PunchlistDescriptionDetail = new PunchlistDescription
                            {
                                Code = item.PunchListDescriptionDetails.Code,
                                Name = item.PunchListDescriptionDetails.Name,
                                GroupCode = item.PunchListDescriptionDetails.GroupCode
                            },
                            PunchListGroup = item.PunchListGroup,
                            PunchListLocation = item.PunchListLocation,
                            CostImpact = item.CostImpact,
                            ScheduleImpact = item.ScheduleImpact,
                            AssignedTo = item.AssignedTo,
                            PunchListStatus = item.PunchListStatus,
                            DueDate = item.DueDate ?? DateTime.Now,
                            Comments = item.Comments.Select(x => {
                                var _createdUser = GetFromDic(x.CreatedByUsername);
                                var _comment = new Comment
                                {
                                    CommentId = x.CommentId,
                                    Message = x.Message,
                                    CreatedBy = new App.Entities.User
                                    {
                                        Id = _createdUser?.Id,
                                        RoleId = _createdUser?.Role?.id,
                                        Email = _createdUser?.Email,
                                        FirstName = _createdUser?.FullName,
                                        LastName = ""
                                    },
                                    AttachmentFileName = x.AttachmentFileName.Select(f => _path + f).ToList(),
                                    DueDate = x.DueDate,
                                    PunchlistStatus = new PunchlistStatus
                                    {
                                        Code = x.PunchlistStatus?.Code,
                                        Name = x.PunchlistStatus?.Name,
                                        IsOpen = DataMapping.MapPunchlistStatusByCode(x.PunchlistStatus?.Code).IsOpen,
                                        IsClosed = DataMapping.MapPunchlistStatusByCode(x.PunchlistStatus?.Code).IsClosed
                                    }
                                };
                                return _comment;
                            }).ToList() ?? new List<Comment>(),
                            Message = item.Message ?? "",
                            PunchListStatusDetail = new PunchlistStatus
                            {
                                Code = item.PunchListStatusDetail.Code,
                                Name = item.PunchListStatusDetail.Name,
                                IsOpen = DataMapping.MapPunchlistStatusByCode(item.PunchListStatusDetail.Code).IsOpen,
                                IsClosed = DataMapping.MapPunchlistStatusByCode(item.PunchListStatusDetail.Code).IsClosed
                            },
                        })
                );
                //time end
                var _timeEndCallMilestoneByUnitToTransfer = DateTime.Now;
                Debug.WriteLine($"{"Total time of transferring Punchlist By Unit  : " + (_timeEndCallMilestoneByUnitToTransfer - _timeStartCallMilestoneByUnitToTransfer).TotalSeconds }");

                return punch;
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

        public async Task<IEnumerable<Punchlist>> GetPunchlistbyProjectAsync(string projectcode, string username, string _path)
        {
            try
            {
                //time start
                var _timeStartCallMilestoneByProjectInWCF = DateTime.Now;
                Debug.WriteLine($"Starting to call Punchlist By Project in WCF ");
                var punchlist = (await _wcfServices.GetPunchlistbyProjectAsync(username, projectcode)).ToList();
                //time end
                var _timeEndCallMilestoneByProjectInWCF = DateTime.Now;
                Debug.WriteLine($"{"Total time of calling Punchlist By Project in WCF  : " + (_timeEndCallMilestoneByProjectInWCF - _timeStartCallMilestoneByProjectInWCF).TotalSeconds }");

                var _usernames = punchlist.SelectMany(x => x.Comments)
                   .Select(x => x.CreatedByUsername).Distinct().ToArray();

                _usersInfo = (await _userInfoProvider.GetUsersAsync(_usernames))
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.First());

                List<Punchlist> punch = new List<Punchlist>();

                //time start
                var _timeStartCallMilestoneByProjectToTransfer = DateTime.Now;
                Debug.WriteLine($"Starting to transfer Punchlist By Project To List ");
                punchlist.ForEach(item =>

                        punch.Add(new Punchlist
                        {
                            PunchListID = item.PunchListID,
                            OTCNumber = item.OTCNumber,
                            ContractorNumber = item.ContractorNumber,
                            ManagingContractorID = item.ManagingContractorID,
                            ConstructionMilestoneId = item.ConstructionMilestoneId,
                            ConstructionMilestoneCode = item.MilestoneCode,
                            PunchListCategory = item.PunchListCategory,
                            PunchListSubCategory = item.PunchListSubCategory,
                            NonCompliantTo = item.NonCompliantTo,
                            ReferenceSheet = item.ReferenceSheet,
                            PunchListDescription = item.PunchListDescription,
                            PunchlistDescriptionDetail = new PunchlistDescription
                            {
                                Code = item.PunchListDescriptionDetails.Code,
                                Name = item.PunchListDescriptionDetails.Name,
                                GroupCode = item.PunchListDescriptionDetails.GroupCode
                            },
                            PunchListGroup = item.PunchListGroup,
                            PunchListLocation = item.PunchListLocation,
                            CostImpact = item.CostImpact,
                            ScheduleImpact = item.ScheduleImpact,
                            AssignedTo = item.AssignedTo,
                            PunchListStatus = item.PunchListStatus,
                            DueDate = item.DueDate ?? DateTime.Now,
                            Comments = item.Comments.Select(x =>
                            {
                                var _createdUser = GetFromDic(x.CreatedByUsername);
                                var _comment = new Comment
                                {
                                    CommentId = x.CommentId,
                                    Message = x.Message,
                                    CreatedBy = new App.Entities.User
                                    {
                                        Id = _createdUser?.Id,
                                        RoleId = _createdUser?.Role?.id,
                                        Email = _createdUser?.Email,
                                        FirstName = _createdUser?.FullName,
                                        LastName = ""
                                    },
                                    AttachmentFileName = x.AttachmentFileName.Select(f => _path + f).ToList(),
                                    DueDate = x.DueDate,
                                    PunchlistStatus = new PunchlistStatus
                                    {
                                        Code = x.PunchlistStatus?.Code,
                                        Name = x.PunchlistStatus?.Name,
                                        IsOpen = DataMapping.MapPunchlistStatusByCode(x.PunchlistStatus?.Code).IsOpen,
                                        IsClosed = DataMapping.MapPunchlistStatusByCode(x.PunchlistStatus?.Code).IsClosed
                                    }
                                };
                                return _comment;
                            }).ToList() ?? new List<Comment>(),
                            Message = item.Message ?? "",
                            PunchListStatusDetail = new PunchlistStatus
                            {
                                Code = item.PunchListStatusDetail.Code,
                                Name = item.PunchListStatusDetail.Name,
                                IsOpen = DataMapping.MapPunchlistStatusByCode(item.PunchListStatusDetail.Code).IsOpen,
                                IsClosed = DataMapping.MapPunchlistStatusByCode(item.PunchListStatusDetail.Code).IsClosed
                            },
                        })
                );
                //time end
                var _timeEndCallMilestoneByProjectToTransfer = DateTime.Now;
                Debug.WriteLine($"{"Total time of transferring Punchlist By Project  : " + (_timeEndCallMilestoneByProjectToTransfer - _timeStartCallMilestoneByProjectToTransfer).TotalSeconds }");


                return punch;
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


        public async Task<string> DeletePunchlistAttachment(string filename)
        {
            try
            {
                return await _fileUploadService.DeletePunchlistImageByName(filename);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<PunchListBinaryImage> GetPunchlistAttachment(string filename)
        {
            try
            {
                return await _fileUploadService.GetPunchlistImageByName(filename);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<string> UploadAllPunchlistImagesFromFTPServerToDatabase(string username)
        {
            var _timeStart = DateTime.Now;
            Debug.WriteLine($"Starting to upload punchlist images to DB");
            
            int count = 0;
            string flname = string.Empty;

            try
            {
                var result = new List<string>();
                var lstfrebas = Task.Run(async () => (await _wcfServices.GetMilestonePunchlistsImagesAsync()).Select(x => x.FileName).Distinct().ToList());
                var lstHI = Task.Run(async () => (await _fileUploadService.GetPunchlistImage()).Select(x => x.FileName).ToList());

                await Task.WhenAll(lstfrebas, lstHI);

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

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/Attachment");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (result.Count > 0)
                {
                    //result = result.Take(300).ToList();

                    foreach (var item in result)
                    {
                        var newData = _fileUploadService.GetPunchlistImageByName(item.ToString()).Result;

                        if (IsExists(_fTPSettings.PostHostName, _fTPSettings.FtpUsername, _fTPSettings.FtpPassword, string.Empty, item.ToString()))
                        {
                            if (newData.FileName == null)
                            {
                                Debug.WriteLine($"Saving Punchlist Image : {item.ToString()}");

                                flname = item.ToString();

                                DownloadFileFromFTPServer(_fTPSettings.PostHostName, _fTPSettings.FtpUsername, _fTPSettings.FtpPassword, string.Empty, item.ToString(), path);

                                await SaveFileToDatabase(item.ToString(), username);

                                count++;

                                Debug.WriteLine($"{item.ToString()} has been successfully inserted.");
                            }
                        }

                    }
                }
                else
                {
                    count = 0;
                    return $"All images from FTP already uploaded to DB";
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

                return $"Successfully inserted {total} punchlist images to database. Total time of uploading milestone images from FTP Server : {_time}";

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



        public async Task<string> SavePunchlistAttachment(IFormFile uploadedFile, string username)
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

                PunchListBinaryImage punchlistImage = new PunchListBinaryImage
                {
                    FileBinary = filebytes,

                    FileName = compfileName,

                    CreatedBy = username
                };

                await _fileUploadService.SavePunchlistImage(punchlistImage);

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

        #region Method
        public void VerifyAndSave(string filename, string username, string path)
        {
            try
            {
                var data = _fileUploadService.GetPunchlistImageByName(filename).Result;

                if (IsExists(_fTPSettings.PostHostName, _fTPSettings.FtpUsername, _fTPSettings.FtpPassword, string.Empty, filename))
                {
                    if (data.FileName == null)
                    {
                        Debug.WriteLine($"Saving Punchlist Image : {filename}");

                        DownloadFileFromFTPServer(_fTPSettings.PostHostName, _fTPSettings.FtpUsername, _fTPSettings.FtpPassword, string.Empty, filename, path);

                        SaveFileToDatabase(filename, username);

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

                PunchListBinaryImage punchlistImage = new PunchListBinaryImage
                {
                    FileBinary = filebytes,

                    FileName = file,

                    CreatedBy = string.IsNullOrEmpty(username) ? "Undefined User" : username
                };

                await _fileUploadService.SavePunchlistImage(punchlistImage);

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
            catch
            {
                IsExists = false;
            }

            return IsExists;
        }
        #endregion

    }
}
