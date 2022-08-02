using CTI.HI.Business.Entities;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business.Contracts
{
    [ServiceContract]
    public interface IContractorService
    {
        [OperationContract]
        Task<Contractor> GetContractorAsync(string ContractorCode);
        [OperationContract]
        Task<IEnumerable<ContractorRepresentative>> GetContractorRepresentativeAsync(string ContractorCode);
        [OperationContract]
        Task<Contractor> GetVendorByRepresentativeAsync(string representative);
    }
}
