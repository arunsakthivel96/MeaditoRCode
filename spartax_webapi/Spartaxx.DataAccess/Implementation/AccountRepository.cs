using Spartaxx.BusinessObjects;
using Spartaxx.Common;
using Spartaxx.Common.BusinessObjects;
using Spartaxx.DataObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DapperConnection _Connection;
        public AccountRepository(DapperConnection Connection)
        {
            _Connection = Connection;
        }

        public List<PTXboReactivationHistory> ReactivationHistory(int AccountId)
        {
            Hashtable _hashtable = new Hashtable();
            _hashtable.Add("@AccountId", AccountId);
            return _Connection.Select<PTXboReactivationHistory>("usp_getReactivationHistory",
                                   _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
        }

        public int CreateQuickNewClientValues(PTXboNewClientQuickSetup objNewClientQuickSetup, List<PTXboPropertiesFromTaxRoll> lstPropertiesFromTaxroll)
        {
            int clientid = 0;

            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@ClientSourceId", objNewClientQuickSetup.ClientSourceId);
                _hashtable.Add("@ClientEntryID", objNewClientQuickSetup.EntryID);
                _hashtable.Add("@ClientSetupId", objNewClientQuickSetup.ClientSetupMethodId);
                _hashtable.Add("@SalesAgentId", objNewClientQuickSetup.AgentId);
                _hashtable.Add("@AgentId", objNewClientQuickSetup.Agent);
                _hashtable.Add("@Agenttype", objNewClientQuickSetup.Agenttype);
                _hashtable.Add("@Email", objNewClientQuickSetup.Email);
                _hashtable.Add("@PhoneNumber", objNewClientQuickSetup.PhoneNumber);
                _hashtable.Add("@Userid", objNewClientQuickSetup.Userid);
                _hashtable.Add("@MailingAddress", objNewClientQuickSetup.MailingAddress);
                _hashtable.Add("@FirstName", objNewClientQuickSetup.FirstName);
                _hashtable.Add("@LastName", objNewClientQuickSetup.LastName);
                _hashtable.Add("@StateId", objNewClientQuickSetup.StateId);
                _hashtable.Add("@CityId", objNewClientQuickSetup.CityId);
                _hashtable.Add("@ZipCode", objNewClientQuickSetup.ZipCode);
                _hashtable.Add("@isJudicial", objNewClientQuickSetup.isJudicial);
                _hashtable.Add("@ClientTypeID", objNewClientQuickSetup.ClientTypeId);
                _hashtable.Add("@IsNextYearSignup", objNewClientQuickSetup.IsNextYearSignup);
                _hashtable.Add("@IsSpanishFlag", objNewClientQuickSetup.IsSpanishFlag);
                _hashtable.Add("@LeadId", objNewClientQuickSetup.LeadId);
                
                _hashtable.Add("@PhoneType", objNewClientQuickSetup.PhoneType);
                _hashtable.Add("@PhoneVerification", objNewClientQuickSetup.PhoneVerification);

                _hashtable.Add("@AdditionalPhoneNumber", objNewClientQuickSetup.AdditionalPhoneNumber);
                _hashtable.Add("@AdditionalPhoneType", objNewClientQuickSetup.AdditionalPhoneType);
                _hashtable.Add("@AdditionalPhoneVerification", objNewClientQuickSetup.AdditionalPhoneVerification);
                _hashtable.Add("@EmpRefId", objNewClientQuickSetup.EmployeeID);

                var result = _Connection.ExecuteScalar(StoredProcedureNames.usp_GeneratingNewClientQuickSetup, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                clientid = int.Parse(result);

                if (Convert.ToInt32(result) > 0)
                {
                    foreach (PTXboPropertiesFromTaxRoll objProperties in lstPropertiesFromTaxroll)
                    {
                        Hashtable _hashtable1 = new Hashtable();

                        if (objProperties.isAddedFromManualSetup == true)
                        {
                            _hashtable1.Add("@State", objProperties.State);
                            _hashtable1.Add("@county", objProperties.County);
                            _hashtable1.Add("@City", objProperties.City);
                            _hashtable1.Add("@AccountNumber", objProperties.AccountNumber);
                            _hashtable1.Add("@ClientId", clientid);
                            _hashtable1.Add("@taxyear", objNewClientQuickSetup.TaxYear);
                            _hashtable1.Add("@UserID", objNewClientQuickSetup.Userid);
                            _hashtable1.Add("@ClientTypeID", objNewClientQuickSetup.ClientTypeId);
                            _hashtable1.Add("@AccountSourceId", objNewClientQuickSetup.ClientSourceId);
                            _hashtable1.Add("@AccountEntryID", objNewClientQuickSetup.EntryID);
                            _hashtable1.Add("@Township", objProperties.Township);
                            _hashtable1.Add("@Parish", objProperties.Parish);
                            _hashtable1.Add("@LeadId", objNewClientQuickSetup.LeadId);
                            _hashtable1.Add("@HotelTeamSalesAgentId", objNewClientQuickSetup.HotelTeamAgentId);
                            //_hashtable1.Add("@isFinancials", objProperties.isFinancials);
                            //_hashtable1.Add("@isFinancialsManual", objProperties.isFinancialsManual);
                            _hashtable1.Add("@EmpRefId", objNewClientQuickSetup.EmployeeID);
                            _hashtable1.Add("@AgentId", objNewClientQuickSetup.Agent);
                            _hashtable1.Add("@Agenttype", objNewClientQuickSetup.Agenttype);
                            _Connection.Execute(StoredProcedureNames.usp_InsertSearchedAccountQuickClientManualSetup, _hashtable1, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                        }
                        else
                        {
                            _hashtable1.Add("@State", objProperties.State);
                            _hashtable1.Add("@county", objProperties.County);
                            _hashtable1.Add("@City", objProperties.City);
                            _hashtable1.Add("@AccountNumber", objProperties.AccountNumber);
                            _hashtable1.Add("@ClientId", clientid);
                            _hashtable1.Add("@taxyear", objNewClientQuickSetup.TaxYear);
                            _hashtable1.Add("@UserID", objNewClientQuickSetup.Userid);
                            _hashtable1.Add("@ClientTypeID", objNewClientQuickSetup.ClientTypeId);
                            _hashtable1.Add("@AccountSourceId", objNewClientQuickSetup.ClientSourceId);
                            _hashtable1.Add("@AccountEntryID", objNewClientQuickSetup.EntryID);
                            _hashtable1.Add("@Township", objProperties.Township);
                            _hashtable1.Add("@Parish", objProperties.Parish);
                            _hashtable1.Add("@LeadId", objNewClientQuickSetup.LeadId);
                            _hashtable1.Add("@HotelTeamSalesAgentId", objNewClientQuickSetup.HotelTeamAgentId);
                            //_hashtable1.Add("@isFinancials", objProperties.isFinancials);
                            //_hashtable1.Add("@isFinancialsManual", objProperties.isFinancialsManual);
                            _hashtable1.Add("@EmpRefId", objNewClientQuickSetup.EmployeeID);
                            _hashtable1.Add("@AgentId", objNewClientQuickSetup.Agent);
                            _hashtable1.Add("@Agenttype", objNewClientQuickSetup.Agenttype);
                            _Connection.Execute(StoredProcedureNames.usp_InsertSearchedAccountQuickClient, _hashtable1, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                        }
                    }

                    Hashtable _hashtable2 = new Hashtable();
                    _hashtable2.Add("@ClientID", clientid);
                    _hashtable2.Add("@StandardContingency", objNewClientQuickSetup.StandardContingency);
                    _hashtable2.Add("@StandardContingencySecondTriennial", objNewClientQuickSetup.StandardContingencySecondTriennial);
                    _hashtable2.Add("@StandardContingencyThirdTriennial", objNewClientQuickSetup.StandardContingencyThirdTriennial);
                    _hashtable2.Add("@StandardFlatFee", objNewClientQuickSetup.StandardFlatFee);
                    _hashtable2.Add("@StandardRemarks", objNewClientQuickSetup.StandardRemarks);
                    _hashtable2.Add("@LitigationContingency", objNewClientQuickSetup.LitigationContingency);
                    _hashtable2.Add("@LitigationFlatFee", objNewClientQuickSetup.LitigationFlatFee);
                    _hashtable2.Add("@LitigationRemarks", objNewClientQuickSetup.LitigationRemarks);
                    _hashtable2.Add("@ArbitrationContingency", objNewClientQuickSetup.ArbitrationContingency);
                    _hashtable2.Add("@ArbitrationFlatFee", objNewClientQuickSetup.ArbitrationFlatFee);
                    _hashtable2.Add("@ArbitrationRemarks", objNewClientQuickSetup.ArbitrationRemarks);
                    _Connection.Execute(StoredProcedureNames.usp_InsertTermsAndGroupsQuickClient, _hashtable2, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                }

                return clientid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}