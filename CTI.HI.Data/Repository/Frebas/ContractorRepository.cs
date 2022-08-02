using CTI.ARM.Business.Entities;
using CTI.HI.Business.Entities;
using CTI.HI.Data.Context;
using CTI.HI.Data.Contracts.Frebas;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Data.Repository.Frebas
{
    [Export(typeof(IContractorRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ContractorRepository : DataRepositoryFrebasBase<VendorMasterData>, IContractorRepository
    {
        public async Task<ContractorRepresentative> GetRepresentativeAsync(int Id)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    var _dteToday = DateTime.Today;
                    var _dteTomorrow = DateTime.Today.AddDays(1).Date;
                    return await (from cau in cntxt.ContractorPersonnelAuthorized.Where(v => v.AuthorizedPersonnelID == Id && DbFunctions.TruncateTime(v.DateTo ?? _dteTomorrow) >= _dteToday)
                                  join ctp in cntxt.ContractorPersonnel
                                  on cau.AuthorizedPersonnelID equals ctp.ID
                                  select new ContractorRepresentative
                                  {
                                      ContactNumber = ctp.PrimaryMobileNumber,
                                      ContractorCode = cau.ContractorCode,
                                      Email = ctp.EmailAddress,
                                      Id = ctp.ID,
                                      Name = ctp.FirstName + " " + ctp.MiddleName + " " + ctp.LastName
                                  }).FirstOrDefaultAsync();
                }
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<VendorMasterData> GetRepresentativeVendorAsync(string Username)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    var _usr = await cntxt.User.Where(u => u.UserName == Username).FirstOrDefaultAsync();

                    if (_usr == null)
                        return null;

                    var _userTypeReference = Convert.ToInt32(_usr.UserTypeReference);

                    //get the contractor code of representative
                    var _dteToday = DateTime.Today;
                    var _dteTomorrow = DateTime.Today.AddDays(1).Date;
                    return await (from ctp in cntxt.ContractorPersonnel.Where(c => c.ID == _userTypeReference)
                                  join cau in cntxt.ContractorPersonnelAuthorized.Where(a => DbFunctions.TruncateTime(a.DateTo ?? _dteTomorrow) >= _dteToday)
                                 on new
                                 {
                                     contractorCode = ctp.ContractorCode,
                                     refId = ctp.ID
                                 } equals
                                 new
                                 {
                                     contractorCode = cau.ContractorCode,
                                     refId = cau.AuthorizedPersonnelID
                                 }
                                  join ven in cntxt.Vendor
                                  on cau.ContractorCode equals ven.Code
                                  select ven).FirstOrDefaultAsync();
                }
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<VendorMasterData> GetVendorAsync(string VendorCode)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    return await cntxt.Vendor.Where(v => v.Code == VendorCode).FirstOrDefaultAsync();
                }
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IEnumerable<ContractorRepresentative>> GetVendorRepresentativeAsync(string VendorCode)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    var _dteToday = DateTime.Today;
                    var _dteTomorrow = DateTime.Today.AddDays(1).Date;

                    return await (from cau in cntxt.ContractorPersonnelAuthorized
                                  .Where(v => v.ContractorCode == VendorCode && DbFunctions.TruncateTime(v.DateTo ?? _dteTomorrow) >= _dteToday)
                                  join ctp in cntxt.ContractorPersonnel
                                  on cau.AuthorizedPersonnelID equals ctp.ID
                                  select new ContractorRepresentative
                                  {
                                      ContactNumber = ctp.PrimaryMobileNumber,
                                      ContractorCode = cau.ContractorCode,
                                      Email = ctp.EmailAddress,
                                      Id = ctp.ID,
                                      Name = ctp.FirstName + " " + ctp.MiddleName + " " + ctp.LastName
                                  }).ToListAsync();
                }
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<Contractor> GetVendorByRepresentativeAsync(string representative)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    return await cntxt.VWContractors.Where(x => x.ContractorUserName == representative)
                        .Select(d => new Contractor
                        {
                            Code = d.VendorCode,
                            Name = d.VendorName
                        }).FirstOrDefaultAsync();
                }
            }
            catch(NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message, ex.InnerException);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        #region "other"


        protected override VendorMasterData AddEntity(FrebasContext entityContext, VendorMasterData entity)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<VendorMasterData> GetEntities(FrebasContext entityContext)
        {
            throw new NotImplementedException();
        }

        protected override VendorMasterData GetEntity(FrebasContext entityContext, int id)
        {
            throw new NotImplementedException();
        }

        protected override VendorMasterData UpdateEntity(FrebasContext entityContext, VendorMasterData entity)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
