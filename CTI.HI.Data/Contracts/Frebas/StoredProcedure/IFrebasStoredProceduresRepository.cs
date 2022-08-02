using CTI.HI.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Data.Contracts.Frebas.StoredProcedure
{
    public interface IFrebasStoredProceduresRepository
    {
        Task<StoredProcedureError> UpdateMilestonePercentage(string userName,
                                                            string otcNumber,
                                                            string contractorNumber,
                                                            string managingContractorId,
                                                            decimal newPercentage,
                                                            string percentageReferenceNumber,
                                                            string milestoneCode,
                                                            DateTime? dateVisited = null);
    }
}
