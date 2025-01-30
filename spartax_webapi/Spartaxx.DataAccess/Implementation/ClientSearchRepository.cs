using Spartaxx.BusinessObjects;
using Spartaxx.Common;
using Spartaxx.DataObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    public class ClientSearchRepository : IClientSearchRepository
    {
        private readonly DapperConnection _Connection;
        public ClientSearchRepository(DapperConnection Connection)
        {
            _Connection = Connection;
        }

        public List<PTXboAccountSearchDetails> getSearchedAccountDetails(PTXboClientSearch clientSearch)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@ClientNumber", clientSearch.clientNumber);
                _hashtable.Add("@ClientName", clientSearch.clientName);
                _hashtable.Add("@AccountNumber", clientSearch.accountNumber);
                _hashtable.Add("@PhoneNumber", clientSearch.phoneNumber);
                _hashtable.Add("@CompanyName", (clientSearch.companyName == null) ? null : clientSearch.companyName.Replace("'", "''"));
                _hashtable.Add("@ProjectName", (clientSearch.projectName == null) ? null : clientSearch.projectName.Replace("'", "''"));
                _hashtable.Add("@CadLegalName", (clientSearch.cadLegalName == null) ? null : clientSearch.cadLegalName.Replace("'", "''"));
                _hashtable.Add("@LegalDesc", (clientSearch.legalDescription == null) ? null : clientSearch.legalDescription.Replace("'", "''"));
                _hashtable.Add("@PropertyAddress", (clientSearch.propertyAddress == null) ? null : clientSearch.propertyAddress.Replace("'", "''"));
                _hashtable.Add("@ClientAddress", (clientSearch.clientAddress == null) ? null : clientSearch.clientAddress.Replace("'", "''"));
                _hashtable.Add("@Email", (clientSearch.email == null) ? null : clientSearch.email.Replace("'", "''"));
                _hashtable.Add("@FirstName", (clientSearch.firstName == null) ? null : clientSearch.firstName.Replace("'", "''"));
                _hashtable.Add("@LastName", (clientSearch.lastName == null) ? null : clientSearch.lastName.Replace("'", "''"));
                _hashtable.Add("@CountyId", Convert.ToString(clientSearch.CountyId));
                _hashtable.Add("@causenumber", clientSearch.CauseNumber);
                _hashtable.Add("@invoiceid", clientSearch.InvoiceNumber);
                _hashtable.Add("@arbitrationid", clientSearch.ArbitrationID);
                _hashtable.Add("@parcelid", clientSearch.ParcelID);
                _hashtable.Add("@causename", Convert.ToString(clientSearch.CauseName));
                _hashtable.Add("@clientstatus", Convert.ToString(clientSearch.StatusID));
                _hashtable.Add("@CountyName", Convert.ToString(clientSearch.CountyName));
                _hashtable.Add("@StateName", Convert.ToString(clientSearch.StateName));
                _hashtable.Add("@SalesAgentName", Convert.ToString(clientSearch.SalesAgentName));

                if (!string.IsNullOrEmpty(clientSearch.propertyAddress))
                {
                    var result = _Connection.Select<PTXboAccountSearchDetails>(StoredProcedureNames.usp_SPARTAXX_SEL_getSearchedAccountDetails_propertyaddress,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                    return result;
                }
                else if (!string.IsNullOrEmpty(clientSearch.clientAddress))
                {
                    var result = _Connection.Select<PTXboAccountSearchDetails>(StoredProcedureNames.usp_SPARTAXX_SEL_getSearchedAccountDetails_clientaddress,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                    return result;
                }
                else
                {
                    var result = _Connection.Select<PTXboAccountSearchDetails>(StoredProcedureNames.usp_SPARTAXX_SEL_getSearchedAccountDetails,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PTXboClientSearchDetails> getSearchedClientDetails(PTXboClientSearch clientSearch)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@ClientNumber", clientSearch.clientNumber);
                _hashtable.Add("@ClientName", clientSearch.clientName);
                _hashtable.Add("@AccountNumber", clientSearch.accountNumber);
                _hashtable.Add("@PhoneNumber", clientSearch.phoneNumber);
                _hashtable.Add("@CompanyName", clientSearch.companyName);
                _hashtable.Add("@ProjectName", clientSearch.projectName);
                _hashtable.Add("@CadLegalName", clientSearch.cadLegalName);
                _hashtable.Add("@LegalDesc", clientSearch.legalDescription);
                _hashtable.Add("@PropertyAddress", (clientSearch.propertyAddress == null) ? null : clientSearch.propertyAddress.Replace("'", "''"));
                _hashtable.Add("@ClientAddress", (clientSearch.clientAddress == null) ? null : clientSearch.clientAddress.Replace("'", "''"));
                _hashtable.Add("@Email", clientSearch.email);
                _hashtable.Add("@FirstName", clientSearch.firstName);
                _hashtable.Add("@LastName", clientSearch.lastName);
                _hashtable.Add("@causenumber", clientSearch.CauseNumber);
                _hashtable.Add("@invoiceid", clientSearch.InvoiceNumber);
                _hashtable.Add("@arbitrationid", clientSearch.ArbitrationID);
                _hashtable.Add("@parcelid", clientSearch.ParcelID);
                _hashtable.Add("@causename", Convert.ToString(clientSearch.CauseName));
                _hashtable.Add("@clientstatus", Convert.ToString(clientSearch.StatusID));
                _hashtable.Add("@CountyName", Convert.ToString(clientSearch.CountyName));
                _hashtable.Add("@StateName", Convert.ToString(clientSearch.StateName));
                _hashtable.Add("@SalesAgentName", Convert.ToString(clientSearch.SalesAgentName));

                if (!string.IsNullOrEmpty(clientSearch.propertyAddress))
                {
                    var result = _Connection.Select<PTXboClientSearchDetails>(StoredProcedureNames.usp_SPARTAXX_SEL_getSearchedClientDetails_propertyaddress,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                    return result;
                }
                else if (!string.IsNullOrEmpty(clientSearch.clientAddress))
                {
                    var result = _Connection.Select<PTXboClientSearchDetails>(StoredProcedureNames.usp_SPARTAXX_SEL_getSearchedClientDetails_clientaddress,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                    return result;
                }
                else
                {
                    var result = _Connection.Select<PTXboClientSearchDetails>(StoredProcedureNames.usp_SPARTAXX_SEL_getSearchedClientDetails,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PTXboClientSearchDetails> getCAFSearchedClientDetails(PTXboClientSearch clientSearch)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@ClientNumber", clientSearch.clientNumber);
                _hashtable.Add("@ClientName", clientSearch.clientName);
                _hashtable.Add("@AccountNumber", clientSearch.accountNumber);
                _hashtable.Add("@PhoneNumber", clientSearch.phoneNumber);
                _hashtable.Add("@CompanyName", clientSearch.companyName);
                _hashtable.Add("@ProjectName", clientSearch.projectName);
                _hashtable.Add("@CadLegalName", clientSearch.cadLegalName);
                _hashtable.Add("@LegalDesc", clientSearch.legalDescription);
                _hashtable.Add("@PropertyAddress", (clientSearch.propertyAddress == null) ? null : clientSearch.propertyAddress.Replace("'", "''"));
                _hashtable.Add("@ClientAddress", (clientSearch.clientAddress == null) ? null : clientSearch.clientAddress.Replace("'", "''"));
                _hashtable.Add("@Email", clientSearch.email);
                _hashtable.Add("@FirstName", clientSearch.firstName);
                _hashtable.Add("@LastName", clientSearch.lastName);

                var result = _Connection.Select<PTXboClientSearchDetails>(StoredProcedureNames.usp_getSearchedClientDetails,
                    _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
