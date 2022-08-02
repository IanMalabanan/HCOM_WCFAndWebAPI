using Core.Common.Contracts;
using CTI.HI.Business.BusinessEngines;
using CTI.HI.Business.BusinessEngines.BusinessEngineContracts;
using CTI.HI.Business.Contracts;
using CTI.HI.Business.Entities;
//using CTI.HI.Business.Entities;
using CTI.HI.Data.Contracts.Frebas;
using CTI.HI.Data.Contracts.Frebas.StoredProcedure;
using CTI.HI.Data.Repository.Frebas;
using CTI.HI.Data.Repository.Frebas.StoredProcedure;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
namespace CTI.HI.Business.Managers
{
    class MilestoneManager : ManagerBase, IMilestoneService
    {
        #region Constructors
        private IConstructionMilestoneRepository _MilestoneRepo;
        private IContractorRepository _ContractorRepo;
        private IPunchlistRepository _PunchlistRepo;
        private IUserProjectRepository _userProjectRepo;
        private IMilestoneEngine _MilestoneEngine;
        private IConstructionMilestoneRepository _constructionRepo;
        private IInventoryUnitRepository _unitRepo;
        private INotificationService _notifService;

        public MilestoneManager()
        {
            _MilestoneRepo = new ConstructionMilestoneRepository();
            _ContractorRepo = new ContractorRepository();
            _PunchlistRepo = new PunchlistRepository();
            _MilestoneEngine = new MilestoneEngine();
            _userProjectRepo = new UserProjectRepository();
            _constructionRepo = new ConstructionMilestoneRepository();
            _unitRepo = new InventoryUnitRepository();
            _notifService = new NotificationManager();
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        public MilestoneManager(IDataRepositoryFactory dataRepositoryFactory) : this()
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;

        public MilestoneManager(IBusinessEngineFactory businessEngineFactory) : this()
        {
            _BusinessEngineFactory = businessEngineFactory;
        }

        public MilestoneManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory) : this()
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
        }
        #endregion


        private async Task<IEnumerable<Punchlist>> GetMilestonePunchlists(int id)
        {
            var punchlist = await _PunchlistRepo.GetMilestonePunchlistsAsync(id).ConfigureAwait(false);

            Parallel.ForEach(punchlist, p => p.Comments = _PunchlistRepo.GetPunchListCommentsAsync(p.PunchListID).Result.ToList() ?? new List<Comment>());

            return punchlist;
        }

        private async Task<IEnumerable<Punchlist>> GetMilestonePunchlists(int[] id)
        {
            var punchlist = await _PunchlistRepo.GetMilestonePunchlistsAsync(id).ConfigureAwait(false);

            return punchlist;
        }

        private async Task<IEnumerable<ContractorRepresentative>> GetVendorRepresentativeAsync(string vendorcode)
        {
            try
            {
                return await _ContractorRepo.GetVendorRepresentativeAsync(vendorcode).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
            }
        }

        private async Task<IEnumerable<MilestoneAttachment>> GetConstructionMilestoneAttachment(int id)
        {
            try
            {
                return await _MilestoneRepo.GetConstructionMilestoneAttachmentAsync(id).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
            }
        }

        private async Task<IEnumerable<MilestoneAttachment>> GetConstructionMilestoneAttachment(int[] id)
        {
            return await _MilestoneRepo.GetConstructionMilestoneAttachmentAsync(id).ConfigureAwait(false);
        }

        private async Task<HCM.Business.Entities.InventoryUnitOTCContractorConstructionMilestonePercentage> GetMilestonesPOC(int id)
        {
            try
            {
                return (await _MilestoneRepo.GetMilestonesPOCAsync(p => p.MilestoneRecordNumber == id).ConfigureAwait(false)).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
            }
        }

        private async Task<IEnumerable<HCM.Business.Entities.InventoryUnitOTCContractorConstructionMilestonePercentage>> GetMilestonesPOC(int[] id)
        {
            return (await _MilestoneRepo.GetMilestonesPOCAsync(p => id.Contains(p.MilestoneRecordNumber)).ConfigureAwait(false));
        }



        public async Task<IEnumerable<Business.Entities.ConstructionMilestone>> GetMilestonesByUnitAsync(string username, string ReferenceObject)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    Log.Information("Milestone : GetMilestonesByUnitAsync by {user} in unit {unit}", username, ReferenceObject);
                    var svcMilestone = (await _MilestoneRepo.GetConstructionMilestoneByUnit(username, ReferenceObject))
                    .Select(x => new Business.Entities.ConstructionMilestone
                    {
                        Id = x.Id,
                        OTCNumber = x.OTCNumber,
                        ContractorNumber = x.ContractorNumber,
                        ManagingContractorID = x.ManagingContractorID,
                        ConstructionMilestoneCode = x.ConstructionMilestoneCode,
                        ConstructionMilestoneDescription = x.ConstructionMilestoneDescription,
                        PercentageCompletion = x.PercentageCompletion ?? 0,
                        PercentageReferenceNumber = x.PercentageReferenceNumber,
                        PONumber = x.PONumber,
                        Sequence = x.Sequence,
                        Weight = x.Weight,
                        OTCTypeCode = x.OTCTypeCode,
                        TradeCode = x.TradeCode,
                        TradeDescription = x.TradeDescription,
                        VendorCode = x.VendorCode,
                        ManagingContractorCode = x.ManagingContractorCode,
                        LoaContractNumber = x.LoaContractNumber,
                        ReferenceObject = x.ReferenceObject,
                        Contractor = new Contractor
                        {
                            Code = x.Contractor?.Code,
                            Name = x.Contractor?.Name
                        },
                        Representative = new ContractorRepresentative
                        {
                            ContactNumber = x.Representative?.ContactNumber,
                            ContractorCode = x.Representative?.ContractorCode,
                            Email = x.Representative?.Email,
                            Id = x.Representative?.Id ?? 0,
                            Name = x.Representative?.Name
                        }


                    }).Distinct().ToList();

                    if (svcMilestone.Count() > 0)
                    {
                        var reps = new List<ContractorRepresentative>();

                        var _Rep = svcMilestone
                        .Select(x => new ContractorRepresentative
                        {
                            ContractorCode = x.Representative?.ContractorCode,
                            Name = x.Representative?.Name,
                            Email = x.Representative?.Email,
                            ContactNumber = x.Representative?.ContactNumber,
                            Id = x.Representative?.Id ?? 0
                        }).ToList();

                        reps = _Rep.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                        var _filtered = svcMilestone.GroupBy(x => x.Id).Select(y => y.First());

                        var mlsids = _filtered.ToList().Select(p => p.Id).ToArray();

                        var punchlist = await GetMilestonePunchlists(mlsids);

                        var punids = punchlist.Select(p => p.PunchListID).ToArray();

                        var attmnts = await GetConstructionMilestoneAttachment(mlsids);

                        var poc = await GetMilestonesPOC(mlsids);

                        var puncmt = await _PunchlistRepo.GetPunchListCommentsAsync(punids, username);

                        punchlist = punchlist.GroupJoin(puncmt,
                            pl => pl.PunchListID,
                            cmt => cmt.PunchlistId,
                            (pl, cmt) => {
                                pl.Comments = cmt.ToList();
                                return pl;
                            });

                        _filtered = _filtered.GroupJoin(punchlist,
                            mil => mil.Id,
                            pl => pl.ConstructionMilestoneId,
                            (mil, pl) => { mil.Punchlists = pl; return mil; });

                        _filtered = _filtered.GroupJoin(attmnts,
                            mil => mil.Id,
                            attc => attc.ConstructionMilestoneId,
                            (mil, attc) => { mil.Attachments = attc; return mil; });

                        _filtered = _filtered.GroupJoin(poc,
                            mil => mil.Id,
                            poc1 => poc1.MilestoneRecordNumber,
                            (mil, poc1) =>
                            {
                                mil.PercentageCompletionQA = poc1.FirstOrDefault()?.QualityAssurancePercentage ?? 0; 
                                mil.PercentageCompletionEngineer = poc1.FirstOrDefault()?.ProjectEngineerPercentage ?? 0;
                                mil.PercentageCompletionContractor = poc1.FirstOrDefault()?.ContractorPercentage ?? 0;
                                return mil;
                            });

                        _filtered = _filtered.Select(x=> { x.Representatives = reps; return x; });

                        return _filtered.ToList();
                    }
                    else
                    {
                        return new List<Business.Entities.ConstructionMilestone>();
                    }
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : GetMilestonesByUnitAsync by {user} in unit {unit}, error found: {error}", username, ReferenceObject, ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : GetMilestonesByUnitAsync by {user} in unit {unit}, error found: {error}", username, ReferenceObject, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : GetMilestonesByUnitAsync by {user} in unit {unit}, error found: {error}", username, ReferenceObject, ex);
                    throw new Exception(errMsg);
                }
            });
        }

        public async Task<IEnumerable<Business.Entities.ConstructionMilestone>> GetMilestonesByUnitByVendorAsync(string username, string ReferenceObject, string vendorCode)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    Log.Information("Milestone : GetConstructionMilestoneByUnitByVendor by {user} in unit {unit} with vendor {vendorCode}", username, ReferenceObject);
                    var svcMilestone = (await _MilestoneRepo.GetConstructionMilestoneByUnitByVendor(username, ReferenceObject, vendorCode))
                    .Select(x => new Business.Entities.ConstructionMilestone
                    {
                        Id = x.Id,
                        OTCNumber = x.OTCNumber,
                        ContractorNumber = x.ContractorNumber,
                        ManagingContractorID = x.ManagingContractorID,
                        ConstructionMilestoneCode = x.ConstructionMilestoneCode,
                        ConstructionMilestoneDescription = x.ConstructionMilestoneDescription,
                        PercentageCompletion = x.PercentageCompletion ?? 0,
                        PercentageReferenceNumber = x.PercentageReferenceNumber,
                        PONumber = x.PONumber,
                        Sequence = x.Sequence,
                        Weight = x.Weight,
                        OTCTypeCode = x.OTCTypeCode,
                        TradeCode = x.TradeCode,
                        TradeDescription = x.TradeDescription,
                        VendorCode = x.VendorCode,
                        ManagingContractorCode = x.ManagingContractorCode,
                        LoaContractNumber = x.LoaContractNumber,
                        ReferenceObject = x.ReferenceObject,
                        Contractor = new Contractor
                        {
                            Code = x.Contractor?.Code,
                            Name = x.Contractor?.Name
                        },
                        Representative = new ContractorRepresentative
                        {
                            ContactNumber = x.Representative?.ContactNumber,
                            ContractorCode = x.Representative?.ContractorCode,
                            Email = x.Representative?.Email,
                            Id = x.Representative?.Id ?? 0,
                            Name = x.Representative?.Name
                        }


                    }).Distinct().ToList();

                    if (svcMilestone.Count() > 0)
                    {
                        var reps = new List<ContractorRepresentative>();

                        var _Rep = svcMilestone
                        .Select(x => new ContractorRepresentative
                        {
                            ContractorCode = x.Representative?.ContractorCode,
                            Name = x.Representative?.Name,
                            Email = x.Representative?.Email,
                            ContactNumber = x.Representative?.ContactNumber,
                            Id = x.Representative?.Id ?? 0
                        }).ToList();

                        reps = _Rep.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                        var _filtered = svcMilestone.GroupBy(x => x.Id).Select(y => y.First());

                        var mlsids = _filtered.ToList().Select(p => p.Id).ToArray();

                        var punchlist = await GetMilestonePunchlists(mlsids);

                        var punids = punchlist.Select(p => p.PunchListID).ToArray();

                        var attmnts = await GetConstructionMilestoneAttachment(mlsids);

                        var poc = await GetMilestonesPOC(mlsids);

                        var puncmt = await _PunchlistRepo.GetPunchListCommentsAsync(punids, username);

                        punchlist = punchlist.GroupJoin(puncmt,
                            pl => pl.PunchListID,
                            cmt => cmt.PunchlistId,
                            (pl, cmt) => {
                                pl.Comments = cmt.ToList();
                                return pl;
                            });

                        _filtered = _filtered.GroupJoin(punchlist,
                            mil => mil.Id,
                            pl => pl.ConstructionMilestoneId,
                            (mil, pl) => { mil.Punchlists = pl; return mil; });

                        _filtered = _filtered.GroupJoin(attmnts,
                            mil => mil.Id,
                            attc => attc.ConstructionMilestoneId,
                            (mil, attc) => { mil.Attachments = attc; return mil; });

                        _filtered = _filtered.GroupJoin(poc,
                            mil => mil.Id,
                            poc1 => poc1.MilestoneRecordNumber,
                            (mil, poc1) =>
                            {
                                mil.PercentageCompletionQA = poc1.FirstOrDefault()?.QualityAssurancePercentage ?? 0;
                                mil.PercentageCompletionEngineer = poc1.FirstOrDefault()?.ProjectEngineerPercentage ?? 0;
                                mil.PercentageCompletionContractor = poc1.FirstOrDefault()?.ContractorPercentage ?? 0;
                                return mil;
                            });

                        _filtered = _filtered.Select(x => { x.Representatives = reps; return x; });

                        return _filtered.ToList();
                    }
                    else
                    {
                        return new List<Business.Entities.ConstructionMilestone>();
                    }
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : GetConstructionMilestoneByUnitByVendor by {user} in unit {unit} by {vendor}, error found: {error}", username, ReferenceObject, vendorCode, ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : GetConstructionMilestoneByUnitByVendor by {user} in unit {unit} by {vendor}, error found: {error}", username, ReferenceObject, vendorCode, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : GetConstructionMilestoneByUnitByVendor by {user} in unit {unit} by {vendor}, error found: {error}", username, ReferenceObject, vendorCode, ex);
                    throw new Exception(errMsg);
                }
            });
        }

        public async Task<IEnumerable<Business.Entities.ConstructionMilestone>> GetMilestonesAsync(string otcNumber, string contractorNumber, int ManagingContractorID)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {

                    Log.Information("Milestone : GetMilestonesAsync with {otc} ", otcNumber);
                    var milestoneList = new List<Business.Entities.ConstructionMilestone>();
                    var svcMilestone = await _MilestoneRepo.GetConstructionMilestoneAsync(m => m.ConstructionMilestone.OTCNumber == otcNumber
                                                                                        && m.ConstructionMilestone.ContractorNumber == contractorNumber
                                                                                        && m.ConstructionMilestone.ManagingContractorID == ManagingContractorID);
                    foreach (var mil in svcMilestone)
                    {
                        milestoneList.Add(new Business.Entities.ConstructionMilestone
                        {
                            OTCNumber = mil.ConstructionMilestone.OTCNumber,
                            ContractorNumber = mil.ConstructionMilestone.ContractorNumber,
                            ManagingContractorID = mil.ConstructionMilestone.ManagingContractorID,
                            ConstructionMilestoneCode = mil.MilestoneDetails.Code,
                            ConstructionMilestoneDescription = mil.MilestoneDetails.Description,
                            PercentageCompletion = mil.ConstructionMilestone.Percentage,
                            PercentageReferenceNumber = mil.ConstructionMilestone.PercentageReferenceNumber,
                            PONumber = mil.ConstructionMilestone.PONumber,
                            Sequence = mil.ConstructionMilestone.Sequence,
                            Weight = mil.ConstructionMilestone.Weight,
                            OTCTypeCode = mil.OTCTypeCode,
                            TradeCode = mil.TradeCode,
                            TradeDescription = mil.TradeDescription,
                            Representatives = await _ContractorRepo.GetVendorRepresentativeAsync(mil.VendorCode),
                            Punchlists = await _PunchlistRepo.GetMilestonePunchlistsAsync(mil.ConstructionMilestone.RecordNumber),
                            ManagingContractorCode = mil.ManagingContractorCode,
                            LoaContractNumber = mil.LoaContractNumber
                        });
                    };



                    return milestoneList;
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : GetMilestonesAsync with OTC Number: {otc}, Contract Number: {contractorNumber}, Managing Contractor ID: {ManagingContractorID}, Error : {error} ", otcNumber, contractorNumber, ManagingContractorID, ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : GetMilestonesAsync with OTC Number: {otc}, Contract Number: {contractorNumber}, Managing Contractor ID: {ManagingContractorID}, Error : {error} ", otcNumber, contractorNumber, ManagingContractorID, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : GetMilestonesAsync with OTC Number: {otc}, Contract Number: {contractorNumber}, Managing Contractor ID: {ManagingContractorID}, Error : {error} ", otcNumber,contractorNumber, ManagingContractorID, ex);
                    throw new Exception(errMsg);
                }
                
             
            });
        }

        public async Task<Business.Entities.ConstructionMilestone> GetMilestoneAsync(string otcNumber, string contractorNumber, int ManagingContractorID, string milestoneCode)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {

                try
                {

                    Log.Information("Milestone : GetMilestonesAsync with {otc} ", otcNumber);
                    var svcMilestone = (await _MilestoneRepo.GetConstructionMilestoneAsync(m => m.ConstructionMilestone.OTCNumber == otcNumber
                                                                                  && m.ConstructionMilestone.ContractorNumber == contractorNumber
                                                                                  && m.ConstructionMilestone.ManagingContractorID == ManagingContractorID
                                                                                  && m.ConstructionMilestone.ConstructionMilestoneCode == milestoneCode)).FirstOrDefault();
                    var x = new Business.Entities.ConstructionMilestone
                    {
                        OTCNumber = svcMilestone.ConstructionMilestone.OTCNumber,
                        ContractorNumber = svcMilestone.ConstructionMilestone.ContractorNumber,
                        ManagingContractorID = svcMilestone.ConstructionMilestone.ManagingContractorID,
                        ConstructionMilestoneCode = svcMilestone.MilestoneDetails.Code,
                        ConstructionMilestoneDescription = svcMilestone.MilestoneDetails.Description,
                        PercentageCompletion = svcMilestone.ConstructionMilestone.Percentage,
                        PercentageReferenceNumber = svcMilestone.ConstructionMilestone.PercentageReferenceNumber,
                        PONumber = svcMilestone.ConstructionMilestone.PONumber,
                        Sequence = svcMilestone.ConstructionMilestone.Sequence,
                        Weight = svcMilestone.ConstructionMilestone.Weight,
                        OTCTypeCode = svcMilestone.OTCTypeCode,
                        TradeCode = svcMilestone.TradeCode,
                        TradeDescription = svcMilestone.TradeDescription,
                        Representatives = await _ContractorRepo.GetVendorRepresentativeAsync(svcMilestone.VendorCode),
                        Punchlists = await _PunchlistRepo.GetMilestonePunchlistsAsync(svcMilestone.ConstructionMilestone.RecordNumber),
                        ManagingContractorCode = svcMilestone.ManagingContractorCode,
                        LoaContractNumber = svcMilestone.LoaContractNumber
                    };

                    return x;
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : GetMilestonesAsync with OTC Number: {otc}, Contract Number: {contractorNumber}, Managing Contractor ID: {ManagingContractorID}, and Milestone Code: {milestoneCode}, Error : {error} ", otcNumber, contractorNumber, ManagingContractorID, milestoneCode, ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : GetMilestonesAsync with OTC Number: {otc}, Contract Number: {contractorNumber}, Managing Contractor ID: {ManagingContractorID}, and Milestone Code: {milestoneCode}, Error : {error} ", otcNumber, contractorNumber, ManagingContractorID, milestoneCode, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : GetMilestonesAsync with OTC Number: {otc}, Contract Number: {contractorNumber}, Managing Contractor ID: {ManagingContractorID}, and Milestone Code: {milestoneCode}, Error : {error} ", otcNumber, contractorNumber, ManagingContractorID, milestoneCode, ex);
                    throw new Exception(errMsg);
                }
            });
        }

        public async Task<Business.Entities.ConstructionMilestone> GetMilestoneByIdAsync(string username, int constructionMilestoneId)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    Log.Information("Milestone : GetMilestoneByIdAsync with Construction Milestone ID: {constructionMilestoneId} ", constructionMilestoneId);
                    var svcMilestone = (await _MilestoneRepo.GetConstructionMilestoneByID(username, constructionMilestoneId)).Distinct().ToList().FirstOrDefault();

                    if (svcMilestone != null)
                    {
                        //var pun = await GetMilestonePunchlists(constructionMilestoneId);
                        var images = await GetConstructionMilestoneAttachment(constructionMilestoneId);
                        var poc = await GetMilestonesPOC(constructionMilestoneId);

                        var lstrep = new List<ContractorRepresentative>();

                        var _Rep = (await _MilestoneRepo.GetConstructionMilestoneByID(username, constructionMilestoneId)).Distinct().ToList()
                                    .Select(d => new ContractorRepresentative
                                    {
                                        ContractorCode = d.Representative.ContractorCode,
                                        Name = d.Representative.Name,
                                        Email = d.Representative.Email,
                                        ContactNumber = d.Representative.ContactNumber,
                                        Id = d.Representative.Id
                                    }).ToList();

                        lstrep = _Rep.GroupBy(d => d.Id).Select(y => y.First()).ToList();

                        var x = new ConstructionMilestone
                        {
                            Id = svcMilestone.Id,
                            VendorCode = svcMilestone.VendorCode,
                            OTCNumber = svcMilestone.OTCNumber,
                            ContractorNumber = svcMilestone.ContractorNumber,
                            ManagingContractorID = svcMilestone.ManagingContractorID,
                            ConstructionMilestoneCode = svcMilestone.ConstructionMilestoneCode,
                            ConstructionMilestoneDescription = svcMilestone.ConstructionMilestoneDescription,
                            PercentageCompletion = svcMilestone.PercentageCompletion ?? 0,
                            PercentageReferenceNumber = svcMilestone.PercentageReferenceNumber,
                            PONumber = svcMilestone.PONumber,
                            Sequence = svcMilestone.Sequence,
                            Weight = svcMilestone.Weight,
                            OTCTypeCode = svcMilestone.OTCTypeCode,
                            TradeCode = svcMilestone.TradeCode,
                            TradeDescription = svcMilestone.TradeDescription,
                            Representatives = lstrep,
                            //Punchlists = pun ?? new List<Punchlist>(),
                            ManagingContractorCode = svcMilestone.ManagingContractorCode,
                            Contractor = new Business.Entities.Contractor
                            {
                                Code = svcMilestone.Contractor?.Code,
                                Name = svcMilestone.Contractor?.Name
                            },
                            Attachments = images ?? new List<MilestoneAttachment>(),
                            LoaContractNumber = svcMilestone.LoaContractNumber,
                            PercentageCompletionContractor = poc?.ContractorPercentage ?? 0,
                            PercentageCompletionEngineer = poc?.ProjectEngineerPercentage ?? 0,
                            PercentageCompletionQA = poc?.QualityAssurancePercentage ?? 0
                        };

                        var punchlist = await GetMilestonePunchlists(new int[] { constructionMilestoneId });

                        x.Punchlists = punchlist ?? new List<Punchlist>();

                        var punids = x.Punchlists.Select(p => p.PunchListID).ToArray();

                        var puncmt = await _PunchlistRepo.GetPunchListCommentsAsync(punids, username);

                        x.Punchlists = x.Punchlists.GroupJoin(puncmt,
                            pl => pl.PunchListID,
                            cmt => cmt.PunchlistId,
                            (pl, cmt) => {
                                pl.Comments = cmt.ToList();
                                return pl;
                            });


                        return x;
                    }
                    else
                    {
                        return new Business.Entities.ConstructionMilestone();
                    }
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : GetMilestoneByIdAsync with Construction Milestone ID: {constructionMilestoneId}, Error : {error} ", constructionMilestoneId, ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : GetMilestoneByIdAsync with Construction Milestone ID: {constructionMilestoneId}, Error : {error} ", constructionMilestoneId, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : GetMilestoneByIdAsync with Construction Milestone ID: {constructionMilestoneId}, Error : {error} ", constructionMilestoneId, ex);
                    throw new Exception(errMsg);
                }
            }); 
        }

         

        public async Task<bool> UpdateMilestonePercentageAsync(string userName,
                                                            int ConstructionMilestoneId,
                                                            string otcNumber,
                                                            string contractorNumber,
                                                            int managingContractorId,
                                                            decimal newPercentage,
                                                            string milestoneCode,
                                                            IEnumerable<MilestoneAttachment> attachments,
                                                            DateTime? dateVisited = null)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {

                try
                {
                    Log.Information("Milestone : UpdateMilestonePercentageAsync by {user}, with Milestone Id {MilestoneId}, OTC Number: {otcNumber}, Contractor Number: {contractorNumber}, Managing Contractor Id: {managingContractorId}, and Milestone Code: {milestoneCode}", userName, ConstructionMilestoneId, otcNumber, contractorNumber, managingContractorId, milestoneCode);

                    await _MilestoneEngine.isValidPercentageAsync(ConstructionMilestoneId, newPercentage, userName);
                    await _MilestoneEngine.isAllowedToChangePercentageAsync(ConstructionMilestoneId, newPercentage, userName);

                    var result = await _MilestoneRepo.UpdateMilestonePercentageAsync(userName,
                                                                                ConstructionMilestoneId,
                                                                                otcNumber,
                                                                                contractorNumber,
                                                                                managingContractorId,
                                                                                newPercentage,
                                                                                milestoneCode,
                                                                                attachments,
                                                                                dateVisited);


                    // Notify if 100%
                    //if (newPercentage >= 100)
                    //{
                    //    var _milestoneData = (await _MilestoneRepo.GetConstructionMilestoneAsync(m => m.ConstructionMilestone.RecordNumber == ConstructionMilestoneId)).Where(m => m.DPIUser == userName).FirstOrDefault();
                    //    var _unitData = (await _unitRepo.GetUnitByOTCNumberAsync(otcNumber));
                    //    var _loaData = (await _constructionRepo.GetLOADetails(l => l.OTCNumber == otcNumber)).FirstOrDefault();

                    //    //Call Send Email
                    //  //  await _notifService.SendCompletePercentageNotification(_milestoneData, _unitData, _loaData);
                    //}

                    return result;
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : UpdateMilestonePercentageAsync by {user}, with Milestone Id {MilestoneId}, OTC Number: {otcNumber}, Contractor Number: {contractorNumber}, Managing Contractor Id: {managingContractorId}, and Milestone Code: {milestoneCode},  Error : {error} ", userName, ConstructionMilestoneId, otcNumber, contractorNumber, managingContractorId, milestoneCode, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("Milestone : UpdateMilestonePercentageAsync by {user}, with Milestone Id {MilestoneId}, OTC Number: {otcNumber}, Contractor Number: {contractorNumber}, Managing Contractor Id: {managingContractorId}, and Milestone Code: {milestoneCode},  Error : {error} ", userName, ConstructionMilestoneId, otcNumber, contractorNumber, managingContractorId, milestoneCode, ex);
                    throw new Exception(errMsg);
                }

            });
        }
        public async Task<IEnumerable<ConstructionMilestone>> GetMilestoneByProjectAsync(string username, string projectCode) 
        {
            try
            {
                Log.Information("Milestone : GetMilestoneByProjectAsync by {user}, with Project Code: {projectCode}", username, projectCode);

                var VendorsInfo = new List<ARM.Business.Entities.VendorMasterData>();
                var _constructionMilestone = new List<ConstructionMilestone>();

                var svcMilestone = (await _MilestoneRepo.GetConstructionMilestoneByProject(username, projectCode))
                    .Select(x => new Business.Entities.ConstructionMilestone
                    {
                        Id = x.Id,
                        OTCNumber = x.OTCNumber,
                        ContractorNumber = x.ContractorNumber,
                        ManagingContractorID = x.ManagingContractorID,
                        ConstructionMilestoneCode = x.ConstructionMilestoneCode,
                        ConstructionMilestoneDescription = x.ConstructionMilestoneDescription,
                        PercentageCompletion = x.PercentageCompletion ?? 0,
                        PercentageReferenceNumber = x.PercentageReferenceNumber,
                        PONumber = x.PONumber,
                        Sequence = x.Sequence,
                        Weight = x.Weight,
                        OTCTypeCode = x.OTCTypeCode,
                        TradeCode = x.TradeCode,
                        TradeDescription = x.TradeDescription,
                        VendorCode = x.VendorCode,
                        ManagingContractorCode = x.ManagingContractorCode,
                        LoaContractNumber = x.LoaContractNumber,
                        ReferenceObject = x.ReferenceObject,
                        Contractor = new Contractor
                        {
                            Code = x.Contractor?.Code,
                            Name = x.Contractor?.Name
                        },
                        Representative = new ContractorRepresentative
                        {
                            ContactNumber = x.Representative?.ContactNumber,
                            ContractorCode = x.Representative?.ContractorCode,
                            Email = x.Representative?.Email,
                            Id = x.Representative?.Id ?? 0,
                            Name = x.Representative?.Name
                        }
                    }).Distinct().ToList();

                if (svcMilestone.Count() > 0)
                {
                    var reps = new List<ContractorRepresentative>();

                    var _Rep = svcMilestone
                    .Select(x => new ContractorRepresentative
                    {
                        ContractorCode = x.Representative?.ContractorCode,
                        Name = x.Representative?.Name,
                        Email = x.Representative?.Email,
                        ContactNumber = x.Representative?.ContactNumber,
                        Id = x.Representative?.Id ?? 0
                    }).ToList();

                    reps = _Rep.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                    var _filtered = svcMilestone.GroupBy(x => x.Id).Select(y => y.First());


                    //_filtered = _filtered.Select(x => { x.Representatives = reps; return x; });
                    Parallel.ForEach(_filtered, m =>
                    {
                        m.Representatives = reps;
                    });

                    return _filtered;
                }
                else
                {
                    return new List<ConstructionMilestone>();
                }
            }
            catch (NullReferenceException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                Log.Error("Milestone : GetMilestoneByProjectAsync by {user} with Project Code: {projectCode},  Error : {error} ", username, projectCode, ex);
                throw new ApplicationException(errMsg);
            }
            catch (ApplicationException ex)
            {
                string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                Log.Error("Milestone : GetMilestoneByProjectAsync by {user} with Project Code: {projectCode},  Error : {error} ", username, projectCode, ex);
                throw new ApplicationException(errMsg);
            }
            catch (Exception ex)
            {
                Log.Error("Milestone : GetMilestoneByProjectAsync by {user},  Error : {error} ", username, ex);
                throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
            }
        }

        public async Task<IEnumerable<MilestoneAttachment>> GetMilestoneImagesAsync()
        {
            try
            {
                return (await _MilestoneRepo.GetMilestoneImagesAsync()).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Milestone : GetMilestoneImagesAsync,  Error : {error} ", ex);
                throw new Exception(ErrorMessageUtil.GetFullExceptionMessage(ex));
            }
        }
    }
}
