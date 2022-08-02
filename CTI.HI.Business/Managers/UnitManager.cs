using Core.Common.Contracts;
using CTI.HI.Business.Contracts;
using CTI.HI.Business.Entities;
using CTI.HI.Data.Contracts.Frebas;
//using CTI.HI.Data.Contracts.Hcom;
using CTI.HI.Data.Repository.Frebas;
//using CTI.HI.Data.Repository.Hcom;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Serilog;
namespace CTI.HI.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                   ConcurrencyMode = ConcurrencyMode.Multiple)]
    class UnitManager : ManagerBase, IUnitService
    {
        #region Constructors
        private IUserProjectRepository _userProjectRepo;
        private IProjectRepository _projectRepo;
        private IInventoryUnitRepository _unitRepo;
        private IConstructionMilestoneRepository _constructionRepo;
        private IUserRepository _userRepo;
        public UnitManager()
        {
            _projectRepo = new ProjectRepository();
            _unitRepo = new InventoryUnitRepository();
            _userProjectRepo = new UserProjectRepository();
            _constructionRepo = new ConstructionMilestoneRepository();
            _userRepo = new UserRepository();
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        public UnitManager(IDataRepositoryFactory dataRepositoryFactory) : this()
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;

        public UnitManager(IBusinessEngineFactory businessEngineFactory) : this()
        {
            _BusinessEngineFactory = businessEngineFactory;
        }

        public UnitManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory) : this()
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
        }
        #endregion

        //public async Task<IEnumerable<Project>> GetProjectByUserAsync(string username, string userRole, double latitude, double longitude)
        //{
        //    try
        //    {
        //        return await ExecuteFaultHandledOperation(async () =>
        //        {
        //            username = username.ToUpper();
        //            var listProject = new List<Project>();
        //            //var _Repo = _DataRepositoryFactory.GetDataRepository<IProjectRepository>();
        //            var _userProject = await _userProjectRepo.GetProjectAsync(username);
        //            var _ongoingConstructionUnit = (await _constructionRepo.GetUserConstructionMilestoneAsync(username, c => _userProject.Contains(c.ProjectCode))).Distinct().Select(c => c.ReferenceObject).ToArray();
        //            var _inspectionRadius = await _userRepo.GetUserInspectionRadius(userRole);
        //            var _internalPTGRefObjs = (await _unitRepo.FilterInternalPTGReferenceObjectsAsync(_ongoingConstructionUnit));
        //            var _nearByUnit = await _unitRepo.GetNearbyUnitAsync(userRole, _inspectionRadius, latitude, longitude, _internalPTGRefObjs);

        //            var _nearByProjects = _nearByUnit.Select(n => n.Project.Code).Distinct().ToArray();
        //            var _projectList = await _projectRepo.GetProjectAsync(p => _nearByProjects.Contains(p.Code));
        //            var _projectMetadata = await _projectRepo.GetProjectCoordinatesAsync(m => _nearByProjects.Contains(m.ProjectCode));



        //            foreach (var pro in _projectList)
        //            {
        //                var hasCoordinates = _projectMetadata.Where(c => c.ProjectCode == pro.Code).FirstOrDefault();
        //                if (hasCoordinates == null)
        //                    continue;

        //                var _metadata = _projectMetadata.Where(m => m.ProjectCode == pro.Code).FirstOrDefault();
        //                listProject.Add(new Project
        //                {
        //                    Code = pro.Code,
        //                    ShortName = pro.ShortName,
        //                    LongName = pro.LongName,
        //                    Longitude = _metadata?.Longitude ?? 0,
        //                    Latitude = _metadata?.Latitude ?? 0,
        //                    ImageUrl = _metadata?.ImageURL ?? ""//Path.GetFileName(_metadata?.ImageURL) ?? ""
        //                });
        //            }

        //            return listProject.OrderBy(p => p.LongName);


        //        });
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //}

        public async Task<IEnumerable<Project>> GetProjectByUserAsync(string username)
        {
            try
            {

                Log.Information("GetProjectByUserAsync by {user}", username);
                return await ExecuteFaultHandledOperation(async () =>
                {
                    username = username.ToUpper();
                    return await _projectRepo.GetProjectByUserAsync(username);


                });
               
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                Log.Error("Exception on {user}", username);
                Log.Error("Exception detail {ex}", ex);
                throw new NullReferenceException(errMsg);
            }
            catch (ApplicationException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                Log.Error("Exception on {user}", username);
                Log.Error("Exception detail {ex}", ex);
                throw new ApplicationException(errMsg);
            }
            catch (Exception ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                Log.Error("Exception on {user}", username);
                Log.Error("Exception detail {ex}", ex);
                throw new Exception(errMsg);
            }


        }




        //public async Task<IEnumerable<PhaseBuilding>> GetPhaseBuildingByProjectAsync(string username, string projectCode, string userRole, double latitude, double longitude)
        //{
        //    return await ExecuteFaultHandledOperation(async () =>
        //    {
        //        username = username.ToUpper();
        //        var listPhaseBuilding = new List<PhaseBuilding>();
        //        var _Repo = _DataRepositoryFactory.GetDataRepository<IPhaseBuildingRepository>();
        //        var _userProject = await _userProjectRepo.GetProjectAsync(username);

        //        var _ongoingConstructionUnit = (await _constructionRepo.GetUserConstructionMilestoneAsync(username, c => _userProject.Contains(c.ProjectCode) && c.ProjectCode == projectCode)).Select(c => c.ReferenceObject).ToArray();
        //        var _inspectionRadius = await _userRepo.GetUserInspectionRadius(userRole);
        //        var _internalPTGRefObjs = (await _unitRepo.FilterInternalPTGReferenceObjectsAsync(_ongoingConstructionUnit));
        //        var _nearByUnit = await _unitRepo.GetNearbyUnitAsync(userRole, _inspectionRadius, latitude, longitude, _internalPTGRefObjs);
        //        var _nearByPhase = _nearByUnit.Select(n => n.PhaseBuilding.Code).Distinct().ToArray();
        //        var _phaseList = await _Repo.GetAllPhaseBuildingAsync(p => _nearByPhase.Contains(p.Code));

        //        foreach (var phase in _phaseList)
        //        {
        //            listPhaseBuilding.Add(new PhaseBuilding
        //            {
        //                Code = phase.Code,
        //                ShortName = phase.ShortName,
        //                LongName = phase.LongName
        //            });
        //        };

        //        return listPhaseBuilding;
        //    });
        //}


        public async Task<IEnumerable<PhaseBuilding>> GetPhaseBuildingByProjectAsync(string username, string projectCode)
        {
            Log.Information("GetPhaseBuildingByProjectAsync by {user} on {Project} ", username, projectCode);
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    username = username.ToUpper();
                   
                    var _Repo = _DataRepositoryFactory.GetDataRepository<IPhaseBuildingRepository>();
                    
                    return await _Repo.GetAllPhaseBuildingAsync(username, projectCode);
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new Exception(errMsg);
                }
            });
        }




        //public async Task<IEnumerable<Block>> GetBlockByPhaseBuildingAsync(string username, string phaseBuildingCode, string userRole, double latitude, double longitude)
        //{
        //    return await ExecuteFaultHandledOperation(async () =>
        //    {
        //        username = username.ToUpper();
        //        var listBlock = new List<Block>();
        //        var _Repo = _DataRepositoryFactory.GetDataRepository<IBlockFloorClusterRepository>();
        //        //var x = await _Repo.GetPhaseBuildingBlockFloorClusterAsync(phaseBuildingCode);
        //        var _userProject = await _userProjectRepo.GetProjectAsync(username);
        //        var _ongoingConstructionUnit = (await _constructionRepo.GetUserConstructionMilestoneAsync(username, c => _userProject.Contains(c.ProjectCode) && c.PhaseCode == phaseBuildingCode)).Select(c => c.ReferenceObject).ToArray();
        //        var _inspectionRadius = await _userRepo.GetUserInspectionRadius(userRole);
        //        var _internalPTGRefObjs = (await _unitRepo.FilterInternalPTGReferenceObjectsAsync(_ongoingConstructionUnit));
        //        var _nearByUnit = await _unitRepo.GetNearbyUnitAsync(userRole, _inspectionRadius, latitude, longitude, _internalPTGRefObjs);
        //        var _nearByBlock = _nearByUnit.Select(n => n.BlockFloor.Code).Distinct().ToArray();
        //        var _blockList = await _Repo.GetBlockFloorClusterAsync(p => _nearByBlock.Contains(p.Code));

        //        foreach (var block in _blockList)
        //        {
        //            listBlock.Add(new Block
        //            {
        //                Code = block.Code,
        //                ShortName = block.ShortName,
        //                LongName = block.LongName
        //            });
        //        };
        //        return listBlock;
        //    });
        //}

        public async Task<IEnumerable<Block>> GetBlockByPhaseBuildingAsync(string username, string phaseBuildingCode)
        {
            Log.Information("GetBlockByPhaseBuildingAsync by {user} on {Project} ", username, phaseBuildingCode);
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    username = username.ToUpper();
                    var listBlock = new List<Block>();
                    
                    var _Repo = _DataRepositoryFactory.GetDataRepository<IBlockFloorClusterRepository>();
                    
                    return await _Repo.GetBlockFloorClusterAsync(username, phaseBuildingCode);
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new Exception(errMsg);
                }
            });
        }





        //public async Task<IEnumerable<Unit>> GetUnitByProjectAsync(string username, string projectCode, string userRole, double latitude, double longitude)
        //{
        //    return await ExecuteFaultHandledOperation(async () =>
        //    {
        //        username = username.ToUpper();
        //        var listUnit = new List<Unit>();
        //        var _Repo = _DataRepositoryFactory.GetDataRepository<IInventoryUnitRepository>();
        //        //var unitModels = await _Repo.GetUnitAsync(u => u.Project.Code == projectCode);

        //        var _userProject = (await _userProjectRepo.GetProjectAsync(username)).Where(p => p.Contains(projectCode));
        //        var _ongoingConstructionUnit = (await _constructionRepo.GetUserConstructionMilestoneAsync(username, c => _userProject.Contains(c.ProjectCode))).Select(c => c.ReferenceObject).ToArray();
        //        var _inspectionRadius = await _userRepo.GetUserInspectionRadius(userRole);
        //        var _internalPTGRefObjs = (await _unitRepo.FilterInternalPTGReferenceObjectsAsync(_ongoingConstructionUnit));

        //        var _nearByUnit = await _unitRepo.GetNearbyUnitAsync(userRole, _inspectionRadius, latitude, longitude, _internalPTGRefObjs);
        //        var _refObjs = _nearByUnit.Select(x => x.ReferenceObject).ToArray();
        //        var _unitPercentageTask = _unitRepo.GetUnitPhysicalConditionAsync(p => _refObjs.Contains(p.ReferenceObject) && p.PhysicalConditionCode == "00005"); //00005 = Percentage Completion
        //        var _unitMetadataTask = _unitRepo.GetUnitCoordinatesAsync(m => _refObjs.Contains(m.ReferenceObject));
        //        var _unitContractorTask = _unitRepo.GetUnitContractorAsync(_refObjs);
        //        var _unitPunchlistCountTask = _unitRepo.GetUnitMilestonePercentageAsync(_refObjs);
        //        var _unitMilestoneTask = _unitRepo.GetUnitMilestonePercentageAsync(_refObjs);

        //        Task.WaitAll(_unitPercentageTask, _unitMetadataTask, _unitContractorTask, _unitPunchlistCountTask, _unitMilestoneTask);
        //        var _unitPercentage = _unitPercentageTask.Result;
        //        var _unitMetadata = _unitMetadataTask.Result;
        //        var _unitContractor = _unitContractorTask.Result;
        //        var _unitPunchlistCount = _unitPunchlistCountTask.Result;
        //        var _unitMilestone = _unitMilestoneTask.Result;


        //        return (from unt in _nearByUnit
        //                join pus in _unitMilestone
        //                on unt.ReferenceObject equals pus.ReferenceObject
        //                into umil
        //                from untMil in umil.DefaultIfEmpty()
        //                join cnt in _unitContractor
        //                on unt.ReferenceObject equals cnt.ReferenceObject
        //                join pcd in _unitPercentage
        //                on unt.ReferenceObject equals pcd.ReferenceObject
        //                into up
        //                from untPcd in up.DefaultIfEmpty()
        //                join meta in _unitMetadata
        //                on unt.ReferenceObject equals meta.ReferenceObject
        //                into um
        //                from untMeta in um.DefaultIfEmpty()
        //                select new Unit
        //                {
        //                    ReferenceObject = unt.ReferenceObject,
        //                    Project = new Project
        //                    {
        //                        Code = unt.Project.Code,
        //                        LongName = unt.Project.LongName,
        //                        ShortName = unt.Project.ShortName
        //                    },
        //                    PhaseBuilding = new PhaseBuilding
        //                    {
        //                        Code = unt.PhaseBuilding.Code,
        //                        ShortName = unt.PhaseBuilding.ShortName,
        //                        LongName = unt.PhaseBuilding.LongName
        //                    },
        //                    BlockFloor = new Block
        //                    {
        //                        Code = unt.BlockFloor.Code,
        //                        ShortName = unt.BlockFloor.ShortName,
        //                        LongName = unt.BlockFloor.LongName
        //                    },
        //                    InventoryUnitNumber = unt.InventoryUnitNumber,
        //                    LotUnitShareNumber = unt.LotUnitShareNumber,
        //                    Longitude = untMeta?.Longitude ?? 0,
        //                    Latitude = untMeta?.Latitude ?? 0,
        //                    MilestonePercentage = new MilestonePercentage()
        //                    {
        //                        PercentageCompletion = untPcd?.Percentage ?? 0,
        //                        OngoingMilestoneActivity = "",
        //                        TargetPercentageCompletion = 100,
        //                        Variance = "",
        //                        BillingPercentage = 0,
        //                        Contractors = string.Join("|", cnt.Contractors.Select(x => x.Name).ToArray()),
        //                        PunchlistCount = untMil?.PunchlistCount ?? 0,
        //                        PunchlistOverdueCount = untMil?.PunchlistOverdueCount ?? 0,
        //                        PunchlistPendingCount = untMil?.PunchlistPendingCount ?? 0
        //                    }
        //                });
        //    });
        //}

        public async Task<IEnumerable<Unit>> GetNewUnitByProjectAsync(string username, string projectCode)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                username = username.ToUpper();
                try
                {
                    Log.Information("GetUnitByProjectAsync by User: {user} and Project Code: {projectCode}", username, projectCode);

                    string contractors = string.Empty;

                    var getunits = await _unitRepo.GetNewUnitByProject(username, projectCode);

                    if (getunits.Count() > 0)
                    {
                        var refObjects = getunits.Select(d => d.ReferenceObject).ToArray();

                        var data = await _unitRepo.GetUnitContractorDetailsAsync(username, refObjects);

                        getunits = getunits.GroupJoin(data,
                                unit => unit.ReferenceObject,
                                ctr => ctr.ReferenceObject,
                                (unit, ctr) =>
                                {
                                    unit.MilestonePercentage.Contractors =
                                  string.Join("|", ctr.SelectMany(c => c.Contractors).Select(a => a.Name).Distinct().ToArray()); return unit;
                                });

                        var percentageCount = await _unitRepo.GetNewUnitMilestonePercentageAsync(username, refObjects);

                        getunits = getunits.GroupJoin(percentageCount,
                                unit => unit.ReferenceObject,
                                prtc => prtc.ReferenceObject,
                                (unit, prtc) =>
                                {
                                    unit.MilestonePercentage.OngoingMilestoneActivity = "";
                                    unit.MilestonePercentage.TargetPercentageCompletion = 100;
                                    unit.MilestonePercentage.Variance = "";
                                    unit.MilestonePercentage.BillingPercentage = 0;
                                    unit.MilestonePercentage.PunchlistCount = prtc.FirstOrDefault()?.PunchlistCount ?? 0;
                                    unit.MilestonePercentage.PunchlistOverdueCount = prtc.FirstOrDefault()?.PunchlistOverdueCount ?? 0;
                                    unit.MilestonePercentage.PunchlistPendingCount = prtc.FirstOrDefault()?.PunchlistPendingCount ?? 0;
                                    return unit;
                                });

                        return getunits;
                    }
                    else
                    {
                        throw new NullReferenceException($"No unit found for project: {projectCode} and user: {username}");
                    }
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new Exception(errMsg);
                }
            });
        }







        public async Task<IEnumerable<Unit>> GetNewUnitByUserAsync(string username)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                username = username.ToUpper();
                try
                {
                    string contractors = string.Empty;

                    var getunits = await _unitRepo.GetNewUnitByUser(username);

                    if (getunits.Count() > 0)
                    {
                        var refObjects = getunits.Select(d => d.ReferenceObject).ToArray();

                        var data = await _unitRepo.GetUnitContractorDetailsAsync(username, refObjects);

                        getunits = getunits.GroupJoin(data,
                                unit => unit.ReferenceObject,
                                ctr => ctr.ReferenceObject,
                                (unit, ctr) =>
                                { unit.MilestonePercentage.Contractors = 
                                    string.Join("|", ctr.SelectMany(c => c.Contractors).Select(a => a.Name).Distinct().ToArray()); return unit; });

                        var percentageCount = await _unitRepo.GetNewUnitMilestonePercentageAsync(username, refObjects);

                        getunits = getunits.GroupJoin(percentageCount,
                                unit => unit.ReferenceObject,
                                prtc => prtc.ReferenceObject,
                                (unit, prtc) =>
                                {
                                    unit.MilestonePercentage.OngoingMilestoneActivity = "";
                                    unit.MilestonePercentage.TargetPercentageCompletion = 100;
                                    unit.MilestonePercentage.Variance = "";
                                    unit.MilestonePercentage.BillingPercentage = 0;
                                    unit.MilestonePercentage.PunchlistCount = prtc.FirstOrDefault()?.PunchlistCount ?? 0;
                                    unit.MilestonePercentage.PunchlistOverdueCount = prtc.FirstOrDefault()?.PunchlistOverdueCount ?? 0;
                                    unit.MilestonePercentage.PunchlistPendingCount = prtc.FirstOrDefault()?.PunchlistPendingCount ?? 0;
                                    return unit;
                                });

                        getunits = getunits.Select(x =>
                        {
                            x.FloorPlan = _unitRepo.GetInventoryFloorPlanAsync(x.ReferenceObject).Result.Select(f => Path.GetFileName(f.FilePath)).ToArray();
                            return x;
                        });

                        return getunits;
                    }
                    else
                    {
                        throw new NullReferenceException($"No unit found for user: {username}");
                    }
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new Exception(errMsg);
                }
            });
        }


        public async Task<IEnumerable<Unit>> GetUnitByUserAsync(string username, string userRole, double latitude, double longitude)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                username = username.ToUpper();
                //var listUnit = new List<Unit>();

                try
                {
                    var _Repo = _DataRepositoryFactory.GetDataRepository<IInventoryUnitRepository>();
                    var _userProject = await _userProjectRepo.GetProjectAsync(username);
                    var _ongoingConstructionUnit = (await _constructionRepo.GetUserConstructionMilestoneAsync(username, c => _userProject.Contains(c.ProjectCode))).Select(c => c.ReferenceObject).ToArray();
                    var _inspectionRadius = await _userRepo.GetUserInspectionRadius(userRole);
                    var _internalPTGRefObjs = (await _unitRepo.FilterInternalPTGReferenceObjectsAsync(_ongoingConstructionUnit));

                    var _nearByUnit = await _unitRepo.GetNearbyUnitAsync(userRole, _inspectionRadius, latitude, longitude, _internalPTGRefObjs);
                    var _nearByRef = _nearByUnit.Select(n => n.ReferenceObject).ToArray();
                    var _unitPercentageTask = _unitRepo.GetUnitPhysicalConditionAsync(p => _nearByRef.Contains(p.ReferenceObject) && p.PhysicalConditionCode == "00005"); //00005 = Percentage Completion
                    var _unitMetadataTask = _unitRepo.GetUnitCoordinatesAsync(m => _nearByRef.Contains(m.ReferenceObject));
                    var _unitContractorTask = _unitRepo.GetUnitContractorAsync(_nearByRef);
                    //var _unitPunchlistCountTask =  _unitRepo.GetUnitMilestonePercentageAsync(_nearByRef);
                    var _unitMilestoneTask = _unitRepo.GetUnitMilestonePercentageAsync(_nearByRef);

                    Task.WaitAll(_unitPercentageTask, _unitMetadataTask, _unitContractorTask, _unitMilestoneTask);
                    var _unitPercentage = _unitPercentageTask.Result;
                    var _unitMetadata = _unitMetadataTask.Result;
                    var _unitContractor = _unitContractorTask.Result;
                    //var _unitPunchlistCount = _unitPunchlistCountTask.Result;
                    var _unitMilestone = _unitMilestoneTask.Result;

                    var xxxx = (from unt in _nearByUnit
                                join cnt in _unitContractor
                                on unt.ReferenceObject equals cnt.ReferenceObject
                                join pcd in _unitPercentage
                                on unt.ReferenceObject equals pcd.ReferenceObject
                                into up
                                from untPcd in up.DefaultIfEmpty()
                                join pus in _unitMilestone
                                on unt.ReferenceObject equals pus.ReferenceObject
                                into umil
                                from untMil in umil.DefaultIfEmpty()
                                join meta in _unitMetadata
                                on unt.ReferenceObject equals meta.ReferenceObject
                                into um
                                from untMeta in um.DefaultIfEmpty()
                                select new Unit
                                {
                                    ReferenceObject = unt.ReferenceObject,
                                    Project = new Project
                                    {
                                        Code = unt.Project.Code,
                                        LongName = unt.Project.LongName,
                                        ShortName = unt.Project.ShortName
                                    },
                                    PhaseBuilding = new PhaseBuilding
                                    {
                                        Code = unt.PhaseBuilding.Code,
                                        ShortName = unt.PhaseBuilding.ShortName,
                                        LongName = unt.PhaseBuilding.LongName
                                    },
                                    BlockFloor = new Block
                                    {
                                        Code = unt.BlockFloor.Code,
                                        ShortName = unt.BlockFloor.ShortName,
                                        LongName = unt.BlockFloor.LongName
                                    },
                                    InventoryUnitNumber = unt.InventoryUnitNumber,
                                    LotUnitShareNumber = unt.LotUnitShareNumber,
                                    Longitude = untMeta?.Longitude ?? 0,
                                    Latitude = untMeta?.Latitude ?? 0,
                                    MilestonePercentage = new MilestonePercentage()
                                    {
                                        PercentageCompletion = untPcd?.Percentage ?? 0,
                                        OngoingMilestoneActivity = "",
                                        TargetPercentageCompletion = 100,
                                        Variance = "",
                                        BillingPercentage = 0,
                                        Contractors = string.Join("|", cnt.Contractors.Select(x => x.Name).ToArray()),
                                        PunchlistCount = untMil?.PunchlistCount ?? 0,
                                        PunchlistOverdueCount = untMil?.PunchlistOverdueCount ?? 0,
                                        PunchlistPendingCount = untMil?.PunchlistPendingCount ?? 0

                                    }
                                }).ToList();
                    return xxxx;
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new Exception(errMsg);
                }




            });
        }









        //public async Task<IEnumerable<Unit>> GetUnitByBlockAsync(string username, string blockCode, string userRole, double latitude, double longitude)
        //{
        //    return await ExecuteFaultHandledOperation(async () =>
        //    {
        //        username = username.ToUpper();
        //        try
        //        {


        //            var listUnit = new List<Unit>();
        //            var _userProject = await _userProjectRepo.GetProjectAsync(username);
        //            var _ongoingConstructionUnit = (await _constructionRepo.GetUserConstructionMilestoneAsync(username, c => _userProject.Contains(c.ProjectCode) && c.BlockCode == blockCode)).Select(c => c.ReferenceObject).ToArray();
        //            var _inspectionRadius = await _userRepo.GetUserInspectionRadius(userRole);
        //            var _internalPTGRefObjs = (await _unitRepo.FilterInternalPTGReferenceObjectsAsync(_ongoingConstructionUnit));

        //            var _nearByUnit = await _unitRepo.GetNearbyUnitAsync(userRole, _inspectionRadius, latitude, longitude, _internalPTGRefObjs);
        //            var _nearByRef = _nearByUnit.Select(n => n.ReferenceObject).ToArray();

        //            var _unitPercentageTask = _unitRepo.GetUnitPhysicalConditionAsync(p => _nearByRef.Contains(p.ReferenceObject) && p.PhysicalConditionCode == "00005"); //00005 = Percentage Completion
        //            var _unitMetadataTask = _unitRepo.GetUnitCoordinatesAsync(m => _nearByRef.Contains(m.ReferenceObject));
        //            var _unitContractorTask = _unitRepo.GetUnitContractorAsync(_nearByRef);
        //            //var _unitPunchlistCount = await _unitRepo.GetUnitMilestonePercentageAsync(_nearByRef);
        //            var _unitMilestoneTask = _unitRepo.GetUnitMilestonePercentageAsync(_nearByRef);

        //            Task.WaitAll(_unitPercentageTask, _unitMetadataTask, _unitContractorTask, _unitMilestoneTask);
        //            var _unitPercentage = _unitPercentageTask.Result;
        //            var _unitMetadata = _unitMetadataTask.Result;
        //            var _unitContractor = _unitContractorTask.Result;
        //            //var _unitPunchlistCount = _unitPunchlistCountTask.Result;
        //            var _unitMilestone = _unitMilestoneTask.Result;



        //            return (from unt in _nearByUnit
        //                    join pus in _unitMilestone
        //                    on unt.ReferenceObject equals pus.ReferenceObject
        //                    into umil
        //                    from untMil in umil.DefaultIfEmpty()
        //                    join cnt in _unitContractor
        //                    on unt.ReferenceObject equals cnt.ReferenceObject
        //                    join pcd in _unitPercentage
        //                    on unt.ReferenceObject equals pcd.ReferenceObject
        //                    into up
        //                    from untPcd in up.DefaultIfEmpty()
        //                    join meta in _unitMetadata
        //                    on unt.ReferenceObject equals meta.ReferenceObject
        //                    into um
        //                    from untMeta in um.DefaultIfEmpty()
        //                    select new Unit
        //                    {
        //                        ReferenceObject = unt.ReferenceObject,
        //                        Project = new Project
        //                        {
        //                            Code = unt.Project.Code,
        //                            LongName = unt.Project.LongName,
        //                            ShortName = unt.Project.ShortName
        //                        },
        //                        PhaseBuilding = new PhaseBuilding
        //                        {
        //                            Code = unt.PhaseBuilding.Code,
        //                            ShortName = unt.PhaseBuilding.ShortName,
        //                            LongName = unt.PhaseBuilding.LongName
        //                        },
        //                        BlockFloor = new Block
        //                        {
        //                            Code = unt.BlockFloor.Code,
        //                            ShortName = unt.BlockFloor.ShortName,
        //                            LongName = unt.BlockFloor.LongName
        //                        },
        //                        InventoryUnitNumber = unt.InventoryUnitNumber,
        //                        LotUnitShareNumber = unt.LotUnitShareNumber,
        //                        Longitude = untMeta?.Longitude ?? 0,
        //                        Latitude = untMeta?.Latitude ?? 0,
        //                        MilestonePercentage = new MilestonePercentage()
        //                        {
        //                            PercentageCompletion = untPcd?.Percentage ?? 0,
        //                            OngoingMilestoneActivity = "",
        //                            TargetPercentageCompletion = 100,
        //                            Variance = "",
        //                            BillingPercentage = 0,
        //                            Contractors = string.Join("|", cnt.Contractors.Select(x => x.Name).ToArray()),
        //                            PunchlistCount = untMil?.PunchlistCount ?? 0,
        //                            PunchlistOverdueCount = untMil?.PunchlistOverdueCount ?? 0,
        //                            PunchlistPendingCount = untMil?.PunchlistPendingCount ?? 0
        //                        },
        //                        FloorPlan = new string[] { }
        //                    });

        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }
        //    });
        //}

        public async Task<Unit> GetUnitAsync(string username, string userRole, double latitude, double longitude, string referenceObject)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                username = username.ToUpper();
                try
                {
                    var listUnit = new List<Unit>();
                    var _userProject = await _userProjectRepo.GetProjectAsync(username);
                    var _ongoingConstructionUnit = (await _constructionRepo.GetUserConstructionMilestoneAsync(username, c => c.ReferenceObject == referenceObject && _userProject.Contains(c.ProjectCode))).FirstOrDefault();
                    if (_ongoingConstructionUnit == null)
                        throw new Exception("Unit is not exist or you dont have access to it.");
                    var _inspectionRadius = await _userRepo.GetUserInspectionRadius(userRole);
                    var _internalPTGRefObjs = (await _unitRepo.FilterInternalPTGReferenceObjectsAsync(new string[] { _ongoingConstructionUnit.ReferenceObject })).FirstOrDefault();
                    var _nearByUnit = (await _unitRepo.GetNearbyUnitAsync(userRole, _inspectionRadius, latitude, longitude, new string[] { _internalPTGRefObjs })).FirstOrDefault();
                    //var _nearByRef = _nearByUnit.Select(n => n.ReferenceObject).ToArray();


                    var _unitPercentageTask = _unitRepo.GetUnitPhysicalConditionAsync(p => p.ReferenceObject == _nearByUnit.ReferenceObject && p.PhysicalConditionCode == "00005"); //00005 = Percentage Completion
                    var _unitMetadataTask = _unitRepo.GetUnitCoordinatesAsync(m => m.ReferenceObject == _nearByUnit.ReferenceObject);
                    var _unitContractorTask = _unitRepo.GetUnitContractorAsync(new string[] { _ongoingConstructionUnit.ReferenceObject });
                    var _unitPunchlistCountTask = _unitRepo.GetUnitMilestonePercentageAsync(new string[] { _ongoingConstructionUnit.ReferenceObject });
                    var _unitMilestoneTask = _unitRepo.GetUnitMilestonePercentageAsync(new string[] { _ongoingConstructionUnit.ReferenceObject });
                    var _floorPlanTask = _unitRepo.GetInventoryFloorPlanAsync(referenceObject);

                    Task.WaitAll(_unitPercentageTask, _unitMetadataTask, _unitContractorTask, _unitPunchlistCountTask, _unitMilestoneTask);
                    var _unitPercentage = (_unitPercentageTask.Result).FirstOrDefault();
                    var _unitMetadata = (_unitMetadataTask.Result).FirstOrDefault();
                    var _unitContractor = (_unitContractorTask.Result).FirstOrDefault();
                    var _unitPunchlistCount = (_unitPunchlistCountTask.Result).FirstOrDefault();
                    var _unitMilestone = (_unitMilestoneTask.Result).FirstOrDefault();
                    var _floorPlan = (_floorPlanTask.Result);


                    //var _unitPercentage = (await _unitRepo.GetUnitPhysicalConditionAsync(p => p.ReferenceObject == _nearByUnit.ReferenceObject && p.PhysicalConditionCode == "00005")).FirstOrDefault(); //00005 = Percentage Completion
                    //var _unitMetadata =( await _unitRepo.GetUnitCoordinatesAsync(m => m.ReferenceObject == _nearByUnit.ReferenceObject)).FirstOrDefault();
                    //var _unitContractor = (await _unitRepo.GetUnitContractorAsync(new string[] { _ongoingConstructionUnit.ReferenceObject })).FirstOrDefault();
                    //var _unitPunchlistCount = (await _unitRepo.GetUnitMilestonePercentageAsync(new string[] { _ongoingConstructionUnit.ReferenceObject })).FirstOrDefault();
                    //var _unitMilestone = (await _unitRepo.GetUnitMilestonePercentageAsync(new string[] { _ongoingConstructionUnit.ReferenceObject })).FirstOrDefault();
                    //var _floorPlan = await _unitRepo.GetInventoryFloorPlanAsync(referenceObject);

                    return new Unit
                    {
                        ReferenceObject = _nearByUnit.ReferenceObject,
                        Project = new Project
                        {
                            Code = _nearByUnit.Project.Code,
                            LongName = _nearByUnit.Project.LongName,
                            ShortName = _nearByUnit.Project.ShortName
                        },
                        PhaseBuilding = new PhaseBuilding
                        {
                            Code = _nearByUnit.PhaseBuilding.Code,
                            ShortName = _nearByUnit.PhaseBuilding.ShortName,
                            LongName = _nearByUnit.PhaseBuilding.LongName
                        },
                        BlockFloor = new Block
                        {
                            Code = _nearByUnit.BlockFloor.Code,
                            ShortName = _nearByUnit.BlockFloor.ShortName,
                            LongName = _nearByUnit.BlockFloor.LongName
                        },
                        InventoryUnitNumber = _nearByUnit.InventoryUnitNumber,
                        LotUnitShareNumber = _nearByUnit.LotUnitShareNumber,
                        Longitude = _unitMetadata?.Longitude ?? 0,
                        Latitude = _unitMetadata?.Latitude ?? 0,
                        MilestonePercentage = new MilestonePercentage()
                        {
                            PercentageCompletion = _unitPercentage?.Percentage ?? 0,
                            OngoingMilestoneActivity = "",
                            TargetPercentageCompletion = 100,
                            Variance = "",
                            BillingPercentage = 0,
                            Contractors = string.Join("|", _unitContractor.Contractors.Select(x => x.Name).ToArray()),
                            PunchlistCount = _unitMilestone?.PunchlistCount ?? 0,
                            PunchlistOverdueCount = _unitMilestone?.PunchlistOverdueCount ?? 0,
                            PunchlistPendingCount = _unitMilestone?.PunchlistPendingCount ?? 0
                        },
                        FloorPlan = _floorPlan.Select(f => Path.GetFileName(f.FilePath)).ToArray()
                    };
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new Exception(errMsg);
                }


            });
        }

        public async Task<string[]> GetUnitFloorPlan(string referenceObject)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                return (await _unitRepo.GetInventoryFloorPlanAsync(referenceObject))
                .Select(f => Path.GetFileName(f.FilePath))
                .ToArray();
            });
        }

        public async Task<IEnumerable<Unit>> GetUnitByReferenceObjects(string[] referenceObject)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    var _Repo = _DataRepositoryFactory.GetDataRepository<IInventoryUnitRepository>();
                    var units = await _Repo.GetUnitAsync(u => referenceObject.Contains(u.ReferenceObject));

                    var xxxx = (from unt in units
                                select new Unit
                                {
                                    ReferenceObject = unt.ReferenceObject,
                                    Project = new Project
                                    {
                                        Code = unt.Project.Code,
                                        LongName = unt.Project.LongName,
                                        ShortName = unt.Project.ShortName
                                    },
                                    PhaseBuilding = new PhaseBuilding
                                    {
                                        Code = unt.PhaseBuilding.Code,
                                        ShortName = unt.PhaseBuilding.ShortName,
                                        LongName = unt.PhaseBuilding.LongName
                                    },
                                    BlockFloor = new Block
                                    {
                                        Code = unt.BlockFloor.Code,
                                        ShortName = unt.BlockFloor.ShortName,
                                        LongName = unt.BlockFloor.LongName
                                    },
                                    InventoryUnitNumber = unt.InventoryUnitNumber,
                                    LotUnitShareNumber = unt.LotUnitShareNumber
                                }).ToList();
                    return xxxx;
                }
                catch (NullReferenceException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<IEnumerable<Unit>> GetUnitByBlockAsync(string username, string blockCode)
        {

            return await ExecuteFaultHandledOperation(async () =>
            {
                username = username.ToUpper();
                try
                {
                    string contractors = string.Empty;

                    var getunits = await _unitRepo.GetUnitByBlockAsync(username, blockCode);

                    if (getunits.Count() > 0)
                    {
                        var refObjects = getunits.Select(d => d.ReferenceObject).ToArray();

                        var data = await _unitRepo.GetUnitContractorDetailsAsync(username, refObjects);

                        //getunits = getunits.GroupJoin(data,
                        //        unit => unit.ReferenceObject,
                        //        ctr => ctr.ReferenceObject,
                        //        (unit, ctr) =>
                        //        {
                        //            unit.MilestonePercentage.Contractors =
                        //              string.Join("|", ctr.SelectMany(c => c.Contractors).Select(a => a.Name).Distinct().ToArray()); return unit;
                        //        });

                        var percentageCount = await _unitRepo.GetNewUnitMilestonePercentageAsync(username, refObjects);

                        getunits = getunits.GroupJoin(percentageCount,
                                unit => unit.ReferenceObject,
                                prtc => prtc.ReferenceObject,
                                (unit, prtc) =>
                                {
                                    unit.MilestonePercentage.OngoingMilestoneActivity = "";
                                    unit.MilestonePercentage.TargetPercentageCompletion = 100;
                                    unit.MilestonePercentage.Variance = "";
                                    unit.MilestonePercentage.BillingPercentage = 0;
                                    unit.MilestonePercentage.PunchlistCount = prtc.FirstOrDefault()?.PunchlistCount ?? 0;
                                    unit.MilestonePercentage.PunchlistOverdueCount = prtc.FirstOrDefault()?.PunchlistOverdueCount ?? 0;
                                    unit.MilestonePercentage.PunchlistPendingCount = prtc.FirstOrDefault()?.PunchlistPendingCount ?? 0;
                                    return unit;
                                });

                        getunits = getunits.Select(x =>
                        {
                            x.FloorPlan = _unitRepo.GetInventoryFloorPlanAsync(x.ReferenceObject).Result.Select(f => Path.GetFileName(f.FilePath)).ToArray();
                            return x;
                        });

                        return getunits;
                    }
                    else
                    {
                        throw new NullReferenceException($"No unit found for block: {blockCode} and user: {username}");
                    }
                }
                catch (NullReferenceException ex)
                {
                    throw new NullReferenceException(ErrorMessageUtil.GetFullExceptionMessage(ex));
                }
                catch (Exception ex)
                {
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
                }
            });
        }

        public async Task<IEnumerable<Unit>> GetUnitByRefObject(string username, string referenceObject)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                username = username.ToUpper();
                try
                {
                    string contractors = string.Empty;

                    var getunits = await _unitRepo.GetUnitByRefObject(username, referenceObject);


                    if (getunits.Count() > 0)
                    {
                        var refObjects = getunits.Select(d => d.ReferenceObject).ToArray();
                        
                        var data = await _unitRepo.GetUnitContractorDetailsAsync(username, refObjects);

                        getunits = getunits.Select(x =>
                        {
                            x.FloorPlan = _unitRepo.GetInventoryFloorPlanAsync(x.ReferenceObject).Result.Select(f => Path.GetFileName(f.FilePath)).ToArray();
                            return x;
                        });

                        getunits = getunits.GroupJoin(data,
                                unit => unit.ReferenceObject,
                                ctr => ctr.ReferenceObject,
                                (unit, ctr) =>
                                {
                                    unit.MilestonePercentage.Contractors =
                                      string.Join("|", ctr.SelectMany(c => c.Contractors).Select(a => a.Name).Distinct().ToArray()); return unit;
                                });

                        var percentageCount = await _unitRepo.GetNewUnitMilestonePercentageAsync(username, refObjects);

                        getunits = getunits.GroupJoin(percentageCount,
                                unit => unit.ReferenceObject,
                                prtc => prtc.ReferenceObject,
                                (unit, prtc) =>
                                {
                                    unit.MilestonePercentage.OngoingMilestoneActivity = "";
                                    unit.MilestonePercentage.TargetPercentageCompletion = 100;
                                    unit.MilestonePercentage.Variance = "";
                                    unit.MilestonePercentage.BillingPercentage = 0;
                                    unit.MilestonePercentage.PunchlistCount = prtc.FirstOrDefault()?.PunchlistCount ?? 0;
                                    unit.MilestonePercentage.PunchlistOverdueCount = prtc.FirstOrDefault()?.PunchlistOverdueCount ?? 0;
                                    unit.MilestonePercentage.PunchlistPendingCount = prtc.FirstOrDefault()?.PunchlistPendingCount ?? 0;
                                    return unit;
                                });

                        return getunits;

                    }
                    else
                    {
                        throw new Exception($"No unit found for reference object: {referenceObject} and user: {username}");
                    }

                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new Exception(errMsg);
                }
            });
        }

        public async Task<IEnumerable<Unit>> GetUnitByRefObjectByVendor(string username, string referenceObject, string vendorCode)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                username = username.ToUpper();
                try
                {
                    Log.Information("GetUnitByRefObjectByVendor with Username: {username}, Reference Object: {ReferenceObject}, and Vendor: {vendorCode} ", username, referenceObject, vendorCode);

                    string contractors = string.Empty;

                    var getunits = await _unitRepo.GetUnitByRefObjectByVendor(username, referenceObject, vendorCode);


                    if (getunits.Count() > 0)
                    {
                        var refObjects = getunits.Select(d => d.ReferenceObject).ToArray();

                        //var data = await _unitRepo.GetUnitContractorDetailsAsync(username, refObjects);

                        getunits = getunits.Select(x =>
                        {
                            x.FloorPlan = _unitRepo.GetInventoryFloorPlanAsync(x.ReferenceObject).Result.Select(f => Path.GetFileName(f.FilePath)).ToArray();
                            return x;
                        });

                        var percentageCount = await _unitRepo.GetNewUnitMilestonePercentageAsync(username, refObjects);

                        getunits = getunits.GroupJoin(percentageCount,
                                unit => unit.ReferenceObject,
                                prtc => prtc.ReferenceObject,
                                (unit, prtc) =>
                                {
                                    unit.MilestonePercentage.OngoingMilestoneActivity = "";
                                    unit.MilestonePercentage.TargetPercentageCompletion = 100;
                                    unit.MilestonePercentage.Variance = "";
                                    unit.MilestonePercentage.BillingPercentage = 0;
                                    unit.MilestonePercentage.PunchlistCount = prtc.FirstOrDefault()?.PunchlistCount ?? 0;
                                    unit.MilestonePercentage.PunchlistOverdueCount = prtc.FirstOrDefault()?.PunchlistOverdueCount ?? 0;
                                    unit.MilestonePercentage.PunchlistPendingCount = prtc.FirstOrDefault()?.PunchlistPendingCount ?? 0;
                                    return unit;
                                });

                        return getunits;

                    }
                    else
                    {
                        throw new Exception($"No unit found for reference object: {referenceObject}, user: {username} and vendor: {vendorCode}");
                    }

                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Exception on {user}", username);
                    Log.Error("Exception detail {ex}", ex);
                    throw new Exception(errMsg);
                }
            });
        }


    }
}
