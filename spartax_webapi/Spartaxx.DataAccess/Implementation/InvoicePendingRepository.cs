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
    public class InvoicePendingRepository:IInvoicePendingRepository
    {
        private readonly DapperConnection _dapperConnection;
        private readonly InvoiceCalculationRepository _invoiceCalculation;
        public InvoicePendingRepository(DapperConnection dapperConnection)
        {
            _dapperConnection = dapperConnection;
            _invoiceCalculation = new InvoiceCalculationRepository(dapperConnection);
        }


        public List<PTXboPendingInvoices> GetPendingInvoiceDetails(PTXboPendingInvoiceSearchCriteria searchCriteria)
        {
            try
            {
                Logger.For(this).Invoice("GetPendingInvoiceDetails-API  reached " + ((object)searchCriteria).ToJson(false));
                Hashtable parameters = new Hashtable();
                parameters.Add("@InvoiceTypeId", string.IsNullOrEmpty(searchCriteria.InvoiceType) ? null : searchCriteria.InvoiceType);
                parameters.Add("@InvoiceYear", string.IsNullOrEmpty(searchCriteria.InvoiceYear) ? null : searchCriteria.InvoiceYear);
                parameters.Add("@AccountNumber", string.IsNullOrEmpty(searchCriteria.AccountNumber) ? null : searchCriteria.AccountNumber);
                parameters.Add("@ClientNumber", string.IsNullOrEmpty(searchCriteria.ClientNumber) ? null : searchCriteria.ClientNumber);
                parameters.Add("@ClientName", string.IsNullOrEmpty(searchCriteria.ClientName) ? null : searchCriteria.ClientName);
                parameters.Add("@ProjectName", string.IsNullOrEmpty(searchCriteria.ProjectName) ? null : searchCriteria.ProjectName);
                
                var objClientDetails = _dapperConnection.Select<PTXboPendingInvoices>(StoredProcedureNames.usp_get_PendingGroupInvoices, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetPendingInvoiceDetails-API  ends successfully ");
                return objClientDetails;
            }
            catch(Exception ex)
            {
                Logger.For(this).Invoice("GetPendingInvoiceDetails-API  error " + ex);
                throw ex;
            }
        }

        public List<PTXboPendingInvoices> GetInvoicePendingAccountDetails(PTXboPendingInvoiceInput pendingInvoiceInput)
        {
            try
            {
                Logger.For(this).Invoice("GetInvoicePendingAccountDetails-API  reached " + ((object)pendingInvoiceInput).ToJson(false));
                if (!string.IsNullOrEmpty(pendingInvoiceInput.InvoiceGroupingLevel))
                {
                    pendingInvoiceInput.ProjectId = pendingInvoiceInput.InvoiceGroupingLevel.Trim().Contains("Term") ? 0 : pendingInvoiceInput.ProjectId;
                }

                Hashtable parameters = new Hashtable();
                parameters.Add( "@ClientNumber", pendingInvoiceInput.ClientNumber);
                parameters.Add( "@Taxyear", pendingInvoiceInput.TaxYear);
                parameters.Add( "@GroupId", pendingInvoiceInput.GroupId);
                parameters.Add( "@invoiceTypeID", Convert.ToInt32(pendingInvoiceInput.InvoiceType));
                parameters.Add("@ProjectID", Convert.ToInt32(pendingInvoiceInput.ProjectId));
                
                var objClientDetails = _dapperConnection.Select<PTXboPendingInvoices>(StoredProcedureNames.usp_get_PendingGroupInvoicesSorted, parameters, Common.Enumerator.Enum_CommandType.StoredProcedure, Common.Enumerator.Enum_ConnectionString.Spartaxx).ToList();
                Logger.For(this).Invoice("GetInvoicePendingAccountDetails-API  ends successfully ");
                return objClientDetails;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetInvoicePendingAccountDetails-API  error " + ex);
                throw ex;
            }
        }
        public PTXboHearingResult GetHearingResult(int hearingResultID)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("GetHearingResult-API  reached " + ((object)hearingResultID).ToJson(false));
                //parameters.Add("@HearingResultID", hearingResultID);

                //var result = _dapperConnection.Select<PTXboHearingResult>(StoredProcedureNames.usp_get_HearingResult, parameters, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx).FirstOrDefault();
                var result = _invoiceCalculation.GetHearingResult(hearingResultID);
                Logger.For(this).Invoice("GetHearingResult-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetHearingResult-API  error " + ex);
                throw ex;
            }
        }


        public bool InsertInvoiceDataAccountLevel(PTXboInvoice objInvoiceFromHearingResult)
        {
            Hashtable parameters = new Hashtable();

            try
            {
                Logger.For(this).Invoice("GetHearingResult-API  reached " + ((object)objInvoiceFromHearingResult).ToJson(false));

                var result = _invoiceCalculation.InsertInvoiceDataAccountLevel(objInvoiceFromHearingResult);
                Logger.For(this).Invoice("GetHearingResult-API  ends successfully ");
                return result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Invoice("GetHearingResult-API  error " + ex);
                throw ex;
            }
        }

    }
}
