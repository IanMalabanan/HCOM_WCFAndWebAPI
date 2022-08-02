using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using CTI.HCM.Business.Entities;
using CTI.HCM.Business.Entities.Models;
using CTI.HI.Business.Entities;
using CTI.HI.Data.Constant;
//using CTI.HI.Business.Entities;
using CTI.HI.Data.Context;
using CTI.HI.Data.Contracts.Frebas;
using CTI.HI.Data.Repository.Frebas.StoredProcedure;

namespace CTI.HI.Data.Repository.Frebas
{
    [Export(typeof(IConstructionMilestoneRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ConstructionMilestoneRepository :
        DataRepositoryFrebasBase<
            InventoryUnitOTCContractorConstructionMilestone>, IConstructionMilestoneRepository
    {
        private IUserRepository _usrRepo;
        private DateTime _DateToday = DateTime.Now.Date;
        private int _NotifRecentDays = 7;
        public ConstructionMilestoneRepository()
        {
            _usrRepo = new UserRepository();
        }
        public async Task<IEnumerable<ConstructionMilestoneModel>> GetConstructionMilestoneAsync(string ReferenceObject)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var xx = await (from otc in cntxt.InventoryUnitOTC.Where(o => o.ReferenceObject == ReferenceObject).Distinct()
                                    join mct in cntxt.InventoryUnitOTCManagingContractor.Distinct()
                                    on otc.OTCNumber equals mct.OTCNumber
                                    join rco in cntxt.InventoryUnitOTCContractorAwarded.Distinct()
                                    on new
                                    {
                                        otcNumber = mct.OTCNumber,
                                        managingContractor = mct.ManagingContractor,
                                        managingContractorID = mct.ID
                                    } equals
                                    new
                                    {
                                        otcNumber = rco.OTCNumber,
                                        managingContractor = rco.ManagingContractor,
                                        managingContractorID = rco.ManagingContractorID
                                    }
                                    join uct in cntxt.InventoryUnitOTCContractor.Distinct()
                                    on new
                                    {
                                        otcNumber = rco.OTCNumber,
                                        managingContractor = rco.ManagingContractor,
                                        managingContractorID = rco.ManagingContractorID,
                                        contractorNumber = rco.ContractorNumber,
                                        vendorCode = rco.VendorCode
                                    } equals
                                     new
                                     {
                                         otcNumber = uct.OTCNumber,
                                         managingContractor = uct.ManagingContractor,
                                         managingContractorID = uct.ManagingContractorID,
                                         contractorNumber = uct.ContractorNumber,
                                         vendorCode = uct.VendorCode
                                     }
                                    join loa in cntxt.InventoryUnitOTCContractorAwardedLOA.Distinct()
                                    on uct.IsIssuedNTPReferenceNumber equals loa.IsIssuedNTPReferenceNumber
                                    join mlc in cntxt.InventoryUnitOTCContractorConstructionMilestone
                                    on new
                                    {
                                        otcNumber = uct.OTCNumber,
                                        managingContractorID = uct.ManagingContractorID,
                                        contractorNumber = uct.ContractorNumber
                                    } equals
                                    new
                                    {
                                        otcNumber = mlc.OTCNumber,
                                        managingContractorID = mlc.ManagingContractorID,
                                        contractorNumber = mlc.ContractorNumber
                                    }
                                    join inv in cntxt.InventoryUnit.Distinct()
                                    on otc.ReferenceObject equals inv.ReferenceObject
                                    join ott in cntxt.OTCType.Distinct()
                                    on otc.OTCTypeCode equals ott.Code
                                    join mas in cntxt.InventoryUnitOTCQualified.Distinct()
                                    on otc.OTCNumber equals mas.OTCNumber
                                    join cmi in cntxt.ConstructionMilestone.Distinct()
                                    on mlc.ConstructionMilestoneCode equals cmi.Code
                                    join acc in cntxt.VWUserAuthorization
                                    on otc.ReferenceObject equals acc.ReferenceObject
                                    join ctd in cntxt.MilestoneTradeTypes
                                    on cmi.ConstructionTradeTypeCode equals ctd.Code
                                    into trdd
                                    from trd in trdd.DefaultIfEmpty()
                                    where uct.IsIssuedNTP == true &&
                                     //(mlc.Percentage ?? 0) < 100 &&
                                     //(uct.PercentageCompletionVendor ?? 0) < 100 &&
                                     uct.IsHold == false //&&
                                                         //acc.UserName == 
                                    select new
                                    {
                                        Id = mlc.RecordNumber,
                                        OTCNumber = mlc.OTCNumber,
                                        ContractorNumber = mlc.ContractorNumber,
                                        ManagingContractorID = mlc.ManagingContractorID,
                                        ConstructionMilestoneCode = cmi.Code,
                                        ConstructionMilestoneDescription = cmi.Description,
                                        PercentageCompletion = mlc.Percentage,
                                        PercentageReferenceNumber = mlc.PercentageReferenceNumber,
                                        PONumber = mlc.PONumber,
                                        Sequence = mlc.Sequence,
                                        Weight = mlc.Weight,
                                        OTCTypeCode = otc.OTCTypeCode,
                                        TradeCode = trd.Code,
                                        TradeDescription = trd.Description,
                                        VendorCode = uct.VendorCode,
                                        ManagingContractorCode = uct.ManagingContractor,
                                        LoaContractNumber = loa.LOAContractNumber,
                                        projectCode = inv.ProjectCode,
                                        phaseCode = inv.PhaseBuildingCode,
                                        blockCode = inv.BlockFloorClusterCode,
                                        ReferenceObject = inv.ReferenceObject,
                                        PercentageTagDate = mlc.PercentageTagDate,
                                        DPIUser = acc.UserName
                                    }).ToListAsync();

                    return xx.Select(x => new ConstructionMilestoneModel
                    {
                        ConstructionMilestone = new InventoryUnitOTCContractorConstructionMilestone
                        {
                            RecordNumber = x.Id,
                            OTCNumber = x.OTCNumber,
                            ContractorNumber = x.ContractorNumber,
                            ManagingContractorID = x.ManagingContractorID,
                            Percentage = x.PercentageCompletion,
                            PercentageReferenceNumber = x.PercentageReferenceNumber,
                            PONumber = x.PONumber,
                            Sequence = x.Sequence,
                            Weight = x.Weight,
                            ConstructionMilestoneCode = x.ConstructionMilestoneCode,
                            PercentageTagDate = x.PercentageTagDate
                        },
                        MilestoneDetails = new HCM.Business.Entities.ConstructionMilestone
                        {
                            Code = x.ConstructionMilestoneCode,
                            Description = x.ConstructionMilestoneDescription,
                        },
                        OTCTypeCode = x.OTCTypeCode,
                        TradeCode = x.TradeCode,
                        TradeDescription = x.TradeDescription,
                        VendorCode = x.VendorCode,
                        ManagingContractorCode = x.ManagingContractorCode,
                        LoaContractNumber = x.LoaContractNumber,
                        ReferenceObject = x.ReferenceObject,
                        Unit = new IMM.Business.Entities.Model.UnitModel
                        {
                            ReferenceObject = x.ReferenceObject,
                            Project = new IMM.Business.Entities.Project
                            {
                                Code = x.projectCode
                            }
                        },
                        ProjectCode = x.projectCode,
                        PhaseCode = x.phaseCode,
                        BlockCode = x.blockCode,
                        DPIUser = x.DPIUser
                    }).ToList();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }

            }
        }


        public async Task<IEnumerable<Business.Entities.ConstructionMilestone>> GetConstructionMilestoneByUnit(string username, string referenceObject)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    var data = await cntxt.VWUnitMilestonesWithLoaAndTradeTypes.Where(x => x.UserName == username 
                                && x.ReferenceObject == referenceObject).Distinct()
                                .Select(x => new Business.Entities.ConstructionMilestone
                                {
                                    Id = x.ConstructionMilestoneID,
                                    OTCNumber = x.ConstructionMilestoneOTCNumber,
                                    ContractorNumber = x.ConstructionMilestoneContractorNumber,
                                    ManagingContractorID = x.ConstructionMilestoneManagingContractorID,
                                    ConstructionMilestoneCode = x.ConstructionMilestoneCode,
                                    ConstructionMilestoneDescription = x.MilestoneDescription,
                                    PercentageCompletion = x.ConstructionMilestonePercentageCompletion ?? 0,
                                    PercentageReferenceNumber = x.ConstructionMilestoneMilestonePercentageReferenceNumber,
                                    PONumber = x.ConstructionMilestoneMilestonePONumber,
                                    Sequence = x.ConstructionMilestoneMilestoneSequence,
                                    Weight = x.ConstructionMilestoneMilestoneWeight ?? 0,
                                    OTCTypeCode = x.OTCTypeCode,
                                    TradeCode = x.TradeTypeCode,
                                    TradeDescription = x.TradeType,
                                    VendorCode = x.VendorCode,
                                    ManagingContractorCode = x.ManagingContractorCode,
                                    LoaContractNumber = x.LOAContractNumber,
                                    ReferenceObject = x.ReferenceObject,
                                    Contractor = new Contractor
                                    {
                                        Code = x.VendorCode,
                                        Name = x.VendorName
                                    },
                                    Representative = new ContractorRepresentative
                                    {
                                        ContactNumber = x.ContractorMobileNumber,
                                        ContractorCode = x.ContractorCode,
                                        Email = x.ContractorEmail,
                                        Id = x.ContractorID ?? 0,
                                        Name = x.ContractorName
                                    },
                                })
                                .Distinct().OrderBy(x=> x.ConstructionMilestoneDescription).ToListAsync();

                    if (data.Count() > 0)
                    {
                        return data;
                    }
                    else
                    {
                        throw new NullReferenceException($"No milestone found for Reference Object: {referenceObject} and user: {username}");
                    }
                }
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

        public async Task<IEnumerable<Business.Entities.ConstructionMilestone>> GetConstructionMilestoneByUnitByVendor(string username, string referenceObject, string vendorCode)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    var data = await cntxt.VWUnitMilestonesWithLoaAndTradeTypes.Where(x => x.UserName == username
                                && x.ReferenceObject == referenceObject && x.VendorCode == vendorCode).Distinct()
                                .Select(x => new Business.Entities.ConstructionMilestone
                                {
                                    Id = x.ConstructionMilestoneID,
                                    OTCNumber = x.ConstructionMilestoneOTCNumber,
                                    ContractorNumber = x.ConstructionMilestoneContractorNumber,
                                    ManagingContractorID = x.ConstructionMilestoneManagingContractorID,
                                    ConstructionMilestoneCode = x.ConstructionMilestoneCode,
                                    ConstructionMilestoneDescription = x.MilestoneDescription,
                                    PercentageCompletion = x.ConstructionMilestonePercentageCompletion ?? 0,
                                    PercentageReferenceNumber = x.ConstructionMilestoneMilestonePercentageReferenceNumber,
                                    PONumber = x.ConstructionMilestoneMilestonePONumber,
                                    Sequence = x.ConstructionMilestoneMilestoneSequence,
                                    Weight = x.ConstructionMilestoneMilestoneWeight ?? 0,
                                    OTCTypeCode = x.OTCTypeCode,
                                    TradeCode = x.TradeTypeCode,
                                    TradeDescription = x.TradeType,
                                    VendorCode = x.VendorCode,
                                    ManagingContractorCode = x.ManagingContractorCode,
                                    LoaContractNumber = x.LOAContractNumber,
                                    ReferenceObject = x.ReferenceObject,
                                    Contractor = new Contractor
                                    {
                                        Code = x.VendorCode,
                                        Name = x.VendorName
                                    },
                                    Representative = new ContractorRepresentative
                                    {
                                        ContactNumber = x.ContractorMobileNumber,
                                        ContractorCode = x.ContractorCode,
                                        Email = x.ContractorEmail,
                                        Id = x.ContractorID ?? 0,
                                        Name = x.ContractorName
                                    },
                                })
                                .Distinct().OrderBy(x => x.ConstructionMilestoneDescription).ToListAsync();

                    if (data.Count() > 0)
                    {
                        return data;
                    }
                    else
                    {
                        throw new NullReferenceException($"No milestone found for Reference Object: {referenceObject} and user: {username}");
                    }
                }
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


        public async Task<IEnumerable<Business.Entities.ConstructionMilestone>> GetConstructionMilestoneByProject(string username, string projectcode)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var usr = await cntxt.User.Where(u => u.UserName == username).FirstOrDefaultAsync();

                    if (usr == null)
                        throw new NullReferenceException("User does not exist");

                    var data = new List<CTI.HI.Business.Entities.ConstructionMilestone>();


                    //if COntractor 
                    if (usr.UserTypeCode == "CONT")
                    {
                        data = await cntxt.VWUnitMilestonesWithLoaAndTradeTypes.Where(x => x.UserName == username
                                    && x.ProjectCode == projectcode && x.IsContractor == true).Distinct()
                                    .Select(x => new Business.Entities.ConstructionMilestone
                                    {
                                        Id = x.ConstructionMilestoneID,
                                        VendorCode = x.VendorCode,
                                        OTCNumber = x.ConstructionMilestoneOTCNumber,
                                        ContractorNumber = x.ConstructionMilestoneContractorNumber,
                                        ManagingContractorID = x.ConstructionMilestoneManagingContractorID,
                                        ConstructionMilestoneCode = x.ConstructionMilestoneCode,
                                        ConstructionMilestoneDescription = x.MilestoneDescription,
                                        PercentageCompletion = x.ConstructionMilestonePercentageCompletion ?? 0,
                                        PercentageReferenceNumber = x.ConstructionMilestoneMilestonePercentageReferenceNumber,
                                        PONumber = x.ConstructionMilestoneMilestonePONumber,
                                        Sequence = x.ConstructionMilestoneMilestoneSequence,
                                        Weight = x.ConstructionMilestoneMilestoneWeight ?? 0,
                                        OTCTypeCode = x.OTCTypeCode,
                                        TradeCode = x.TradeTypeCode,
                                        TradeDescription = x.TradeType,
                                        ManagingContractorCode = x.ManagingContractorCode,
                                        Contractor = new Business.Entities.Contractor
                                        {
                                            Code = x.VendorCode,
                                            Name = x.VendorName
                                        },
                                        LoaContractNumber = x.LOAContractNumber,
                                        Representative = new ContractorRepresentative
                                        {
                                            ContactNumber = x.ContractorMobileNumber,
                                            ContractorCode = x.ContractorCode,
                                            Email = x.ContractorEmail,
                                            Id = x.ContractorID ?? 0,
                                            Name = x.ContractorName
                                        },
                                    }).Distinct().OrderBy(x => x.ConstructionMilestoneDescription).ToListAsync();
                    }
                    else
                    {
                        data = await cntxt.VWUnitMilestonesWithLoaAndTradeTypes.Where(x => x.UserName == username
                                    && x.ProjectCode == projectcode && x.IsContractor == false && x.ProjectRoleCode != null).Distinct()
                                    .Select(x => new Business.Entities.ConstructionMilestone
                                    {
                                        Id = x.ConstructionMilestoneID,
                                        VendorCode = x.VendorCode,
                                        OTCNumber = x.ConstructionMilestoneOTCNumber,
                                        ContractorNumber = x.ConstructionMilestoneContractorNumber,
                                        ManagingContractorID = x.ConstructionMilestoneManagingContractorID,
                                        ConstructionMilestoneCode = x.ConstructionMilestoneCode,
                                        ConstructionMilestoneDescription = x.MilestoneDescription,
                                        PercentageCompletion = x.ConstructionMilestonePercentageCompletion ?? 0,
                                        PercentageReferenceNumber = x.ConstructionMilestoneMilestonePercentageReferenceNumber,
                                        PONumber = x.ConstructionMilestoneMilestonePONumber,
                                        Sequence = x.ConstructionMilestoneMilestoneSequence,
                                        Weight = x.ConstructionMilestoneMilestoneWeight ?? 0,
                                        OTCTypeCode = x.OTCTypeCode,
                                        TradeCode = x.TradeTypeCode,
                                        TradeDescription = x.TradeType,
                                        ManagingContractorCode = x.ManagingContractorCode,
                                        Contractor = new Business.Entities.Contractor
                                        {
                                            Code = x.VendorCode,
                                            Name = x.VendorName
                                        },
                                        LoaContractNumber = x.LOAContractNumber,
                                        Representative = new ContractorRepresentative
                                        {
                                            ContactNumber = x.ContractorMobileNumber,
                                            ContractorCode = x.ContractorCode,
                                            Email = x.ContractorEmail,
                                            Id = x.ContractorID ?? 0,
                                            Name = x.ContractorName
                                        },
                                    }).Distinct().OrderBy(x => x.ConstructionMilestoneDescription).ToListAsync();
                    }



                    if (data.Count() > 0)
                    {
                        return data;
                    }
                    else
                    {
                        throw new NullReferenceException($"No milestone found for project: {projectcode} and user: {username}");
                    }
                }
                catch(NullReferenceException ex)
                {
                    throw new NullReferenceException(ex.Message, ex.InnerException);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }
        }








        private async Task<IEnumerable<ConstructionMilestoneModel>> GetOnGoingConstructionAsync(FrebasContext cntxt)
        {
            var xx = await (from otc in cntxt.InventoryUnitOTC.Distinct()
                            join mct in cntxt.InventoryUnitOTCManagingContractor.Distinct()
                            on otc.OTCNumber equals mct.OTCNumber
                            join rco in cntxt.InventoryUnitOTCContractorAwarded.Distinct()
                            on new
                            {
                                otcNumber = mct.OTCNumber,
                                managingContractor = mct.ManagingContractor,
                                managingContractorID = mct.ID
                            } equals
                            new
                            {
                                otcNumber = rco.OTCNumber,
                                managingContractor = rco.ManagingContractor,
                                managingContractorID = rco.ManagingContractorID
                            }
                            join uct in cntxt.InventoryUnitOTCContractor.Distinct()
                            on new
                            {
                                otcNumber = rco.OTCNumber,
                                managingContractor = rco.ManagingContractor,
                                managingContractorID = rco.ManagingContractorID,
                                contractorNumber = rco.ContractorNumber,
                                vendorCode = rco.VendorCode
                            } equals
                             new
                             {
                                 otcNumber = uct.OTCNumber,
                                 managingContractor = uct.ManagingContractor,
                                 managingContractorID = uct.ManagingContractorID,
                                 contractorNumber = uct.ContractorNumber,
                                 vendorCode = uct.VendorCode
                             }
                            join loa in cntxt.InventoryUnitOTCContractorAwardedLOA.Distinct()
                            on uct.IsIssuedNTPReferenceNumber equals loa.IsIssuedNTPReferenceNumber
                            join mlc in cntxt.InventoryUnitOTCContractorConstructionMilestone
                            on new
                            {
                                otcNumber = uct.OTCNumber,
                                managingContractorID = uct.ManagingContractorID,
                                contractorNumber = uct.ContractorNumber
                            } equals
                            new
                            {
                                otcNumber = mlc.OTCNumber,
                                managingContractorID = mlc.ManagingContractorID,
                                contractorNumber = mlc.ContractorNumber
                            }
                            join inv in cntxt.InventoryUnit.Distinct()
                            on otc.ReferenceObject equals inv.ReferenceObject
                            join ott in cntxt.OTCType.Distinct()
                            on otc.OTCTypeCode equals ott.Code
                            join mas in cntxt.InventoryUnitOTCQualified.Distinct()
                            on otc.OTCNumber equals mas.OTCNumber
                            join cmi in cntxt.ConstructionMilestone.Distinct()
                            on mlc.ConstructionMilestoneCode equals cmi.Code
                            join acc in cntxt.VWUserAuthorization
                            on otc.ReferenceObject equals acc.ReferenceObject
                            join ctd in cntxt.MilestoneTradeTypes
                            on cmi.ConstructionTradeTypeCode equals ctd.Code
                            into trdd
                            from trd in trdd.DefaultIfEmpty()
                            where uct.IsIssuedNTP == true &&
                             //(mlc.Percentage ?? 0) < 100 &&
                             //(uct.PercentageCompletionVendor ?? 0) < 100 &&
                             uct.IsHold == false
                            select new
                            {
                                Id = mlc.RecordNumber,
                                OTCNumber = mlc.OTCNumber,
                                ContractorNumber = mlc.ContractorNumber,
                                ManagingContractorID = mlc.ManagingContractorID,
                                ConstructionMilestoneCode = cmi.Code,
                                ConstructionMilestoneDescription = cmi.Description,
                                PercentageCompletion = mlc.Percentage,
                                PercentageReferenceNumber = mlc.PercentageReferenceNumber,
                                PONumber = mlc.PONumber,
                                Sequence = mlc.Sequence,
                                Weight = mlc.Weight,
                                OTCTypeCode = otc.OTCTypeCode,
                                TradeCode = trd.Code,
                                TradeDescription = trd.Description,
                                VendorCode = uct.VendorCode,
                                ManagingContractorCode = uct.ManagingContractor,
                                LoaContractNumber = loa.LOAContractNumber,
                                projectCode = inv.ProjectCode,
                                phaseCode = inv.PhaseBuildingCode,
                                blockCode = inv.BlockFloorClusterCode,
                                ReferenceObject = inv.ReferenceObject,
                                PercentageTagDate = mlc.PercentageTagDate,
                                DPIUser = acc.UserName
                            }).ToListAsync();

            return xx.Select(x => new ConstructionMilestoneModel
            {
                ConstructionMilestone = new InventoryUnitOTCContractorConstructionMilestone
                {
                    RecordNumber = x.Id,
                    OTCNumber = x.OTCNumber,
                    ContractorNumber = x.ContractorNumber,
                    ManagingContractorID = x.ManagingContractorID,
                    Percentage = x.PercentageCompletion,
                    PercentageReferenceNumber = x.PercentageReferenceNumber,
                    PONumber = x.PONumber,
                    Sequence = x.Sequence,
                    Weight = x.Weight,
                    ConstructionMilestoneCode = x.ConstructionMilestoneCode,
                    PercentageTagDate = x.PercentageTagDate
                },
                MilestoneDetails = new HCM.Business.Entities.ConstructionMilestone
                {
                    Code = x.ConstructionMilestoneCode,
                    Description = x.ConstructionMilestoneDescription,
                },
                OTCTypeCode = x.OTCTypeCode,
                TradeCode = x.TradeCode,
                TradeDescription = x.TradeDescription,
                VendorCode = x.VendorCode,
                ManagingContractorCode = x.ManagingContractorCode,
                LoaContractNumber = x.LoaContractNumber,
                ReferenceObject = x.ReferenceObject,
                Unit = new IMM.Business.Entities.Model.UnitModel
                {
                    ReferenceObject = x.ReferenceObject,
                    Project = new IMM.Business.Entities.Project
                    {
                        Code = x.projectCode
                    }
                },
                ProjectCode = x.projectCode,
                PhaseCode = x.phaseCode,
                BlockCode = x.blockCode,
                DPIUser = x.DPIUser
            }).ToList();
        }
        private async Task<IEnumerable<NotificationModel>> GetOnGoingConstructionNotificationAsync(FrebasContext cntxt, string username)
        {
            try
            {
                var data = await cntxt.VWUnitMilestonesWithLoaAndTradeTypes
                        .Distinct().Where(a => a.UserName == username
                            && a.ConstructionMilestonePercentageCompletion == 100 &&
                                                 (DbFunctions.DiffDays(a.ConstructionMilestoneMilestonePercentageTagDate, _DateToday)
                                                    <= _NotifRecentDays &&
                                                  DbFunctions.DiffDays(a.ConstructionMilestoneMilestonePercentageTagDate, _DateToday) >= 0)
                                )
                        .Select(x => new NotificationModel
                        {
                            ConstructionMilestoneId = x.ConstructionMilestoneID,
                            DatePosted = x.ConstructionMilestoneMilestonePercentageTagDate ?? _DateToday,
                            Subject = x.MilestoneDescription,
                            Message = x.PercentageRemarks,
                            MessageBy = x.PercentageTagBy,
                            Unit = new Unit
                            {
                                ReferenceObject = x.ReferenceObject,
                                Project = new Project
                                {
                                    Code = x.ProjectCode,
                                    ShortName = x.ProjectShortName,
                                    LongName = x.ProjectName
                                },
                                PhaseBuilding = new PhaseBuilding
                                {
                                    Code = x.PhaseCode,
                                    ShortName = x.PhaseShortName,
                                    LongName = x.Phase
                                },
                                BlockFloor = new Block
                                {
                                    Code = x.BlockCode,
                                    ShortName = x.BlockShortName,
                                    LongName = x.Block
                                },
                                InventoryUnitNumber = x.InventoryUnitNumber,
                                LotUnitShareNumber = x.Lot
                            }
                        }).Distinct().ToListAsync();

                if (data.Count() > 0)
                {
                    return data;
                }
                else
                {
                    throw new NullReferenceException($"No recently closed milestone found for user: {username}");
                }
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







        public async Task<IEnumerable<ConstructionMilestoneModel>> GetConstructionMilestoneAsync(Func<ConstructionMilestoneModel, bool> expression)
        {
            using (var cntxt = new FrebasContext())
            {
                var query = await GetOnGoingConstructionAsync(cntxt);
                //var x = await query.ToListAsync();
                var xxx = query.Where(expression).Distinct().ToList();
                return xxx;
            }
        }

        public async Task<IEnumerable<Business.Entities.ConstructionMilestone>> GetConstructionMilestoneByID(string username, int mlsid)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    try
                    {

                        var data = await cntxt.VWUnitMilestonesWithLoaAndTradeTypes.Where(x => x.UserName == username 
                                    && x.ConstructionMilestoneID == mlsid).Distinct()
                                    .Select(x => new Business.Entities.ConstructionMilestone
                                    {
                                        Id = x.ConstructionMilestoneID,
                                        VendorCode = x.VendorCode,
                                        OTCNumber = x.ConstructionMilestoneOTCNumber,
                                        ContractorNumber = x.ConstructionMilestoneContractorNumber,
                                        ManagingContractorID = x.ConstructionMilestoneManagingContractorID,
                                        ConstructionMilestoneCode = x.ConstructionMilestoneCode,
                                        ConstructionMilestoneDescription = x.MilestoneDescription,
                                        PercentageCompletion = x.ConstructionMilestonePercentageCompletion ?? 0,
                                        PercentageReferenceNumber = x.ConstructionMilestoneMilestonePercentageReferenceNumber,
                                        PONumber = x.ConstructionMilestoneMilestonePONumber,
                                        Sequence = x.ConstructionMilestoneMilestoneSequence,
                                        Weight = x.ConstructionMilestoneMilestoneWeight ?? 0,
                                        OTCTypeCode = x.OTCTypeCode,
                                        TradeCode = x.TradeTypeCode,
                                        TradeDescription = x.TradeType,
                                        ManagingContractorCode = x.ManagingContractorCode,
                                        Contractor = new Business.Entities.Contractor
                                        {
                                            Code = x.VendorCode,
                                            Name = x.VendorName
                                        },
                                        LoaContractNumber = x.LOAContractNumber,
                                        Representative = new ContractorRepresentative
                                        {
                                            ContactNumber = x.ContractorMobileNumber,
                                            ContractorCode = x.ContractorCode,
                                            Email = x.ContractorEmail,
                                            Id = x.ContractorID ?? 0,
                                            Name = x.ContractorName
                                        },
                                    }).Distinct().OrderBy(x=> x.ConstructionMilestoneDescription).ToListAsync();


                        if (data.Count() > 0)
                        {
                            return data;
                        }
                        else
                        {
                            throw new NullReferenceException($"No milestone found with id: {mlsid} and for user: {username}");
                        }
                    }
                    catch(NullReferenceException ex)
                    {
                        throw new NullReferenceException(ex.Message, ex.InnerException);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex.InnerException);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


















        public async Task<IEnumerable<ConstructionMilestoneModel>> GetUserConstructionMilestoneAsync(string userName, Func<ConstructionMilestoneModel, bool> expression)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var query = await GetOnGoingConstructionAsync(cntxt);






                    query = query.Where(q => q.DPIUser == userName);

                    var _usr = await cntxt.User.Where(u => u.UserName == userName).FirstOrDefaultAsync();

                    if (_usr == null)
                        return null;

                    //if COntractor 
                    if (_usr.UserTypeCode == "CONT")
                    {
                        var _userTypeReference = Convert.ToInt32(_usr.UserTypeReference);

                        //get the contractor code of representative
                        var _dteToday = DateTime.Today;
                        var _dteTomorrow = DateTime.Today.AddDays(1).Date;
                        var _contractorCode = await (from ctp in cntxt.ContractorPersonnel.Where(c => c.ID == _userTypeReference)
                                                     join cau in cntxt.ContractorPersonnelAuthorized.Where(a => DbFunctions.TruncateTime(a.DateTo ?? _dteTomorrow) >= _dteToday)
                                                    on new
                                                    {
                                                        contractorCode = ctp.ContractorCode,
                                                        refId = ctp.ID
                                                    } equals
                                                    new
                                                    {
                                                        contractorCode = cau.ContractorCode,
                                                        refId = cau.AuthorizedPersonnelID
                                                    }
                                                     select cau.ContractorCode).FirstOrDefaultAsync();

                        if (_contractorCode == null)
                            return null;

                        //get project code using contractor code
                        var contractorUnits = await (from uct in cntxt.InventoryUnitOTCContractor.Where(c => c.VendorCode == _contractorCode)
                                                     join otc in cntxt.InventoryUnitOTC
                                                     on uct.OTCNumber equals otc.OTCNumber
                                                     join inv in cntxt.InventoryUnit
                                                     on otc.ReferenceObject equals inv.ReferenceObject
                                                     select inv.ReferenceObject).Distinct().ToArrayAsync();

                        var ongoingUnit = query.Where(expression).Distinct().ToList();

                        return ongoingUnit.Where(m => contractorUnits.Contains(m.ReferenceObject)).ToList();
                    }

                    var xx = query.Where(expression).Distinct().ToList();
                    return xx;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }



        public async Task<IEnumerable<NotificationModel>> GetUserConstructionMilestoneNotificationAsync(string userName)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var query = await GetOnGoingConstructionNotificationAsync(cntxt, userName);
                    var _usr = await cntxt.User.Where(u => u.UserName == userName).FirstOrDefaultAsync();

                    if (_usr == null)
                        return null;

                    //if COntractor 
                    if (_usr.UserTypeCode == "CONT")
                    {
                        var _userTypeReference = Convert.ToInt32(_usr.UserTypeReference);

                        //get the contractor code of representative
                        var _dteToday = DateTime.Today;
                        var _dteTomorrow = DateTime.Today.AddDays(1).Date;
                        var _contractorCode = await (from ctp in cntxt.ContractorPersonnel.Where(c => c.ID == _userTypeReference)
                                                     join cau in cntxt.ContractorPersonnelAuthorized.Where(a => DbFunctions.TruncateTime(a.DateTo ?? _dteTomorrow) >= _dteToday)
                                                    on new
                                                    {
                                                        contractorCode = ctp.ContractorCode,
                                                        refId = ctp.ID
                                                    } equals
                                                    new
                                                    {
                                                        contractorCode = cau.ContractorCode,
                                                        refId = cau.AuthorizedPersonnelID
                                                    }
                                                     select cau.ContractorCode).FirstOrDefaultAsync();

                        if (_contractorCode == null)
                            return null;

                        //get project code using contractor code
                        var contractorUnits = await (from uct in cntxt.InventoryUnitOTCContractor.Where(c => c.VendorCode == _contractorCode)
                                                     join otc in cntxt.InventoryUnitOTC
                                                     on uct.OTCNumber equals otc.OTCNumber
                                                     join inv in cntxt.InventoryUnit
                                                     on otc.ReferenceObject equals inv.ReferenceObject
                                                     select inv.ReferenceObject).Distinct().ToArrayAsync();

                        var ongoingUnit = query.Distinct().ToList();

                        var data = ongoingUnit.Where(m => contractorUnits.Contains(m.Unit.ReferenceObject)).ToList();

                        if (data.Count() > 0)
                        {
                            return data;
                        }
                        else
                        {
                            throw new NullReferenceException($"No recently closed milestone found for user: {userName}");
                        }
                    }

                    var dataNormal = query.ToList();

                    if (dataNormal.Count() > 0)
                    {
                        return dataNormal;
                    }
                    else
                    {
                        throw new NullReferenceException($"No recently closed milestone found for user: {userName}");
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }








        public async Task<bool> UpdateMilestonePercentageAsync(string userName, int ConstructionMilestoneId, string otcNumber, string contractorNumber, int managingContractorId, decimal newPercentage, string milestoneCode, IEnumerable<HI.Business.Entities.MilestoneAttachment> attachments, DateTime? dateVisited = null)
        {
            using (var cntxt = new FrebasContext())
            {
                using (var transaction = cntxt.Database.BeginTransaction())
                {

                    try
                    {
                        var _isSuccess = false;
                        var oldMilestone = await cntxt.InventoryUnitOTCContractorConstructionMilestone.Where(m => m.RecordNumber == ConstructionMilestoneId).FirstOrDefaultAsync();

                        if (oldMilestone.Percentage != newPercentage)
                        {
                            var _spRepo = new FrebasStoredProceduresRepository(cntxt);
                            var svcMilestone = await _spRepo.UpdateMilestonePercentage(userName,
                                                                             otcNumber,
                                                                             contractorNumber,
                                                                             managingContractorId,
                                                                             newPercentage,
                                                                             milestoneCode,
                                                                             dateVisited);
                            _isSuccess = svcMilestone.IsSucess;
                            if (!_isSuccess)
                                throw new ApplicationException("Error in updating of Milestone percentage.", new Exception(svcMilestone.Message));
                        }

                        //Save POC
                        await UpdatePOCAsync(cntxt, ConstructionMilestoneId, userName, newPercentage);

                        //Save CommentAttachment
                        var maxIndex = await cntxt.MilestoneImage.Where(i => i.MilestoneRecordNumber == oldMilestone.RecordNumber).Select(i => i.FileIndex).DefaultIfEmpty(0).MaxAsync();
                        var attIndex = maxIndex + 1;
                        foreach (var itm in attachments)
                        {
                            var _milestoneAttachment = new InventoryUnitOTCContractorConstructionMilestoneImage();
                            _milestoneAttachment.MilestoneRecordNumber = ConstructionMilestoneId;
                            _milestoneAttachment.FileName = itm.FileName;
                            _milestoneAttachment.FilePath = itm.FilePath;
                            _milestoneAttachment.FileIndex = attIndex;
                            _milestoneAttachment.DateCreated = DateTime.Now;
                            _milestoneAttachment.CreatedBy = userName;
                            attIndex += 1;

                            cntxt.MilestoneImage.Add(_milestoneAttachment);
                        }
                        await cntxt.SaveChangesAsync();

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public async Task<IEnumerable<HI.Business.Entities.MilestoneAttachment>> GetConstructionMilestoneAttachmentAsync(Expression<Func<HI.Business.Entities.MilestoneAttachment, bool>> expression)
        {
            using (var cntxt = new FrebasContext())
            {
                var qry = cntxt.MilestoneImage.Select(i => new HI.Business.Entities.MilestoneAttachment
                {
                    ConstructionMilestoneId = i.MilestoneRecordNumber,
                    FileName = i.FileName,
                    FilePath = i.FilePath
                });

                return await qry.Where(expression).ToListAsync();
            }
        }

        public async Task<IEnumerable<MilestoneAttachment>> GetConstructionMilestoneAttachmentAsync(int id)
        {
            using (var cntxt = new FrebasContext())
            {
                var qry = cntxt.MilestoneImage.Where(x => x.MilestoneRecordNumber == id)
                    .Select(i => new HI.Business.Entities.MilestoneAttachment
                {
                    ConstructionMilestoneId = i.MilestoneRecordNumber,
                    FileName = i.FileName,
                    FilePath = i.FilePath
                });

                return await qry.ToListAsync();
            }
        }

        public async Task<IEnumerable<MilestoneAttachment>> GetConstructionMilestoneAttachmentAsync(int[] id)
        {
            using (var cntxt = new FrebasContext())
            {
                var qry = cntxt.MilestoneImage.Where(x => id.Contains(x.MilestoneRecordNumber))
                    .Select(i => new HI.Business.Entities.MilestoneAttachment
                    {
                        ConstructionMilestoneId = i.MilestoneRecordNumber,
                        FileName = i.FileName,
                        FilePath = i.FilePath
                    });

                return await qry.ToListAsync();
            }
        }

        public async Task<IEnumerable<InventoryUnitOTCContractorConstructionMilestone>> GetConstructionMilestonesAsync(Expression<Func<InventoryUnitOTCContractorConstructionMilestone, bool>> expression)
        {
            using (var cntxt = new FrebasContext())
            {
                return await cntxt.InventoryUnitOTCContractorConstructionMilestone.Where(expression).ToListAsync();
            }
        }

        public async Task<string> GetMilestoneReferenceObjectAsync(int ConstructionMilestoneId)
        {
            using (var cntxt = new FrebasContext())
            {
                return await (from otc in cntxt.InventoryUnitOTC
                              join mlc in cntxt.InventoryUnitOTCContractorConstructionMilestone.Where(m => m.RecordNumber == ConstructionMilestoneId)
                              on otc.OTCNumber equals mlc.OTCNumber
                              select otc.ReferenceObject).FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<ContractorAwardedLoa>> GetLOADetails(Expression<Func<ContractorAwardedLoa, bool>> expression)
        {
            using (var cntxt = new FrebasContext())
            {
                return await (from otc in cntxt.InventoryUnitOTC.Distinct()
                              join mct in cntxt.InventoryUnitOTCManagingContractor.Distinct()
                              on otc.OTCNumber equals mct.OTCNumber
                              join rco in cntxt.InventoryUnitOTCContractorAwarded.Distinct()
                              on new
                              {
                                  otcNumber = mct.OTCNumber,
                                  managingContractor = mct.ManagingContractor,
                                  managingContractorID = mct.ID
                              } equals
                              new
                              {
                                  otcNumber = rco.OTCNumber,
                                  managingContractor = rco.ManagingContractor,
                                  managingContractorID = rco.ManagingContractorID
                              }
                              join uct in cntxt.InventoryUnitOTCContractor.Distinct()
                              on new
                              {
                                  otcNumber = rco.OTCNumber,
                                  managingContractor = rco.ManagingContractor,
                                  managingContractorID = rco.ManagingContractorID,
                                  contractorNumber = rco.ContractorNumber,
                                  vendorCode = rco.VendorCode
                              } equals
                               new
                               {
                                   otcNumber = uct.OTCNumber,
                                   managingContractor = uct.ManagingContractor,
                                   managingContractorID = uct.ManagingContractorID,
                                   contractorNumber = uct.ContractorNumber,
                                   vendorCode = uct.VendorCode
                               }
                              join loa in cntxt.InventoryUnitOTCContractorAwardedLOA.Distinct()
                              on uct.IsIssuedNTPReferenceNumber equals loa.IsIssuedNTPReferenceNumber
                              select new ContractorAwardedLoa()
                              {
                                  OTCNumber = otc.OTCNumber,
                                  LoaContractNumber = loa.LOAContractNumber,
                                  LoaDate = loa.IsSignedLOATagDate,
                                  LoaReferenceNumber = loa.ReferenceNumber,
                                  NTPDate = loa.IsIssuedNTPDate,
                                  NTPNumber = loa.IsIssuedNTPNumber,
                                  NTPReferenceNumber = loa.IsIssuedNTPReferenceNumber,
                                  NTPTagDate = loa.IsIssuedNTPTagDate,
                                  TagBy = loa.IsIssuedNTPTagBy
                              }).Where(expression).ToListAsync();

            }
        }

        public async Task<IEnumerable<MilestoneAttachment>> GetMilestoneImagesAsync()
        {
            using (var cntxt = new FrebasContext())
            {
                var data = await cntxt.MilestoneImage.Distinct().ToListAsync();
                return data.Select(x => new MilestoneAttachment { FileName = x.FileName }).Distinct().ToList();
            }
        }

        public async Task<InventoryUnitOTCContractorConstructionMilestonePercentage> GetMilestonePOCAsync(Expression<Func<InventoryUnitOTCContractorConstructionMilestonePercentage, bool>> expression)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                    return await cntxt.InventoryUnitOTCContractorConstructionMilestonePercentage.Where(expression).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<IEnumerable<InventoryUnitOTCContractorConstructionMilestonePercentage>> GetMilestonesPOCAsync(int id)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                    return await cntxt.InventoryUnitOTCContractorConstructionMilestonePercentage.Where(x=>x.MilestoneRecordNumber == id).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<InventoryUnitOTCContractorConstructionMilestonePercentage>> GetMilestonesPOCAsync(Expression<Func<InventoryUnitOTCContractorConstructionMilestonePercentage, bool>> expression)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                    return await cntxt.InventoryUnitOTCContractorConstructionMilestonePercentage.Where(expression).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region "methods"
        private async Task<bool> UpdatePOCAsync(FrebasContext cntxt, int constructionMilestoneId, string userName, decimal percentage)
        {
            try
            {


                var usr = await _usrRepo.GetUserInfoAsync(userName);
                var mil = await cntxt.InventoryUnitOTCContractorConstructionMilestone.Where(m => m.RecordNumber == constructionMilestoneId).FirstOrDefaultAsync();
                var poc = await cntxt.InventoryUnitOTCContractorConstructionMilestonePercentage.Where(p => p.MilestoneRecordNumber == constructionMilestoneId).FirstOrDefaultAsync();
                var actualPOC = (percentage / 100) * mil.Weight;
                var isPOCIsEmpty = (poc == null);
                if (poc == null)
                {
                    poc = new InventoryUnitOTCContractorConstructionMilestonePercentage();
                    poc.CreatedBy = userName;
                    poc.DateCreated = DateTime.Now;
                    poc.MilestoneRecordNumber = constructionMilestoneId;
                }

                if (usr.RoleCode == UserRoleCode.QA)
                {
                    poc.QualityAssurancePercentage = percentage;
                    poc.QualityAssuranceUpdatedBy = userName;
                    poc.QualityAssuranceDateUpdated = DateTime.Now;
                    poc.QualityAssurancePercentageActual = actualPOC;
                    poc.GroupUpdated = POCGroupUpdated.QA;
                }
                else if (usr.RoleCode == UserRoleCode.ProjectEngineer)
                {
                    poc.ProjectEngineerPercentage = percentage;
                    poc.ProjectEngineerUpdatedBy = userName;
                    poc.ProjectEngineerDateUpdated = DateTime.Now;
                    poc.ProjectEngineerPercentageActual = actualPOC;
                    poc.GroupUpdated = POCGroupUpdated.ProjectEngineer;
                }
                else if (usr.RoleCode == UserRoleCode.Contractor)
                {
                    poc.ContractorPercentage = percentage;
                    poc.ContractorPercentageUpdatedBy = userName;
                    poc.ContractorPercentageDateUpdated = DateTime.Now;
                    poc.ContractorPercentageActual = actualPOC;
                    poc.GroupUpdated = POCGroupUpdated.Contractor;
                }

                if (isPOCIsEmpty)
                    cntxt.InventoryUnitOTCContractorConstructionMilestonePercentage.Add(poc);


                await cntxt.SaveChangesAsync();

                await ComputePOCOfUnit(cntxt, constructionMilestoneId, userName);

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private async Task ComputePOCOfUnit(FrebasContext cntxt, int milestoneId, string userName)
        {

            var refObj = await (from mlc in cntxt.InventoryUnitOTCContractorConstructionMilestone
                                join otc in cntxt.InventoryUnitOTC
                                on mlc.OTCNumber equals otc.OTCNumber
                                where mlc.RecordNumber == milestoneId
                                select otc.ReferenceObject).FirstOrDefaultAsync();
            //var constructionMilestones = await GetConstructionMilestoneAsync(refObj);
            //var milestones = constructionMilestones.Select(m => m.ConstructionMilestone).ToList();

            var conMiles = await (from m in cntxt.InventoryUnitOTCContractorConstructionMilestone
                                  join c in cntxt.InventoryUnitOTCContractor
                                  on m.OTCNumber equals c.OTCNumber
                                  where c.IsIssuedNTP == true && c.IsHold == false && m.RecordNumber == milestoneId
                                  select new
                                  {
                                      OTCNumber = c.OTCNumber,
                                      ManagingContractorID = c.ManagingContractorID,
                                      ContractorNumber = c.ContractorNumber
                                  }).ToListAsync();


            var otcs = conMiles.Select(o => o.OTCNumber).ToArray();
            var otcMilestone = await cntxt.InventoryUnitOTCContractorConstructionMilestone
                                    .Where(o => otcs.Contains(o.OTCNumber))
                                    .Select(m => new
                                    {
                                        OTCNumber = m.OTCNumber,
                                        ManagingContractorID = m.ManagingContractorID,
                                        ContractorNumber = m.ContractorNumber,
                                        RecordNumber = m.RecordNumber
                                    })
                                    .ToListAsync();

            var milestones = (from m in otcMilestone
                              join c in conMiles
                              on new
                              {
                                  OTCNumber = m.OTCNumber,
                                  ManagingContractorID = m.ManagingContractorID,
                                  ContractorNumber = m.ContractorNumber
                              }
                              equals new
                              {
                                  OTCNumber = c.OTCNumber,
                                  ManagingContractorID = c.ManagingContractorID,
                                  ContractorNumber = c.ContractorNumber
                              }
                              select m).ToList();

            var recordNumbers = milestones.Select(r => r.RecordNumber).ToArray();
            var pocs = await (cntxt.InventoryUnitOTCContractorConstructionMilestonePercentage.Where(p => recordNumbers.Contains(p.MilestoneRecordNumber))).ToListAsync();

            var unitPOC = await cntxt.InventoryUnitPercentage.Where(p => p.ReferenceObject == refObj).FirstOrDefaultAsync();

            //If not existing then insert
            if (unitPOC == null)
            {
                unitPOC = new InventoryUnitPercentage();
                unitPOC.ReferenceObject = refObj;
                unitPOC.ProjectEngineerPercentage = pocs.Select(p => p.ProjectEngineerPercentageActual).Sum();
                unitPOC.QualityAssurancePercentage = pocs.Select(p => p.QualityAssurancePercentageActual).Sum();
                unitPOC.ContractorPercentage = pocs.Select(p => p.ContractorPercentageActual).Sum();
                unitPOC.DateCreated = DateTime.Now;
                unitPOC.CreatedBy = userName;

                cntxt.InventoryUnitPercentage.Add(unitPOC);
            }
            else
            {
                //if existing then modify
                unitPOC.ProjectEngineerPercentage = pocs.Select(p => p.ProjectEngineerPercentageActual).Sum();
                unitPOC.QualityAssurancePercentage = pocs.Select(p => p.QualityAssurancePercentageActual).Sum();
                unitPOC.ContractorPercentage = pocs.Select(p => p.ContractorPercentageActual).Sum();
                unitPOC.DateModified = DateTime.Now;
                unitPOC.ModifiedBy = userName;

            }
            await cntxt.SaveChangesAsync();

        }


        #endregion

        #region "other"
        protected override InventoryUnitOTCContractorConstructionMilestone AddEntity(FrebasContext entityContext, InventoryUnitOTCContractorConstructionMilestone entity)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<InventoryUnitOTCContractorConstructionMilestone> GetEntities(FrebasContext entityContext)
        {
            throw new NotImplementedException();
        }

        protected override InventoryUnitOTCContractorConstructionMilestone GetEntity(FrebasContext entityContext, int id)
        {
            throw new NotImplementedException();
        }

        protected override InventoryUnitOTCContractorConstructionMilestone UpdateEntity(FrebasContext entityContext, InventoryUnitOTCContractorConstructionMilestone entity)
        {
            throw new NotImplementedException();
        }

        









        #endregion
    }
}
