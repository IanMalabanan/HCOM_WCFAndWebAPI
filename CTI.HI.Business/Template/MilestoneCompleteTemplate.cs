using CTI.HCM.Business.Entities.Models;
using CTI.HI.Business.Entities;
using CTI.IMM.Business.Entities.Model;
using System.Text;

namespace CTI.HI.Business.Template
{
    public class MilestoneCompleteTemplate
    {
        public ConstructionMilestoneModel Milestone { get; set; }
        public UnitModel Unit { get; set; }
        public ContractorAwardedLoa Loa { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("BILLING TYPE / NUMBER : PROGRESS BILLING<br><br>");
            sb.AppendLine();
            sb.AppendLine($"PHASE BLK & LOT : {Unit.PhaseBuilding.LongName} {Unit.BlockFloor.LongName} {Unit.LotUnitShareNumber}<br><br>");
            sb.AppendLine();
            sb.AppendLine($"CONTRACT NUMBER : {Loa.LoaContractNumber}<br><br>");
            sb.AppendLine();
            sb.AppendLine($"NTP NUMBER : {Loa.NTPNumber}<br><br>");

            return sb.ToString();
        }
    }
}
