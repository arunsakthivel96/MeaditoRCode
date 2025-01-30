using Spartaxx.BusinessObjects;
using Spartaxx.Common;
using Spartaxx.DataObjects;
using Spartaxx.Utilities.Extenders;
using Spartaxx.Utilities.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
   public  class EditInvoiceRepository:IEditInvoiceRepository
    {
        private readonly DapperConnection _dapperConnection;
        public EditInvoiceRepository(DapperConnection dapperConnection)
        {
            _dapperConnection = dapperConnection;
        }

        public List<PTXboEditInvoiceSearchResult> GetEditInvoiceDetails(PTXboEditInvoiceSearchCriteria SearchCriteria)
        {
            try
            {
                Logger.For(this).Invoice("GetEditInvoiceDetails-API  reached " + ((object)SearchCriteria).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceID", SearchCriteria.InvoiceId);
                parameters.Add("@ClientNumber", SearchCriteria.ClientNumber);
                parameters.Add("@AccountNumber", SearchCriteria.AccountNumber);
                parameters.Add("@InvoiceType", SearchCriteria.InvoiceType);
                parameters.Add("@County", SearchCriteria.County);
                parameters.Add("@TaxYear", SearchCriteria.TaxYear);
                parameters.Add("@Project", SearchCriteria.Project);
                parameters.Add("@ClientName", SearchCriteria.ClientName);
                parameters.Add("@Address", SearchCriteria.Address);
                parameters.Add("@SelectedValueType", SearchCriteria.SelectedValueType);
                parameters.Add("@FromValue", SearchCriteria.FromValue);
                parameters.Add("@ToValue", SearchCriteria.ToValue);
                var result = _dapperConnection.Select<PTXboEditInvoiceSearchResult>(PTXdoStoredProcedureNames.usp_getInvoiceEditRequest, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetEditInvoiceDetails-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetEditInvoiceDetails-API  error " + ex);
                throw ex;
            }
        }

        
        public PTXboEditInvoiceAccountDetails GetEditInvoiceClientDetails(int invoiceID)
        {
            try
            {
                Logger.For(this).Invoice("GetEditInvoiceClientDetails-API  reached " + ((object)invoiceID).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceID", invoiceID);

                var result = _dapperConnection.Select<PTXboEditInvoiceAccountDetails>(PTXdoStoredProcedureNames.usp_GetClientDetailsforEditInvoice, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                Logger.For(this).Invoice("GetEditInvoiceClientDetails-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetEditInvoiceClientDetails-API  error " + ex);
                throw ex;
            }
        }


        public List<PTXboCSInvoiceHistory> GetEditInvoicePayments(int invoiceID,int invoiceType)
        {
            try
            {
                Logger.For(this).Invoice("GetEditInvoicePayments-API  reached " + ((object)"invoiceID="+invoiceID.ToString()+ "invoiceType="+invoiceType.ToString()).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceID", invoiceID);
                parameters.Add("@InvoiceType", invoiceType);
                var result = _dapperConnection.Select<PTXboCSInvoiceHistory>(PTXdoStoredProcedureNames.usp_getEditInvoicePayments, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetEditInvoicePayments-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetEditInvoicePayments-API  error " + ex);
                throw ex;
            }
        }

        public List<PTXboEditInvoiceAccountDetails> GetEditInvoiceAccountDetails(int invoiceID)
        {
            try
            {
                Logger.For(this).Invoice("GetEditInvoiceAccountDetails-API  reached " + ((object)invoiceID).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceID", invoiceID);

                var result = _dapperConnection.Select<PTXboEditInvoiceAccountDetails>(PTXdoStoredProcedureNames.usp_getEditInvoiceAccountDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetEditInvoiceAccountDetails-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetEditInvoiceAccountDetails-API  error " + ex);
                throw ex;
            }
        }

        public PTXboEditInvoiceDetails GetEditInvoiceDetailsSave(int invoiceID,int invoiceAdjustmentRequestID)
        {
            PTXboEditInvoiceDetails editInvoiceDetails = new PTXboEditInvoiceDetails();
            try
            {
                Logger.For(this).Invoice("GetEditInvoiceDetailsSave-API  reached " + ((object)"invoiceID="+invoiceID.ToString()+ "invoiceAdjustmentRequestID="+invoiceAdjustmentRequestID.ToString()).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceID", invoiceID);
                parameters.Add("@InvoiceAdjustmentRequestID", invoiceAdjustmentRequestID);

                var result = _dapperConnection.SelectMultiple(PTXdoStoredProcedureNames.usp_getEditInvoiceAdjustmentDetails, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                              gr => gr.Read<PTXboInvoice>(),
                              gr => gr.Read<PTXboInvoiceAccount>());
                editInvoiceDetails.Invoice = result.Item1.Count() > 0 ? result.Item1.FirstOrDefault() : new PTXboInvoice();
                editInvoiceDetails.InvoiceLineItem = result.Item2.Count() > 0 ? (List<PTXboInvoiceAccount>)result.Item2 : new List<PTXboInvoiceAccount>();
                Logger.For(this).Invoice("GetEditInvoiceDetailsSave-API  ends successfully ");
                return editInvoiceDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetEditInvoiceDetailsSave-API  error " + ex);
                throw ex;
            }

        }



    }
}
