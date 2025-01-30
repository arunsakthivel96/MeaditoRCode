using Spartaxx.BusinessObjects;
using Spartaxx.Common;
using Spartaxx.DataObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    public class ExistingClientRepository : IExistingClientRepository
    {
        private readonly DapperConnection _Connection;
        public ExistingClientRepository(DapperConnection Connection)
        {
            _Connection = Connection;
        }

        public DataSet getClientCodesDetails(int ClientID)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@ClientID", ClientID);

                var result = _Connection.SelectDataSet(StoredProcedureNames.usp_get_ClientCodes,
                    _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PTXboSaveAddressContactAndPhoneResult SaveAddressContactAndPhone(int AddressId, int AddressTypeId, int GroupID = 0)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@AddressId", AddressId);
                _hashtable.Add("@AddressTypeID", AddressTypeId);
                _hashtable.Add("@GroupID", GroupID);

                var result = _Connection.SelectDataSet(StoredProcedureNames.usp_Save_Address_ContactPhone,
                    _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);

                var saveAddressContactAndPhoneResult = new PTXboSaveAddressContactAndPhoneResult();

                if (result.Tables.Count > 0 || result.Tables[0].Rows.Count > 0)
                {
                    saveAddressContactAndPhoneResult.SuccessMessage = result.Tables[0].Rows[0]["Success_Message"].ToString();
                    if (result.Tables[0].Columns.Count > 1)
                        saveAddressContactAndPhoneResult.CopiedAddressID = Convert.ToInt32(result.Tables[0].Rows[0][1]);
                }
                else
                {
                    saveAddressContactAndPhoneResult.CopiedAddressID = 0;
                }

                return saveAddressContactAndPhoneResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PTXboAccountDetails> getAccountSummary(int ClientID, int UserID)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@ClientID", ClientID);
                _hashtable.Add("@UserID", UserID);

                var result = _Connection.Select<PTXboAccountDetails>(StoredProcedureNames.usp_getAccountDetailsSummary,
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
