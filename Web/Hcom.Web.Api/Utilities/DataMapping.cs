using Hcom.App.Core.Enums;
using Hcom.App.Entities.HCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hcom.Web.Api.Utilities
{
    public class DataMapping
    {
        public static UserRoleType MapRoleByCode(string RoleCode)
        {
            if (RoleCode == "PREN")
                return UserRoleType.Engineer;
            else if (RoleCode == "CONT")
                return UserRoleType.Contractor;
            else if (RoleCode == "QASS")
                return UserRoleType.QA;
            else if (RoleCode == "PTCH")
                return UserRoleType.Project_Technical_Head;
            else
                return UserRoleType.InvalidRole;
        }

        
        public static PunchlistStatus MapPunchlistStatusByCode(string Code)
        {
            var userPunchlistStatus = new List<PunchlistStatus> {
                new PunchlistStatus {Code = "OPEN",IsOpen = true,IsClosed = false,Name="Open"}, 
                new PunchlistStatus {Code = "CLOS",IsOpen = false,IsClosed = true,Name="Closed"}, 
                new PunchlistStatus {Code = "COMP",IsOpen = false,IsClosed = false,Name="Completed"}, 
                new PunchlistStatus {Code = "INPR",IsOpen = false,IsClosed = false,Name="In-Progress"}, 
                new PunchlistStatus {Code = "REJE",IsOpen = false,IsClosed = false,Name="Rejected"},
                new PunchlistStatus {Code = "REOP",IsOpen = false,IsClosed = false,Name="Re-Opened"},
                new PunchlistStatus {Code = "VOID",IsOpen = false,IsClosed = true,Name="Rejected"}
                };

            return userPunchlistStatus.Where(s => s.Code == Code).FirstOrDefault();
        }

        public static string MapManagingContractorByCode(string Code)
        {
            var dicManCon = new Dictionary<string, string>();
            dicManCon.Add("GENCON", "General Contractor");
            dicManCon.Add("TRDCON", "Trade Contractor");

            var xx = dicManCon.Where(x => x.Key == Code).FirstOrDefault();
            return xx.Value.ToString();

        }
    }
}
