using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business.BusinessEngines.BusinessEngineContracts
{ 
    public interface IMilestoneEngine : IBusinessEngine
    {
        Task<bool> isValidPercentageAsync(int constructionMilestoneId,decimal percentage, string userName);
        Task<bool> isAllowedToChangePercentageAsync(int constructionMilestoneId, decimal percentage,string userName); 
    }
}
