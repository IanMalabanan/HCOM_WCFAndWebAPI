using Core.Common.Contracts;
using CTI.HI.Business.BusinessEngines;
using CTI.HI.Business.BusinessEngines.BusinessEngineContracts;
using CTI.HI.Business.Contracts;
using CTI.HI.Business.Entities;
using CTI.HI.Data.Contracts.Frebas;
using CTI.HI.Data.Repository.Frebas;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity.SqlServer;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business.Managers
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
    ConcurrencyMode = ConcurrencyMode.Multiple)]
    class PunchlistManager : ManagerBase, IPunchlistService
    {
        #region Constructors 

        private IUserRepository _UserRepo;
        private IPunchlistEngine _PunchlistEngine;

        private IUserProjectRepository _userProjectRepo;
        private IConstructionMilestoneRepository _MilestoneRepo;
        private IConstructionMilestoneRepository _constructionRepo;
        private IInventoryUnitRepository _unitRepo;


        public PunchlistManager()
        {
            _PunchlistEngine = new PunchlistEngine();
            _UserRepo = new UserRepository();
            _userProjectRepo = new UserProjectRepository();
            _MilestoneRepo = new ConstructionMilestoneRepository();
            _constructionRepo = new ConstructionMilestoneRepository();
            _unitRepo = new InventoryUnitRepository();
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        public PunchlistManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        #endregion

        public async Task<Punchlist> GetPunchlistAsync(int PunchlistId)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    Log.Information("Punchlist : GetPunchlistsAsync by Punchlist Id: {PunchlistId}", PunchlistId);

                    var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();

                    var punchlist = await _Repo.GetMilestonePunchlistAsync(PunchlistId);

                    punchlist.Comments = (await _Repo.GetPunchListCommentsAsync(punchlist.PunchListID)).ToList() ?? new List<Comment>();

                    Parallel.ForEach(punchlist.Comments, cmt => cmt.CreatedBy = new User
                    {
                        Id = _UserRepo.GetUserInfoBaseOnDateAsync(cmt.CreatedByUsername, cmt.CreatedDate).Result.Id,
                        FullName = _UserRepo.GetUserInfoBaseOnDateAsync(cmt.CreatedByUsername, cmt.CreatedDate).Result.FullName,
                        Email = _UserRepo.GetUserInfoBaseOnDateAsync(cmt.CreatedByUsername, cmt.CreatedDate).Result.Email,
                        RoleCode = _UserRepo.GetUserInfoBaseOnDateAsync(cmt.CreatedByUsername, cmt.CreatedDate).Result.RoleCode,
                        IsActive = _UserRepo.GetUserInfoBaseOnDateAsync(cmt.CreatedByUsername, cmt.CreatedDate).Result.IsActive,
                        InspectionRadius = _UserRepo.GetUserInfoBaseOnDateAsync(cmt.CreatedByUsername, cmt.CreatedDate).Result.InspectionRadius
                    });

                    return punchlist;
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Punchlist : GetPunchlistsAsync by Punchlist Id: {PunchlistId}, Error found: {ex}", PunchlistId, ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Punchlist : GetPunchlistsAsync by Punchlist Id: {PunchlistId}, Error found: {ex}", PunchlistId, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Punchlist : GetPunchlistsAsync by Punchlist Id: {PunchlistId}, Error found: {ex}", PunchlistId, ex);
                    throw new Exception(errMsg);
                }
            });

        }

        public async Task<IEnumerable<Punchlist>> GetPunchlistsAsync(int ConstructionMilestoneId)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    Log.Information("Punchlist : GetPunchlistsAsync by Construction Milestone Id: {ConstructionMilestoneId}", ConstructionMilestoneId);
                    
                    var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();

                    var punchlist = await _Repo.GetMilestonePunchlistsAsync(ConstructionMilestoneId);

                    punchlist = punchlist.Select(p => { p.Comments = _Repo.GetPunchListCommentsAsync(p.PunchListID).Result.ToList() ?? new List<Comment>(); return p; });

                    return punchlist;
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Punchlist : GetPunchlistsAsync by Construction Milestone Id: {ConstructionMilestoneId}, Error found: {ex}", ConstructionMilestoneId, ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Punchlist : GetPunchlistsAsync by Construction Milestone Id: {ConstructionMilestoneId}, Error found: {ex}", ConstructionMilestoneId, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Punchlist : GetPunchlistsAsync by Construction Milestone Id: {ConstructionMilestoneId}, Error found: {ex}", ConstructionMilestoneId, ex);
                    throw new Exception(errMsg);
                }
            });
        }

        public async Task<IEnumerable<PunchlistDescription>> GetPunchlistDescriptionsAsync()
        {
            return await ExecuteFaultHandledOperation(async () =>
           {
               try
               {
                   var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();
                   return await _Repo.GetPunchlistAsync();
               }
               catch (Exception ex)
               {
                   throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
               }
           });

        }

        public async Task<IEnumerable<PunchlistGroup>> GetPunchlistGroupAsync()
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();
                    return await _Repo.GetPunchlistGroupAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
                }
            });
        }

        public async Task<IEnumerable<PunchlistCategory>> GetPunchlistCategoryAsync()
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();
                    var x = (await _Repo.GetPunchlistCategoryAsync());
                    var xx = (await _Repo.GetPunchlistCategoryAsync())
                                .Select(c => new PunchlistCategory
                                {
                                    Code = c.Code,
                                    Name = c.Description,
                                    isCompliance = c.IsNonCompliance
                                });
                    return xx;
                }
                catch (Exception ex)
                {
                    throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
                }
            });
        }
        public async Task<IEnumerable<PunchlistSubCategory>> GetPunchlistSubCategoryAsync()
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();
                    return (await _Repo.GetPunchListSubCategoryAsync())
                                .Select(sc => new CTI.HI.Business.Entities.PunchlistSubCategory
                                {
                                    Code = sc.Code,
                                    Name = sc.Description,
                                    OfficeDays = sc.NoDaysRectification
                                });
                }
                catch (Exception ex)
                {
                    throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
                }
            });
        }

        public async Task<IEnumerable<NonComplianceTo>> GetNonComplianceToAsync()
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();
                    return (await _Repo.GetPunchlistNonComplianceAsync())
                        .Select(nc => new CTI.HI.Business.Entities.NonComplianceTo
                        {
                            Code = nc.Code,
                            Name = nc.Description
                        });
                }
                catch (Exception ex)
                {
                    throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
                }
            });
        }

        public async Task<IEnumerable<PunchlistLocation>> GetPunchlistLocationAync()
        {
            try
            {
                return await ExecuteFaultHandledOperation(async () =>
                {
                    var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();
                    return (await _Repo.GetPunchListLocationAsync())
                            .Select(l => new CTI.HI.Business.Entities.PunchlistLocation
                            {
                                Code = l.Code,
                                Name = l.Description
                            });
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
            }
        }
        public async Task<IEnumerable<CostImpact>> GetCostImpactAsync()
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();
                    return (await _Repo.GetPunchlistCostImpactAsync())
                                .Select(ci => new CTI.HI.Business.Entities.CostImpact
                                {
                                    Code = ci.Code,
                                    Name = ci.Description
                                });
                }
                catch (Exception ex)
                {
                    throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
                }
            });
        }

        public async Task<IEnumerable<ScheduleImpact>> GetScheduleImpactAsync()
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();
                    return (await _Repo.GetPunchListScheduleImpactAsync())
                                .Select(si => new ScheduleImpact
                                {
                                    Code = si.Code,
                                    Name = si.Description
                                });
                }
                catch (Exception ex)
                {
                    throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
                }
            });
        }

        public async Task<IEnumerable<PunchlistStatus>> GetPunchlistStatusAsync()
        {
            try
            {
                return await ExecuteFaultHandledOperation(async () =>
                {
                    var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();
                    return (await _Repo.GetPunchListStatusAsync())
                            .Select(s => new PunchlistStatus
                            {
                                Code = s.Code,
                                Name = s.Description
                            });
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
            }
        }
        public async Task<IEnumerable<PunchlistStatus>> GetPunchlistStatusByRoleAsync(string roleCode)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();
                    var _puchlistStatuses = new List<PunchlistStatus>();
                    var _statuses = (await _Repo.GetPunchListStatusAsync())
                            .Select(s => new
                            {
                                Code = s.Code,
                                Name = s.Description,
                                NextStatus = s.NextStatusCode,
                                ProjectRoleCode = s.ProjectRoleCode
                            });

                    foreach (var itm in _statuses)
                    {
                        //check if the nxt status is empty then continue loop
                        if (string.IsNullOrEmpty(itm.NextStatus)) continue;

                        //loop the nxt statuses
                        foreach (var nxtStatus in itm.NextStatus.Split(','))
                        {
                            //check if the role is authorized to the next status if not then continue for
                            if (_statuses.Where(s => s.Code == nxtStatus && s.ProjectRoleCode.Contains(roleCode)).FirstOrDefault() == null) continue;

                            var PunNxtStatus = (await _Repo.GetPunchListStatusAsync(s => s.Code == nxtStatus)).FirstOrDefault();
                            _puchlistStatuses.Add(new PunchlistStatus
                            {
                                Code = PunNxtStatus.Code,
                                Name = PunNxtStatus.Description,
                                CurrentStatus = itm.Code
                            });
                        }
                    }

                    return _puchlistStatuses;
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetPunchlistStatusByRoleAsync with Role Code: {roleCode}, Error found {ex}", roleCode, ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetPunchlistStatusByRoleAsync with Role Code: {roleCode}, Error found {ex}", roleCode, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetPunchlistStatusByRoleAsync with Role Code: {roleCode}, Error found {ex}", roleCode, ex);
                    throw new Exception(errMsg);
                }
            });
        }

        public async Task<IEnumerable<Punchlist>> GetPunchlistbyProject(string username, string projectCode)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    Log.Information("GetPunchlistbyProject with Username: {username}, and Project: {projectCode}", username, projectCode);

                    //get Ongoing Units By Project
                    var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();

                    //time start
                    var _timeStartCallMilestoneByProject = DateTime.Now;
                    var _punchlist = await _Repo.GetMilestonePunchlistsByProjectAsync(username, projectCode);
                    //time end
                    var _timeEndCallMilestoneByProject = DateTime.Now;
                    Debug.WriteLine($"{"Total time of calling Punchlist By Project  : " + (_timeEndCallMilestoneByProject - _timeStartCallMilestoneByProject).TotalSeconds }");

                    if (_punchlist == null)
                        return _punchlist;

                    //time start
                    var _timeStartCallPunchlistComments = DateTime.Now;


                    var punids = _punchlist.Select(p => p.PunchListID).ToArray();

                    var puncmt = await _Repo.GetPunchListCommentsAsync(punids, username);

                    _punchlist = _punchlist.GroupJoin(puncmt,
                                pl => pl.PunchListID,
                                cmt => cmt.PunchlistId,
                                (pl, cmt) =>
                                {
                                    pl.Comments = cmt.ToList();
                                    return pl;
                                });

                    //Parallel.ForEach(_punchlist, p =>
                    //    p.Comments = _Repo.GetPunchListCommentsAsync(p.PunchListID).Result.ToList()
                    //?? new List<Comment>()
                    //);


                    //time end
                    var _timeEndCallPunchlistComments = DateTime.Now;
                    Debug.WriteLine($"{"Total time of calling Punchlist Comments : " + (_timeEndCallPunchlistComments - _timeStartCallPunchlistComments).TotalSeconds }");

                    var _a = _punchlist.ToList();
                    return _punchlist;
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetPunchlistbyProject with Username: {username}, and Project: {projectCode}, Error found {ex}", username, projectCode, ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetPunchlistbyProject with Username: {username}, and Project: {projectCode}, Error found {ex}", username, projectCode, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetPunchlistbyProject with Username: {username}, and Project: {projectCode}, Error found {ex}", username, projectCode, ex);
                    throw new Exception(errMsg);
                }
            });
        }






        public async Task<IEnumerable<Punchlist>> GetPunchlistsByUnitAsync(string username, string ReferenceObject)
        {
            try
            {
                Log.Information("GetPunchlistsByUnitByVendorAsync with Username: {username}, and Reference Object: {ReferenceObject}", username, ReferenceObject);

                var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();

                    var _punchlist = await _Repo.GetMilestonePunchlistsByUnitAsync(username, ReferenceObject);

                    var punids = _punchlist.Select(p => p.PunchListID).ToArray();

                    var puncmt = await _Repo.GetPunchListCommentsAsync(punids, username);

                    _punchlist = _punchlist.GroupJoin(puncmt,
                            pl => pl.PunchListID,
                            cmt => cmt.PunchlistId,
                            (pl, cmt) =>
                            {
                                pl.Comments = cmt.ToList();
                                return pl;
                            });

                    return _punchlist;

            }
            catch (NullReferenceException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                Log.Error("GetPunchlistsByUnitByVendorAsync with Username: {username}, and Reference  Object: {ReferenceObject}, Error found {ex}", username, ReferenceObject, ex);
                throw new NullReferenceException(errMsg);
            }
            catch (ApplicationException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                Log.Error("GetPunchlistsByUnitByVendorAsync with Username: {username}, and Reference Object: {ReferenceObject}, Error found {ex}", username, ReferenceObject, ex);
                throw new ApplicationException(errMsg);
            }
            catch (Exception ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                Log.Error("GetPunchlistsByUnitByVendorAsync with Username: {username}, and Reference Object: {ReferenceObject}, Error found {ex}", username, ReferenceObject, ex);
                throw new Exception(errMsg);
            }
        }

        public async Task<IEnumerable<Punchlist>> GetPunchlistsByUnitByVendorAsync(string username, string ReferenceObject, string vendorCode)
        {
            try
            {
                Log.Information("GetPunchlistsByUnitByVendorAsync with Username: {username}, Reference Object: {ReferenceObject}, and Vendor: {vendorCode} ", username, ReferenceObject, vendorCode);
                var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();

                    var _punchlist = await _Repo.GetMilestonePunchlistsByUnitByVendorAsync(username, ReferenceObject, vendorCode);

                    var punids = _punchlist.Select(p => p.PunchListID).ToArray();

                    var puncmt = await _Repo.GetPunchListCommentsAsync(punids, username);

                    _punchlist = _punchlist.GroupJoin(puncmt,
                                pl => pl.PunchListID,
                                cmt => cmt.PunchlistId,
                                (pl, cmt) =>
                                {
                                    pl.Comments = cmt.ToList();
                                    return pl;
                                });

                    return _punchlist;

            }
            catch (NullReferenceException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                Log.Error("GetPunchlistsByUnitByVendorAsync with Username: {username}, Reference Object: {ReferenceObject}, and Vendor: {vendorCode}, Error found {ex}", username, ReferenceObject, vendorCode, ex);
                throw new NullReferenceException(errMsg);
            }
            catch (ApplicationException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                Log.Error("GetPunchlistsByUnitByVendorAsync with Username: {username}, Reference Object: {ReferenceObject}, and Vendor: {vendorCode}, Error found {ex}", username, ReferenceObject, vendorCode, ex);
                throw new ApplicationException(errMsg);
            }
            catch (Exception ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                Log.Error("GetPunchlistsByUnitByVendorAsync with Username: {username}, Reference Object: {ReferenceObject}, and Vendor: {vendorCode}, Error found {ex}", username, ReferenceObject, vendorCode, ex);
                throw new Exception(errMsg);
            }
        }






        public Task<IEnumerable<PunchlistComment>> GetPunchlistCommentAsync(int punchlistid)
        {
            throw new NotImplementedException();
        }

        public async Task<int> SavePunchlistAsync(string username, Punchlist model)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    Log.Information("Punchlist : SavePunchlistAsync by User: {username}", username);
                    switch (model.PunchListStatus.Trim())
                    {
                        case "VOID":
                            await _PunchlistEngine.canVoidPunchlistAsync(username, model);
                            break;
                        case "CLOS":
                            await _PunchlistEngine.canClosePunchlistAsync(username, model.PunchListID, model.PunchListStatus);
                            break;
                        default:
                            break;
                    }

                    await _PunchlistEngine.canCreatePunchlistAsync(username, model.ConstructionMilestoneId, model);
                    await _PunchlistEngine.validatePunchlistInputsAsync(model);

                    var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();
                    return await _Repo.SavePunchlistAsync(username, model);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Punchlist : SavePunchlistAsync by User: {username}, Error details: {ex}", username, ex);
                    throw new Exception(errMsg);
                }
            });
        }

        public async Task<IEnumerable<PunchlistAttachment>> GetMilestonePunchlistsImagesAsync()
        {
            try
            {
                var _Repo = _DataRepositoryFactory.GetDataRepository<IPunchlistRepository>();

                return (await _Repo.GetMilestonePunchlistsImagesAsync()).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Punchlist : GetMilestonePunchlistsImagesAsync,  Error : {error} ", ex);
                throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
            }
        }
    }
}
