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
    public class MainScreenInvoiceController : ApiController
    {
        private readonly IMainscreenInvoiceService _mainscreenInvoice;
        public MainScreenInvoiceController(IMainscreenInvoiceService mainscreenInvoice)
        {
            _mainscreenInvoice= mainscreenInvoice;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> MainScreenInvoiceStatusCollection([FromBody]int clientID)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.MainScreenInvoiceStatusCollection(clientID));
                return Request.CreateResponse(HttpStatusCode.OK,result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceComments([FromBody]PTXboInvoiceCommentsInput invoiceComments)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.GetInvoiceComments(invoiceComments)) ;
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> SaveCollectionComments([FromBody]PTXboInvoiceRemarks objInvoiceRemarks)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.SaveCollectionComments(objInvoiceRemarks));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> SaveorUpdateInvoiceRemarks([FromBody]PTXboInvoiceRemarks objboInvoiceRemarks)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.SaveorUpdateInvoiceRemarks(objboInvoiceRemarks));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> DeleteCSInvoiceComments([FromBody]PTXboDeletecsInvoiceCommentsInput commentsInput)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.DeleteCSInvoiceComments(commentsInput));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

       
        [HttpPost]
        public async Task<HttpResponseMessage> GetCSInvoiceTrasactions([FromBody] PTXboInvoiceCommentsInput invoiceComments)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.GetCSInvoiceTrasactions(invoiceComments));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoicecollectionDetails([FromBody] PTXboInvoiceFilter gridvalues)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.GetInvoicecollectionDetails(gridvalues));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoicecounts([FromBody] int clientid)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.GetInvoicecounts(clientid));
                return Request.CreateResponse(HttpStatusCode.OK,result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetUserMessage([FromBody] string userMessageCode)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.GetUserMessage(userMessageCode));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetPastDueNotices([FromBody] int invoiceId)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.GetPastDueNotices(invoiceId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetFlagCollectionDetails([FromBody] int clientId)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.GetFlagCollectionDetails(clientId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetCCPaymentInvoiceDetails([FromBody] int ccpaymentId)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.GetCCPaymentInvoiceDetails(ccpaymentId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> SaveManualPaymentPosting([FromBody] PTXboManualPaymentPostingInput manualPaymentPosting)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.SaveManualPaymentPosting(manualPaymentPosting));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> SaveCreditManualPaymentPosting([FromBody] PTXboManualCreditPaymentPostingInput creditPaymentPosting)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.SaveCreditManualPaymentPosting(creditPaymentPosting));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> CheckLegalInvoice([FromBody]string invoiceIds)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.CheckLegalInvoice(invoiceIds));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdatePaymentOrSettlementPlan([FromBody]PTXboPaymentSettlementPlanInput paymentSettlement)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.UpdatePaymentOrSettlementPlan(paymentSettlement));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> LegalInvoiceGeneration([FromBody]PTXboLegalInvoiceGenerationInput legalInvoice)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.LegalInvoiceGeneration(legalInvoice));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateCSInvoiceTransaction([FromBody]PTXboUpdateCSInvoiceTranscationInput objUpdateCSInvoice)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.UpdateCSInvoiceTransaction(objUpdateCSInvoice));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> WaiveCompoundInterest([FromBody]string invoiceId)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.WaiveCompoundInterest(invoiceId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

        [HttpPost]
        public async Task<HttpResponseMessage> CheckCompoundInterestAvailability([FromBody] string invoiceId)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.CheckCompoundInterestAvailability(invoiceId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> WaiveSimpleInterest([FromBody] string invoiceId)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.WaiveSimpleInterest(invoiceId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateSettlementDetails([FromBody]PTXboUpdateSettlementDetailsInput objUpdateSettlement)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.UpdateSettlementDetails(objUpdateSettlement));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetCollectionDetailsSummary([FromBody]int userId)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.GetCollectionDetailsSummary(userId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetCCPaymentInvoiceListDetails([FromBody]int clientID)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.GetCCPaymentInvoiceListDetails(clientID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetCCPaymentDetailsForInvoice([FromBody]int ccpaymentID)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.GetCCPaymentDetailsForInvoice(ccpaymentID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SaveCCPaymentDetails([FromBody]PTXboSaveCCPaymentsInput objSaveCCPayments)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.SaveCCPaymentDetails(objSaveCCPayments));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> RemoveCCPaymentDetails([FromBody]PTXboCCPayment objCCPayment)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.RemoveCCPaymentDetails(objCCPayment));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> InsertInvoiceData([FromBody]PTXboInvoice objInvoiceFromHearingResult)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.InsertInvoiceData(objInvoiceFromHearingResult));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceCheckPaymentStatus([FromBody]int invoiceId)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.GetInvoiceCheckPaymentStatus(invoiceId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage>  WaiveInvoiceInterest([FromBody]List<PTXboPayment> paymentList)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.WaiveInvoiceInterest(paymentList));

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            
        }

        /// <summary>
        /// Created by Saravanans. tfs id:61797
        /// </summary>
        /// <param name="ccPaymentId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> DownloadCCPaymentReceipt([FromBody]int ccPaymentId)
        {
            try
            {
                var result = await Task.FromResult(_mainscreenInvoice.DownloadCCPaymentReceipt(ccPaymentId));

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

        }


    }
}
