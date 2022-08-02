using Core.Common.Contracts;
using CTI.ARM.Business.Entities;
using CTI.HCM.Business.Entities;
using CTI.HI.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Data.Contracts.Frebas
{
    public interface IContractorRepository : IDataRepository<VendorMasterData>
    {
        Task<VendorMasterData> GetVendorAsync(string VendorCode);
        Task<VendorMasterData> GetRepresentativeVendorAsync(string Username);
        Task<IEnumerable<ContractorRepresentative>> GetVendorRepresentativeAsync(string VendorCode);
        Task<ContractorRepresentative> GetRepresentativeAsync(int Id);

        Task<Contractor> GetVendorByRepresentativeAsync(string representative);
    } 
}
