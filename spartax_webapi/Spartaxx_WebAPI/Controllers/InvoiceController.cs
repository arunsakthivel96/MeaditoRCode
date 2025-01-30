using Newtonsoft.Json;
using Spartaxx.BusinessObjects;
using Spartaxx.BusinessObjects.Invoice;
using Spartaxx.BusinessService;
using Spartaxx.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Spartaxx_WebAPI.Controllers
{
    public class InvoiceController : ApiController
    {
        private readonly IInvoiceService _invoiceService;
        public InvoiceController(IInvoiceService IInvoiceService)
        {
            _invoiceService = IInvoiceService;
        }

        /// <summary>
        /// Retrieving the basic details for generate invoice
        /// </summary>
        /// <param name="objSearchCriteria"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceGenerationDetails([FromBody]PTXboInvoiceSearchCriteria objSearchCriteria)
        {
            //List<PTXboInvoiceDetails> lstInvoiceDetails = null;
            try
            {
                var result =await Task.FromResult( _invoiceService.GetInvoiceGenerationDetails(objSearchCriteria));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        /// <summary>
        /// Get specific invoice details
        /// </summary>
        /// <param name="invoiceID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GetSpecificInvoiceGenerationDetails([FromBody]int invoiceID)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetSpecificInvoiceGenerationDetails(invoiceID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        /// <summary>
        /// Get common drop down details such as state,county and so on.
        /// </summary>
        /// <param name="objListDefinition"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GetCommonListDefinitions([FromBody] PXTboListDefinition objListDefinition)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetCommonListDefinitions(ref objListDefinition));
                return Request.CreateResponse(HttpStatusCode.OK, objListDefinition);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get invoice remarks
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GetClientRemarks([FromBody] int clientID)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetClientRemarks(clientID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get service pacakage Id 
        /// </summary>
        [HttpPost]
        public async Task<HttpResponseMessage> GetServicePackageIDforInvoice([FromBody]string servicePackageName)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetServicePackageIDforInvoice(servicePackageName));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get the location of existing generated invoice file
        /// </summary>
        /// <param name="invoiceID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GetLatestInvoiceFile([FromBody]int invoiceID)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetLatestInvoiceFile(invoiceID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get invoice adjustment details
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceAdjustmentReportDetails([FromBody] int invoiceId)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetInvoiceAdjustmentReportDetails(invoiceId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

        /// <summary>
        /// Get invoice auto adjustment details
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GetAutoAdjustmentInvoiceDetails([FromBody] int invoiceId)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetAutoAdjustmentInvoiceDetails(invoiceId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

        /// <summary>
        /// Submit the location details of newly generated invoice.
        /// </summary>
        /// <param name="invoiceFileBasics"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> SubmitInvoiceFiles([FromBody] PTXboInvoiceFileBasics invoiceFileBasics)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.SubmitInvoiceFiles(invoiceFileBasics.InvoiceID, invoiceFileBasics.CorrQID, invoiceFileBasics.FileLocation, invoiceFileBasics.UserID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Update the invoice location .
        /// </summary>
        /// <param name="invoiceFileBasics"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateInvoiceFiles([FromBody] PTXboInvoiceFileBasics invoiceFileBasics)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.UpdateInvoiceFiles(invoiceFileBasics.InvoiceID, invoiceFileBasics.CorrQID, invoiceFileBasics.FileLocation, invoiceFileBasics.UserID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetParamValue([FromBody]int paramID)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetParamValue(paramID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetCADNavigatorDetails([FromBody]PTXboCADNavigatorInput objCADNavigatorInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetCADNavigatorDetails(objCADNavigatorInput.AccountNumber, objCADNavigatorInput.CountyName));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        
         [HttpPost]
        public async Task<HttpResponseMessage> GetClientDefaultDeliveryMethod([FromBody]int clientId)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetClientDefaultDeliveryMethod(clientId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceGenerationInputData([FromBody]PTXInvoiceGenerationInput invoiceGenerationInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetInvoiceGenerationInputData(invoiceGenerationInput.ClientId, invoiceGenerationInput.Taxyear, invoiceGenerationInput.InvoiceTypeId, invoiceGenerationInput.IsOutOfTexas, invoiceGenerationInput.IsDisasterInvoice));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetCreditInvoiceGenerationInputData([FromBody]PTXInvoiceGenerationInput invoiceGenerationInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetCreditInvoiceGenerationInputData(invoiceGenerationInput.ClientId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> UpdateClientCorrQueueRecordInvoice([FromBody]PTXboClientCorrqueue objCorrQueueList)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.UpdateClientCorrQueueRecordInvoice(objCorrQueueList.CorrQueueList, objCorrQueueList.GroupID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> InsertUpdateCorrQCreditInvoice([FromBody]PTXboClientCorrqueue objCorrQueueList)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.InsertUpdateCorrQCreditInvoice(objCorrQueueList.CorrQueueList));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceReportDetails([FromBody]PTXboInvoiceReportDetailsInput invoiceReportDetailsInput)
        {
            try
            {
                //var result = await Task.FromResult(_invoiceService.GetInvoiceReportDetails(invoiceReportDetailsInput.InvoiceId, invoiceReportDetailsInput.InvoiceTypeId, invoiceReportDetailsInput.IsInvoiceDefect, invoiceReportDetailsInput.IsOutOfTexas, invoiceReportDetailsInput.IsOTEntryscreen));
                var result = await Task.FromResult(_invoiceService.GetInvoiceReportDetails(invoiceReportDetailsInput));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AccountLevelInvoiceGeneration([FromBody]PTXboAccountLevelInvoiceInput accountLevelInvoiceInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.AccountLevelInvoiceGeneration(accountLevelInvoiceInput.InvoiceDetails, accountLevelInvoiceInput.AccountDetails, accountLevelInvoiceInput.UpdatedBy));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> ProjectorTermInvoiceGeneration([FromBody]PTXboAccountLevelInvoiceInput accountLevelInvoiceInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.ProjectorTermInvoiceGeneration(accountLevelInvoiceInput.InvoiceDetails, accountLevelInvoiceInput.AccountDetails, accountLevelInvoiceInput.UpdatedBy));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateInvoiceProcessingStatus([FromBody]PTXboInvoiceProcessingStatusInput invoiceProcessingStatusInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.UpdateInvoiceProcessingStatus(invoiceProcessingStatusInput.InvoicingProcessingStatusId,invoiceProcessingStatusInput.InvoiceDetails));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> SubmitInvoiceSummary([FromBody]PTXboInvoiceSummary invoiceSummary)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.SubmitInvoiceSummary(invoiceSummary));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceDetailsById([FromBody]int invoiceId)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetInvoiceDetailsById(invoiceId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceLineItemByInvoiceId([FromBody]int invoiceId)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetInvoiceLineItemByInvoiceId(invoiceId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateClientCorrQueueRecord([FromBody]PTXboCorrQRecordUpdateInput corrQRecordUpdateInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.UpdateClientCorrQueueRecord(corrQRecordUpdateInput.CorrQueueList, corrQRecordUpdateInput.Invoice));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> InsertAccountLevelInvoiceGeneration([FromBody] PTXboAccountLevelInvoiceGenerationInput accountLevelInvoiceInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.InsertAccountLevelInvoiceGeneration(accountLevelInvoiceInput.Invoice, accountLevelInvoiceInput.InvoiceDetails, accountLevelInvoiceInput.HearingType, accountLevelInvoiceInput.InvoicingGroupType, accountLevelInvoiceInput.CreatedBy));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateLetterProcessStatus([FromBody] PTXboUpdateLetterProcessStatusInput letterProcessStatus)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.UpdateLetterProcessStatus(letterProcessStatus));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> GetActiveCorrQueueRecordsForSelectedInvoices([FromBody] PTXboActiveCorrqRecordsInput activeCorrqRecords)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetActiveCorrQueueRecordsForSelectedInvoices(activeCorrqRecords));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceIdFromInvoiceAdjustment([FromBody] int invoiceAdjustMentRequestId)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetInvoiceIdFromInvoiceAdjustment(invoiceAdjustMentRequestId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SaveOrUpdateInvoice([FromBody] PTXboInvoice invoice)
        {
            try
            {
                int invoiceID = 0;
                var result = await Task.FromResult(_invoiceService.SaveOrUpdateInvoice(invoice,out invoiceID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> GetClientEmailInvoice([FromBody]PTXboGetClientEmailInput objGetClientEmail)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetClientEmailInvoice(objGetClientEmail));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceBasicDetails([FromBody]int invoiceID)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetInvoiceBasicDetails(invoiceID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceTermsType([FromBody]PTXboInvoice objInvoice)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetInvoiceTermsType(objInvoice));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> GenerateInvoicereports([FromBody]PTXboInvoiceReportInput objInvoiceReportInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GenerateInvoicereports(objInvoiceReportInput));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Created by SaravananS. tfs id:60248
        /// </summary>
        /// <param name="objInvoiceReportInput"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GenerateInvoiceUsmailCoverLetter([FromBody]PTXboInvoiceReportInput objInvoiceReportInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GenerateInvoiceUsmailCoverLetter(objInvoiceReportInput));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateInvoiceDetails([FromBody]PTXboInvoice objInvoice)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.UpdateInvoiceDetails(objInvoice));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetFailedCorrQueueRecords()
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetFailedCorrQueueRecords());
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CheckInvoicefilepath([FromBody] string fileLocation)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.CheckInvoicefilepath(fileLocation));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SubmitOutOfTexasInvoice([FromBody]PTXboOutOfTexasInvoiceDetails outOfTexasInvoice)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.SubmitOutOfTexasInvoice(outOfTexasInvoice));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> SubmitOutOfTexasProjectLevelInvoice([FromBody]PTXboOutOfTexasInvoiceDetails outOfTexasInvoice)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.SubmitOutOfTexasProjectLevelInvoice(outOfTexasInvoice));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Created by SaravananS. tfs id:61434
        /// </summary>
        /// <param name="servicePakageId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GetClientCommunicationEmailDetails([FromBody]int servicePakageId)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetClientCommunicationEmailDetails(servicePakageId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Created by SaravananS. tfs id:61434
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceRegenerationDetails([FromBody]int invoiceId)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetInvoiceRegenerationDetails(invoiceId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        /// <summary>
        /// Created by SaravananS. tfs id:61434
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> IsOutOfTexas([FromBody]int invoiceId)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.IsOutOfTexas(invoiceId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Created by SaravananS. tfs id:61434
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> IsMultiyear([FromBody]int invoiceId)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.IsMultiyear(invoiceId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// //Added by SaravananS. tfs id:62016
        /// </summary>
        /// <param name="corrqRecordsInput"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GenerateLetterForSelectedInvoices([FromBody]PTXboActiveCorrqRecordsInput corrqRecordsInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GenerateLetterForSelectedInvoices(corrqRecordsInput));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        /// <summary>
        /// //Added by SaravananS. tfs id:63159
        /// </summary>
        /// <param name="objGetClientEmail"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GetClientDeliveryAddressForInvoice([FromBody]PTXboGetClientEmailInput objGetClientEmail)
        {
            try
            {
                var result = await Task.FromResult(_invoiceService.GetClientDeliveryAddressForInvoice(objGetClientEmail));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }



    }
}
