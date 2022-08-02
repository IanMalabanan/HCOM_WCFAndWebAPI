using Core.Common.Contracts;
using CTI.HCM.Business.Entities.Models;
using CTI.HI.Business.Contracts;
using CTI.HI.Business.Entities;
using CTI.HI.Business.Entities.Notification;
using CTI.HI.Business.Managers.External;
using CTI.HI.Data.Contracts.Frebas;
using CTI.HI.Data.Repository.Frebas;
using CTI.IMM.Business.Entities.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                   ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class NotificationManager : ManagerBase, INotificationService
    {

        #region Constructors 
        private IPunchlistRepository _PunchlistRepo;
        private IUserRepository _UserRepo;
        private IInventoryUnitRepository _UnitRepo;
        private IUserProjectRepository _UserProjectRepo;
        private IConstructionMilestoneRepository _constructionRepo;
        private IContractorRepository _contractorRepo;
        private IExternalMessagingService _externalMsgService;
        private DateTime _DateToday = DateTime.Now.Date;
        private DateTime _DateTomorrow = DateTime.Now.AddDays(1).Date;
        private DateTime _DateMin = DateTime.Now.AddDays(-100).Date;
        private int _NotifRecentDays = 7;
        public NotificationManager()
        {
            _PunchlistRepo = new PunchlistRepository();
            _constructionRepo = new ConstructionMilestoneRepository();
            _UserRepo = new UserRepository();
            _UserProjectRepo = new UserProjectRepository();
            _UnitRepo = new InventoryUnitRepository();
            _contractorRepo = new ContractorRepository();
            _externalMsgService = new MessagingManager();//new External.InfoBipManager();
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        public NotificationManager(IDataRepositoryFactory dataRepositoryFactory) : this()
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;

        public NotificationManager(IBusinessEngineFactory businessEngineFactory) : this()
        {
            _BusinessEngineFactory = businessEngineFactory;
        }

        public NotificationManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory) : this()
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
        }
        #endregion



        public async Task<IEnumerable<NotificationModel>> GetOpenPunchlistsAsync(string userName)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    return await _PunchlistRepo.OpenPunchlistsNotificationAsync(userName, "OPEN");
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", userName);
                    Log.Error("Exception detail {ex}", ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", userName);
                    Log.Error("Exception detail {ex}", ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", userName);
                    Log.Error("Exception detail {ex}", ex);
                    throw new Exception(errMsg);
                }
            });
        }

        public async Task<IEnumerable<NotificationModel>> GetOpenOverduePunchlistsAsync(string userName)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    var data = (await _PunchlistRepo.OpenOverduePunchlistNotificationAsync(userName, "OPEN")).ToList();

                    return data;
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", userName);
                    Log.Error("Exception detail {ex}", ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", userName);
                    Log.Error("Exception detail {ex}", ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", userName);
                    Log.Error("Exception detail {ex}", ex);
                    throw new Exception(errMsg);
                }
            });
        }

        public async Task<IEnumerable<NotificationModel>> GetRecentlyClosedPunchlistsAsync(string userName)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    //Get Open punchlist and overdue and Lessthan 7 days
                    return await _PunchlistRepo.RecentlyClosedPunchlistNotificationAsync(userName, "CLOS");
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", userName);
                    Log.Error("Exception detail {ex}", ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", userName);
                    Log.Error("Exception detail {ex}", ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", userName);
                    Log.Error("Exception detail {ex}", ex);
                    throw new Exception(errMsg);
                }
            });
        }

        public async Task<IEnumerable<NotificationModel>> GetDelayedMilestonesAsync(string userName)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    await Task.Delay(100);
                    //Theres no delayed Milestone
                    //[8:50 AM] Elsie R. Gaasis - ang alam ko parang wala pa yan kasi ung target completion date is walang breakdown per milestone

                    return new List<NotificationModel>();
                }
                catch (Exception ex)
                {
                    throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
                }
            });
        }

        public async Task<IEnumerable<NotificationModel>> GetRecentlyClosedMilestonesAsync(string userName)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    //get the recent 7Days 100% milestone of the user 
                    var _ConstructionUnit = (await _constructionRepo.GetUserConstructionMilestoneNotificationAsync(userName));

                    return _ConstructionUnit;
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", userName);
                    Log.Error("Exception detail {ex}", ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", userName);
                    Log.Error("Exception detail {ex}", ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", userName);
                    Log.Error("Exception detail {ex}", ex);
                    throw new Exception(errMsg);
                }
            });
        }

        public async Task<MessagingInformation> GetMessagingInformation(int punchlistId)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    Log.Information("GetMessagingInformation by punchlist id: {punchlistId}", punchlistId);

                    var _punchlist = await _PunchlistRepo.GetMilestonePunchlistAsync(punchlistId);
                    var _representative = await _contractorRepo.GetRepresentativeAsync(_punchlist.AssignedTo);
                    var _contractor = await _contractorRepo.GetVendorAsync(_representative.ContractorCode);
                    var _refObject = await _constructionRepo.GetMilestoneReferenceObjectAsync(_punchlist.ConstructionMilestoneId);
                    var _unit = await _UnitRepo.GetUnitAsync(_refObject);

                    var _info = new MessagingInformation
                    {
                        PunchlistDescription = _punchlist.PunchListDescription,
                        PunchlistReferenceNumber = _punchlist.ReferenceNumber,
                        Representative = _representative,
                        Contractor = new Contractor
                        {
                            Code = _contractor.Code,
                            Name = _contractor.Name
                        },
                        Unit = new Unit
                        {
                            Project = new Project
                            {
                                Code = _unit.Project.Code,
                                ShortName = _unit.Project.ShortName,
                                LongName = _unit.Project.LongName
                            },
                            PhaseBuilding = new PhaseBuilding
                            {
                                Code = _unit.PhaseBuilding.Code,
                                ShortName = _unit.PhaseBuilding.ShortName,
                                LongName = _unit.PhaseBuilding.LongName
                            },
                            BlockFloor = new Block
                            {
                                Code = _unit.BlockFloor.Code,
                                ShortName = _unit.BlockFloor.ShortName,
                                LongName = _unit.BlockFloor.LongName
                            },
                            InventoryUnitNumber = _unit.InventoryUnitNumber,
                            LotUnitShareNumber = _unit.LotUnitShareNumber,
                            ReferenceObject = _unit.ReferenceObject
                        },

                    };

                    return _info;
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetMessagingInformation by punchlist id: {punchlistId}, Error found {ex}", punchlistId, ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetMessagingInformation by punchlist id: {punchlistId}, Error found {ex}", punchlistId, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetMessagingInformation by punchlist id: {punchlistId}, Error found {ex}", punchlistId, ex);
                    throw new Exception(errMsg);
                }
            });
        }
        public async Task SendPunchlistNotificationAsync(int punchlistId)
        {
            await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    Log.Information("GetMessagingInformation by punchlist id: {punchlistId}", punchlistId);

                    var _punchlist = await _PunchlistRepo.GetMilestonePunchlistAsync(punchlistId);
                    var _representative = await _contractorRepo.GetRepresentativeAsync(_punchlist.AssignedTo);
                    var _contractor = await _contractorRepo.GetVendorAsync(_representative.ContractorCode);
                    var _refObject = await _constructionRepo.GetMilestoneReferenceObjectAsync(_punchlist.ConstructionMilestoneId);
                    var _unit = await _UnitRepo.GetUnitAsync(_refObject);

                    var _info = new MessagingInformation
                    {
                        PunchlistDescription = _punchlist.PunchListDescription,
                        PunchlistReferenceNumber = _punchlist.ReferenceNumber,
                        Representative = _representative,
                        Contractor = new Contractor
                        {
                            Code = _contractor.Code,
                            Name = _contractor.Name
                        },
                        Unit = new Unit
                        {
                            Project = new Project
                            {
                                Code = _unit.Project.Code,
                                ShortName = _unit.Project.ShortName,
                                LongName = _unit.Project.LongName
                            },
                            PhaseBuilding = new PhaseBuilding
                            {
                                Code = _unit.PhaseBuilding.Code,
                                ShortName = _unit.PhaseBuilding.ShortName,
                                LongName = _unit.PhaseBuilding.LongName
                            },
                            BlockFloor = new Block
                            {
                                Code = _unit.BlockFloor.Code,
                                ShortName = _unit.BlockFloor.ShortName,
                                LongName = _unit.BlockFloor.LongName
                            },
                            InventoryUnitNumber = _unit.InventoryUnitNumber,
                            LotUnitShareNumber = _unit.LotUnitShareNumber,
                            ReferenceObject = _unit.ReferenceObject
                        },
                    };
                    var Contactnumber = _info.Representative.ContactNumber;
                    var punchlistInfo = $"{_info.Unit.Project.LongName} {_info.Unit.PhaseBuilding.LongName} {_info.Unit.BlockFloor.LongName} {_info.Unit.LotUnitShareNumber}";
                    //Construct Message

                    //var x = new EmailModel();
                    var message = "Please be advised that a punch list report has been created for " + punchlistInfo;
                    var msgResult = await _externalMsgService.SendSmsAsync(message, Contactnumber);
                    // var emailResult = await _externalMsgService.SendEmailAsync(x);
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("SendPunchlistNotificationAsync by punchlist id: {punchlistId}, Error found {ex}", punchlistId, ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("SendPunchlistNotificationAsync by punchlist id: {punchlistId}, Error found {ex}", punchlistId, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("SendPunchlistNotificationAsync by punchlist id: {punchlistId}, Error found {ex}", punchlistId, ex);
                    throw new Exception(errMsg);
                }
            });
        }

        public async Task SendCompletePercentageNotification(ConstructionMilestoneModel milestone, UnitModel unit, ContractorAwardedLoa loa)
        {
            Template.MilestoneCompleteTemplate x = new Template.MilestoneCompleteTemplate();
            x.Unit = unit;
            x.Loa = loa;
            x.Milestone = milestone;
            await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    Log.Error("SendCompletePercentageNotification for Project: {projectname}, Phase Building: {phasebuilding}, Block: {block} with Contract Number: {contractnumber} and NTP Number: {ntpnumber}", unit.Project.LongName, unit.PhaseBuilding.LongName, unit.BlockFloor.LongName, loa.LoaContractNumber, loa.NTPNumber);

                    var _messageinfo = new EmailModel()
                    {
                        To = "alice.alcantara@filinvestland.com;jhudenmae.macas@filinvestland.com;charles.pascual@filinvestland.com",//"charles.pascual@filinvestland.com",
                        From = "donotreply@filinvestland.com",
                        Subject = x.Unit.Project.LongName,
                        Format = "HTML",
                        Id = "1",
                        Body = x.ToString()
                    };

                    var msgResultt = await _externalMsgService.SendEmailAsync(_messageinfo);

                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("SendCompletePercentageNotification for Project: {projectname}, Phase Building: {phasebuilding}, Block: {block} with Contract Number: {contractnumber} and NTP Number: {ntpnumber}, Error found {ex}", unit.Project.LongName, unit.PhaseBuilding.LongName, unit.BlockFloor.LongName, loa.LoaContractNumber, loa.NTPNumber, ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("SendCompletePercentageNotification for Project: {projectname}, Phase Building: {phasebuilding}, Block: {block} with Contract Number: {contractnumber} and NTP Number: {ntpnumber}, Error found {ex}", unit.Project.LongName, unit.PhaseBuilding.LongName, unit.BlockFloor.LongName, loa.LoaContractNumber, loa.NTPNumber, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("SendCompletePercentageNotification for Project: {projectname}, Phase Building: {phasebuilding}, Block: {block} with Contract Number: {contractnumber} and NTP Number: {ntpnumber}, Error found {ex}", unit.Project.LongName, unit.PhaseBuilding.LongName, unit.BlockFloor.LongName, loa.LoaContractNumber, loa.NTPNumber, ex);
                    throw new Exception(errMsg);
                }
            });

        }

        public async Task SendTextTest(string number, string text)
        {
            try
            {
                var msgResult = await _externalMsgService.SendSmsAsync(text, number);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
