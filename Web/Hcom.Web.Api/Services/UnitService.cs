using Hcom.Web.Api.Interface;
using Hcom.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCF = UnitSvcReference;
using UserEP = UserSvcRefence;
using Microsoft.Extensions.Options;
using Hcom.Web.Api.Utilities.Security;
using Hcom.App.Entities;
using System.Text.RegularExpressions;
using Hcom.App.Entities.Models;
using Hcom.App.Entities.HCOM;

namespace Hcom.Web.Api.Services
{
    public class UnitService : IUnit
    {
        private readonly ServiceEndpoints _endpointSettings;
        private WCF.IUnitService _wcfServices;
        private UserEP.IUserService _userServices;
        public const string unitServiceEndpoint = "UnitService.svc";
        public const string userServiceEndpoint = "UserService.svc";

        public UnitService(IOptionsMonitor<ServiceEndpoints> endpointAccessor)
        {
            _endpointSettings = endpointAccessor.CurrentValue;

            _wcfServices = new WCF.UnitServiceClient(WCF.UnitServiceClient.EndpointConfiguration.BasicHttpBinding_IUnitService,
                 _endpointSettings.BaseUrl + unitServiceEndpoint);

            _userServices = new UserEP.UserServiceClient(UserEP.UserServiceClient.EndpointConfiguration.BasicHttpBinding_IUserService,
                _endpointSettings.BaseUrl + userServiceEndpoint);
        }

        public async Task<IEnumerable<Project>> GetProjectByUserAsync(string username,string keyword = "")
        {
            try
            {
                var _filteredProj = "";//_config.GetConfigurationSection("FilteredProjects").Value;

                if (keyword == "")
                    keyword = _filteredProj;

                var usr = await _userServices.GetUserAsync(username);
                if (usr == null)
                    throw new ApplicationException("User does not exists");
                if (usr.RoleCode == null)
                    throw new ApplicationException("No defined Role for this User.");

                var projs = (await _wcfServices.GetProjectByUserAsync(username));

                List<Project> projects = new List<Project>();
                foreach (var item in projs)
                {
                    projects.Add(new Project()
                    {
                        ProjectCode = item.Code,
                        LongName = item.LongName,
                        ShortName = item.ShortName,
                        ImageUrl = item.ImageUrl,
                        Address = new App.Entities.Models.Address
                        {
                            Longitude = (double)(item.Longitude ?? 0),
                            Latitude = (double)(item.Latitude ?? 0)
                        }
                    });
                }
                return projects;
            }
            catch (ApplicationException ex)
            {
                throw new ApplicationException(ex.Message,ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        
        public async Task<IEnumerable<PhaseBuilding>> GetPhaseByProjectAsync(string projectCode, string username, string keyword = "")
        {
            try
            {
                var usr = await _userServices.GetUserAsync(username);
                if (usr == null)
                    throw new ApplicationException("User does not exists");
                if (usr.RoleCode == null)
                    throw new ApplicationException("No defined Role for this User.");

                var phse = (await _wcfServices.GetPhaseBuildingByProjectAsync(username, projectCode));

                List<PhaseBuilding> phases = new List<PhaseBuilding>();
                foreach (var item in phse)
                {
                    phases.Add(new PhaseBuilding()
                    {
                        Id = item.Code,
                        LongName = item.LongName,
                        ShortName = item.ShortName,
                        Address = new Address
                        {
                            Longitude = item.Longitude,
                            Latitude = item.Latitude
                        }
                    });
                }

                phases = phases.OrderBy(x => PadNumbers(x.LongName)).ToList();

                return phases;
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
        
        public async Task<IEnumerable<BlockFloorCluster>> GetBlockByPhaseAsync(string phaseCode,string username, string keyword = "")
        {
            try
            {
                var usr = await _userServices.GetUserAsync(username);
                if (usr == null)
                    throw new ApplicationException("User does not exists");
                if (usr.RoleCode == null)
                    throw new ApplicationException("No defined Role for this User.");

                var blck = (await _wcfServices.GetBlockByPhaseBuildingAsync(username, phaseCode));

                List<BlockFloorCluster> blocks = new List<BlockFloorCluster>();
                foreach (var item in blck)
                {
                    blocks.Add(new BlockFloorCluster()
                    {
                        Id = item.Code,
                        LongName = item.LongName,
                        ShortName = item.ShortName,
                        Address = new Address
                        {
                            Longitude = item.Longitude,
                            Latitude = item.Latitude
                        }
                    });
                }

                blocks = blocks.OrderBy(x => PadNumbers(x.LongName)).ToList();

                return blocks;
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

        public async Task<IEnumerable<Unit>> GetUnitByBlockAsync(string blockCode, string username, string keyword = "")
        {
            try
            {
                var usr = await _userServices.GetUserAsync(username);
                if (usr == null)
                    throw new ApplicationException("User does not exists");
                if (usr.RoleCode == null)
                    throw new ApplicationException("No defined Role for this User.");

                var units = (await _wcfServices.GetUnitByBlockAsync(username, blockCode));
                List<Unit> AppUnits = new List<Unit>();
                foreach (var item in units)
                {
                    AppUnits.Add(new Unit()
                    {
                        ReferenceObject = item.ReferenceObject,
                        ProjectCode = item.Project.Code,
                        Project = new Project
                        {
                            ProjectCode = item.Project.Code,
                            ShortName = item.Project.ShortName,
                            LongName = item.Project.LongName
                        },
                        PhaseBuildingCode = item.PhaseBuilding.Code,
                        PhaseBuilding = new PhaseBuilding
                        {
                            Id = item.PhaseBuilding.Code,
                            LongName = item.PhaseBuilding.LongName,
                            ShortName = item.PhaseBuilding.ShortName
                        },
                        BlockFloorCode = item.BlockFloor.Code,
                        BlockFloor = new BlockFloorCluster
                        {
                            Id = item.BlockFloor.Code,
                            LongName = item.BlockFloor.LongName,
                            ShortName = item.BlockFloor.ShortName
                        },
                        InventoryUnitNumber = item.InventoryUnitNumber,
                        LotUnitShareNumber = item.LotUnitShareNumber,
                        Address = new Address
                        {
                            Longitude = (double)(item.Longitude ?? 0),
                            Latitude = (double)(item.Latitude ?? 0)
                        },
                        FloorPlanUrls = (item.FloorPlan != null ? item.FloorPlan.ToList() : new List<string>()),
                        MilestonePercentage = new MilestonePercentage
                        {
                            BillingPercentage = item.MilestonePercentage.BillingPercentage ?? 0,
                            ContractorsName = item.MilestonePercentage.Contractors,
                            CurrentPOC = item.MilestonePercentage.PercentageCompletion ?? 0,
                            OngoingActivityMilestone = item.MilestonePercentage.OngoingMilestoneActivity,
                            TargetPOC = item.MilestonePercentage.TargetPercentageCompletion ?? 0,
                            TotalOverdue = item.MilestonePercentage.PunchlistOverdueCount,
                            TotalPending = item.MilestonePercentage.PunchlistPendingCount,
                            TotalPunchlist = item.MilestonePercentage.PunchlistCount,
                            Variance = item.MilestonePercentage.Variance
                        },
                        VendorCode = item.VendorCode
                    });
                }
                return AppUnits;
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

        public async Task<Unit> GetUnitAsync(string referenceObject, string username)
        {
            try
            {
                var usr = await _userServices.GetUserAsync(username);
                if (usr == null)
                    throw new ApplicationException("User does not exists");
                if (usr.RoleCode == null)
                    throw new ApplicationException("No defined Role for this User.");


                var x = (await _wcfServices.GetUnitByRefObjectAsync(username, referenceObject)).FirstOrDefault();
                var _encryptionProvider = RijndaelEncryptionProvider.GetInstance();

                return new Unit()
                {
                    EncryptReferenceObject = _encryptionProvider.Encrypt(x.ReferenceObject),
                    ReferenceObject = x.ReferenceObject,
                    ProjectCode = x.Project.Code,
                    Project = new Project
                    {
                        ProjectCode = x.Project.Code,
                        ShortName = x.Project.ShortName,
                        LongName = x.Project.LongName,
                        Address = new Address {
                            Longitude = (double)(x.Project.Longitude ?? 0),
                            Latitude = (double)(x.Project.Latitude ?? 0)
                        }
                    },
                    PhaseBuildingCode = x.PhaseBuilding.Code,
                    PhaseBuilding = new PhaseBuilding
                    {
                        Id = x.PhaseBuilding.Code,
                        LongName = x.PhaseBuilding.LongName,
                        ShortName = x.PhaseBuilding.ShortName
                    },
                    BlockFloorCode = x.BlockFloor.Code,
                    BlockFloor = new BlockFloorCluster
                    {
                        Id = x.BlockFloor.Code,
                        LongName = x.BlockFloor.LongName,
                        ShortName = x.BlockFloor.ShortName
                    },
                    InventoryUnitNumber = x.InventoryUnitNumber,
                    LotUnitShareNumber = x.LotUnitShareNumber,
                    Address = new Address
                    {
                        Longitude = (double)(x.Longitude ?? 0),
                        Latitude = (double)(x.Latitude ?? 0)
                    },
                    FloorPlanUrls = (x.FloorPlan != null ? x.FloorPlan.ToList() : new List<string>()),
                    MilestonePercentage = new MilestonePercentage
                    {
                        BillingPercentage = x.MilestonePercentage.BillingPercentage ?? 0,
                        ContractorsName = x.MilestonePercentage.Contractors,
                        CurrentPOC = x.MilestonePercentage.PercentageCompletion ?? 0,
                        OngoingActivityMilestone = x.MilestonePercentage.OngoingMilestoneActivity,
                        TargetPOC = x.MilestonePercentage.TargetPercentageCompletion ?? 0,
                        TotalOverdue = x.MilestonePercentage?.PunchlistOverdueCount ?? 0,
                        TotalPending = x.MilestonePercentage?.PunchlistPendingCount ?? 0,
                        TotalPunchlist = x.MilestonePercentage?.PunchlistCount ?? 0,
                        Variance = x.MilestonePercentage.Variance
                    }
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


        public async Task<Unit> GetUnitByVendorAsync(string referenceObject, string username, string vendorCode)
        {
            try
            {
                var usr = await _userServices.GetUserAsync(username);
                if (usr == null)
                    return null;
                if (usr.RoleCode == null)
                    throw new ApplicationException("No defined Role for this User.");


                var x = (await _wcfServices.GetUnitByRefObjectByVendorAsync(username, referenceObject,vendorCode)).FirstOrDefault();
                var _encryptionProvider = RijndaelEncryptionProvider.GetInstance();

                var data = new Unit()
                {
                    EncryptReferenceObject = _encryptionProvider.Encrypt(x.ReferenceObject),
                    ReferenceObject = x.ReferenceObject,
                    ProjectCode = x.Project.Code,
                    Project = new Project
                    {
                        ProjectCode = x.Project.Code,
                        ShortName = x.Project.ShortName,
                        LongName = x.Project.LongName,
                        Address = new Address
                        {
                            Longitude = (double)(x.Project.Longitude ?? 0),
                            Latitude = (double)(x.Project.Latitude ?? 0)
                        }
                    },
                    PhaseBuildingCode = x.PhaseBuilding.Code,
                    PhaseBuilding = new PhaseBuilding
                    {
                        Id = x.PhaseBuilding.Code,
                        LongName = x.PhaseBuilding.LongName,
                        ShortName = x.PhaseBuilding.ShortName
                    },
                    BlockFloorCode = x.BlockFloor.Code,
                    BlockFloor = new BlockFloorCluster
                    {
                        Id = x.BlockFloor.Code,
                        LongName = x.BlockFloor.LongName,
                        ShortName = x.BlockFloor.ShortName
                    },
                    InventoryUnitNumber = x.InventoryUnitNumber,
                    LotUnitShareNumber = x.LotUnitShareNumber,
                    Address = new Address
                    {
                        Longitude = (double)(x.Longitude ?? 0),
                        Latitude = (double)(x.Latitude ?? 0)
                    },
                    FloorPlanUrls = (x.FloorPlan != null ? x.FloorPlan.ToList() : new List<string>()),
                    MilestonePercentage = new MilestonePercentage
                    {
                        BillingPercentage = x.MilestonePercentage.BillingPercentage ?? 0,
                        ContractorsName = x.MilestonePercentage.Contractors,
                        CurrentPOC = x.MilestonePercentage.PercentageCompletion ?? 0,
                        OngoingActivityMilestone = x.MilestonePercentage.OngoingMilestoneActivity,
                        TargetPOC = x.MilestonePercentage.TargetPercentageCompletion ?? 0,
                        TotalOverdue = x.MilestonePercentage?.PunchlistOverdueCount ?? 0,
                        TotalPending = x.MilestonePercentage?.PunchlistPendingCount ?? 0,
                        TotalPunchlist = x.MilestonePercentage?.PunchlistCount ?? 0,
                        Variance = x.MilestonePercentage.Variance
                    },
                    VendorCode = x.VendorCode
                };


                return data;
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


        public async Task<IEnumerable<Unit>> GetUnitsByUserAsync(string username, string keyword = "")
        {
            try
            {
                var usr = await _userServices.GetUserAsync(username);
                if (usr == null)
                    throw new ApplicationException("User does not exists");
                if (usr.RoleCode == null)
                    throw new ApplicationException("No defined Role for this User.");


                var units = (await _wcfServices.GetNewUnitByUserAsync(username)); 

                var data = units.Select(u => new Unit()
                {
                    ReferenceObject = u.ReferenceObject,
                    ProjectCode = u.Project.Code,
                    Project = new Project
                    {
                        ProjectCode = u.Project.Code,
                        ShortName = u.Project.ShortName,
                        LongName = u.Project.LongName
                    },
                    PhaseBuildingCode = u.PhaseBuilding.Code,
                    PhaseBuilding = new PhaseBuilding
                    {
                        Id = u.PhaseBuilding.Code,
                        LongName = u.PhaseBuilding.LongName,
                        ShortName = u.PhaseBuilding.ShortName
                    },
                    BlockFloorCode = u.BlockFloor.Code,
                    BlockFloor = new BlockFloorCluster
                    {
                        Id = u.BlockFloor.Code,
                        LongName = u.BlockFloor.LongName,
                        ShortName = u.BlockFloor.ShortName
                    },
                    InventoryUnitNumber = u.InventoryUnitNumber,
                    LotUnitShareNumber = u.LotUnitShareNumber,
                    Address = new Address
                    {
                        Longitude = (double)(u.Longitude ?? 0),
                        Latitude = (double)(u.Latitude ?? 0)
                    },
                    MilestonePercentage = new MilestonePercentage
                    {
                        BillingPercentage = u.MilestonePercentage.BillingPercentage ?? 0,
                        ContractorsName = u.MilestonePercentage.Contractors,
                        CurrentPOC = u.MilestonePercentage.PercentageCompletion ?? 0,
                        OngoingActivityMilestone = u.MilestonePercentage.OngoingMilestoneActivity,
                        TargetPOC = u.MilestonePercentage.TargetPercentageCompletion ?? 0,
                        TotalOverdue = u.MilestonePercentage.PunchlistOverdueCount,
                        TotalPending = u.MilestonePercentage.PunchlistPendingCount,
                        TotalPunchlist = u.MilestonePercentage.PunchlistCount,
                        Variance = u.MilestonePercentage.Variance
                    }
                });

                return data;
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
        
        public async Task<IEnumerable<Unit>> GetUserunitsbyProjectAsync(string projectcode, string username)
        {
            try
            {
                var usr = await _userServices.GetUserAsync(username);
                if (usr == null)
                    throw new ApplicationException("User does not exists");
                if (usr.RoleCode == null)
                    throw new ApplicationException("No defined Role for this User.");

                var unitsproj = (await _wcfServices.GetNewUnitByProjectAsync(username, projectcode));
                var _encryptionProvider = RijndaelEncryptionProvider.GetInstance();

                var data = unitsproj.Select(unit => new Unit()
                {
                    EncryptReferenceObject = _encryptionProvider.Encrypt(unit.ReferenceObject),
                    ReferenceObject = unit.ReferenceObject,
                    ProjectCode = unit.Project.Code,
                    Project = new Project
                    {
                        ProjectCode = unit.Project.Code,
                        ShortName = unit.Project.ShortName,
                        LongName = unit.Project.LongName
                    },
                    PhaseBuildingCode = unit.PhaseBuilding.Code,
                    PhaseBuilding = new PhaseBuilding
                    {
                        Id = unit.PhaseBuilding.Code,
                        ShortName = unit.PhaseBuilding.ShortName,
                        LongName = unit.PhaseBuilding.LongName
                    },
                    BlockFloorCode = unit.BlockFloor.Code,
                    BlockFloor = new BlockFloorCluster
                    {
                        Id = unit.BlockFloor.Code,
                        ShortName = unit.BlockFloor.ShortName,
                        LongName = unit.BlockFloor.LongName
                    },
                    LotUnitShareNumber = unit.LotUnitShareNumber,
                    InventoryUnitNumber = unit.InventoryUnitNumber,
                    Address = new Address
                    {
                        Longitude = (double)(unit.Longitude ?? 0),
                        Latitude = (double)(unit.Latitude ?? 0)
                    },
                    MilestonePercentage = new MilestonePercentage
                    {
                        BillingPercentage = unit.MilestonePercentage.BillingPercentage ?? 0,
                        ContractorsName = unit.MilestonePercentage.Contractors,
                        CurrentPOC = unit.MilestonePercentage.PercentageCompletion ?? 0,
                        TargetPOC = unit.MilestonePercentage.TargetPercentageCompletion ?? 0,
                        TotalOverdue = unit.MilestonePercentage.PunchlistOverdueCount,
                        TotalPending = unit.MilestonePercentage.PunchlistPendingCount,
                        TotalPunchlist = unit.MilestonePercentage.PunchlistCount,
                        Variance = unit.MilestonePercentage.Variance,
                        OngoingActivityMilestone = unit.MilestonePercentage.OngoingMilestoneActivity
                    }
                });

                return data;
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


        private string PadNumbers(string input)
        {
            return Regex.Replace(input, "[0-9]+", match => match.Value.PadLeft(20, '0'));
        }
    }
}
