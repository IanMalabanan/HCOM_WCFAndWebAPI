using Hcom.Web.Api.Interface;
using Hcom.Web.Api.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserEP = UserSvcRefence;
using WCF = NotificationSvcReference;
using Hcom.App.Entities;
using Hcom.App.Entities.HCOM;

namespace Hcom.Web.Api.Services
{
    public class NotificationService : INotification
    {
        private readonly ServiceEndpoints _endpointSettings;
        private WCF.INotificationService _wcfServices;
        private UserEP.IUserService _userServices;
        public const string notificationServiceEndpoint = "NotificationService.svc";
        public const string userServiceEndpoint = "UserService.svc";

        public NotificationService(IOptionsMonitor<ServiceEndpoints> endpointAccessor)
        {
            _endpointSettings = endpointAccessor.CurrentValue;

            _wcfServices = new WCF.NotificationServiceClient(WCF.NotificationServiceClient.EndpointConfiguration.BasicHttpBinding_INotificationService,
                 _endpointSettings.BaseUrl + notificationServiceEndpoint);

            _userServices = new UserEP.UserServiceClient(UserEP.UserServiceClient.EndpointConfiguration.BasicHttpBinding_IUserService,
                _endpointSettings.BaseUrl + userServiceEndpoint);
        }

        public async Task<IEnumerable<TransactionNotificationModel>> GetOpenPunchlistAsync(string username)
        {
            try
            {
                var punchlistdetails = await _wcfServices.GetOpenPunchlistsAsync(username);
                List<TransactionNotificationModel> openpunchlist = new List<TransactionNotificationModel>();
                foreach (var item in punchlistdetails)
                {
                    openpunchlist.Add(new TransactionNotificationModel
                    {
                        UnitInfo = new Unit
                        {
                            ReferenceObject = item.Unit.ReferenceObject,
                            ProjectCode = item.Unit.Project.Code,
                            Project = new Project
                            {
                                ProjectCode = item.Unit.Project.Code,
                                ShortName = item.Unit.Project.ShortName,
                                LongName = item.Unit.Project.LongName,
                                ImageUrl = item.Unit.Project.ImageUrl ?? ""
                            },
                            PhaseBuildingCode = item.Unit.PhaseBuilding.Code,
                            PhaseBuilding = new PhaseBuilding
                            {
                                SapCodeProperty = item.Unit.PhaseBuilding.Code,
                                ShortName = item.Unit.PhaseBuilding.ShortName,
                                LongName = item.Unit.PhaseBuilding.LongName
                            },
                            BlockFloorCode = item.Unit.BlockFloor.Code,
                            BlockFloor = new BlockFloorCluster
                            {
                                Id = item.Unit.BlockFloor.Code,
                                ShortName = item.Unit.BlockFloor.ShortName,
                                LongName = item.Unit.BlockFloor.LongName

                            },
                            LotUnitShareNumber = item.Unit.LotUnitShareNumber,
                            InventoryUnitNumber = item.Unit.InventoryUnitNumber
                        },
                        DatePosted = item.DatePosted,
                        DueDate = item.DueDate,
                        Message = item.Message,
                        MessageBy = item.MessageBy,
                        MilestoneID = item.ConstructionMilestoneId,
                        PunchlistID = item.PunchlistId,
                        Subject = item.Subject
                    });


                }
                return openpunchlist;
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

        public async Task<IEnumerable<TransactionNotificationModel>> GetOpenOverduePunchlistsAsync(string username)
        {
            try
            {
                var punchlistdetails = await _wcfServices.GetOpenOverduePunchlistsAsync(username);
                List<TransactionNotificationModel> openpunchlist = new List<TransactionNotificationModel>();
                foreach (var item in punchlistdetails)
                {
                    openpunchlist.Add(new TransactionNotificationModel
                    {
                        UnitInfo = new Unit
                        {
                            ReferenceObject = item.Unit.ReferenceObject,
                            ProjectCode = item.Unit.Project.Code,
                            Project = new Project
                            {
                                ProjectCode = item.Unit.Project.Code,
                                ShortName = item.Unit.Project.ShortName,
                                LongName = item.Unit.Project.LongName,
                                ImageUrl = item.Unit.Project.ImageUrl ?? ""
                            },
                            PhaseBuildingCode = item.Unit.PhaseBuilding.Code,
                            PhaseBuilding = new PhaseBuilding
                            {
                                SapCodeProperty = item.Unit.PhaseBuilding.Code,
                                ShortName = item.Unit.PhaseBuilding.ShortName,
                                LongName = item.Unit.PhaseBuilding.LongName
                            },
                            BlockFloorCode = item.Unit.BlockFloor.Code,
                            BlockFloor = new BlockFloorCluster
                            {
                                Id = item.Unit.BlockFloor.Code,
                                ShortName = item.Unit.BlockFloor.ShortName,
                                LongName = item.Unit.BlockFloor.LongName

                            },
                            LotUnitShareNumber = item.Unit.LotUnitShareNumber,
                            InventoryUnitNumber = item.Unit.InventoryUnitNumber
                        },
                        DatePosted = item.DatePosted,
                        DueDate = item.DueDate,
                        Message = item.Message,
                        MessageBy = item.MessageBy,
                        MilestoneID = item.ConstructionMilestoneId,
                        PunchlistID = item.PunchlistId,
                        Subject = item.Subject
                    });
                }
                return openpunchlist;

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

        public async Task<IEnumerable<TransactionNotificationModel>> GetRecentlyClosedPunchlistsAsync(string username)
        {
            try
            {
                var punchlistdetails = (await _wcfServices.GetRecentlyClosedPunchlistsAsync(username));
                List<TransactionNotificationModel> openpunchlist = new List<TransactionNotificationModel>();
                foreach (var item in punchlistdetails)
                {
                    openpunchlist.Add(new TransactionNotificationModel
                    {
                        UnitInfo = new Unit
                        {
                            ReferenceObject = item.Unit.ReferenceObject,
                            ProjectCode = item.Unit.Project.Code,
                            Project = new Project
                            {
                                ProjectCode = item.Unit.Project.Code,
                                ShortName = item.Unit.Project.ShortName,
                                LongName = item.Unit.Project.LongName,
                                ImageUrl = item.Unit.Project.ImageUrl ?? ""
                            },
                            PhaseBuildingCode = item.Unit.PhaseBuilding.Code,
                            PhaseBuilding = new PhaseBuilding
                            {
                                SapCodeProperty = item.Unit.PhaseBuilding.Code,
                                ShortName = item.Unit.PhaseBuilding.ShortName,
                                LongName = item.Unit.PhaseBuilding.LongName
                            },
                            BlockFloorCode = item.Unit.BlockFloor.Code,
                            BlockFloor = new BlockFloorCluster
                            {
                                Id = item.Unit.BlockFloor.Code, //Check
                                ShortName = item.Unit.BlockFloor.ShortName,
                                LongName = item.Unit.BlockFloor.LongName

                            },
                            LotUnitShareNumber = item.Unit.LotUnitShareNumber,
                            InventoryUnitNumber = item.Unit.InventoryUnitNumber
                        },
                        DatePosted = item.DatePosted,
                        DueDate = item.DueDate,
                        Message = item.Message,
                        MessageBy = item.MessageBy,
                        MilestoneID = item.ConstructionMilestoneId,
                        PunchlistID = item.PunchlistId,
                        Subject = item.Subject
                    });
                }
                return openpunchlist;
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

        public async Task<IEnumerable<TransactionNotificationModel>> GetDelayedMilestonesAsync(string username)
        {
            try
            {
                var punchlistdetails = (await _wcfServices.GetDelayedMilestonesAsync(username));
                List<TransactionNotificationModel> openpunchlist = new List<TransactionNotificationModel>();
                foreach (var item in punchlistdetails)
                {
                    openpunchlist.Add(new TransactionNotificationModel
                    {
                        UnitInfo = new Unit
                        {
                            ReferenceObject = item.Unit.ReferenceObject,
                            ProjectCode = item.Unit.Project.Code,
                            Project = new Project
                            {
                                ProjectCode = item.Unit.Project.Code,
                                ShortName = item.Unit.Project.ShortName,
                                LongName = item.Unit.Project.LongName,
                                ImageUrl = item.Unit.Project.ImageUrl ?? ""
                            },
                            PhaseBuildingCode = item.Unit.PhaseBuilding.Code,
                            PhaseBuilding = new PhaseBuilding
                            {
                                SapCodeProperty = item.Unit.PhaseBuilding.Code,
                                ShortName = item.Unit.PhaseBuilding.ShortName,
                                LongName = item.Unit.PhaseBuilding.LongName
                            },
                            BlockFloorCode = item.Unit.BlockFloor.Code,
                            BlockFloor = new BlockFloorCluster
                            {
                                Id = item.Unit.BlockFloor.Code,
                                ShortName = item.Unit.BlockFloor.ShortName,
                                LongName = item.Unit.BlockFloor.LongName

                            },
                            LotUnitShareNumber = item.Unit.LotUnitShareNumber,
                            InventoryUnitNumber = item.Unit.InventoryUnitNumber
                        },
                        DatePosted = item.DatePosted,
                        DueDate = item.DueDate,
                        Message = item.Message,
                        MessageBy = item.MessageBy,
                        MilestoneID = item.ConstructionMilestoneId,
                        PunchlistID = item.PunchlistId,
                        Subject = item.Subject
                    });
                }
                return openpunchlist;
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

        public async Task<IEnumerable<TransactionNotificationModel>> GetRecentlyClosedMilestoneAsync(string username)
        {
            try
            {
                var punchlistdetails = (await _wcfServices.GetRecentlyClosedMilestonesAsync(username));
                List<TransactionNotificationModel> openpunchlist = new List<TransactionNotificationModel>();
                foreach (var item in punchlistdetails)
                {
                    openpunchlist.Add(new TransactionNotificationModel
                    {
                        UnitInfo = new Unit
                        {
                            ReferenceObject = item.Unit.ReferenceObject,
                            ProjectCode = item.Unit.Project.Code,
                            Project = new Project
                            {
                                ProjectCode = item.Unit.Project.Code,
                                ShortName = item.Unit.Project.ShortName,
                                LongName = item.Unit.Project.LongName,
                                ImageUrl = item.Unit.Project.ImageUrl ?? ""
                            },
                            PhaseBuildingCode = item.Unit.PhaseBuilding.Code,
                            PhaseBuilding = new PhaseBuilding
                            {
                                SapCodeProperty = item.Unit.PhaseBuilding.Code,
                                ShortName = item.Unit.PhaseBuilding.ShortName,
                                LongName = item.Unit.PhaseBuilding.LongName
                            },
                            BlockFloorCode = item.Unit.BlockFloor.Code,
                            BlockFloor = new BlockFloorCluster
                            {
                                Id = item.Unit.BlockFloor.Code, //Check
                                ShortName = item.Unit.BlockFloor.ShortName,
                                LongName = item.Unit.BlockFloor.LongName

                            },
                            LotUnitShareNumber = item.Unit.LotUnitShareNumber,
                            InventoryUnitNumber = item.Unit.InventoryUnitNumber
                        },
                        DatePosted = item.DatePosted,
                        DueDate = item.DueDate,
                        Message = item.Message,
                        MessageBy = item.MessageBy,
                        MilestoneID = item.ConstructionMilestoneId,
                        PunchlistID = item.PunchlistId,
                        Subject = item.Subject
                    });
                }
                return openpunchlist;
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
    }
}
