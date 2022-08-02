using Core.Common.Contracts;
using CTI.HI.Business.BusinessEngines.BusinessEngineContracts;
using CTI.HI.Data.Constant;
using CTI.HI.Data.Contracts.Frebas;
using CTI.HI.Data.Repository.Frebas;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business.BusinessEngines
{
    [Export(typeof(IMilestoneEngine))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MilestoneEngine : IMilestoneEngine
    {
        private IConstructionMilestoneRepository _MilestoneRepo;
        private IPunchlistRepository _PunchlistRepo;
        private IUserRepository _UserRepo;

        public MilestoneEngine()
        {
            _MilestoneRepo = new ConstructionMilestoneRepository();
            _PunchlistRepo = new PunchlistRepository();
            _UserRepo = new UserRepository();
        }

        [ImportingConstructor]
        public MilestoneEngine(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        IDataRepositoryFactory _DataRepositoryFactory;

        public async Task<bool> isValidPercentageAsync(int constructionMilestoneId, decimal percentage, string userName)
        {
            //var oldMilestone = (await _MilestoneRepo.GetConstructionMilestonesAsync(m => m.RecordNumber == constructionMilestoneId)).FirstOrDefault();

            //check if percentage is between 0 - 100
            if (percentage > 100 || percentage < 0)
                throw new ApplicationException("Percentage must not exceed 100.");

            return true;
        }

        public async Task<bool> isAllowedToChangePercentageAsync(int constructionMilestoneId, decimal percentage, string userName)
        {
            var _roleCode = (await _UserRepo.GetUserInfoAsync(userName)).RoleCode;
            var _punchlistStatuses = (await _PunchlistRepo.GetMilestonePunchlistsAsync(constructionMilestoneId)).Select(p => p.PunchListStatus).ToList();
            var oldMilestone = (await _MilestoneRepo.GetConstructionMilestonesAsync(m => m.RecordNumber == constructionMilestoneId)).FirstOrDefault();

            //if the new percentage is 100 but theres an open or inprogress punchlist then throw an error
            if (percentage == 100 && _punchlistStatuses.FirstOrDefault(s => s == "OPEN" || s == "INPR") != null)
                throw new ApplicationException("Cannot complete Milestone percentage with open or in-progress Punchlist.");

            /*- if RoleCode is QASS - Quality Assurance
             then it should not increase percentage*/
            //if (_roleCode == "QASS" && (oldMilestone.Percentage ?? 0) < percentage)
            //    throw new ApplicationException("Quality Assurance can only decrease the Milestone percentage.");

            /*- if RoleCode is PREN - Project Engineer
              then it should not decrease the percentage*/
            if ((_roleCode == UserRoleCode.ProjectEngineer || _roleCode == UserRoleCode.ProjectTechnicalHead || _roleCode == UserRoleCode.DPIOfficer) && (oldMilestone.Percentage ?? 0) > percentage)
            {
                if(_roleCode == UserRoleCode.ProjectEngineer)
                    throw new ApplicationException("Project Engineer can only increase the Milestone percentage.");
                else if (_roleCode == UserRoleCode.DPIOfficer)
                    throw new ApplicationException("DPI Officer can only increase the Milestone percentage.");
                else
                    throw new ApplicationException("Project Technical Head can only increase the Milestone percentage.");
            }
                
                

            //if (_roleCode != "PREN" && _roleCode != "QASS")
            //    throw new ApplicationException("Only Project Engineer and Quality Assurance can change the Milestone percentage.");

            return true;
        }
    }
}
