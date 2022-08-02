using Hcom.App.Entities;
using Hcom.App.Entities.HCOM;
using Hcom.App.Entities.Models;
using Hcom.Web.Api.Interface;
using Hcom.Web.Api.Utilities;
using Hcom.Web.Api.Utilities.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.HI.Controllers
{
    [ApiExplorerSettings(GroupName = "HCOM")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    public class DownloadController : Controller
    {
        private readonly IUnit _unit;
        private readonly IMilestone _milestoneService;
        private readonly IPunchlist _punchlist;

        public DownloadController(IUnit unit, IMilestone milestoneService, IPunchlist punchlist)
        {
            _unit = unit;
            _milestoneService = milestoneService;
            _punchlist = punchlist;
        }

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("api/hi/downloadmilestone")]
        //public async Task<IActionResult> Download([FromQuery] string referenceObject, string username)
        //{
        //    Debug.WriteLine($"Starting To Download Unit By Reference Object");
        //    //time start
        //    var _timeStartOverAllProcess = DateTime.Now;
        //    var _encryptionProvider = RijndaelEncryptionProvider.GetInstance();
        //    //time start
        //    var _timeStartSaveCache = DateTime.Now;
        //    var x = await _unit.GetUnitAsync(referenceObject, username);
            
        //    var units = new Unit
        //    {
        //        EncryptReferenceObject = _encryptionProvider.Encrypt(x.ReferenceObject),
        //        ReferenceObject = x.ReferenceObject,
        //        ProjectCode = x.Project.ProjectCode,
        //        Project = new Project
        //        {
        //            ProjectCode = x.Project.ProjectCode,
        //            ShortName = x.Project.ShortName,
        //            LongName = x.Project.LongName
        //        },
        //        PhaseBuildingCode = x.PhaseBuilding.Id,
        //        PhaseBuilding = new PhaseBuilding
        //        {
        //            Id = x.PhaseBuilding.Id,
        //            LongName = x.PhaseBuilding.LongName,
        //            ShortName = x.PhaseBuilding.ShortName
        //        },
        //        BlockFloorCode = x.BlockFloor.Id,
        //        BlockFloor = new BlockFloorCluster
        //        {
        //            Id = x.BlockFloor.Id,
        //            LongName = x.BlockFloor.LongName,
        //            ShortName = x.BlockFloor.ShortName
        //        },
        //        InventoryUnitNumber = x.InventoryUnitNumber,
        //        LotUnitShareNumber = x.LotUnitShareNumber,
        //        Address = new Address
        //        {
        //            Longitude = (double)(x.Address?.Longitude ?? 0),
        //            Latitude = (double)(x.Address?.Latitude ?? 0)
        //        },
        //        FloorPlanUrls = (x.FloorPlanUrls != null ? x.FloorPlanUrls.ToList() : new List<string>()),
        //        MilestonePercentage = new MilestonePercentage
        //        {
        //            BillingPercentage = x.MilestonePercentage?.BillingPercentage ?? 0,
        //            ContractorsName = x.MilestonePercentage?.ContractorsName,
        //            CurrentPOC = x.MilestonePercentage?.CurrentPOC ?? 0,
        //            OngoingActivityMilestone = x.MilestonePercentage?.OngoingActivityMilestone,
        //            TargetPOC = x.MilestonePercentage?.TargetPOC ?? 0,
        //            TotalOverdue = x.MilestonePercentage?.TotalOverdue ?? 0,
        //            TotalPending = x.MilestonePercentage?.TotalPending ?? 0,
        //            TotalPunchlist = x.MilestonePercentage?.TotalPunchlist ?? 0,
        //            Variance = x.MilestonePercentage.Variance
        //        }
        //    };

        //    var generatedUnits = units;

        //    //time end
        //    var _timeEndSaveCache = DateTime.Now;
        //    TimeSpan t = TimeSpan.FromSeconds((_timeEndSaveCache - _timeStartSaveCache).TotalSeconds);
        //    string _time1 = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
        //                    t.Hours,
        //                    t.Minutes,
        //                    t.Seconds,
        //                    t.Milliseconds);

            




        //    //time start
        //    var _timeStartDLMilestone = DateTime.Now;
        //    var unitsWithMilestone = await _milestoneService.GetMilestonesByUnitAsync(username, Fileurl(), referenceObject);
        //    //time end
        //    var _timeEndDLMilestone = DateTime.Now;
        //    TimeSpan t2 = TimeSpan.FromSeconds((_timeEndDLMilestone - _timeStartDLMilestone).TotalSeconds);
        //    string _time2 = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
        //                    t2.Hours,
        //                    t2.Minutes,
        //                    t2.Seconds,
        //                    t2.Milliseconds);

            


        //    //time start
        //    var _timeStartDLPunchlist = DateTime.Now;
        //    await _punchlist.GetPunchlistbyUnitAsync(referenceObject, username, Fileurl());
        //    //time end
        //    var _timeEndDLPunchlist = DateTime.Now;
        //    TimeSpan t3 = TimeSpan.FromSeconds((_timeEndDLPunchlist - _timeStartDLPunchlist).TotalSeconds);
        //    string _time3 = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
        //                    t3.Hours,
        //                    t3.Minutes,
        //                    t3.Seconds,
        //                    t3.Milliseconds);

            



        //    //time end
        //    var _timeEndOverallProcess = DateTime.Now;
        //    TimeSpan t4 = TimeSpan.FromSeconds((_timeEndOverallProcess - _timeStartOverAllProcess).TotalSeconds);
        //    string _time4 = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
        //                    t4.Hours,
        //                    t4.Minutes,
        //                    t4.Seconds,
        //                    t4.Milliseconds);




        //    Debug.WriteLine($"{"Total time to Get Unit By Reference Object : " + _time1}");
        //    Debug.WriteLine($"{"Total time to Get Milestone By Reference Object : " + _time2}");
        //    Debug.WriteLine($"{"Total time to Get Punchlist By Reference Object : " + _time3}");
        //    Debug.WriteLine($"{"Total time to Download Unit By Reference Object : " + _time4}");

        //    return Ok ($"{"Total time to Download Unit By Reference Object From API : " + _time4}");
        //}

        string Fileurl()
        {
            string Path = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}/api/util/Attachment?filename=";
            return Path;
        }

    }
}
