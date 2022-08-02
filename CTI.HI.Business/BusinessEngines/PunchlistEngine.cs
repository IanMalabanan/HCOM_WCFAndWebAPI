using Core.Common.Contracts;
using CTI.HI.Business.BusinessEngines.BusinessEngineContracts;
using CTI.HI.Business.Entities;
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
    [Export(typeof(IPunchlistEngine))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PunchlistEngine : IPunchlistEngine
    {
        private IConstructionMilestoneRepository _MilestoneRepo;
        private IPunchlistRepository _PunchlistRepo;
        private IUserRepository _UserRepo;

        public PunchlistEngine()
        {
            _MilestoneRepo = new ConstructionMilestoneRepository();
            _PunchlistRepo = new PunchlistRepository();
            _UserRepo = new UserRepository();
        }

        [ImportingConstructor]
        public PunchlistEngine(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        IDataRepositoryFactory _DataRepositoryFactory;

        public async Task<bool> canClosePunchlistAsync(string userName, int punchlistId, string punchlistStatus)
        {
            var userCreatedPunchlist = (await _PunchlistRepo.GetMilestonePunchListStatusAsync(p => p.PunchlistID == punchlistId && p.PunchlistStatusCode == "OPEN")).Select(s => s.StatusTagBy).FirstOrDefault();
            var usr = await _UserRepo.GetUserInfoAsync(userName);
            //check if the updator and creator of this puchlist is the same, if the updator role is QA then it can close
            if (
                !userCreatedPunchlist.Equals(userName, StringComparison.OrdinalIgnoreCase)
                //(userCreatedPunchlist != userName) 
                && (usr.RoleCode != "QASS"))
                throw new ApplicationException("Only the punchlist creator can update the status as closed.");

            return true;
        }

        public async Task<bool> canCreatePunchlistAsync(string userName, int constructionMilestoneId, Punchlist model)
        {
            var _milestone = (await _MilestoneRepo.GetConstructionMilestonesAsync(m => m.RecordNumber == constructionMilestoneId)).FirstOrDefault();

            if (_milestone.Percentage == 0)
                throw new ApplicationException("Cannot create a punchlist in 0 percent Milestone.");

            return true;
        }

        public async Task<bool> canVoidPunchlistAsync(string userName, Punchlist model)
        {
            var _punCreator = (await _PunchlistRepo.GetPunchlistAsync(model.PunchListID)).CreatedBy;

            var usrCreator = await _UserRepo.GetUserInfoAsync(_punCreator);

            if(usrCreator.RoleCode == "QASS")
            {
                var usr = await _UserRepo.GetUserInfoAsync(userName);
                if (usr.RoleCode != "QASS")
                    throw new ApplicationException("Only the QA can void this punchlist.");
            }

            return true;
        }


        public async Task<bool> validatePunchlistInputsAsync(Punchlist model)
        {
            if ((model.Message ?? "") == "")
                throw new ApplicationException("Punchlist comment is required.");

            //check if description is already assigned
            if (await _PunchlistRepo.NotCloseDescriptionAndLocationAlreadyExistsAsync(model.ConstructionMilestoneId, model))
                throw new ApplicationException("Punchlist Description in the same location is already exists.");

            //get the category if nonCompliance
            var _punCat = (await _PunchlistRepo.GetPunchlistCategoryAsync(c => c.Code == model.PunchListCategory)).FirstOrDefault();

            var _oldPunchlist = (await _PunchlistRepo.GetMilestonePunchlistAsync(model.PunchListID));

            //Check if the old Status is not the same of new status and
            //new status is either OPEN REOP COMP CLOS then validate the attachment
            if (((_oldPunchlist?.PunchListStatus ?? "") != model.PunchListStatus) &&
                (model.PunchListStatus == "OPEN" || model.PunchListStatus == "REOP" || model.PunchListStatus == "COMP" || model.PunchListStatus == "CLOS") &&
                model.AttachmentFileNames.Count() == 0)
                throw new ApplicationException("Punchlist attachment is required.");


            //create punchlist
            if (model.PunchListID <= 0)
            {
                if ((model.PunchListCategory ?? "") == "")
                    throw new ApplicationException("Punchlist category is required.");

                if ((model.PunchListSubCategory ?? "") == "")
                    throw new ApplicationException("Punchlist sub category is required.");

                if (_punCat.IsNonCompliance)
                {
                    if ((model.NonCompliantTo ?? "") == "")
                        throw new ApplicationException("Punchlist non-compliant to is required.");

                    if ((model.ReferenceSheet ?? "") == "")
                        throw new ApplicationException("Punchlist reference sheet is required.");
                }

                if ((model.PunchListDescription ?? "") == "")
                    throw new ApplicationException("Punchlist description is required.");

                if ((model.PunchListLocation ?? "") == "")
                    throw new ApplicationException("Punchlist location is required.");

                if ((model.CostImpact ?? "") == "")
                    throw new ApplicationException("Punchlist cost impact is required.");

                if ((model.ScheduleImpact ?? "") == "")
                    throw new ApplicationException("Punchlist schedule impact is required.");

                if ((model.PunchListStatus ?? "") == "")
                    throw new ApplicationException("Punchlist status is required.");


                if (model.DueDate.Value.Date < DateTime.Now.Date)
                    throw new ApplicationException("Backdating of due date is not valid.");
            }
            else //modify punchlist
            {
                var _punchRepo = await _PunchlistRepo.GetMilestonePunchlistAsync(model.PunchListID);
                if (_punchRepo == null)
                    throw new ApplicationException("PunchlistId does not exist.");

                if (_punchRepo.PunchListCategory != model.PunchListCategory)
                    throw new ApplicationException("Punchlist category is not editable.");

                if (_punchRepo.PunchListSubCategory != model.PunchListSubCategory)
                    throw new ApplicationException("Punchlist sub category is not editable.");

                if (_punchRepo.NonCompliantTo != model.NonCompliantTo)
                    throw new ApplicationException("Punchlist non-compliant to is not editable.");

                if (_punchRepo.ReferenceSheet != model.ReferenceSheet)
                    throw new ApplicationException("Punchlist reference sheet is not editable.");

                if (_oldPunchlist.DueDate.Value.Date != model.DueDate.Value.Date)
                    if (model.DueDate.Value.Date < DateTime.Now.Date)
                        throw new ApplicationException("Backdating of due date is not valid.");
            }

            return true;
        }
    }
}
