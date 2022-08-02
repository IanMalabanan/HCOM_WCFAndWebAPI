using Core.Common.Contracts; 
using CTI.HI.Data.Constant;
using CTI.HI.Data.Context;
using CTI.HI.Data.Contracts.Frebas;
using CTI.SSM.Business.Entities;
using GlobalBusinessComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTI.HI.Data.Repository.Frebas
{
    [Export(typeof(IUserRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class UserRepository : DataRepositoryFrebasBase<User>, IUserRepository
    {
        public async Task<Business.Entities.User> AuthenticateUserAsync(string userName, string password)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    var loginHelper = new clsObjectControl();
                    var encryptedPassword = loginHelper.EncryptDecrypt(clsObjectControl.ProcessType.Encrypt, password, CustomLoginKey.LoginKeyDate);

                    var _usr = await cntxt.User.Where(u => u.UserName.ToLower() == userName.ToLower() && u.Password == encryptedPassword).FirstOrDefaultAsync();

                    if (_usr == null)
                        throw new ApplicationException("User does not exists");
                    else if (!_usr.Active)
                        throw new ApplicationException("User account is not activated"); ;

                    return await GetUserInfoAsync(_usr.UserName);
                }
            }
            catch(ApplicationException ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
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

        public async Task<Business.Entities.User> GetUserInfoAsync(string userName)
        {
            try
            { 
                using (var cntxt = new FrebasContext())
                {
                    var _usr = await cntxt.User.Where(u => u.UserName.ToLower() == userName.ToLower()).FirstOrDefaultAsync();

                    if (_usr == null)
                        throw new ApplicationException("User does not exists"); ;

                    ///if Contractor
                    if (_usr.UserTypeCode == "CONT")
                    {

                        var _dteToday = DateTime.Today;
                        var _dteTomorrow = DateTime.Today.AddDays(1).Date;
                        var _userTypeReference = !string.IsNullOrEmpty(_usr.UserTypeReference) ? Convert.ToInt32(_usr.UserTypeReference) : 0;
                        var _cont= await (from ctp in cntxt.ContractorPersonnel.Where(c => c.ID == _userTypeReference)
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
                                      select new Business.Entities.User
                                      {
                                          Id = _usr.UserName,
                                          FullName = _usr.FullName,
                                          Email = ctp.EmailAddress,
                                          IsActive = _usr.Active,
                                          RoleCode = _usr.UserTypeCode
                                      }).FirstOrDefaultAsync();

                        if (_cont == null)
                            throw new Exception("Contractor Personnel is not authorized.");

                        return _cont;
                    }
                    else if (_usr.UserTypeCode == "EMPL" || _usr.UserTypeCode == "SUBS")
                    {

                        var _dteToday = DateTime.Today;
                        var _dteTomorrow = DateTime.Today.AddDays(1).Date;
                        var x = await (from upr in cntxt.UserProjectRole.Where(u => u.UserName == userName && DbFunctions.TruncateTime(u.DateTo ?? _dteTomorrow) >= _dteToday)
                                      join prl in cntxt.ProjectRole
                                      on upr.ProjectRoleCode equals prl.Code
                                      select new Business.Entities.User
                                      {
                                          Id = _usr.UserName,
                                          FullName = _usr.FullName,
                                          Email = _usr.EmailAddress,
                                          RoleCode = upr.ProjectRoleCode,
                                          IsActive = _usr.Active,
                                          InspectionRadius = prl.Radius
                                      }).FirstOrDefaultAsync();
                        if (x == null)
                            throw new ApplicationException("No project role assigned to user or assignment already ended.");

                        return x;
                    }
                    else
                    {
                        return new Business.Entities.User
                        {
                            Id = _usr.UserName,
                            FullName = _usr.FullName,
                            Email = _usr.EmailAddress,
                            RoleCode = _usr.UserTypeCode,
                            IsActive = _usr.Active
                        };
                    }
                }
            }
            catch (ApplicationException ex)
            {
                throw new ApplicationException(ex.Message,ex.InnerException);
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

        public async Task<Business.Entities.User> GetUserInfoBaseOnDateAsync(string userName, DateTime ReferenceDate)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    var _usr = await cntxt.User.Where(u => u.UserName.ToLower() == userName.ToLower()).FirstOrDefaultAsync();

                    if (_usr == null)
                        throw new ApplicationException("User does not exists"); ;

                    ///if Contractor
                    if (_usr.UserTypeCode == "CONT")
                    {

                        var _dteToday = ReferenceDate.Date;
                        var _dteTomorrow = DateTime.Today.AddDays(1).Date;
                        var _userTypeReference = Convert.ToInt32(_usr.UserTypeReference);
                        var _cont = await (from ctp in cntxt.ContractorPersonnel.Where(c => c.ID == _userTypeReference)
                                           join cau in cntxt.ContractorPersonnelAuthorized.Where(a => (a.DateFrom <= _dteToday && (a.DateTo ?? _dteTomorrow) >= _dteToday))
                                           //join cau in cntxt.ContractorPersonnelAuthorized.Where(a => DbFunctions.TruncateTime(a.DateTo ?? _dteTomorrow) >= _dteToday)
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
                                           select new Business.Entities.User
                                           {
                                               Id = _usr.UserName,
                                               FullName = _usr.FullName,
                                               Email = ctp.EmailAddress,
                                               IsActive = _usr.Active,
                                               RoleCode = _usr.UserTypeCode
                                           }).FirstOrDefaultAsync();

                        if (_cont == null)
                            throw new ApplicationException("Contractor Personnel is not authorized.");

                        return _cont;
                    }
                    else if (_usr.UserTypeCode == "EMPL")
                    {

                        var _dteToday = DateTime.Today;
                        var _dteTomorrow = DateTime.Today.AddDays(1).Date;
                        var x = await (from upr in cntxt.UserProjectRole.Where(u => u.UserName == userName && (u.DateFrom <= ReferenceDate && (u.DateTo ?? _dteTomorrow) >= ReferenceDate))
                                       join prl in cntxt.ProjectRole
                                       on upr.ProjectRoleCode equals prl.Code
                                       select new Business.Entities.User
                                       {
                                           Id = _usr.UserName,
                                           FullName = _usr.FullName,
                                           Email = _usr.EmailAddress,
                                           RoleCode = upr.ProjectRoleCode,
                                           IsActive = _usr.Active,
                                           InspectionRadius = prl.Radius
                                       }).FirstOrDefaultAsync();


                        if (x == null)
                            throw new ApplicationException("No project role assigned to user");

                        return x;
                    }
                    else
                    {
                        return new Business.Entities.User
                        {
                            Id = _usr.UserName,
                            FullName = _usr.FullName,
                            Email = _usr.EmailAddress,
                            RoleCode = _usr.UserTypeCode,
                            IsActive = _usr.Active
                        };
                    }
                }
            }
            catch(ApplicationException ex)
            {
                throw new ApplicationException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<double> GetUserInspectionRadius(string RoleCode)
        {
            try
            {
                using (var cntxt = new FrebasContext())
                {
                    return await cntxt.ProjectRole.Where(r => r.Code == RoleCode).Select(r => (double)(r.Radius ?? 0)).FirstOrDefaultAsync();
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

        protected override User AddEntity(FrebasContext entityContext, User entity)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<User> GetEntities(FrebasContext entityContext)
        {
            throw new NotImplementedException();
        }

        protected override User GetEntity(FrebasContext entityContext, int id)
        {
            throw new NotImplementedException();
        }

        protected override User UpdateEntity(FrebasContext entityContext, User entity)
        {
            throw new NotImplementedException();
        }
    }
}
