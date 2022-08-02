using Core.Common.Contracts;
using CTI.HI.Business.Contracts;
using CTI.HI.Business.Entities;
using CTI.HI.Data.Contracts.Frebas;
using CTI.HI.Data.Repository.Frebas;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                   ConcurrencyMode = ConcurrencyMode.Multiple)]
    class ContractorManager : ManagerBase, IContractorService
    {

        #region Constructors 
        private IContractorRepository _ContractorRepo; 
        public ContractorManager()
        { 
            _ContractorRepo = new ContractorRepository(); 
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        public ContractorManager(IDataRepositoryFactory dataRepositoryFactory) : this()
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;

        public ContractorManager(IBusinessEngineFactory businessEngineFactory) : this()
        {
            _BusinessEngineFactory = businessEngineFactory;
        }

        public ContractorManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory) : this()
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
        }
        #endregion


        public async Task<Contractor> GetContractorAsync(string ContractorCode)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    Log.Information("GetContractorAsync by Contractor Code: {ContractorCode}", ContractorCode);
                    var vendor = await _ContractorRepo.GetVendorAsync(ContractorCode);
                    return new Contractor
                    {
                        Code = vendor.Code,
                        Name = vendor.Name,
                        Representative = await _ContractorRepo.GetVendorRepresentativeAsync(ContractorCode)
                    };
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetContractorAsync by Contractor Code: {ContractorCode}, Error found: {ex}", ContractorCode, ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetContractorAsync by Contractor Code: {ContractorCode}, Error found: {ex}", ContractorCode, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetContractorAsync by Contractor Code: {ContractorCode}, Error found: {ex}", ContractorCode, ex);
                    throw new Exception(errMsg);
                }
            });
        }

        public async Task<IEnumerable<ContractorRepresentative>> GetContractorRepresentativeAsync(string ContractorCode)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    Log.Information("GetContractorRepresentativeAsync by Contractor Code: {ContractorCode}", ContractorCode);
                    var vendor = await _ContractorRepo.GetVendorAsync(ContractorCode);
                    return await _ContractorRepo.GetVendorRepresentativeAsync(ContractorCode);
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetContractorRepresentativeAsync by Contractor Code: {ContractorCode}, Error found: {ex}", ContractorCode, ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetContractorRepresentativeAsync by Contractor Code: {ContractorCode}, Error found: {ex}", ContractorCode, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetContractorRepresentativeAsync by Contractor Code: {ContractorCode}, Error found: {ex}", ContractorCode, ex);
                    throw new Exception(errMsg);
                }
            });
        }


        public async Task<Contractor> GetVendorByRepresentativeAsync(string representative)
        {
            return await ExecuteFaultHandledOperation(async () =>
            {
                try
                {
                    Log.Information("GetVendorByRepresentativeAsync by Representative: {representative}", representative);
                    var vendor = await _ContractorRepo.GetVendorByRepresentativeAsync(representative);
                    return vendor;
                }
                catch (NullReferenceException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetVendorByRepresentativeAsync by Representative: {representative}, Error found: {ex}", representative, ex);
                    throw new NullReferenceException(errMsg);
                }
                catch (ApplicationException ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetVendorByRepresentativeAsync by Representative: {representative}, Error found: {ex}", representative, ex);
                    throw new ApplicationException(errMsg);
                }
                catch (Exception ex)
                {
                    string errMsg = ErrorMessageUtil.GetFullExceptionMessage(ex);
                    Log.Error("GetVendorByRepresentativeAsync by Representative: {representative}, Error found: {ex}", representative, ex);
                    throw new Exception(errMsg);
                }
            });
        }
    }
}
