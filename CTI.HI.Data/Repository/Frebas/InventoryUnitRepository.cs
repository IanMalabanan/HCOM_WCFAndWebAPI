
using CTI.IMM.Business.Entities;
using CTI.IMM.Business.Entities.Model;
using CTI.HI.Data.Context;
using CTI.HI.Data.Contracts.Frebas;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CTI.HCM.Business.Entities;
using CTI.ARM.Business.Entities;
using GeoCoordinatePortable;
using CTI.HI.Business.Entities;
using CTI.HCM.Business.Entities.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace CTI.HI.Data.Repository.Frebas
{
    [Export(typeof(IInventoryUnitRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class InventoryUnitRepository : DataRepositoryFrebasBase<InventoryUnit>, IInventoryUnitRepository
    {
        private IConstructionMilestoneRepository _constructionRepo;
        public InventoryUnitRepository()
        {
            _constructionRepo = new ConstructionMilestoneRepository();
        }

        public static string DigitsOnly(string strRawData)
        {
            return Regex.Replace(strRawData, "[^0-9]", "");
        }

        private IQueryable<UnitModel> GetUnitModelQueryable(FrebasContext cntxt)
        {
            try
            {
                return (from imm in cntxt.InventoryUnit
                        join pro in cntxt.Project
                        on imm.ProjectCode equals pro.Code
                        join phb in cntxt.PhaseBuilding
                        on imm.PhaseBuildingCode equals phb.Code
                        join bfc in cntxt.BlockFloorCluster
                        on imm.BlockFloorClusterCode equals bfc.Code
                        select new UnitModel
                        {
                            ReferenceObject = imm.ReferenceObject,
                            Project = pro,
                            PhaseBuilding = phb,
                            BlockFloor = bfc,
                            LotUnitShareNumber = imm.LotUnitShareNumber,
                            InventoryUnitNumber = imm.InventoryUnitNumber
                        });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<UnitModel> GetUnitAsync(string referenceObject)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    return await GetUnitModelQueryable(cntxt).Where(i => i.ReferenceObject == referenceObject).FirstOrDefaultAsync();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        public async Task<IEnumerable<UnitModel>> GetUnitAsync(Expression<Func<UnitModel, bool>> expression)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    return await GetUnitModelQueryable(cntxt).Where(expression).ToListAsync();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        public async Task<IEnumerable<InventoryUnit>> GetInventoryUnitAsync(Expression<Func<InventoryUnit, bool>> expression)
        {
            using (var cntxt = new FrebasContext())
            {
                return await cntxt.InventoryUnit.Where(expression).ToListAsync();
            }
        }
        public async Task<IEnumerable<HouseModelLayout>> GetInventoryFloorPlanAsync(string ReferenceObject)
        {
            using (var cntxt = new FrebasContext())
            {
                var _houseModelCode = await cntxt.InventoryUnitConstructionTarget.Where(u => u.ReferenceObject == ReferenceObject).Select(u => u.HouseModeCode).FirstOrDefaultAsync();
                return await cntxt.HouseModelLayout.Where(h => h.HouseModelCode == _houseModelCode).ToListAsync();
            }
        }

        public async Task<IEnumerable<HouseModelLayout>> GetInventoryFloorPlanAsync(string[] ReferenceObject)
        {
            using (var cntxt = new FrebasContext())
            {
                var _houseModelCode = await cntxt.InventoryUnitConstructionTarget.Where(u => ReferenceObject.Contains(u.ReferenceObject)).Select(u => u.HouseModeCode).FirstOrDefaultAsync();
                return await cntxt.HouseModelLayout.Where(h => h.HouseModelCode == _houseModelCode).ToListAsync();
            }
        }


        public async Task<IEnumerable<HouseModelLayout>> GetInventoryFloorPlanAsync(Expression<Func<HouseModelLayout, bool>> expression)
        {
            using (var cntxt = new FrebasContext())
            {
                return await cntxt.HouseModelLayout.Where(expression).ToListAsync();
            }
        }
        public async Task<IEnumerable<InventoryUnitCoordinates>> GetUnitCoordinatesAsync(Expression<Func<InventoryUnitCoordinates, bool>> expression)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    return await cntxt.InventoryUnitCoordinates.Where(expression).ToListAsync();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        public async Task<IEnumerable<UnitModel>> GetNearbyUnitAsync(string RoleCode, double inspectionRadius, double latitude, double longitude, string[] referenceObjectsArray)
        {

            using (var cntxt = new FrebasContext())
            {
                try
                {
                    return await GetUnitModelQueryable(cntxt).Where(u => referenceObjectsArray.Contains(u.ReferenceObject)).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }

                //Old way of getting the nearby

                //if (RoleCode == "CONT")
                //{
                //    return await GetUnitModelQueryable(cntxt).Where(u => referenceObjectsArray.Contains(u.ReferenceObject)).ToListAsync();
                //}
                //else
                //{
                //    try
                //    {
                //        var userCoordinate = new GeoCoordinate(latitude, longitude);
                //        var invLocation = (await cntxt.InventoryUnitCoordinates
                //                            .Where(c => referenceObjectsArray.Contains(c.ReferenceObject))
                //                            .Select(c => new
                //                            {
                //                                ReferenceObject = c.ReferenceObject,
                //                                Latitude = (double)c.Latitude,
                //                                Longitude = (double)c.Longitude
                //                            }).ToListAsync()).Select(c => new
                //                            {
                //                                ReferenceObject = c.ReferenceObject,
                //                                UnitDistanceToUser = new GeoCoordinate((double)c.Latitude, (double)c.Longitude)
                //                                                                            .GetDistanceTo(userCoordinate)
                //                            }).ToList();

                //        var nearUnitArray = invLocation.Where(a => a.UnitDistanceToUser <= inspectionRadius).Select(u => u.ReferenceObject).ToArray();
                //        return await GetUnitModelQueryable(cntxt).Where(u => nearUnitArray.Contains(u.ReferenceObject)).ToListAsync();

                //    }
                //    catch (Exception ex)
                //    {

                //        throw ex;
                //    }
                //    }
            }
        }

        public async Task<IEnumerable<AssignInventoryUnitPhysicalCondition>> GetUnitPhysicalConditionAsync(Expression<Func<AssignInventoryUnitPhysicalCondition, bool>> expression)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    return await cntxt.AssignInventoryUnitPhysicalCondition.Where(expression).ToListAsync();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        public async Task<IEnumerable<UnitContractor>> GetUnitContractorAsync(string[] ReferenceObjects)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    //Get the Signed only contractor
                    var c = await (from otc in cntxt.InventoryUnitOTC.Where(o => ReferenceObjects.Contains(o.ReferenceObject))
                                   join mct in cntxt.InventoryUnitOTCManagingContractor.Where(m => m.CurrentStatus == "SIGN" && m.IsCurrentTerminated == false)
                                   on otc.OTCNumber equals mct.OTCNumber
                                   join uct in cntxt.InventoryUnitOTCContractor
                                   on new
                                   {
                                       OTCNumber = mct.OTCNumber,
                                       ContractorNumber = mct.CurrentContractorNumber,
                                       MCId = mct.ID
                                   } equals new
                                   {
                                       OTCNumber = uct.OTCNumber,
                                       ContractorNumber = uct.ContractorNumber,
                                       MCId = uct.ManagingContractorID
                                   }
                                   join ven in cntxt.Vendor
                                   on uct.VendorCode equals ven.Code
                                   select new
                                   {
                                       ReferenceObject = otc.ReferenceObject,
                                       Contractor = new Contractor
                                       {
                                           Code = ven.Code,
                                           Name = ven.Name
                                       }
                                   }
                                  ).ToListAsync();

                    var lstUnitContractor = new List<UnitContractor>();
                    foreach (var inv in c.Select(i => i.ReferenceObject).Distinct())
                    {
                        lstUnitContractor.Add(new UnitContractor
                        {
                            ReferenceObject = inv,
                            Contractors = c.Where(u => u.ReferenceObject == inv).Select(u => u.Contractor).ToList()
                        });
                    }

                    return lstUnitContractor;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IEnumerable<UnitContractor>> GetUnitContractorDetailsAsync(string username, string ReferenceObjects)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    //Get the Signed only contractor
                    //var c = await (from otc in cntxt.InventoryUnitOTC
                    //               join mct in cntxt.InventoryUnitOTCManagingContractor.Where(m => m.CurrentStatus == "SIGN" && m.IsCurrentTerminated == false)
                    //               on otc.OTCNumber equals mct.OTCNumber
                    //               join uct in cntxt.InventoryUnitOTCContractor
                    //               on new
                    //               {
                    //                   OTCNumber = mct.OTCNumber,
                    //                   ContractorNumber = mct.CurrentContractorNumber,
                    //                   MCId = mct.ID
                    //               } equals new
                    //               {
                    //                   OTCNumber = uct.OTCNumber,
                    //                   ContractorNumber = uct.ContractorNumber,
                    //                   MCId = uct.ManagingContractorID
                    //               }
                    //               join ven in cntxt.Vendor
                    //               on uct.VendorCode equals ven.Code
                    //               select new
                    //               {
                    //                   ReferenceObject = otc.ReferenceObject,
                    //                   Contractor = new Contractor
                    //                   {
                    //                       Code = ven.Code,
                    //                       Name = ven.Name
                    //                   }
                    //               }
                    //              ).Where(x => x.ReferenceObject == ReferenceObjects).ToListAsync();

                    var usr = await cntxt.User.Where(x => x.UserName == username).FirstOrDefaultAsync();

                    if (usr == null)
                        throw new ApplicationException("User does not exists");

                    var contractors = new List<VWContractors>();

                    if (usr.UserTypeCode == "CONT")
                    {
                        contractors = await cntxt.VWContractors.Where(x => x.ReferenceObject == ReferenceObjects && x.ContractorUserName == username).ToListAsync();
                    }
                    else
                    {
                        contractors = await cntxt.VWContractors.Where(x => x.ReferenceObject == ReferenceObjects).ToListAsync();
                    }

                    var lstUnitContractor = new List<UnitContractor>();

                    if (contractors.Count() > 0)
                    {
                        lstUnitContractor = contractors.GroupBy(x => x.ReferenceObject)
                            .Select(x => new UnitContractor()
                            {
                                ReferenceObject = x.Key,
                                Contractors = x.GroupBy(d => d.VendorCode).Select(y => new Contractor()
                                {
                                    Code = y.First().VendorCode,
                                    Name = y.First().VendorName
                                })
                            }).ToList();
                    }
                    else
                    {
                        lstUnitContractor = new List<UnitContractor>();
                    }

                    return lstUnitContractor;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }
        }


        public async Task<IEnumerable<UnitContractor>> GetUnitContractorDetailsAsync(string username, string[] ReferenceObjects)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var usr = await cntxt.User.Where(x => x.UserName == username).FirstOrDefaultAsync();

                    if (usr == null)
                        throw new ApplicationException("User does not exists");

                    var contractors = new List<VWContractors>();

                    if (usr.UserTypeCode == "CONT")
                    {
                        contractors = await cntxt.VWContractors.Where(x => ReferenceObjects.Contains(x.ReferenceObject) && x.ContractorUserName == username).ToListAsync();
                    }
                    else
                    {
                        contractors = await cntxt.VWContractors.Where(x => ReferenceObjects.Contains(x.ReferenceObject)).ToListAsync();
                    }

                    var lstUnitContractor = new List<UnitContractor>();

                    if (contractors.Count() > 0)
                    {
                        lstUnitContractor = contractors.GroupBy(x => x.ReferenceObject)
                            .Select(x => new UnitContractor()
                            {
                                ReferenceObject = x.Key,
                                Contractors = x.GroupBy(d => d.VendorCode).Select(y => new Contractor()
                                {
                                    Code = y.First().VendorCode,
                                    Name = y.First().VendorName
                                })
                            }).ToList();
                    }
                    else
                    {
                        lstUnitContractor = new List<UnitContractor>();
                    }

                    return lstUnitContractor;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }
        }


        public async Task<IEnumerable<MilestonePercentage>> GetUnitMilestonePercentageAsync(string[] ReferenceObjects)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    //Get the Signed only contractor
                    var x = await (from otc in cntxt.InventoryUnitOTC.Where(o => ReferenceObjects.Contains(o.ReferenceObject))
                                   join mct in cntxt.InventoryUnitOTCManagingContractor
                                   on otc.OTCNumber equals mct.OTCNumber
                                   join rco in cntxt.InventoryUnitOTCContractorAwarded
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
                                   join uct in cntxt.InventoryUnitOTCContractor
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
                                   join loa in cntxt.InventoryUnitOTCContractorAwardedLOA
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
                                   join pun in cntxt.MilestonePunchlist
                                   on mlc.RecordNumber equals pun.MilestoneRecordNumber
                                   join sts in cntxt.MilestonePunchListStatus
                                   on pun.CurrentStatusID equals sts.StatusID
                                   join ott in cntxt.OTCType
                                   on otc.OTCTypeCode equals ott.Code
                                   join mas in cntxt.InventoryUnitOTCQualified
                                   on otc.OTCNumber equals mas.OTCNumber
                                   join cmi in cntxt.ConstructionMilestone
                                   on mlc.ConstructionMilestoneCode equals cmi.Code
                                   where uct.IsIssuedNTP == true &&
                                    uct.IsHold == false
                                   select new
                                   {
                                       ReferenceObject = otc.ReferenceObject,
                                       Punchlist = pun,
                                       PunchlistStatus = sts
                                   }).Distinct().ToListAsync();

                var lstUnitContractor = new List<MilestonePercentage>();
                foreach (var inv in x.Select(i => i.ReferenceObject).Distinct())
                {
                    lstUnitContractor.Add(new MilestonePercentage
                    {
                        ReferenceObject = inv,
                        PunchlistCount = x.Where(c => c.ReferenceObject == inv).Select(c => new { c.ReferenceObject, c.Punchlist }).Distinct().Count(),
                        PunchlistOverdueCount = x.Where(c => c.ReferenceObject == inv && c.PunchlistStatus.PunchlistDueDate.Value.Date < DateTime.Now.Date && c.PunchlistStatus.PunchlistStatusCode != "CLOS").Select(c => new { c.ReferenceObject, c.PunchlistStatus }).Distinct().Count(),
                        PunchlistPendingCount = x.Where(c => c.ReferenceObject == inv && (c.PunchlistStatus.PunchlistStatusCode == "OPEN" || c.PunchlistStatus.PunchlistStatusCode == "INPR")).Select(c => new { c.ReferenceObject, c.PunchlistStatus }).Distinct().Count()
                    });
                }

                    return lstUnitContractor;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }






        public async Task<MilestonePercentage> GetNewUnitMilestonePercentageAsync(string username, string ReferenceObjects)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var data = await cntxt.VWUnitMilestonesPunchlists
                                .Where(x => x.UserName == username
                                    && x.ReferenceObject == ReferenceObjects)
                                .Distinct().ToListAsync();

                    var punCount = data.Count();

                    var punOverdueCount = data.Where(x => x.PunchlistDueDate < DateTime.Now.Date).Count();

                    var punPendingCount = data.Where(x => x.PunchlistStatusCode == "OPEN" || x.PunchlistStatusCode == "INPR").Count();


                    var milestonePercentage = new MilestonePercentage();

                    if (data.Count() > 0)
                    {
                        milestonePercentage = new MilestonePercentage
                        {
                            ReferenceObject = ReferenceObjects,
                            PunchlistCount = punCount,
                            PunchlistOverdueCount = punOverdueCount,
                            PunchlistPendingCount = punPendingCount
                        };

                        return milestonePercentage;
                    }
                    else
                    {
                        throw new NullReferenceException($"No punchlist found for user: {username}");
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
        }

        public async Task<IEnumerable<MilestonePercentage>> GetNewUnitMilestonePercentageAsync(string username, string[] ReferenceObjects)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var data = await cntxt.VWUnitMilestonesPunchlists
                                .Where(x => x.UserName == username
                                    && ReferenceObjects.Contains(x.ReferenceObject))
                                .ToListAsync();

                    if (data != null)
                    {
                        var milestonePercentageCount = data.GroupBy(x => new { x.ReferenceObject, x.PunchlistID })
                        .Select(p => new
                        {
                            ReferenceObject = p.Key.ReferenceObject,
                            PunchId = p.Key.PunchlistID,
                            PunchlistStatusCode = p.First().PunchlistStatusCode,
                            PunchlistDueDate = p.First().PunchlistDueDate
                        })
                        .GroupBy(p => p.ReferenceObject)
                        .Select(p => new MilestonePercentage
                        {
                            ReferenceObject = p.Key,
                            PunchlistCount = p.Count(),
                            PunchlistOverdueCount = p.Where(x => x.PunchlistDueDate.HasValue && x.PunchlistDueDate.Value.Date < DateTime.Now.Date && x.PunchlistStatusCode != "CLOS").Count(),
                            PunchlistPendingCount = p.Where(x => x.PunchlistStatusCode == "OPEN" || x.PunchlistStatusCode == "INPR").Count()
                        });

                        return milestonePercentageCount;
                    }
                    else
                    {
                        throw new NullReferenceException($"No punchlist found for user: {username}");
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
        }

        public async Task<IEnumerable<Unit>> GetUnitByRefObject(string username, string ReferenceObject)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var usr = await cntxt.User.Where(u => u.UserName == username).FirstOrDefaultAsync();

                    if (usr == null)
                        throw new NullReferenceException("User does not exist");

                    var data = new List<CTI.HI.Business.Entities.Unit>();


                    //if COntractor 
                    if (usr.UserTypeCode == "CONT")
                    {
                        data = await cntxt.VWUnitMilestones
                                .Where(x => x.UserName == username
                                    && x.ReferenceObject == ReferenceObject
                                    && x.IsContractor == true
                                    && x.UnitLatitude != null && x.UnitLatitude > 0 && x.UnitLongitude != null && x.UnitLongitude > 0).Distinct()
                                .Select(x => new Unit
                                {
                                    ReferenceObject = x.ReferenceObject,
                                    Project = new Business.Entities.Project
                                    {
                                        Code = x.ProjectCode,
                                        LongName = x.ProjectName,
                                        ShortName = x.ProjectShortName,
                                        Latitude = x.Latitude ?? 0,
                                        Longitude = x.Longitude ?? 0
                                    },
                                    PhaseBuilding = new Business.Entities.PhaseBuilding
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
                                    LotUnitShareNumber = x.Lot,
                                    Longitude = x.UnitLongitude ?? 0,
                                    Latitude = x.UnitLatitude ?? 0,
                                    MilestonePercentage = new MilestonePercentage()
                                    {
                                        PercentageCompletion = cntxt.AssignInventoryUnitPhysicalCondition
                                            .Where(d => d.ReferenceObject == x.ReferenceObject && d.PhysicalConditionCode == "00005")
                                            .Select(p => p.Percentage).FirstOrDefault() ?? 0,
                                    },
                                }).Distinct().OrderBy(x => x.LotUnitShareNumber).ToListAsync();
                    }
                    else
                    {
                        data = await cntxt.VWUnitMilestones
                                .Where(x => x.UserName == username
                                    && x.ReferenceObject == ReferenceObject
                                    && x.IsContractor == false && x.ProjectRoleCode != null
                                    && x.UnitLatitude != null && x.UnitLatitude > 0 && x.UnitLongitude != null && x.UnitLongitude > 0).Distinct()
                                .Select(x => new Unit
                                {
                                    ReferenceObject = x.ReferenceObject,
                                    Project = new Business.Entities.Project
                                    {
                                        Code = x.ProjectCode,
                                        LongName = x.ProjectName,
                                        ShortName = x.ProjectShortName,
                                        Latitude = x.Latitude ?? 0,
                                        Longitude = x.Longitude ?? 0
                                    },
                                    PhaseBuilding = new Business.Entities.PhaseBuilding
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
                                    LotUnitShareNumber = x.Lot,
                                    Longitude = x.UnitLongitude ?? 0,
                                    Latitude = x.UnitLatitude ?? 0,
                                    MilestonePercentage = new MilestonePercentage()
                                    {

                                        PercentageCompletion = cntxt.AssignInventoryUnitPhysicalCondition
                                            .Where(d => d.ReferenceObject == x.ReferenceObject && d.PhysicalConditionCode == "00005")
                                            .Select(p => p.Percentage).FirstOrDefault() ?? 0,
                                    },
                                }).Distinct().OrderBy(x => x.LotUnitShareNumber).ToListAsync();
                    }

                    if (data.Count() > 0)
                    {
                        return data;
                    }
                    else
                    {
                        throw new NullReferenceException($"No unit found for reference object: {ReferenceObject} and user: {username}");
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
        }

        public async Task<IEnumerable<Unit>> GetUnitByRefObjectByVendor(string username, string ReferenceObject,string vendorCode)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var usr = await cntxt.User.Where(u => u.UserName == username).FirstOrDefaultAsync();

                    if (usr == null)
                        throw new NullReferenceException("User does not exist");

                    var data = new List<CTI.HI.Business.Entities.Unit>();


                    //if COntractor 
                    if (usr.UserTypeCode == "CONT")
                    {
                        data = await cntxt.VWUnitMilestones
                                .Where(x => x.UserName == username
                                    && x.ReferenceObject == ReferenceObject
                                    && x.VendorCode == vendorCode
                                    && x.IsContractor == true
                                    && x.UnitLatitude != null && x.UnitLatitude > 0 && x.UnitLongitude != null && x.UnitLongitude > 0).Distinct()
                                .Select(x => new Unit
                                {
                                    ReferenceObject = x.ReferenceObject,
                                    Project = new Business.Entities.Project
                                    {
                                        Code = x.ProjectCode,
                                        LongName = x.ProjectName,
                                        ShortName = x.ProjectShortName,
                                        Latitude = x.Latitude ?? 0,
                                        Longitude = x.Longitude ?? 0
                                    },
                                    PhaseBuilding = new Business.Entities.PhaseBuilding
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
                                    LotUnitShareNumber = x.Lot,
                                    Longitude = x.UnitLongitude ?? 0,
                                    Latitude = x.UnitLatitude ?? 0,
                                    MilestonePercentage = new MilestonePercentage()
                                    {
                                        Contractors = x.VendorName,
                                        PercentageCompletion = cntxt.AssignInventoryUnitPhysicalCondition
                                            .Where(d => d.ReferenceObject == x.ReferenceObject && d.PhysicalConditionCode == "00005")
                                            .Select(p => p.Percentage).FirstOrDefault() ?? 0,
                                    },
                                    VendorCode = x.VendorCode
                                }).Distinct().OrderBy(x => x.LotUnitShareNumber).ToListAsync();
                    }
                    else
                    {
                        data = await cntxt.VWUnitMilestones
                                .Where(x => x.UserName == username
                                    && x.ReferenceObject == ReferenceObject
                                    && x.IsContractor == false
                                    && x.VendorCode == vendorCode
                                    && x.ProjectRoleCode != null
                                    && x.UnitLatitude != null && x.UnitLatitude > 0 && x.UnitLongitude != null && x.UnitLongitude > 0).Distinct()
                                .Select(x => new Unit
                                {
                                    ReferenceObject = x.ReferenceObject,
                                    Project = new Business.Entities.Project
                                    {
                                        Code = x.ProjectCode,
                                        LongName = x.ProjectName,
                                        ShortName = x.ProjectShortName,
                                        Latitude = x.Latitude ?? 0,
                                        Longitude = x.Longitude ?? 0
                                    },
                                    PhaseBuilding = new Business.Entities.PhaseBuilding
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
                                    LotUnitShareNumber = x.Lot,
                                    Longitude = x.UnitLongitude ?? 0,
                                    Latitude = x.UnitLatitude ?? 0,
                                    MilestonePercentage = new MilestonePercentage()
                                    {
                                        Contractors = x.VendorName,
                                        PercentageCompletion = cntxt.AssignInventoryUnitPhysicalCondition
                                            .Where(d => d.ReferenceObject == x.ReferenceObject && d.PhysicalConditionCode == "00005")
                                            .Select(p => p.Percentage).FirstOrDefault() ?? 0,
                                    },
                                    VendorCode = x.VendorCode
                                }).Distinct().OrderBy(x => x.LotUnitShareNumber).ToListAsync();
                    }

                    if (data != null)
                    {
                        return data;
                    }
                    else
                    {
                        throw new NullReferenceException($"No unit found for reference object: {ReferenceObject} and user: {username}");
                    }
                }
                catch (NullReferenceException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<Unit>> GetNewUnitByUser(string username)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var usr = await cntxt.User.Where(u => u.UserName == username).FirstOrDefaultAsync();

                    if (usr == null)
                        throw new NullReferenceException("User does not exist");

                    var data = new List<CTI.HI.Business.Entities.Unit>();


                    //if COntractor 
                    if (usr.UserTypeCode == "CONT")
                    {
                        data = await cntxt.VWUnitMilestones.Where(x => x.UserName == username && x.IsContractor == true 
                                && x.UnitLatitude != null && x.UnitLatitude > 0  && x.UnitLongitude != null && x.UnitLongitude > 0).Distinct()
                                .Select(x => new Unit
                                {
                                    ReferenceObject = x.ReferenceObject,
                                    Project = new Business.Entities.Project
                                    {
                                        Code = x.ProjectCode,
                                        LongName = x.ProjectName,
                                        ShortName = x.ProjectShortName,
                                        Latitude = x.Latitude ?? 0,
                                        Longitude = x.Longitude ?? 0
                                    },
                                    PhaseBuilding = new Business.Entities.PhaseBuilding
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
                                    LotUnitShareNumber = x.Lot,
                                    Longitude = x.UnitLongitude ?? 0,
                                    Latitude = x.UnitLatitude ?? 0,
                                    MilestonePercentage = new MilestonePercentage()
                                    {
                                        PercentageCompletion = cntxt.AssignInventoryUnitPhysicalCondition
                                            .Where(d => d.ReferenceObject == x.ReferenceObject && d.PhysicalConditionCode == "00005")
                                            .Select(p => p.Percentage).FirstOrDefault() ?? 0,
                                        //PunchlistCount = x.PunchListCount,
                                        //PunchlistOverdueCount = x.PunchListOverDueCount,
                                        //PunchlistPendingCount = x.PunchListPendingCount
                                    },

                                }).Distinct().OrderBy(x => x.LotUnitShareNumber).ToListAsync();
                    }
                    else
                    {
                        data = await cntxt.VWUnitMilestones.Where(x => x.UserName == username && x.IsContractor == false && x.ProjectRoleCode != null
                                && x.UnitLatitude != null && x.UnitLatitude > 0 && x.UnitLongitude != null && x.UnitLongitude > 0).Distinct()
                                .Select(x => new Unit
                                {
                                    ReferenceObject = x.ReferenceObject,
                                    Project = new Business.Entities.Project
                                    {
                                        Code = x.ProjectCode,
                                        LongName = x.ProjectName,
                                        ShortName = x.ProjectShortName,
                                        Latitude = x.Latitude ?? 0,
                                        Longitude = x.Longitude ?? 0
                                    },
                                    PhaseBuilding = new Business.Entities.PhaseBuilding
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
                                    LotUnitShareNumber = x.Lot,
                                    Longitude = x.UnitLongitude ?? 0,
                                    Latitude = x.UnitLatitude ?? 0,
                                    MilestonePercentage = new MilestonePercentage()
                                    {
                                        PercentageCompletion = cntxt.AssignInventoryUnitPhysicalCondition
                                            .Where(d => d.ReferenceObject == x.ReferenceObject && d.PhysicalConditionCode == "00005")
                                            .Select(p => p.Percentage).FirstOrDefault() ?? 0,
                                        //PunchlistCount = x.PunchListCount,
                                        //PunchlistOverdueCount = x.PunchListOverDueCount,
                                        //PunchlistPendingCount = x.PunchListPendingCount
                                    },

                                }).Distinct().OrderBy(x => x.LotUnitShareNumber).ToListAsync();
                    }

                    if (data.Count() > 0)
                    {
                        return data;
                    }
                    else
                    {
                        throw new NullReferenceException($"No unit found for user: {username}");
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
        }

        public async Task<IEnumerable<Unit>> GetNewUnitByProject(string username, string projectcode)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var usr = await cntxt.User.Where(u => u.UserName == username).FirstOrDefaultAsync();

                    if (usr == null)
                        throw new NullReferenceException("User does not exist");

                    var data = new List<CTI.HI.Business.Entities.Unit>();


                    //if COntractor 
                    if (usr.UserTypeCode == "CONT")
                    {
                        data = await cntxt.VWUnitMilestones
                                 .Where(x => x.UserName == username
                                 && x.ProjectCode == projectcode && x.IsContractor == true
                                 && x.Latitude != null && x.Latitude > 0 && x.Longitude != null && x.Longitude > 0).Distinct()
                                 .Select(x => new Unit
                                 {
                                     ReferenceObject = x.ReferenceObject,
                                     Project = new Business.Entities.Project
                                     {
                                         Code = x.ProjectCode,
                                         LongName = x.ProjectName,
                                         ShortName = x.ProjectShortName,
                                         Latitude = x.Latitude ?? 0,
                                         Longitude = x.Longitude ?? 0
                                     },
                                     PhaseBuilding = new Business.Entities.PhaseBuilding
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
                                     LotUnitShareNumber = x.Lot,
                                     Longitude = x.UnitLongitude ?? 0,
                                     Latitude = x.UnitLatitude ?? 0,
                                     MilestonePercentage = new MilestonePercentage()
                                     {
                                         PercentageCompletion = cntxt.AssignInventoryUnitPhysicalCondition
                                            .Where(d => d.ReferenceObject == x.ReferenceObject && d.PhysicalConditionCode == "00005")
                                            .Select(p => p.Percentage).FirstOrDefault() ?? 0,
                                     },
                                 }).Distinct().OrderBy(x => x.LotUnitShareNumber).ToListAsync();
                    }
                    else
                    {
                        data = await cntxt.VWUnitMilestones
                                 .Where(x => x.UserName == username
                                 && x.ProjectCode == projectcode && x.IsContractor == false
                                 && x.ProjectRoleCode != null
                                 && x.Latitude != null && x.Latitude > 0 && x.Longitude != null && x.Longitude > 0).Distinct()
                                 .Select(x => new Unit
                                 {
                                     ReferenceObject = x.ReferenceObject,
                                     Project = new Business.Entities.Project
                                     {
                                         Code = x.ProjectCode,
                                         LongName = x.ProjectName,
                                         ShortName = x.ProjectShortName,
                                         Latitude = x.Latitude ?? 0,
                                         Longitude = x.Longitude ?? 0
                                     },
                                     PhaseBuilding = new Business.Entities.PhaseBuilding
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
                                     LotUnitShareNumber = x.Lot,
                                     Longitude = x.UnitLongitude ?? 0,
                                     Latitude = x.UnitLatitude ?? 0,
                                     MilestonePercentage = new MilestonePercentage()
                                     {
                                         PercentageCompletion = cntxt.AssignInventoryUnitPhysicalCondition
                                            .Where(d => d.ReferenceObject == x.ReferenceObject && d.PhysicalConditionCode == "00005")
                                            .Select(p => p.Percentage).FirstOrDefault() ?? 0,
                                     },
                                 }).Distinct().OrderBy(x => x.LotUnitShareNumber).ToListAsync();
                    }



                    if (data.Count() > 0)
                    {
                        return data;
                    }
                    else
                    {
                        throw new NullReferenceException($"No unit found for project: {projectcode} and user: {username}");
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
        }









        public async Task<string[]> FilterInternalPTGReferenceObjectsAsync(string[] referenceObjectArray)
        {
            using (var cntxt = new FrebasContext())
            {
                //Get Internal PTG Only
                //var xx= await cntxt.InventoryUnitPreConstruct.Where(p => p.ContractorType == "INTE" &&
                //                                                    referenceObjectArray.Contains(p.ReferenceObject))
                //                                            .Select(p => p.ReferenceObject).ToArrayAsync();

                await Task.Delay(100);
                return referenceObjectArray;
            }
        }

        public async Task<UnitModel> GetUnitByOTCNumberAsync(string otcNumber)
        {
            using (var cntxt = new FrebasContext())
            {
                //Get the Signed only contractor
                return await (from otc in cntxt.InventoryUnitOTC.Where(o => o.OTCNumber == otcNumber)
                              join inv in GetUnitModelQueryable(cntxt)
                              on otc.ReferenceObject equals inv.ReferenceObject
                              select inv
                              ).FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<Unit>> GetUnitByBlockAsync(string username, string block)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {

                    var usr = await cntxt.User.Where(u => u.UserName == username).FirstOrDefaultAsync();

                    if (usr == null)
                        throw new NullReferenceException("User does not exist");

                    var data = new List<CTI.HI.Business.Entities.Unit>();


                    //if COntractor 
                    if (usr.UserTypeCode == "CONT")
                    {
                        data = await cntxt.VWUnitMilestones.Where(x => x.UserName == username
                                 && x.BlockCode == block && x.IsContractor == true
                                 && x.UnitLatitude != null && x.UnitLatitude > 0 && x.UnitLongitude != null && x.UnitLongitude > 0).Distinct()
                                 .Select(x => new Unit
                                 {
                                     ReferenceObject = x.ReferenceObject,
                                     Project = new Business.Entities.Project
                                     {
                                         Code = x.ProjectCode,
                                         LongName = x.ProjectName,
                                         ShortName = x.ProjectShortName
                                     },
                                     PhaseBuilding = new Business.Entities.PhaseBuilding
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
                                     LotUnitShareNumber = x.Lot,
                                     Longitude = x.UnitLongitude ?? 0,
                                     Latitude = x.UnitLatitude ?? 0,
                                     MilestonePercentage = new MilestonePercentage()
                                     {
                                         Contractors = x.VendorName,
                                         PercentageCompletion = cntxt.AssignInventoryUnitPhysicalCondition
                                            .Where(d => d.ReferenceObject == x.ReferenceObject && d.PhysicalConditionCode == "00005")
                                            .Select(p => p.Percentage).FirstOrDefault() ?? 0,
                                     },
                                     VendorCode = x.VendorCode
                                 }).OrderBy(x => x.LotUnitShareNumber).Distinct().ToListAsync();
                    }
                    else
                    {
                        data = await cntxt.VWUnitMilestones.Where(x => x.UserName == username
                                && x.BlockCode == block && x.IsContractor == false && x.ProjectRoleCode != null
                                && x.UnitLatitude != null && x.UnitLatitude > 0 && x.UnitLongitude != null && x.UnitLongitude > 0).Distinct()
                                .Select(x => new Unit
                                {
                                    ReferenceObject = x.ReferenceObject,
                                    Project = new Business.Entities.Project
                                    {
                                        Code = x.ProjectCode,
                                        LongName = x.ProjectName,
                                        ShortName = x.ProjectShortName,
                                        Latitude = x.Latitude ?? 0,
                                        Longitude = x.Longitude ?? 0
                                    },
                                    PhaseBuilding = new Business.Entities.PhaseBuilding
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
                                    LotUnitShareNumber = x.Lot,
                                    Longitude = x.UnitLongitude ?? 0,
                                    Latitude = x.UnitLatitude ?? 0,
                                    MilestonePercentage = new MilestonePercentage()
                                    {
                                        Contractors = x.VendorName,
                                        PercentageCompletion =
                                        cntxt.AssignInventoryUnitPhysicalCondition
                                            .Where(d => d.ReferenceObject == x.ReferenceObject && d.PhysicalConditionCode == "00005")
                                            .Select(p => p.Percentage).FirstOrDefault() ?? 0,
                                    },
                                    VendorCode = x.VendorCode
                                }).OrderBy(x => x.LotUnitShareNumber).Distinct().ToListAsync();
                    }



                    if (data.Count() > 0)
                    {
                        return data;
                    }
                    else
                    {
                        throw new NullReferenceException($"No unit found for block: {block} and user: {username}");
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

        #region "Others"
        protected override InventoryUnit AddEntity(FrebasContext entityContext, InventoryUnit entity)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<InventoryUnit> GetEntities(FrebasContext entityContext)
        {
            throw new NotImplementedException();
        }

        protected override InventoryUnit GetEntity(FrebasContext entityContext, int id)
        {
            throw new NotImplementedException();
        }

        protected override InventoryUnit UpdateEntity(FrebasContext entityContext, InventoryUnit entity)
        {
            throw new NotImplementedException();
        }







        #endregion

    }
}
