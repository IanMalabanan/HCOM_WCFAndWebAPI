using CTI.HCM.Business.Entities;
using CTI.HI.Business.Entities;
using CTI.HI.Data.Context;
using CTI.HI.Data.Contracts.Frebas;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Data.Repository.Frebas
{
    [Export(typeof(IPunchlistRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PunchlistRepository : DataRepositoryFrebasBase<PunchList>, IPunchlistRepository
    {
        private IContractorRepository _conRepo;
        private IUserProjectRepository _userProjRepo;
        private DateTime _DateToday = DateTime.Now.Date;
        private int _NotifRecentDays = 7;
        public PunchlistRepository()
        {
            _conRepo = new ContractorRepository();
            _userProjRepo = new UserProjectRepository();
        }

        private IQueryable<Business.Entities.Punchlist> GetMilestonePunchlistQueryable(FrebasContext cntxt)
        {
            return cntxt.VWUnitMilestonesPunchlists.Distinct()
                .Select(x => new Punchlist
                {
                    PunchListID = x.PunchlistID,
                    ConstructionMilestoneId = x.ConstructionMilestoneID,
                    OTCNumber = x.ConstructionMilestoneOTCNumber,
                    ContractorNumber = x.ConstructionMilestoneContractorNumber,
                    ManagingContractorID = x.ConstructionMilestoneManagingContractorID,
                    MilestoneCode = x.MilestoneCode,
                    PunchListCategory = x.PunchListCategoryCode,
                    PunchListSubCategory = x.PunchListSubCategoryCode,
                    NonCompliantTo = x.PunchListCategoryNonComplianceCode,
                    ReferenceSheet = x.ReferenceSheet,
                    PunchListDescription = x.PunchListDescription,
                    PunchListDescriptionDetails = new PunchlistDescription
                    {
                        Code = x.PunchListCode,
                        Name = x.PunchListDescription,
                        GroupCode = x.PunchlistGroupCode
                    },
                    PunchListGroup = x.PunchlistGroupCode,
                    PunchListLocation = x.PunchListLocationCode,
                    CostImpact = x.PunchListCostImpactCode,
                    ScheduleImpact = x.PunchListScheduleImpactCode,
                    AssignedTo = x.AuthorizedPersonnelID,
                    PunchListStatus = x.PunchlistStatusCode,
                    PunchListStatusDetail = new Business.Entities.PunchlistStatus
                    {
                        Code = x.PunchlistStatusCode,
                        Name = x.PunchListStatus
                    },
                    DueDate = x.PunchlistDueDate,
                    ReferenceNumber = x.ReferenceNumber,
                }).Distinct().OrderBy(x => x.PunchListDescription);
        }
        private IQueryable<Business.Entities.Comment> GetPunchlistCommentQueryable(FrebasContext cntxt, int[] PunchlistIds)
        {
            var _dateToday = DateTime.Now.Date;
            //var _statuses = await cntxt.MilestonePunchListStatus.Where(s => s.PunchlistID == _punchlist.PunchListID).ToListAsync();
            return (from cmt in cntxt.MilestonePunchListComments
                    join sts in cntxt.MilestonePunchListStatus.Where(s => PunchlistIds.Contains(s.PunchlistID))
                    on cmt.StatusID equals sts.StatusID
                    join std in cntxt.PunchListStatus
                    on sts.PunchlistStatusCode equals std.Code
                    select new Comment
                    {
                        PunchlistId = sts.PunchlistID,
                        CommentId = cmt.CommentID,
                        AttachmentFileName = cntxt.PunchListImage
                                                .Where(i => i.CommentID == cmt.CommentID)
                                                .Select(i => i.FileName)
                                                .ToList(),
                        ImageUrl = cntxt.PunchListImage
                                        .Where(i => i.CommentID == cmt.CommentID)
                                        .Select(i => i.FileName)
                                        .ToList(),
                        CreatedByUsername = cmt.CommentTagBy,
                        CreatedDate = cmt.DateCreated,
                        Message = cmt.Comments,
                        DueDate = sts.PunchlistDueDate ?? _dateToday,
                        PunchlistStatus = new PunchlistStatus
                        {
                            Code = sts.PunchlistStatusCode,
                            Name = std.Description
                        }
                    });

            //return cntxt.VWUnitMilestonesPunchlistComments
            //        .Select(d => new Comment
            //        {
            //            CommentId = d.CommentID,
            //            AttachmentFileName = cntxt.PunchListImage
            //                                      .Where(i => i.CommentID == d.CommentID)
            //                                      .Select(i => i.FileName)
            //                                      .ToList(),
            //            ImageUrl = cntxt.PunchListImage
            //                              .Where(i => i.CommentID == d.CommentID)
            //                              .Select(i => i.FileName)
            //                              .ToList(),
            //            CreatedByUsername = d.CommentTagBy,
            //            CreatedDate = d.CommentDateCreated ?? DateTime.Now,
            //            Message = d.PunchlistComments,
            //            DueDate = d.PunchListCommentDueDate ?? DateTime.Now,
            //            PunchlistStatus = new PunchlistStatus
            //            {
            //                Code = d.PunchlistStatusCode,
            //                Name = d.PunchListStatus
            //            }
            //        });
        }

        public async Task<Business.Entities.Punchlist> GetPunchlistAsync(int PunchlistId)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    return await cntxt.MilestonePunchlist.Where(x => x.PunchlistID == PunchlistId).Select(x => new Business.Entities.Punchlist { CreatedBy = x.CreatedBy }).FirstOrDefaultAsync();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        public async Task<Business.Entities.Punchlist> GetMilestonePunchlistAsync(int PunchlistId)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    var _dteToday = DateTime.Today;

                    var _punchlist = await cntxt.VWUnitMilestonesPunchlists.Where(p => p.PunchlistID == PunchlistId)
                                        .Select(x => new Punchlist
                                        {
                                            PunchListID = x.PunchlistID,
                                            ConstructionMilestoneId = x.ConstructionMilestoneID,
                                            OTCNumber = x.ConstructionMilestoneOTCNumber,
                                            ContractorNumber = x.ConstructionMilestoneContractorNumber,
                                            ManagingContractorID = x.ConstructionMilestoneManagingContractorID,
                                            MilestoneCode = x.MilestoneCode,
                                            PunchListCategory = x.PunchListCategoryCode,
                                            PunchListSubCategory = x.PunchListSubCategoryCode,
                                            NonCompliantTo = x.PunchListCategoryNonComplianceCode,
                                            ReferenceSheet = x.ReferenceSheet,
                                            PunchListDescription = x.PunchListDescription,
                                            PunchListDescriptionDetails = new PunchlistDescription
                                            {
                                                Code = x.PunchListCode,
                                                Name = x.PunchListDescription,
                                                GroupCode = x.PunchlistGroupCode
                                            },
                                            PunchListGroup = x.PunchlistGroupCode,
                                            PunchListLocation = x.PunchListLocationCode,
                                            CostImpact = x.PunchListCostImpactCode,
                                            ScheduleImpact = x.PunchListScheduleImpactCode,
                                            AssignedTo = x.AuthorizedPersonnelID,
                                            PunchListStatus = x.PunchlistStatusCode,

                                            PunchListStatusDetail = new Business.Entities.PunchlistStatus
                                            {
                                                Code = x.PunchlistStatusCode,
                                                Name = x.PunchListStatus
                                            },
                                            DueDate = x.PunchlistDueDate,
                                            ReferenceNumber = x.ReferenceNumber,

                                        })
                                        .FirstOrDefaultAsync();

                    if (_punchlist == null)
                        return _punchlist;

                    return _punchlist;
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

        public async Task<IEnumerable<Business.Entities.Comment>> GetPunchlistCommentsAsync(int PunchlistID)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    var data = await cntxt.VWUnitMilestonesPunchlistComments
                        .Where(c => c.PunchlistID == PunchlistID)
                                                                   .Select(d => new Comment
                                                                   {
                                                                       CommentId = d.CommentID,
                                                                       AttachmentFileName = cntxt.PunchListImage
                                                                                                 .Where(i => i.CommentID == d.CommentID)
                                                                                                 .Select(i => i.FileName)
                                                                                                 .ToList() ?? new List<string>() { },
                                                                       ImageUrl = cntxt.PunchListImage
                                                                                         .Where(i => i.CommentID == d.CommentID)
                                                                                         .Select(i => i.FileName).ToList() ?? new List<string>() {  },

                                                                       CreatedByUsername = d.CommentTagBy,
                                                                       CreatedDate = d.CommentDateCreated ?? DateTime.Now,
                                                                       Message = d.PunchlistComments,
                                                                       DueDate = d.PunchListCommentDueDate ?? DateTime.Now,
                                                                       PunchlistStatus = new PunchlistStatus
                                                                       {
                                                                           Code = d.PunchlistStatusCode,
                                                                           Name = d.PunchListStatus
                                                                       }
                                                                   }).OrderByDescending(g => g.CreatedDate).ToListAsync() ?? new List<Comment>();
                    return data;
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


        public async Task<IEnumerable<Business.Entities.Punchlist>> GetMilestonePunchlistsByUnitAsync(string username, string referenceObject)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var _dteToday = DateTime.Today;
                    var _dteTomorrow = DateTime.Today.AddDays(1).Date;

                    var data = await cntxt.VWUnitMilestonesPunchlists
                        .Where(x => x.UserName == username && x.ReferenceObject == referenceObject).Distinct()
                        .Select(x => new Punchlist
                        {
                            PunchListID = x.PunchlistID,
                            OTCNumber = x.ConstructionMilestoneOTCNumber,
                            ContractorNumber = x.ConstructionMilestoneContractorNumber,
                            ManagingContractorID = x.ConstructionMilestoneManagingContractorID,
                            ConstructionMilestoneId = x.ConstructionMilestoneID,
                            PunchListCategory = x.PunchListCategoryCode,
                            PunchListSubCategory = x.PunchListSubCategoryCode,
                            NonCompliantTo = x.PunchListCategoryNonComplianceCode,
                            ReferenceSheet = x.ReferenceSheet,
                            PunchListDescription = x.PunchListDescription,
                            PunchListDescriptionDetails = new PunchlistDescription
                            {
                                Code = x.PunchListCode,
                                Name = x.PunchListDescription,
                            },
                            PunchListGroup = x.PunchlistGroupCode,
                            PunchListLocation = x.PunchListLocationCode,
                            CostImpact = x.PunchListCostImpactCode,
                            ScheduleImpact = x.PunchListScheduleImpactCode,
                            AssignedTo = x.AuthorizedPersonnelID,
                            PunchListStatus = x.PunchlistStatusCode,
                            DueDate = x.PunchlistDueDate ?? _dteToday,
                            PunchListStatusDetail = new Business.Entities.PunchlistStatus
                            {
                                Code = x.PunchlistStatusCode,
                                Name = x.PunchListStatus,
                            },
                        }).OrderBy(x => x.PunchListDescription).ToListAsync();

                    var _punchlist = new List<Punchlist>();

                    if (data.Count() > 0)
                    {
                        Parallel.ForEach(data, item =>

                             _punchlist.Add(new Punchlist
                             {
                                 PunchListID = item.PunchListID,
                                 OTCNumber = item.OTCNumber,
                                 ContractorNumber = item.ContractorNumber,
                                 ManagingContractorID = item.ManagingContractorID,
                                 ConstructionMilestoneId = item.ConstructionMilestoneId,
                                 PunchListCategory = item.PunchListCategory,
                                 PunchListSubCategory = item.PunchListSubCategory,
                                 NonCompliantTo = item.NonCompliantTo,
                                 ReferenceSheet = item.ReferenceSheet,
                                 PunchListDescription = item.PunchListDescription,
                                 PunchListDescriptionDetails = new PunchlistDescription
                                 {
                                     Code = item.PunchListDescriptionDetails.Code,
                                     Name = item.PunchListDescriptionDetails.Name,
                                 },
                                 PunchListGroup = item.PunchListGroup,
                                 PunchListLocation = item.PunchListLocation,
                                 CostImpact = item.CostImpact,
                                 ScheduleImpact = item.ScheduleImpact,
                                 AssignedTo = item.AssignedTo,
                                 PunchListStatus = item.PunchListStatus,
                                 DueDate = item.DueDate ?? _dteToday,
                                 PunchListStatusDetail = new Business.Entities.PunchlistStatus
                                 {
                                     Code = item.PunchListStatusDetail.Code,
                                     Name = item.PunchListStatusDetail.Name,
                                 },
                             })

                            );

                        return _punchlist;
                    }
                    else
                    {
                        return data;
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

        public async Task<IEnumerable<Business.Entities.Punchlist>> GetMilestonePunchlistsByUnitByVendorAsync(string username, string referenceObject, string vendorCode)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var _dteToday = DateTime.Today;
                    var _dteTomorrow = DateTime.Today.AddDays(1).Date;

                    var data = await cntxt.VWUnitMilestonesPunchlists
                        .Where(x => x.UserName == username 
                                && x.ReferenceObject == referenceObject
                                && x.VendorCode == vendorCode).Distinct()
                        .Select(x => new Punchlist
                        {
                            PunchListID = x.PunchlistID,
                            OTCNumber = x.ConstructionMilestoneOTCNumber,
                            ContractorNumber = x.ConstructionMilestoneContractorNumber,
                            ManagingContractorID = x.ConstructionMilestoneManagingContractorID,
                            ConstructionMilestoneId = x.ConstructionMilestoneID,
                            PunchListCategory = x.PunchListCategoryCode,
                            PunchListSubCategory = x.PunchListSubCategoryCode,
                            NonCompliantTo = x.PunchListCategoryNonComplianceCode,
                            ReferenceSheet = x.ReferenceSheet,
                            PunchListDescription = x.PunchListDescription,
                            PunchListDescriptionDetails = new PunchlistDescription
                            {
                                Code = x.PunchListCode,
                                Name = x.PunchListDescription,
                            },
                            PunchListGroup = x.PunchlistGroupCode,
                            PunchListLocation = x.PunchListLocationCode,
                            CostImpact = x.PunchListCostImpactCode,
                            ScheduleImpact = x.PunchListScheduleImpactCode,
                            AssignedTo = x.AuthorizedPersonnelID,
                            PunchListStatus = x.PunchlistStatusCode,
                            DueDate = x.PunchlistDueDate ?? _dteToday,
                            PunchListStatusDetail = new Business.Entities.PunchlistStatus
                            {
                                Code = x.PunchlistStatusCode,
                                Name = x.PunchListStatus,
                            },
                        }).OrderBy(x => x.PunchListDescription).ToListAsync();

                    return data;
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

        public async Task<IEnumerable<Punchlist>> GetMilestonePunchlistsByProjectAsync(string username, string projectCode)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var _dteToday = DateTime.Today;
                    var _dteTomorrow = DateTime.Today.AddDays(1).Date;

                    var data = await cntxt.VWUnitMilestonesPunchlists
                        .Where(x => x.UserName == username && x.ProjectCode == projectCode).Distinct()
                        .Select(x => new Punchlist
                        {
                            PunchListID = x.PunchlistID,
                            OTCNumber = x.ConstructionMilestoneOTCNumber,
                            ContractorNumber = x.ConstructionMilestoneContractorNumber,
                            ManagingContractorID = x.ConstructionMilestoneManagingContractorID,
                            ConstructionMilestoneId = x.ConstructionMilestoneID,
                            PunchListCategory = x.PunchListCategoryCode,
                            PunchListSubCategory = x.PunchListSubCategoryCode,
                            NonCompliantTo = x.PunchListCategoryNonComplianceCode,
                            ReferenceSheet = x.ReferenceSheet,
                            PunchListDescription = x.PunchListDescription,
                            PunchListDescriptionDetails = new PunchlistDescription
                            {
                                Code = x.PunchListCode,
                                Name = x.PunchListDescription,
                            },
                            PunchListGroup = x.PunchlistGroupCode,
                            PunchListLocation = x.PunchListLocationCode,
                            CostImpact = x.PunchListCostImpactCode,
                            ScheduleImpact = x.PunchListScheduleImpactCode,
                            AssignedTo = x.AuthorizedPersonnelID,
                            PunchListStatus = x.PunchlistStatusCode,
                            DueDate = x.PunchlistDueDate ?? _dteToday,
                            PunchListStatusDetail = new Business.Entities.PunchlistStatus
                            {
                                Code = x.PunchlistStatusCode,
                                Name = x.PunchListStatus,
                            },

                        }).OrderBy(x => x.PunchListDescription).ToListAsync();

                    if (data.Count() > 0)
                    {
                        data = data.GroupBy(d => d.PunchListID).Select(y => y.First()).ToList();



                        var _punchlist = data.Select(item => new Punchlist
                        {
                            PunchListID = item.PunchListID,
                            OTCNumber = item.OTCNumber,
                            ContractorNumber = item.ContractorNumber,
                            ManagingContractorID = item.ManagingContractorID,
                            ConstructionMilestoneId = item.ConstructionMilestoneId,
                            PunchListCategory = item.PunchListCategory,
                            PunchListSubCategory = item.PunchListSubCategory,
                            NonCompliantTo = item.NonCompliantTo,
                            ReferenceSheet = item.ReferenceSheet,
                            PunchListDescription = item.PunchListDescription,
                            PunchListDescriptionDetails = new PunchlistDescription
                            {
                                Code = item.PunchListDescriptionDetails.Code,
                                Name = item.PunchListDescriptionDetails.Name,
                            },
                            PunchListGroup = item.PunchListGroup,
                            PunchListLocation = item.PunchListLocation,
                            CostImpact = item.CostImpact,
                            ScheduleImpact = item.ScheduleImpact,
                            AssignedTo = item.AssignedTo,
                            PunchListStatus = item.PunchListStatus,
                            DueDate = item.DueDate ?? _dteToday,
                            PunchListStatusDetail = new Business.Entities.PunchlistStatus
                            {
                                Code = item.PunchListStatusDetail.Code,
                                Name = item.PunchListStatusDetail.Name,
                            },
                        });



                        //Parallel.ForEach(data, item => _punchlist.Add(new Punchlist
                        //{
                        //    PunchListID = item.PunchListID,
                        //    OTCNumber = item.OTCNumber,
                        //    ContractorNumber = item.ContractorNumber,
                        //    ManagingContractorID = item.ManagingContractorID,
                        //    ConstructionMilestoneId = item.ConstructionMilestoneId,
                        //    PunchListCategory = item.PunchListCategory,
                        //    PunchListSubCategory = item.PunchListSubCategory,
                        //    NonCompliantTo = item.NonCompliantTo,
                        //    ReferenceSheet = item.ReferenceSheet,
                        //    PunchListDescription = item.PunchListDescription,
                        //    PunchListDescriptionDetails = new PunchlistDescription
                        //    {
                        //        Code = item.PunchListDescriptionDetails.Code,
                        //        Name = item.PunchListDescriptionDetails.Name,
                        //    },
                        //    PunchListGroup = item.PunchListGroup,
                        //    PunchListLocation = item.PunchListLocation,
                        //    CostImpact = item.CostImpact,
                        //    ScheduleImpact = item.ScheduleImpact,
                        //    AssignedTo = item.AssignedTo,
                        //    PunchListStatus = item.PunchListStatus,
                        //    DueDate = item.DueDate ?? _dteToday,
                        //    PunchListStatusDetail = new Business.Entities.PunchlistStatus
                        //    {
                        //        Code = item.PunchListStatusDetail.Code,
                        //        Name = item.PunchListStatusDetail.Name,
                        //    },
                        //}));

                        return _punchlist;
                    }
                    else
                    {
                        return data;
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


        public async Task<IEnumerable<Business.Entities.Punchlist>> GetMilestonePunchlistsAsync(Expression<Func<Punchlist, bool>> expression)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var _punchlist = await GetMilestonePunchlistQueryable(cntxt).Where(expression).ToListAsync();

                    if (_punchlist == null)
                        return _punchlist;

                    var _comments = await GetPunchlistCommentQueryable(cntxt, _punchlist.Select(p => p.PunchListID).ToArray()).ToListAsync();

                    _punchlist.ForEach(p =>
                    {
                        p.Comments = _comments.Where(c => c.PunchlistId == p.PunchListID).ToList();
                    });

                    return _punchlist;
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


        public async Task<IEnumerable<Business.Entities.Punchlist>> GetMilestonePunchlistsAsync(int ConstructionMilestoneId)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var data = await (cntxt.VWUnitMilestonesPunchlists.Where(p => p.ConstructionMilestoneID == ConstructionMilestoneId)
                                    .Select(x => new Punchlist
                                    {
                                        PunchListID = x.PunchlistID,
                                        ConstructionMilestoneId = x.ConstructionMilestoneID,
                                        OTCNumber = x.ConstructionMilestoneOTCNumber,
                                        ContractorNumber = x.ConstructionMilestoneContractorNumber,
                                        ManagingContractorID = x.ConstructionMilestoneManagingContractorID,
                                        MilestoneCode = x.MilestoneCode,
                                        PunchListCategory = x.PunchListCategoryCode,
                                        PunchListSubCategory = x.PunchListSubCategoryCode,
                                        NonCompliantTo = x.PunchListCategoryNonComplianceCode,
                                        ReferenceSheet = x.ReferenceSheet,
                                        PunchListDescription = x.PunchListDescription,
                                        PunchListDescriptionDetails = new PunchlistDescription
                                        {
                                            Code = x.PunchListCode,
                                            Name = x.PunchListDescription,
                                            GroupCode = x.PunchlistGroupCode
                                        },
                                        PunchListGroup = x.PunchlistGroupCode,
                                        PunchListLocation = x.PunchListLocationCode,
                                        CostImpact = x.PunchListCostImpactCode,
                                        ScheduleImpact = x.PunchListScheduleImpactCode,
                                        AssignedTo = x.AuthorizedPersonnelID,
                                        PunchListStatus = x.PunchlistStatusCode,
                                        PunchListStatusDetail = new Business.Entities.PunchlistStatus
                                        {
                                            Code = x.PunchlistStatusCode,
                                            Name = x.PunchListStatus
                                        },
                                        DueDate = x.PunchlistDueDate,
                                        ReferenceNumber = x.ReferenceNumber,
                                    }).Distinct()
                                    .OrderBy(x => x.PunchListDescription)).ToListAsync();

                    data = data.GroupBy(d => d.PunchListID).Select(y => y.First()).ToList();

                    return data;
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

        public async Task<IEnumerable<Business.Entities.Punchlist>> GetMilestonePunchlistsAsync(int[] ConstructionMilestoneId)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var data = await (cntxt.VWUnitMilestonesPunchlists.Where(p => ConstructionMilestoneId.Contains(p.ConstructionMilestoneID))
                                    .Select(x => new Punchlist
                                    {
                                        PunchListID = x.PunchlistID,
                                        ConstructionMilestoneId = x.ConstructionMilestoneID,
                                        OTCNumber = x.ConstructionMilestoneOTCNumber,
                                        ContractorNumber = x.ConstructionMilestoneContractorNumber,
                                        ManagingContractorID = x.ConstructionMilestoneManagingContractorID,
                                        MilestoneCode = x.MilestoneCode,
                                        PunchListCategory = x.PunchListCategoryCode,
                                        PunchListSubCategory = x.PunchListSubCategoryCode,
                                        NonCompliantTo = x.PunchListCategoryNonComplianceCode,
                                        ReferenceSheet = x.ReferenceSheet,
                                        PunchListDescription = x.PunchListDescription,
                                        PunchListDescriptionDetails = new PunchlistDescription
                                        {
                                            Code = x.PunchListCode,
                                            Name = x.PunchListDescription,
                                            GroupCode = x.PunchlistGroupCode
                                        },
                                        PunchListGroup = x.PunchlistGroupCode,
                                        PunchListLocation = x.PunchListLocationCode,
                                        CostImpact = x.PunchListCostImpactCode,
                                        ScheduleImpact = x.PunchListScheduleImpactCode,
                                        AssignedTo = x.AuthorizedPersonnelID,
                                        PunchListStatus = x.PunchlistStatusCode,
                                        PunchListStatusDetail = new Business.Entities.PunchlistStatus
                                        {
                                            Code = x.PunchlistStatusCode,
                                            Name = x.PunchListStatus
                                        },
                                        DueDate = x.PunchlistDueDate,
                                        ReferenceNumber = x.ReferenceNumber,
                                    }).Distinct()
                                    .OrderBy(x => x.PunchListDescription)).ToListAsync();

                    data = data.GroupBy(d => d.PunchListID).Select(y => y.First()).ToList();

                    return data;
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

        public async Task<IEnumerable<Comment>> GetPunchListCommentsAsync(int PunchlistId)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    return await GetPunchlistCommentQueryable(cntxt, new int[] { PunchlistId }).ToListAsync();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IEnumerable<Comment>> GetPunchListCommentsAsync(int[] PunchlistId)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var data = await GetPunchlistCommentQueryable(cntxt, PunchlistId).ToListAsync();
                    return data;
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<Comment>> GetPunchListCommentsAsync(int[] PunchlistId,string username)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var data = await cntxt.VWUnitMilestonesPunchlistComments.Where(p => p.UserName == username && PunchlistId.Contains(p.PunchlistID))
                    .Select(d => new Comment
                    {
                        PunchlistId = d.PunchlistID,
                        CommentId = d.CommentID,
                        AttachmentFileName = cntxt.PunchListImage
                                                  .Where(i => i.CommentID == d.CommentID)
                                                  .Select(i => i.FileName)
                                                  .ToList(),
                        ImageUrl = cntxt.PunchListImage
                                          .Where(i => i.CommentID == d.CommentID)
                                          .Select(i => i.FileName)
                                          .ToList(),
                        CreatedByUsername = d.CommentTagBy,
                        CreatedDate = d.CommentDateCreated ?? DateTime.Now,
                        Message = d.PunchlistComments,
                        DueDate = d.PunchListCommentDueDate ?? DateTime.Now,
                        PunchlistStatus = new PunchlistStatus
                        {
                            Code = d.PunchlistStatusCode,
                            Name = d.PunchListStatus
                        }
                    }).ToListAsync();

                    return data;
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

        public async Task<int> SavePunchlistAsync(string username, Business.Entities.Punchlist model)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    var _pnchlist = await cntxt.MilestonePunchlist.Where(p => p.PunchlistID == model.PunchListID).FirstOrDefaultAsync();
                    if (_pnchlist == null)
                        return await InsertPunchlistAsync(cntxt, username, model);
                    else
                        return await UpdatePunchlistAsync(_pnchlist.PunchlistID, cntxt, username, model);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        private async Task<int> InsertPunchlistAsync(FrebasContext cntxt, string username, Business.Entities.Punchlist model)
        {
            using (var transaction = cntxt.Database.BeginTransaction())
            {
                try
                {
                    //Get Punchlist Description
                    //var _pnchlistDescription = cntxt.PunchListSubject.Where(s => s.Name.ToUpper() == model.PunchListDescription.ToUpper()).FirstOrDefault();
                    //if (_pnchlistDescription == null)
                    //{
                    //    var _code = await cntxt.PunchListSubject.CountAsync() + 1;
                    //    _pnchlistDescription = new CTI.HCM.Business.Entities.PunchList();
                    //    _pnchlistDescription.Code = _code.ToString().PadLeft(5, '0');
                    //    _pnchlistDescription.Name = model.PunchListDescription;
                    //    _pnchlistDescription.DateCreated = DateTime.Now;
                    //    _pnchlistDescription.CreatedBy = username;

                    //    cntxt.PunchListSubject.Add(_pnchlistDescription);
                    //    await cntxt.SaveChangesAsync();
                    //}
                    //Get Project Code
                    var _otcNumber = await cntxt.InventoryUnitOTCContractorConstructionMilestone
                                    .Where(m => m.RecordNumber == model.ConstructionMilestoneId)
                                    .Select(m => m.OTCNumber).FirstOrDefaultAsync();
                    var _refObj = await cntxt.InventoryUnitOTC
                                    .Where(o => o.OTCNumber == _otcNumber)
                                    .Select(o => o.ReferenceObject).FirstOrDefaultAsync();
                    var _projectCode = await cntxt.InventoryUnit
                                    .Where(u => u.ReferenceObject == _refObj)
                                    .Select(u => u.ProjectCode).FirstOrDefaultAsync();

                    var _projectDetails = await cntxt.Project.Where(p => p.Code == _projectCode).FirstOrDefaultAsync();

                    //UpdatePunchlist Project Sequence
                    var _proSequence = await cntxt.PunchListProjectSequence
                                        .Where(s => s.ProjectCode == _projectCode).FirstOrDefaultAsync();
                    if (_proSequence == null)
                    {
                        _proSequence = new PunchListProjectSequence
                        {
                            ProjectCode = _projectCode,
                            Sequence = 1,
                            DateCreated = DateTime.Now.ToLocalTime(),
                            CreatedBy = username
                        };
                        cntxt.PunchListProjectSequence.Add(_proSequence);
                    }
                    else
                    {
                        _proSequence.Sequence += 1;
                        _proSequence.DateModified = DateTime.Now.ToLocalTime();
                        _proSequence.ModifiedBy = username;
                    }

                    await cntxt.SaveChangesAsync();



                    //Save PunchlistMainTable
                    var _punchlist = new InventoryUnitOTCContractorConstructionMilestonePunchlist();
                    _punchlist.MilestoneRecordNumber = model.ConstructionMilestoneId;
                    _punchlist.AuthorizedPersonnelID = model.AssignedTo;
                    _punchlist.PunchListCode = model.PunchListDescriptionDetails.Code;
                    _punchlist.PunchListCategoryCode = model.PunchListCategory;
                    _punchlist.PunchListSubCategoryCode = model.PunchListSubCategory;
                    _punchlist.PunchListCategoryNonComplianceCode = model.NonCompliantTo;
                    _punchlist.ReferenceSheet = model.ReferenceSheet;
                    _punchlist.PunchListLocationCode = model.PunchListLocation;
                    _punchlist.PunchListCostImpactCode = model.CostImpact;
                    _punchlist.PunchListScheduleImpactCode = model.ScheduleImpact;
                    _punchlist.ReferenceNumber = $"PL-{_projectDetails.ShortName}-{DateTime.Now.Year.ToString()}-{_proSequence.Sequence.ToString()}";
                    _punchlist.PunchListTagDate = model.DateCreated ?? DateTime.Now.ToLocalTime();
                    _punchlist.PunchListTagBy = username;
                    _punchlist.Source = "HINS"; //House Inspection
                    _punchlist.PunchListTagBy = username;
                    _punchlist.PunchListTagDate = model.DateCreated ?? DateTime.Now.ToLocalTime();
                    _punchlist.DateCreated = DateTime.Now.ToLocalTime();
                    _punchlist.CreatedBy = username;

                    cntxt.MilestonePunchlist.Add(_punchlist);
                    await cntxt.SaveChangesAsync();

                    //Save Status
                    var _punchlistStatus = new InventoryUnitOTCContractorConstructionMilestonePunchListStatus();
                    _punchlistStatus.PunchlistID = _punchlist.PunchlistID;
                    _punchlistStatus.PunchlistStatusCode = model.PunchListStatus;
                    _punchlistStatus.PunchlistDueDate = model.DueDate;
                    _punchlistStatus.Source = "HINS";
                    _punchlistStatus.StatusTagDate = model.DateCreated ?? DateTime.Now.ToLocalTime();
                    _punchlistStatus.StatusTagBy = username;
                    _punchlistStatus.DateCreated = DateTime.Now.ToLocalTime();
                    _punchlistStatus.CreatedBy = username;

                    cntxt.MilestonePunchListStatus.Add(_punchlistStatus);
                    await cntxt.SaveChangesAsync();

                    //Save Current Status Id of Punchlist
                    //var _punchlistCurrentStatusId = await cntxt.MilestonePunchlist.Where(p => p.PunchlistID == _punchlist.PunchlistID).FirstOrDefaultAsync();
                    //_punchlistCurrentStatusId.CurrentStatusID = _punchlistStatus.StatusID;
                    _punchlist.CurrentStatusID = _punchlistStatus.StatusID;
                    await cntxt.SaveChangesAsync();

                    //Save Comment
                    var _punchlistComment = new InventoryUnitOTCContractorConstructionMilestonePunchListComments();
                    _punchlistComment.StatusID = _punchlistStatus.StatusID;
                    _punchlistComment.Comments = model.Message;
                    _punchlistComment.PunchlistDueDate = model.DueDate;
                    _punchlistComment.CommentTagDate = model.DateCreated ?? DateTime.Now.ToLocalTime();
                    _punchlistComment.CommentTagBy = username;
                    _punchlistComment.TraceData = $"DateCreated:{DateTime.Now} | CreatedBy:{username} | DeviceModel:{model.DeviceInformation.Name} | DevicePlatform:{model.DeviceInformation.Platform} | Latitude:{model.Coordinates.Latitude} | Longitude:{model.Coordinates.Longitude}";
                    _punchlistComment.DateCreated = DateTime.Now.ToLocalTime();
                    _punchlistComment.CreatedBy = username;

                    cntxt.MilestonePunchListComments.Add(_punchlistComment);
                    await cntxt.SaveChangesAsync();

                    //Save Current Comment Id of PunchlistStatus 
                    _punchlistStatus.CurrentCommentID = _punchlistComment.CommentID;
                    await cntxt.SaveChangesAsync();

                    //Save CommentAttachment
                    var attIndex = 1;
                    foreach (var itm in model.AttachmentFileNames)
                    {
                        var _punchlistAttachment = new InventoryUnitOTCContractorConstructionMilestonePunchListImage();
                        _punchlistAttachment.CommentID = _punchlistComment.CommentID;
                        _punchlistAttachment.FileName = itm.FileName;
                        _punchlistAttachment.FilePath = itm.FilePath;
                        _punchlistAttachment.FileIndex = attIndex;
                        _punchlistAttachment.DateCreated = DateTime.Now.ToLocalTime();
                        _punchlistAttachment.CreatedBy = username;
                        attIndex += 1;

                        cntxt.PunchListImage.Add(_punchlistAttachment);
                    }
                    await cntxt.SaveChangesAsync();

                    transaction.Commit();
                    return _punchlist.PunchlistID;
                }
                catch (NullReferenceException ex)
                {
                    transaction.Rollback();
                    throw new NullReferenceException(ex.Message, ex.InnerException);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message, ex.InnerException);
                }

            }
        }

        private async Task<int> UpdatePunchlistAsync(int punchlistId, FrebasContext cntxt, string username, Business.Entities.Punchlist model)
        {
            using (var transaction = cntxt.Database.BeginTransaction())
            {
                try
                {
                    //Get Punchlist Description
                    //var _pnchlistDescription = cntxt.PunchListSubject.Where(s => s.Name.ToUpper() == model.PunchListDescription.ToUpper()).FirstOrDefault();
                    //if (_pnchlistDescription == null)
                    //{
                    //    var _code = await cntxt.PunchListSubject.CountAsync() + 1;
                    //    _pnchlistDescription = new CTI.HCM.Business.Entities.PunchList();
                    //    _pnchlistDescription.Code = _code.ToString().PadLeft(5, '0');
                    //    _pnchlistDescription.Name = model.PunchListDescription;
                    //    _pnchlistDescription.DateCreated = DateTime.Now;
                    //    _pnchlistDescription.CreatedBy = username;

                    //    cntxt.PunchListSubject.Add(_pnchlistDescription);
                    //    await cntxt.SaveChangesAsync();
                    //}

                    //Get Project Code
                    var _otcNumber = await cntxt.InventoryUnitOTCContractorConstructionMilestone
                                    .Where(m => m.RecordNumber == model.ConstructionMilestoneId)
                                    .Select(m => m.OTCNumber).FirstOrDefaultAsync();
                    var _refObj = await cntxt.InventoryUnitOTC
                                    .Where(o => o.OTCNumber == _otcNumber)
                                    .Select(o => o.ReferenceObject).FirstOrDefaultAsync();
                    var _projectCode = await cntxt.InventoryUnit
                                    .Where(u => u.ReferenceObject == _refObj)
                                    .Select(u => u.ProjectCode).FirstOrDefaultAsync();

                    var _projectDetails = await cntxt.Project.Where(p => p.Code == _projectCode).FirstOrDefaultAsync();

                    //Save PunchlistMainTable
                    var _punchlist = await cntxt.MilestonePunchlist.Where(p => p.PunchlistID == punchlistId).FirstOrDefaultAsync();

                    //insert to history
                    if (_punchlist.AuthorizedPersonnelID != model.AssignedTo ||
                        _punchlist.PunchListCategoryCode != model.PunchListCategory ||
                        _punchlist.PunchListCategoryNonComplianceCode != model.NonCompliantTo ||
                        _punchlist.PunchListCode != model.PunchListDescriptionDetails.Code ||
                        _punchlist.PunchListScheduleImpactCode != model.ScheduleImpact ||
                        _punchlist.PunchListSubCategoryCode != model.PunchListSubCategory ||
                        _punchlist.ReferenceSheet != model.ReferenceSheet ||
                        _punchlist.PunchListLocationCode != model.PunchListLocation ||
                        _punchlist.PunchListCostImpactCode != model.CostImpact)
                    {
                        var _punchlistHistory = new InventoryUnitOTCContractorConstructionMilestonePunchlistChanged
                        {
                            AuthorizedPersonnelID = _punchlist.AuthorizedPersonnelID,
                            CreatedBy = _punchlist.CreatedBy,
                            CurrentStatusID = _punchlist.CurrentStatusID,
                            DateCreated = _punchlist.DateModified ?? DateTime.Now.ToLocalTime(),
                            DateModified = _punchlist.DateModified,
                            MilestoneRecordNumber = _punchlist.MilestoneRecordNumber,
                            ModifiedBy = _punchlist.ModifiedBy,
                            PunchListCategoryCode = _punchlist.PunchListCategoryCode,
                            PunchListCategoryNonComplianceCode = _punchlist.PunchListCategoryNonComplianceCode,
                            PunchListCode = _punchlist.PunchListCode,
                            PunchListCostImpactCode = _punchlist.PunchListCostImpactCode,
                            PunchlistID = _punchlist.PunchlistID,
                            PunchListLocationCode = _punchlist.PunchListLocationCode,
                            PunchListScheduleImpactCode = _punchlist.PunchListScheduleImpactCode,
                            PunchListSubCategoryCode = _punchlist.PunchListSubCategoryCode,
                            PunchListTagBy = _punchlist.PunchListTagBy,
                            PunchListTagDate = _punchlist.PunchListTagDate,
                            ReferenceNumber = _punchlist.ReferenceNumber,
                            ReferenceSheet = _punchlist.ReferenceSheet,
                            Source = _punchlist.Source
                        };
                        cntxt.MilestonePunchlistChanged.Add(_punchlistHistory);
                    }

                    //update mainTable
                    _punchlist.AuthorizedPersonnelID = model.AssignedTo;
                    _punchlist.PunchListCode = model.PunchListDescriptionDetails.Code;
                    _punchlist.PunchListLocationCode = model.PunchListLocation;
                    _punchlist.PunchListCostImpactCode = model.CostImpact;
                    _punchlist.PunchListScheduleImpactCode = model.ScheduleImpact;
                    _punchlist.DateModified = DateTime.Now.ToLocalTime();
                    _punchlist.ModifiedBy = username;

                    await cntxt.SaveChangesAsync();

                    //Save Status 
                    var _punchlistStatus = await cntxt.MilestonePunchListStatus.Where(s => s.StatusID == _punchlist.CurrentStatusID).FirstOrDefaultAsync();
                    if (_punchlistStatus.PunchlistStatusCode != model.PunchListStatus ||
                        _punchlistStatus.PunchlistDueDate != model.DueDate)
                    {
                        _punchlistStatus = null;
                        _punchlistStatus = new InventoryUnitOTCContractorConstructionMilestonePunchListStatus();
                        _punchlistStatus.PunchlistID = _punchlist.PunchlistID;
                        _punchlistStatus.PunchlistStatusCode = model.PunchListStatus;
                        _punchlistStatus.PunchlistDueDate = model.DueDate;
                        _punchlistStatus.Source = "HINS";
                        _punchlistStatus.StatusTagDate = model.DateModified ?? DateTime.Now.ToLocalTime();
                        _punchlistStatus.StatusTagBy = username;
                        _punchlistStatus.DateCreated = DateTime.Now.ToLocalTime();
                        _punchlistStatus.CreatedBy = username;
                        cntxt.MilestonePunchListStatus.Add(_punchlistStatus);
                        await cntxt.SaveChangesAsync();

                        //Save Current Status Id of Punchlist 
                        _punchlist.CurrentStatusID = _punchlistStatus.StatusID;
                        await cntxt.SaveChangesAsync();
                    }

                    //Save Comment
                    var _punchlistComment = new InventoryUnitOTCContractorConstructionMilestonePunchListComments();
                    _punchlistComment.StatusID = _punchlistStatus.StatusID;
                    _punchlistComment.Comments = model.Message;
                    _punchlistComment.PunchlistDueDate = model.DueDate;
                    _punchlistComment.CommentTagDate = model.DateModified ?? DateTime.Now.ToLocalTime();
                    _punchlistComment.CommentTagBy = username;
                    _punchlistComment.TraceData = $"DateCreated:{DateTime.Now.ToLocalTime()} | CreatedBy:{username} | DeviceModel:{model.DeviceInformation.Name} | DevicePlatform:{model.DeviceInformation.Platform} | Latitude:{model.Coordinates.Latitude} | Longitude:{model.Coordinates.Longitude}";
                    _punchlistComment.DateCreated = DateTime.Now.ToLocalTime();
                    _punchlistComment.CreatedBy = username;

                    cntxt.MilestonePunchListComments.Add(_punchlistComment);
                    await cntxt.SaveChangesAsync();

                    //Save Current Comment Id of PunchlistStatus 
                    _punchlistStatus.CurrentCommentID = _punchlistComment.CommentID;
                    await cntxt.SaveChangesAsync();

                    //Save CommentAttachment
                    var attIndex = 1;
                    foreach (var itm in model.AttachmentFileNames)
                    {
                        var _punchlistAttachment = new InventoryUnitOTCContractorConstructionMilestonePunchListImage();
                        _punchlistAttachment.CommentID = _punchlistComment.CommentID;
                        _punchlistAttachment.FileName = itm.FileName;
                        _punchlistAttachment.FilePath = itm.FilePath;
                        _punchlistAttachment.FileIndex = attIndex;
                        _punchlistAttachment.DateCreated = DateTime.Now.ToLocalTime();
                        _punchlistAttachment.CreatedBy = username;
                        attIndex += 1;

                        cntxt.PunchListImage.Add(_punchlistAttachment);
                    }
                    await cntxt.SaveChangesAsync();

                    transaction.Commit();
                    return _punchlist.PunchlistID;
                }
                catch (NullReferenceException ex)
                {
                    transaction.Rollback();
                    throw new NullReferenceException(ex.Message, ex.InnerException);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message, ex.InnerException);
                }

            }
        }

        public async Task<IEnumerable<InventoryUnitOTCContractorConstructionMilestonePunchListStatus>> GetMilestonePunchListStatusAsync(Expression<Func<InventoryUnitOTCContractorConstructionMilestonePunchListStatus, bool>> expression)
        {
            using (var cntxt = new FrebasContext())
            {
                return await cntxt.MilestonePunchListStatus.Where(expression).ToListAsync();
            }
        }

        public async Task<bool> MilestonePunchlistDescriptionAlreadyExistsAsync(int ConstructionMilestoneId, int PunchlistId, string Description)
        {
            using (var cntxt = new FrebasContext())
            {
                var _pnchlistDescription = cntxt.PunchListSubject.Where(s => s.Code.ToUpper() == Description.ToUpper()).FirstOrDefault();

                if (_pnchlistDescription == null)
                    return false;

                var _descriptionExists = await cntxt.MilestonePunchlist
                                                .Where(p => p.MilestoneRecordNumber == ConstructionMilestoneId &&
                                                p.PunchlistID != PunchlistId &&
                                                p.PunchListCode == _pnchlistDescription.Code).FirstOrDefaultAsync();

                return (_descriptionExists != null);
            }
        }
        public async Task<IEnumerable<NotificationModel>> GetPunchlistsNotificationAsync(string username, Expression<Func<NotificationModel, bool>> expression = null)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var _usr = await cntxt.User.Where(u => u.UserName == username).FirstOrDefaultAsync();

                    if (_usr == null)
                        return null;

                    //Get the Signed only contractor
                    var x = (from otc in cntxt.InventoryUnitOTC
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
                             join pds in cntxt.PunchListSubject
                             on pun.PunchListCode equals pds.Code
                             join sts in cntxt.MilestonePunchListStatus
                             on pun.CurrentStatusID equals sts.StatusID
                             join cmt in cntxt.MilestonePunchListComments
                             on sts.CurrentCommentID equals cmt.CommentID
                             join ott in cntxt.OTCType
                             on otc.OTCTypeCode equals ott.Code
                             join mas in cntxt.InventoryUnitOTCQualified
                             on otc.OTCNumber equals mas.OTCNumber
                             join cmi in cntxt.ConstructionMilestone
                             on mlc.ConstructionMilestoneCode equals cmi.Code
                             join inv in cntxt.InventoryUnit
                             on otc.ReferenceObject equals inv.ReferenceObject
                             join pro in cntxt.Project
                             on inv.ProjectCode equals pro.Code
                             join phb in cntxt.PhaseBuilding
                             on inv.PhaseBuildingCode equals phb.Code
                             join bfc in cntxt.BlockFloorCluster
                             on inv.BlockFloorClusterCode equals bfc.Code
                             where uct.IsIssuedNTP == true &&
                              uct.IsHold == false
                             select new
                             {
                                 ReferenceObject = otc.ReferenceObject,
                                 InvUnit = inv,
                                 Punchlist = pun,
                                 PunchlistStatus = sts,
                                 PunchlistComment = cmt,
                                 PunchlistDescription = pds.Name,
                                 VendorCode = uct.VendorCode,
                                 Project = pro,
                                 PhaseBuilding = phb,
                                 Block = bfc
                             });

                    //if COntractor 
                    if (_usr.UserTypeCode == "CONT")
                    {
                        var _vndorCode = await _conRepo.GetRepresentativeVendorAsync(username);
                        x = x.Where(c => c.VendorCode == _vndorCode.Code);
                    }
                    else if (_usr.UserTypeCode == "EMPL")
                    {
                        var _empProjects = await _userProjRepo.GetProjectAsync(username);
                        x = x.Where(c => _empProjects.Contains(c.Project.Code));
                    }


                    if (expression == null)
                    {
                        var xxx = await x.Select(a => new NotificationModel
                        {
                            DatePosted = a.PunchlistStatus.DateModified ?? a.PunchlistStatus.DateCreated,
                            Message = a.PunchlistComment.Comments,
                            MessageBy = a.PunchlistComment.CreatedBy,
                            PunchlistId = a.Punchlist.PunchlistID,
                            ConstructionMilestoneId = a.Punchlist.MilestoneRecordNumber,
                            DueDate = a.PunchlistStatus.PunchlistDueDate,
                            PunchlistStatusCode = a.PunchlistStatus.PunchlistStatusCode,
                            Subject = a.PunchlistDescription,
                            Unit = new Unit
                            {
                                ReferenceObject = a.ReferenceObject,
                                Project = new Project
                                {
                                    Code = a.Project.Code,
                                    ShortName = a.Project.ShortName,
                                    LongName = a.Project.LongName
                                },
                                PhaseBuilding = new PhaseBuilding
                                {
                                    Code = a.PhaseBuilding.Code,
                                    ShortName = a.PhaseBuilding.ShortName,
                                    LongName = a.PhaseBuilding.LongName
                                },
                                BlockFloor = new Block
                                {
                                    Code = a.Block.Code,
                                    ShortName = a.Block.ShortName,
                                    LongName = a.Block.LongName
                                },
                                InventoryUnitNumber = a.InvUnit.InventoryUnitNumber,
                                LotUnitShareNumber = a.InvUnit.LotUnitShareNumber
                            }
                        }).ToListAsync();

                        return xxx;
                    }
                    else
                    {
                        //var y = await x.ToListAsync();
                        var xx = await x.Select(a => new NotificationModel
                        {
                            DatePosted = a.Punchlist.DateModified ?? a.Punchlist.DateCreated,
                            Message = a.PunchlistComment.Comments,
                            MessageBy = a.PunchlistComment.CreatedBy,
                            PunchlistId = a.Punchlist.PunchlistID,
                            ConstructionMilestoneId = a.Punchlist.MilestoneRecordNumber,
                            DueDate = a.PunchlistStatus.PunchlistDueDate,
                            PunchlistStatusCode = a.PunchlistStatus.PunchlistStatusCode,
                            Subject = a.PunchlistDescription,
                            Unit = new Unit
                            {
                                ReferenceObject = a.ReferenceObject,
                                Project = new Project
                                {
                                    Code = a.Project.Code,
                                    ShortName = a.Project.ShortName,
                                    LongName = a.Project.LongName
                                },
                                PhaseBuilding = new PhaseBuilding
                                {
                                    Code = a.PhaseBuilding.Code,
                                    ShortName = a.PhaseBuilding.ShortName,
                                    LongName = a.PhaseBuilding.LongName
                                },
                                BlockFloor = new Block
                                {
                                    Code = a.Block.Code,
                                    ShortName = a.Block.ShortName,
                                    LongName = a.Block.LongName
                                },
                                InventoryUnitNumber = a.InvUnit.InventoryUnitNumber,
                                LotUnitShareNumber = a.InvUnit.LotUnitShareNumber
                            }
                        }).Where(expression).ToListAsync();

                        return xx;
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

        public async Task<IEnumerable<NotificationModel>> OpenPunchlistsNotificationAsync(string username, string punchlistStatusCode)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var data = await cntxt.VWUnitMilestonesPunchlists
                        .Where(x => x.UserName == username
                            && x.PunchlistStatusCode == punchlistStatusCode
                            && x.PunchlistStatDueDate >= _DateToday).Distinct()
                        .Select(x => new NotificationModel
                        {
                            DatePosted = !x.PunchlistStatDateModified.HasValue ? x.PunchlistStatDateCreated.Value
                                           : x.PunchlistStatDateModified.Value,
                            PunchlistId = x.PunchlistID,
                            ConstructionMilestoneId = x.ConstructionMilestoneID,
                            DueDate = x.PunchlistStatDueDate,
                            PunchlistStatusCode = x.PunchlistStatusCode,
                            Subject = x.PunchListDescription,
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
                        })
                        .OrderBy(x => x.Subject).ToListAsync();

                    foreach (var x in data)
                    {
                        x.MessageBy = (from cmt in cntxt.MilestonePunchListComments
                                       join pst in cntxt.MilestonePunchListStatus.Where(d => d.PunchlistID == x.PunchlistId
                                                                                        && d.PunchlistStatusCode == x.PunchlistStatusCode)
                                       on new
                                       {
                                           A = cmt.StatusID,
                                           B = cmt.CommentID
                                       }
                                       equals new
                                       {
                                           A = pst.StatusID,
                                           B = pst.CurrentCommentID
                                       }
                                       select new
                                       {
                                           Message = cmt.Comments,
                                           MessageBy = cmt.CreatedBy,
                                           CreationDate = cmt.DateCreated
                                       })
                                       .ToList().OrderByDescending(h => h.CreationDate)
                                       .GroupBy(g => g).First().FirstOrDefault().MessageBy ?? string.Empty;

                        x.Message = (from cmt in cntxt.MilestonePunchListComments
                                     join pst in cntxt.MilestonePunchListStatus.Where(d => d.PunchlistID == x.PunchlistId
                                                                                        && d.PunchlistStatusCode == x.PunchlistStatusCode)
                                     on new
                                     {
                                         A = cmt.StatusID,
                                         B = cmt.CommentID
                                     }
                                     equals new
                                     {
                                         A = pst.StatusID,
                                         B = pst.CurrentCommentID
                                     }
                                     select new
                                     {
                                         Message = cmt.Comments,
                                         MessageBy = cmt.CreatedBy,
                                         CreationDate = cmt.DateCreated
                                     })
                                     .ToList().OrderByDescending(h => h.CreationDate)
                                     .GroupBy(g => g).First().FirstOrDefault().Message ?? string.Empty;
                    }

                    if (data.Count() > 0)
                    {
                        return data;
                    }
                    else
                    {
                        throw new NullReferenceException($"No open punchlist found for user: {username}");
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

        public async Task<IEnumerable<NotificationModel>> OpenOverduePunchlistNotificationAsync(string username, string punchlistStatusCode)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var data = await cntxt.VWUnitMilestonesPunchlists
                        .Where(x => x.UserName == username
                            && x.PunchlistStatusCode == punchlistStatusCode
                            && x.PunchlistDueDate < _DateToday).Distinct()
                            .Select(x => new NotificationModel
                            {
                                DatePosted = !x.PunchlistStatDateModified.HasValue ? x.PunchlistStatDateCreated.Value
                                           : x.PunchlistStatDateModified.Value,
                                
                                PunchlistId = x.PunchlistID,
                                ConstructionMilestoneId = x.ConstructionMilestoneID,
                                DueDate = x.PunchlistStatDueDate,
                                PunchlistStatusCode = x.PunchlistStatusCode,
                                Subject = x.PunchListDescription,
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
                            })
                        .OrderBy(x => x.Subject).ToListAsync();
                    
                    foreach (var x in data)
                    {
                        x.MessageBy = (from cmt in cntxt.MilestonePunchListComments
                                       join pst in cntxt.MilestonePunchListStatus.Where(d => d.PunchlistID == x.PunchlistId 
                                                                                        && d.PunchlistStatusCode == x.PunchlistStatusCode)
                                       on new
                                       {
                                           A = cmt.StatusID,
                                           B = cmt.CommentID
                                       }
                                       equals new
                                       {
                                           A = pst.StatusID,
                                           B = pst.CurrentCommentID
                                       }
                                       select new
                                       {
                                           Message = cmt.Comments,
                                           MessageBy = cmt.CreatedBy,
                                           CreationDate = cmt.DateCreated
                                       })
                                       .ToList().OrderByDescending(h => h.CreationDate)
                                       .GroupBy(g => g).First().FirstOrDefault().MessageBy ?? string.Empty;

                        x.Message = (from cmt in cntxt.MilestonePunchListComments
                                     join pst in cntxt.MilestonePunchListStatus.Where(d => d.PunchlistID == x.PunchlistId 
                                                                                        && d.PunchlistStatusCode == x.PunchlistStatusCode)
                                     on new
                                     {
                                         A = cmt.StatusID,
                                         B = cmt.CommentID
                                     }
                                     equals new
                                     {
                                         A = pst.StatusID,
                                         B = pst.CurrentCommentID
                                     }
                                     select new
                                     {
                                         Message = cmt.Comments,
                                         MessageBy = cmt.CreatedBy,
                                         CreationDate = cmt.DateCreated
                                     })
                                     .ToList().OrderByDescending(h => h.CreationDate)
                                     .GroupBy(g => g).First().FirstOrDefault().Message ?? string.Empty;
                    }


                    if (data.Count() > 0)
                    {
                        return data;
                    }
                    else
                    {
                        throw new NullReferenceException($"No open overdue punchlist found for user: {username}");
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

        public async Task<IEnumerable<NotificationModel>> RecentlyClosedPunchlistNotificationAsync(string username, string punchlistStatusCode)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    var data = await cntxt.VWUnitMilestonesPunchlists
                        .Distinct().Where(x => x.UserName == username
                            && x.PunchlistStatusCode == punchlistStatusCode)
                        .Select(x => new NotificationModel
                        {
                            DatePosted = !x.PunchlistStatDateModified.HasValue ? x.PunchlistStatDateCreated.Value
                                           : x.PunchlistStatDateModified.Value,
                           
                            PunchlistId = x.PunchlistID,
                            ConstructionMilestoneId = x.ConstructionMilestoneID,
                            DueDate = x.PunchlistStatDueDate,
                            PunchlistStatusCode = x.PunchlistStatusCode,
                            Subject = x.PunchListDescription,
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
                        })
                        .Where(x => DbFunctions.DiffDays(x.DatePosted, _DateToday) <= 7)
                        .Distinct().OrderBy(x => x.Subject)
                        .ToListAsync();

                    foreach (var x in data)
                    {
                        x.MessageBy = (from cmt in cntxt.MilestonePunchListComments
                                       join pst in cntxt.MilestonePunchListStatus.Where(d => d.PunchlistID == x.PunchlistId
                                                                                        && d.PunchlistStatusCode == x.PunchlistStatusCode)
                                       on new
                                       {
                                           A = cmt.StatusID,
                                           B = cmt.CommentID
                                       }
                                       equals new
                                       {
                                           A = pst.StatusID,
                                           B = pst.CurrentCommentID
                                       }
                                       select new
                                       {
                                           Message = cmt.Comments,
                                           MessageBy = cmt.CreatedBy,
                                           CreationDate = cmt.DateCreated
                                       })
                                       .ToList().OrderByDescending(h => h.CreationDate)
                                       .GroupBy(g => g).First().FirstOrDefault().MessageBy ?? string.Empty;

                        x.Message = (from cmt in cntxt.MilestonePunchListComments
                                     join pst in cntxt.MilestonePunchListStatus.Where(d => d.PunchlistID == x.PunchlistId
                                                                                        && d.PunchlistStatusCode == x.PunchlistStatusCode)
                                     on new
                                     {
                                         A = cmt.StatusID,
                                         B = cmt.CommentID
                                     }
                                     equals new
                                     {
                                         A = pst.StatusID,
                                         B = pst.CurrentCommentID
                                     }
                                     select new
                                     {
                                         Message = cmt.Comments,
                                         MessageBy = cmt.CreatedBy,
                                         CreationDate = cmt.DateCreated
                                     })
                                     .ToList().OrderByDescending(h => h.CreationDate)
                                     .GroupBy(g => g).First().FirstOrDefault().Message ?? string.Empty;
                    }

                    if (data.Count() > 0)
                    {
                        return data;
                    }
                    else
                    {
                        throw new NullReferenceException($"No closed punchlist found for user: {username}");
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

        public async Task<bool> NotCloseDescriptionAndLocationAlreadyExistsAsync(int ConstructionMilestoneId, Punchlist model)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    var _descriptionExists = await cntxt.MilestonePunchlist
                                                    .Where(p => p.MilestoneRecordNumber == ConstructionMilestoneId &&
                                                    p.PunchlistID != model.PunchListID &&
                                                    p.PunchListLocationCode == model.PunchListLocation &&
                                                    p.PunchListCode == model.PunchListDescription).FirstOrDefaultAsync();

                    //if theres no same description and locationCode then return false
                    if (_descriptionExists == null)
                        return false;

                    //get the status
                    var _status = await cntxt.MilestonePunchListStatus.Where(s => s.StatusID == _descriptionExists.CurrentStatusID).FirstOrDefaultAsync();

                    //return true if status is not close or void  CLOS  VOID
                    return (_status.PunchlistStatusCode != "CLOS" && _status.PunchlistStatusCode != "VOID");

                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        #region Maintenance

        private IQueryable<Business.Entities.PunchlistGroup> GetPunchlistGroupQuery(FrebasContext cntxt)
        {
            return (from grp in cntxt.PunchListSubjectGroup
                        //from cat in cntxt.PunchListCategory.Where(c => c.IsNonCompliance == true)
                    select new PunchlistGroup
                    {
                        Code = grp.Code,
                        Name = grp.Description,
                        CategoryCode = grp.PunchlistCategoryCode
                    });
        }

        public async Task<IEnumerable<Business.Entities.PunchlistDescription>> GetPunchlistAsync(Expression<Func<Business.Entities.PunchlistDescription, bool>> expression = null)
        {
            using (var cntxt = new FrebasContext())
            {
                var qry = (from pun in cntxt.PunchListSubject
                           join grp in GetPunchlistGroupQuery(cntxt)
                           on pun.PunchlistGroupCode equals grp.Code
                           into pungrp
                           from grp in pungrp.DefaultIfEmpty()
                           select new PunchlistDescription
                           {
                               Code = pun.Code,
                               Name = pun.Name,
                               GroupCode = grp.Code
                           });
                if (expression != null)
                    return await qry.Where(expression).ToListAsync();
                else
                    return await qry.ToListAsync();
            }
        }

        public async Task<IEnumerable<PunchlistGroup>> GetPunchlistGroupAsync(Expression<Func<PunchlistGroup, bool>> expression = null)
        {
            using (var cntxt = new FrebasContext())
            {
                if (expression != null)
                    return await GetPunchlistGroupQuery(cntxt).Where(expression).ToListAsync();
                else
                    return await GetPunchlistGroupQuery(cntxt).ToListAsync();
            }
        }

        public async Task<IEnumerable<PunchListCategory>> GetPunchlistCategoryAsync(Expression<Func<PunchListCategory, bool>> expression = null)
        {
            using (var cntxt = new FrebasContext())
            {
                try
                {
                    if (expression != null)
                        return await cntxt.PunchListCategory.Where(expression).ToListAsync();
                    else
                    {
                        var xx = await cntxt.PunchListCategory.ToListAsync();
                        return xx;
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public async Task<IEnumerable<PunchListCostImpact>> GetPunchlistCostImpactAsync(Expression<Func<PunchListCostImpact, bool>> expression = null)
        {
            using (var cntxt = new FrebasContext())
            {
                if (expression != null)
                    return await cntxt.PunchListCostImpact.Where(expression).ToListAsync();
                else
                    return await cntxt.PunchListCostImpact.ToListAsync();
            }
        }

        public async Task<IEnumerable<PunchListLocation>> GetPunchListLocationAsync(Expression<Func<PunchListLocation, bool>> expression = null)
        {
            using (var cntxt = new FrebasContext())
            {
                if (expression != null)
                    return await cntxt.PunchListLocation.Where(expression).ToListAsync();

                return await cntxt.PunchListLocation.ToListAsync();
            }
        }

        public async Task<IEnumerable<PunchListCategoryNonCompliance>> GetPunchlistNonComplianceAsync(Expression<Func<PunchListCategoryNonCompliance, bool>> expression = null)
        {
            using (var cntxt = new FrebasContext())
            {
                if (expression != null)
                    return await cntxt.PunchListCategoryNonCompliance.Where(expression).ToListAsync();

                return await cntxt.PunchListCategoryNonCompliance.ToListAsync();
            }
        }

        public async Task<IEnumerable<PunchListScheduleImpact>> GetPunchListScheduleImpactAsync(Expression<Func<PunchListScheduleImpact, bool>> expression = null)
        {
            using (var cntxt = new FrebasContext())
            {
                if (expression != null)
                    return await cntxt.PunchListScheduleImpact.Where(expression).ToListAsync();

                return await cntxt.PunchListScheduleImpact.ToListAsync();
            }
        }

        public async Task<IEnumerable<PunchListStatus>> GetPunchListStatusAsync(Expression<Func<PunchListStatus, bool>> expression = null)
        {
            using (var cntxt = new FrebasContext())
            {
                if (expression != null)
                    return await cntxt.PunchListStatus.Where(expression).ToListAsync();

                return await cntxt.PunchListStatus.ToListAsync();
            }
        }

        public async Task<IEnumerable<PunchListSubCategory>> GetPunchListSubCategoryAsync(Expression<Func<PunchListSubCategory, bool>> expression = null)
        {
            using (var cntxt = new FrebasContext())
            {
                if (expression != null)
                    return await cntxt.PunchListSubCategory.Where(expression).ToListAsync();

                return await cntxt.PunchListSubCategory.ToListAsync();
            }
        }

        public async Task<IEnumerable<PunchlistAttachment>> GetMilestonePunchlistsImagesAsync()
        {
            using (var cntxt = new FrebasContext())
            {
                var data = await cntxt.PunchListImage.Distinct().ToListAsync();

                return data.Select(x => new PunchlistAttachment { FileName = x.FileName}).Distinct().OrderBy(x=> x.FileName);
            }
        }


        protected override PunchList AddEntity(FrebasContext entityContext, PunchList entity)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<PunchList> GetEntities(FrebasContext entityContext)
        {
            throw new NotImplementedException();
        }

        protected override PunchList GetEntity(FrebasContext entityContext, int id)
        {
            throw new NotImplementedException();
        }

        protected override PunchList UpdateEntity(FrebasContext entityContext, PunchList entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Punchlist>> GetProjectMilestonePunchlist(string username, string ProjectCode)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
