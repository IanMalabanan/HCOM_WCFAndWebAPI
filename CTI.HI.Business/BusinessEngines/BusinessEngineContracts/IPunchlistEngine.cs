using Core.Common.Contracts;
using CTI.HI.Business.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business.BusinessEngines.BusinessEngineContracts
{ 
    public interface IPunchlistEngine : IBusinessEngine
    {
        Task<bool> canVoidPunchlistAsync(string userName, Punchlist model);
        Task<bool> canClosePunchlistAsync(string userName,int punchlistId, string punchlistStatus); 
        Task<bool> canCreatePunchlistAsync(string userName, int constructionMilestoneId, Punchlist model); 
        Task<bool> validatePunchlistInputsAsync(Punchlist model); 
    }
}
