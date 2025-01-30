using Spartaxx.BusinessObjects;
using Spartaxx.BusinessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Spartaxx_WebAPI.Controllers
{
    public class ClientIndexController : ApiController
    {
        // GET: ClientIndex
        private readonly IClientIndexService _IClientIndexService;

        public ClientIndexController(IClientIndexService IClientIndexService)
        {
            _IClientIndexService = IClientIndexService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getListDefinitions([FromBody]PXTboListDefinition objListDefinition)
        {
            try
            {
                var result = _IClientIndexService.getListDefinitions(objListDefinition);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getProjectDetails([FromBody]int ClientId)
        {
            try
            {
                var result = _IClientIndexService.getProjectDetails(ClientId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getClientDeliveryMethods([FromBody]int ClientId)
        {
            try
            {
                var result = _IClientIndexService.getClientDeliveryMethods(ClientId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getAccountDetailsCount([FromBody]string ClientNumber)
        {
            try
            {
                var result = _IClientIndexService.getAccountDetailsCount(ClientNumber);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getAccountDetailsCount_Dashboard([FromBody]int ClientID)
        {
            try
            {
                var result = _IClientIndexService.getAccountDetailsCount_Dashboard(ClientID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getAccountsForBinding([FromBody]PTXboGetAccountsForBinding model)
        {
            try
            {
                var result = _IClientIndexService.getAccountsForBinding(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getMSPropertyCondition([FromBody]PTXboPropertyConditionModel objmodel)
        {
            try
            {
                var result = _IClientIndexService.getMSPropertyCondition(objmodel);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        //[HttpPost]
        //public async Task<HttpResponseMessage> getClientAndAccountDetails([FromBody]PTXboClientAndAccountDetails model)
        //{
        //    try
        //    {
        //        var result = _IClientIndexService.getClientAndAccountDetails(model);
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
        //        //throw ex;
        //    }
        //}
        [HttpPost]
        public async Task<HttpResponseMessage> getParamValue([FromBody]int paramID)
        {
            try
            {
                var result = _IClientIndexService.getParamValue(paramID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getClientNumber([FromBody]string ClientNumber)
        {
            try
            {
                var result = _IClientIndexService.getClientNumber(ClientNumber);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getResultState([FromBody]int ClientID)
        {
            try
            {
                var result = _IClientIndexService.getResultState(ClientID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getClientGroupContact([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.getClientGroupContact(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getMainScreenFieldName([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.getMainScreenFieldName(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> LoadDropDownForTermsFilter([FromBody]int clienID)
        {
            try
            {
                var result = _IClientIndexService.LoadDropDownForTermsFilter(clienID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getGiftCardSentDateHistory([FromBody]int ClientId)
        {
            try
            {
                var result = _IClientIndexService.getGiftCardSentDateHistory(ClientId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getClientTerms([FromBody]PTXboClientTerms model)
        {
            try
            {
                var result = _IClientIndexService.getClientTerms(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getTermLevelHistory([FromBody]int GroupId)
        {
            try
            {
                var result = _IClientIndexService.getTermLevelHistory(GroupId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getClientGroupRecentPackageDetails([FromBody]PTXboParameters model)
        {
            try
            {
                int clientId = 0;  int stateID = 0; string County = "";
                if (model.Value1 != null)
                    clientId = Convert.ToInt32(model.Value1);
                if (model.Value2 != null)
                    stateID = Convert.ToInt32(model.Value2);
                if (model.County != null)
                    County = model.County;

                var result = _IClientIndexService.getClientGroupRecentPackageDetails(clientId, stateID, County);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetClientPackageAccountDetails([FromBody]PTXboParameters model)
        {
            try
            {
                int clientId = 0; int packageTypeId = 0;int stateID = 0;string County = "";
                string PackageTypeMode = "1";
                if (model.Value1 != null)
                    clientId = Convert.ToInt32(model.Value1);
                if (model.Value3 != null)
                    stateID = Convert.ToInt32(model.Value3);
                if (model.Value2 != null)
                    packageTypeId = Convert.ToInt32(model.Value2);
                if (model.Value5 != null)
                    PackageTypeMode = model.Value5;
                if (model.County != null)
                    County = model.County;

                var result = _IClientIndexService.GetClientPackageAccountDetails(clientId, stateID, County, packageTypeId, PackageTypeMode);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getClientContactLog([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.getClientContactLog(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getClientAndDeliveryMethod([FromBody]string ClientNumber)
        {
            try
            {
                var result = _IClientIndexService.getClientAndDeliveryMethod(ClientNumber);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetOldCorrLog([FromBody]string ClientNumber)
        {
            try
            {
                var result = _IClientIndexService.GetOldCorrLog(ClientNumber);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getClientConsolidationClientDetails([FromBody]string ClientNumber)
        {
            try
            {
                var result = _IClientIndexService.getClientConsolidationClientDetails(ClientNumber);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getCAFHistory([FromBody]int ClientId)
        {
            try
            {
                var result = _IClientIndexService.getCAFHistory(ClientId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getTaxBillAuditClients([FromBody]PTXboTaxBillClientSearchRequest objPTXboTaxBillClientSearch)
        {
            try
            {
                var result = _IClientIndexService.getTaxBillAuditClients(objPTXboTaxBillClientSearch);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getPTAXCAFHistory([FromBody]string ClientNumber)
        {
            try
            {
                var result = _IClientIndexService.getPTAXCAFHistory(ClientNumber);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetAccountNotes([FromBody]int accountId)
        {
            try
            {
                var result = _IClientIndexService.GetAccountNotes(accountId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetHearingRemarks([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetHearingRemarks(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getAofARemarks([FromBody]int accountId)
        {
            try
            {
                var result = _IClientIndexService.getAofARemarks(accountId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetAgentRemarks([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetAgentRemarks(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetPropertyRemarks([FromBody]int accountId)
        {
            try
            {
                var result = _IClientIndexService.GetPropertyRemarks(accountId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getInvoicecollectionDetails([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.getInvoicecollectionDetails(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceAdjRequestDetails([FromBody]int Invoiceid)
        {
            try
            {
                var result = _IClientIndexService.GetInvoiceAdjRequestDetails(Invoiceid);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetCCPaymentDetails([FromBody]int ClientID)
        {
            try
            {
                var result = _IClientIndexService.GetCCPaymentDetails(ClientID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceAdjustmentAuditDetails([FromBody]int Invoiceid)
        {
            try
            {
                var result = _IClientIndexService.GetInvoiceAdjustmentAuditDetails(Invoiceid);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceExemptionJurisdictionHistory([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetInvoiceExemptionJurisdictionHistory(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getClientPaymentPlan([FromBody]string invoiceIdList)
        {
            try
            {
                var result = _IClientIndexService.getClientPaymentPlan(invoiceIdList);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getClientSettlementPlan([FromBody]string invoiceIdList)
        {
            try
            {
                var result = _IClientIndexService.getClientSettlementPlan(invoiceIdList);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getClientPaymentPlanHistory([FromBody]int invoiceId)
        {
            try
            {
                var result = _IClientIndexService.getClientPaymentPlanHistory(invoiceId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getClientSettlementPlanHistory([FromBody]int invoiceId)
        {
            try
            {
                var result = _IClientIndexService.getClientSettlementPlanHistory(invoiceId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getFinancialDetails([FromBody]int Accountid)
        {
            try
            {
                var result = _IClientIndexService.getFinancialDetails(Accountid);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetDNDFlagStatus([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetDNDFlagStatus(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetAccountDetailFlags([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetAccountDetailFlags(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getAccountSetupParameterDetails([FromBody]int AccountID)
        {
            try
            {
                var result = _IClientIndexService.getAccountSetupParameterDetails(AccountID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetMainscreenGotoDDLValues([FromBody]PTXboMainScreenFieldNames requestInput)
        {
            string errorMessage = string.Empty;
            try
            {
                List<PTXboMainScreenFieldNames> getGotoDDLVal = new List<PTXboMainScreenFieldNames>();
                var result = _IClientIndexService.GetMainscreenGotoDDLValues(requestInput, out getGotoDDLVal, out errorMessage);
                return Request.CreateResponse(HttpStatusCode.OK, getGotoDDLVal);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getPropertyDetailsData([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.getPropertyDetailsData(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getValueNoticeData([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.getValueNoticeData(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getHearingDetailsData([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.getHearingDetailsData(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getHearingResultData([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.getHearingResultData(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getPriorYearTaxRateForSelectedAccount([FromBody]PTXboParameters modelrate)
        {
            try
            {
                var result = _IClientIndexService.getPriorYearTaxRateForSelectedAccount(modelrate);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getAccountJurisdiction([FromBody]PTXboParameters modelrate)
        {
            try
            {
                var result = _IClientIndexService.getAccountJurisdiction(modelrate);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getAccountTaxRollJurisdiction([FromBody]PTXboParameters modelrate)
        {
            try
            {
                var result = _IClientIndexService.getAccountTaxRollJurisdiction(modelrate);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getCountyJurisdiction([FromBody]PTXboParameters modelrate)
        {
            try
            {
                var result = _IClientIndexService.getCountyJurisdiction(modelrate);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getFCIPropertyDetails([FromBody]PTXboFCIPropertyDetails propertyDetails)
        {
            try
            {
                var result = _IClientIndexService.getFCIPropertyDetails(propertyDetails);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetFCIDataRemarksDetails([FromBody]int PropertyDetailsId)
        {
            try
            {
                var result = _IClientIndexService.GetFCIDataRemarksDetails(PropertyDetailsId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getmenuIsActivebasedOnUserRole([FromBody]PTXbomenuIsActivebasedOnUserRole model)
        {
            try
            {
                var result = _IClientIndexService.getmenuIsActivebasedOnUserRole(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getACCLlogDetails([FromBody]PTXboACCLlogSearchRequest objACCLlogSearchRequest)
        {
            try
            {
                var result = _IClientIndexService.getACCLlogDetails(objACCLlogSearchRequest);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getACCLlogDetailsNew([FromBody]PTXboACCLlogSearchRequest objACCLlogSearchRequest)
        {
            try
            {
                var result = _IClientIndexService.getACCLlogDetailsNew(objACCLlogSearchRequest);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> MainScreenupdateClientCodes([FromBody]PTXboInsertUpdateClientCode model)
        {
            try
            {
                var result = _IClientIndexService.MainScreenupdateClientCodes(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> generateTemporaryClientNumber([FromBody]string TaxYear)
        {
            try
            {
                var result = _IClientIndexService.generateTemporaryClientNumber(TaxYear);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> CheckClientDetailsBeforeGeneration([FromBody]int ClientId)
        {
            try
            {
                var result = _IClientIndexService.CheckClientDetailsBeforeGeneration(ClientId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> updateClientDeliveryMethods([FromBody]PTXboClientDeliveryMethods objClientDeliveryMethods)
        {
            try
            {
                var result = _IClientIndexService.updateClientDeliveryMethods(objClientDeliveryMethods);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getCAFRequestonExpiry([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.getCAFRequestonExpiry(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> updateGroupDetails([FromBody]PTXboGroup objGroupDetails)
        {
            try
            {
                var result = _IClientIndexService.updateGroupDetails(objGroupDetails);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getTermAccountLevelHistory([FromBody]int GroupId)
        {
            try
            {
                var result = _IClientIndexService.getTermAccountLevelHistory(GroupId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getTermsDetails([FromBody]int GroupId)
        {
            try
            {
                var result = _IClientIndexService.getTermsDetails(GroupId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getAssandUnAssAccountscount([FromBody]PTXboSearchAccountCriteria objSearchAccountCriteria)
        {
            try
            {
                var result = _IClientIndexService.getAssandUnAssAccountscount(objSearchAccountCriteria);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getAgreementsHistory([FromBody]int GroupID)
        {
            try
            {
                var result = _IClientIndexService.getAgreementsHistory(GroupID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getTermsAccounts([FromBody]PTXboSearchAccountCriteria objSearchAccountCriteria)
        {
            try
            {
                var result = _IClientIndexService.getTermsAccounts(objSearchAccountCriteria);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getAccountTermHistory([FromBody]int AccountId)
        {
            try
            {
                var result = _IClientIndexService.getAccountTermHistory(AccountId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getAccountCurrentTermDetails([FromBody]int AccountId)
        {
            try
            {
                var result = _IClientIndexService.getAccountCurrentTermDetails(AccountId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getAgreementDetails([FromBody]int GroupID)
        {
            try
            {
                var result = _IClientIndexService.getAgreementDetails(GroupID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getCafAccountCountbasedonStatus([FromBody]int clientID)
        {
            try
            {
                var result = _IClientIndexService.getCafAccountCountbasedonStatus(clientID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetHearingDetails([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetHearingDetails(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> loadInformalInformalFlag([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.loadInformalInformalFlag(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetHearingDateHistory([FromBody]int HearingDetailsId)
        {
            try
            {
                var result = _IClientIndexService.GetHearingDateHistory(HearingDetailsId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetCorrectionMotionFiledDetails([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetCorrectionMotionFiledDetails(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetPanelDocketIDHistory([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetPanelDocketIDHistory(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetHearingNoticeLetterDateHistory([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetHearingNoticeLetterDateHistory(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getAofADetails([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.getAofADetails(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getHearingInfo([FromBody]int Accountid)
        {
            try
            {
                var result = _IClientIndexService.getHearingInfo(Accountid);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getPastHearingInfo([FromBody]int Accountid)
        {
            try
            {
                var result = _IClientIndexService.getPastHearingInfo(Accountid);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getAofAPreviousRevisedDateHistory([FromBody]int Accountid)
        {
            try
            {
                var result = _IClientIndexService.getAofAPreviousRevisedDateHistory(Accountid);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getDateCodedEndHistory([FromBody]int Accountid)
        {
            try
            {
                var result = _IClientIndexService.getDateCodedEndHistory(Accountid);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getCADLegalNameHistory([FromBody]int Accountid)
        {
            try
            {
                var result = _IClientIndexService.getCADLegalNameHistory(Accountid);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getClientContactLogLitigation([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.getClientContactLogLitigation(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getDateCodedHistory([FromBody]int AccountId)
        {
            try
            {
                var result = _IClientIndexService.getDateCodedHistory(AccountId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetTaxyearBasedArbitration([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetTaxyearBasedArbitration(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getArbitratorDetails([FromBody]int Arbitrator)
        {
            try
            {
                var result = _IClientIndexService.getArbitratorDetails(Arbitrator);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getDocumentsfromPaperwiseUsingMultiDoctype([FromBody]PTXboDocumentsFromPaperwise objSearchCriteria)
        {
            try
            {
                var result = _IClientIndexService.getDocumentsfromPaperwiseUsingMultiDoctype(objSearchCriteria);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> LoadLitigationDetails([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.LoadLitigationDetails(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetPendingLitigationTaxYear([FromBody]int AccountID)
        {
            try
            {
                var result = _IClientIndexService.GetPendingLitigationTaxYear(AccountID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> ViewDocuments([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.ViewDocuments(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> LoadSupplementHistory([FromBody]int LitigationID)
        {
            try
            {
                var result = _IClientIndexService.LoadSupplementHistory(LitigationID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetTrialDateHistory([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetTrialDateHistory(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> LoadLitigationDetailsII([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.LoadLitigationDetailsII(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetPendingLitigationTaxYearII([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetPendingLitigationTaxYearII(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> SecGetTrialDateHistory([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.SecGetTrialDateHistory(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getMainScreenValues([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.getMainScreenValues(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getMSPropertySurvey([FromBody]PTXboPropertyConditionModel objmodel)
        {
            try
            {
                var result = _IClientIndexService.getMSPropertySurvey(objmodel);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetClientCreditDetails([FromBody]int clientID)
        {
            try
            {
                var result = _IClientIndexService.GetClientCreditDetails(clientID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getMainScreenBPPPropertyValues([FromBody]PTXboBPPPropertyValues objmodel)
        {
            try
            {
                var result = _IClientIndexService.getMainScreenBPPPropertyValues(objmodel);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetILHearingRemarks([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetILHearingRemarks(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        } 
        [HttpPost]
        public async Task<HttpResponseMessage> GetILAgentRemarks([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetILAgentRemarks(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetILHearingDetails([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetILHearingDetails(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetDMflag([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetDMflag(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getPOAReceivedDateHistory([FromBody]int ClientId)
        {
            try
            {
                var result = _IClientIndexService.getPOAReceivedDateHistory(ClientId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetIOWAHearingRemarks([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetIOWAHearingRemarks(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetIOWAAgentRemarks([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetIOWAAgentRemarks(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetIOWAHearingDetails([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetIOWAHearingDetails(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> GetGAHearingDetails([FromBody]PTXboParameters model)
        {
            try
            {
                var result = _IClientIndexService.GetGAHearingDetails(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
    }
}