using CTI.HI.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CTI.HI.Business.Entities;
using CTI.HI.Data.Contracts.Frebas;
using CTI.HI.Data.Constant;

namespace CTI.HI.Data.Repository.Frebas.StoredProcedure
{
    public class FrebasStoredProceduresRepository
    {
        private FrebasContext _dbContext;
        private IUserRepository _userRepo;
        public FrebasStoredProceduresRepository(FrebasContext dbContext)
        {
            _dbContext = dbContext;
            _userRepo = new UserRepository();
        }
        public async Task<StoredProcedureError> UpdateMilestonePercentage(string userName,
                                                                          string otcNumber,
                                                                          string contractorNumber,
                                                                          int managingContractorId,
                                                                          decimal newPercentage,
                                                                          string milestoneCode,
                                                                          DateTime? dateVisited = null)
        {

            //Get RoleCode of User
            var _usr = await _userRepo.GetUserInfoAsync(userName);
             

            if (_usr.RoleCode == UserRoleCode.ProjectEngineer || _usr.RoleCode == UserRoleCode.DPIOfficer || _usr.RoleCode == UserRoleCode.ProjectTechnicalHead) //Project Engr | //DPI Officer 
            {
                //mode 3, update milestone percentage
                var milestoneUpdateResult = await ActualConstructionOnGoingConstruction(3, userName, otcNumber, contractorNumber, managingContractorId, newPercentage, milestoneCode, dateVisited);

                //if success call Inv Percetage update
                if (milestoneUpdateResult.IsSucess)
                {
                    //mode 6, update Inv %Completion
                    var invPercentageResult = await ActualConstructionOnGoingConstruction(6, userName, otcNumber, contractorNumber, managingContractorId, newPercentage, milestoneCode, dateVisited);

                    //success or not return 
                    return invPercentageResult;
                }
                //if not success then return
                return milestoneUpdateResult;
            }
            else if(_usr.RoleCode == UserRoleCode.QA) //Quality Assurance
            {
                //mode 3, update milestone percentage
                var milestoneUpdateResult = await ActualConstructionQualityAssurance(3, userName, otcNumber, contractorNumber, managingContractorId, newPercentage, milestoneCode, dateVisited);

                //if success call Inv Percetage update
                if (milestoneUpdateResult.IsSucess)
                {
                    //mode 6, update Inv %Completion
                    var invPercentageResult = await ActualConstructionQualityAssurance(4, userName, otcNumber, contractorNumber, managingContractorId, newPercentage, milestoneCode, dateVisited);

                    //success or not return 
                    return invPercentageResult;
                }
                //if not success then return
                return milestoneUpdateResult;
            }
            else if (_usr.RoleCode == UserRoleCode.Contractor) //Quality Assurance
            {
                //return new StoredProcedureError
                //{
                //    ErrorNumber = 1111,
                //    Message = "User Role is not valid to update Milestone Percentage"
                //};
                return new StoredProcedureError
                {
                    ErrorNumber = 8888,
                    Message = "Contractor is now available to update POC but not change the POC of Unit"
                };
            }
            else
            {
                return new StoredProcedureError
                {
                    ErrorNumber = 1111,
                    Message = "User Role is not valid to update Milestone Percentage"
                };
            }
        }

        private async Task<StoredProcedureError> ActualConstructionOnGoingConstruction(int mode,
                                                                                        string userName,
                                                                                        string otcNumber,
                                                                                        string contractorNumber,
                                                                                        int managingContractorId,
                                                                                        decimal newPercentage = 0,
                                                                                        string milestoneCode = "",
                                                                                        DateTime? dateVisited = null)
        {
            try
            {


                var _mode = new SqlParameter("@pintMode", Convert.ToInt32(mode));
                var _username = new SqlParameter("@pstrUserName", userName);
                var _referenceObject = new SqlParameter("@pstrReferenceObject", "");
                var _pstrSearch = new SqlParameter("@pstrSearch", "");
                var _pintStart = new SqlParameter("@pintStart", Convert.ToInt32(0));
                var _pintLength = new SqlParameter("@pintLength", Convert.ToInt32(0));
                var _pintSortCol = new SqlParameter("@pintSortCol", Convert.ToInt32(0));
                var _pstrSortDir = new SqlParameter("@pstrSortDir", "");
                var _projectAuthorization = new SqlParameter("@pstrProjectAuthorization", "");
                var _newPercentage = new SqlParameter("@pintNewPercentage", newPercentage);
                var _percentageReferenceNumber = new SqlParameter("@pstrPercentageReferenceNumber", "");
                var _otcNumber = new SqlParameter("@pstrOTCNumber", otcNumber);
                var _contractorNumber = new SqlParameter("@pstrContractorNumber", contractorNumber);
                var _managingContractorID = new SqlParameter("@pintManagingContractorID", Convert.ToInt32(managingContractorId));
                var _constructionMilestoneCode = new SqlParameter("@pstrConstructionMilestoneCode", milestoneCode);
                var _dateVisited = new SqlParameter("@pdteDateVisited", dateVisited);
                var _remarks = new SqlParameter("@pstrRemarks", "Update from Mobile App");
                var _isDelayed = new SqlParameter("@pbitIsDelayed", true);
                var _reasonCode = new SqlParameter("@pstrReasonCode", "");
                var _reasonRemarks = new SqlParameter("@pstrReasonRemarks", "");


                var _pstrXmlColumnAlias = new SqlParameter("@pstrXmlColumnAlias", "");
                var _pstrXmlColumns = new SqlParameter("@pstrXmlColumns", "");
                var _pxmlUpload = new SqlParameter("@pxmlUpload", "");
                var _pstrDownloadSelectedProject = new SqlParameter("@pstrDownloadSelectedProject", "");
                var _pbitIsAccessHCOM = new SqlParameter("@pbitIsAccessHCOM", false);
                               
                
                var _errorNo = new SqlParameter("@pintErrorNumber", System.Data.SqlDbType.Int, 4);
                var _errorMessage = new SqlParameter("@pstrErrorMessage", System.Data.SqlDbType.VarChar, 200);

                _errorNo.Direction = System.Data.ParameterDirection.Output;
                _errorMessage.Direction = System.Data.ParameterDirection.Output;

                await _dbContext.Database.ExecuteSqlCommandAsync("sp_hcmActualConstructionOnGoingConstruction " +
                    "@pintMode, " +
                    "@pstrUserName, " +
                    "@pstrReferenceObject, " +
                    "@pstrSearch," +
                    "@pintStart," +
                    "@pintLength," +
                    "@pintSortCol," +
                    "@pstrSortDir," +
                    "@pstrProjectAuthorization, " +
                    "@pintNewPercentage,  " +
                    "@pstrPercentageReferenceNumber, " +
                    "@pstrOTCNumber, " +
                    "@pstrContractorNumber, " +
                    "@pintManagingContractorID, " +
                    "@pstrConstructionMilestoneCode, " +
                    "@pdteDateVisited, " +
                    "@pstrRemarks, " +
                    "@pbitIsDelayed, " +
                    "@pstrReasonCode, " +
                    "@pstrReasonRemarks, " +
                    "@pstrXmlColumnAlias, " +
                    "@pstrXmlColumns, " +
                    "@pxmlUpload, " +
                    "@pstrDownloadSelectedProject," +
                    "@pbitIsAccessHCOM," +
                    "@pintErrorNumber OUTPUT, " +
                    "@pstrErrorMessage OUTPUT ",
                    _mode,
                    _username,
                    _referenceObject,
                    _pstrSearch,
                    _pintStart,
                    _pintLength,
                    _pintSortCol,
                    _pstrSortDir,
                    _projectAuthorization,
                    _newPercentage,
                    _percentageReferenceNumber,
                    _otcNumber,
                    _contractorNumber,
                    _managingContractorID,
                    _constructionMilestoneCode,
                    _dateVisited,
                    _remarks,
                    _isDelayed,
                    _reasonCode,
                    _reasonRemarks,
                    _pstrXmlColumnAlias,
                    _pstrXmlColumns,
                    _pxmlUpload,
                    _pstrDownloadSelectedProject,
                    _pbitIsAccessHCOM,
                _errorNo, _errorMessage);

                return new Business.Entities.StoredProcedureError() { ErrorNumber = Convert.ToInt32(_errorNo.Value), Message = _errorMessage.Value.ToString() };
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }

        private async Task<StoredProcedureError> ActualConstructionQualityAssurance(int mode,
                                                                                    string userName,
                                                                                    string otcNumber,
                                                                                    string contractorNumber,
                                                                                    int managingContractorId,
                                                                                    decimal newPercentage = 0,
                                                                                    string milestoneCode = "",
                                                                                    DateTime? dateVisited = null)
        {
            try
            {
                var _mode = new SqlParameter("@pintMode", Convert.ToInt32(mode));
                var _username = new SqlParameter("@pstrUserName", userName);
                var _referenceObject = new SqlParameter("@pstrReferenceObject", "");
                var _pstrSearch = new SqlParameter("@pstrSearch", "");
                var _pintStart = new SqlParameter("@pintStart", Convert.ToInt32(0));
                var _pintLength = new SqlParameter("@pintLength", Convert.ToInt32(0));
                var _pintSortCol = new SqlParameter("@pintSortCol", Convert.ToInt32(0));
                var _pstrSortDir = new SqlParameter("@pstrSortDir", "");
                var _projectAuthorization = new SqlParameter("@pstrProjectAuthorization", "");
                var _newPercentage = new SqlParameter("@pintNewPercentage", newPercentage);
                var _percentageReferenceNumber = new SqlParameter("@pstrPercentageReferenceNumber", "");
                var _otcNumber = new SqlParameter("@pstrOTCNumber", otcNumber);
                var _contractorNumber = new SqlParameter("@pstrContractorNumber", contractorNumber);
                var _managingContractorID = new SqlParameter("@pintManagingContractorID", Convert.ToInt32(managingContractorId));
                var _constructionMilestoneCode = new SqlParameter("@pstrConstructionMilestoneCode", milestoneCode);
                var _remarks = new SqlParameter("@pstrOverrideRemarks", "Update from Mobile App");
                var _dateVisited = new SqlParameter("@pdteDateVisited", dateVisited);
                var _isDelayed = new SqlParameter("@pbitIsDelayed", true);
                var _pbitIsAccessHCOM = new SqlParameter("@pbitIsAccessHCOM", false);

                var _errorNo = new SqlParameter("@pintErrorNumber", System.Data.SqlDbType.Int, 4);
                var _errorMessage = new SqlParameter("@pstrErrorMessage", System.Data.SqlDbType.VarChar, 200);

                _errorNo.Direction = System.Data.ParameterDirection.Output;
                _errorMessage.Direction = System.Data.ParameterDirection.Output;

                await _dbContext.Database.ExecuteSqlCommandAsync("sp_hcmActualConstructionQualityAssurance " +
                    "@pintMode, " +
                    "@pstrUserName, " +
                    "@pstrReferenceObject, " +
                    "@pstrSearch," +
                    "@pintStart," +
                    "@pintLength," +
                    "@pintSortCol," +
                    "@pstrSortDir," +
                    "@pstrProjectAuthorization, " +
                    "@pintNewPercentage,  " +
                    "@pstrPercentageReferenceNumber, " +
                    "@pstrOTCNumber, " +
                    "@pstrContractorNumber, " +
                    "@pintManagingContractorID, " +
                    "@pstrConstructionMilestoneCode, " +
                    "@pstrOverrideRemarks, " +
                    "@pdteDateVisited, " +
                    "@pbitIsDelayed, " +
                    "@pbitIsAccessHCOM," +
                    "@pintErrorNumber OUTPUT, " +
                    "@pstrErrorMessage OUTPUT ",
                    _mode,
                    _username,
                    _referenceObject,
                    _pstrSearch,
                    _pintStart,
                    _pintLength,
                    _pintSortCol,
                    _pstrSortDir,
                    _projectAuthorization,
                    _newPercentage,
                    _percentageReferenceNumber,
                    _otcNumber,
                    _contractorNumber,
                    _managingContractorID,
                    _constructionMilestoneCode,
                    _remarks,
                    _dateVisited,
                    _isDelayed,
                    _pbitIsAccessHCOM,
                _errorNo, _errorMessage);

                return new Business.Entities.StoredProcedureError() { ErrorNumber = Convert.ToInt32(_errorNo.Value), Message = _errorMessage.Value.ToString() };
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
    }
}
