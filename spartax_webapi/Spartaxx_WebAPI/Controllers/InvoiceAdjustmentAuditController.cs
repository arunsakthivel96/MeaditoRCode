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
    public class InvoiceAdjustmentAuditController : ApiController
    {
        private readonly IInvoiceAdjustmentAuditService _invoiceadjustmentAuditService;

       public InvoiceAdjustmentAuditController(IInvoiceAdjustmentAuditService InvoiceAdjustmentService)
        {
            _invoiceadjustmentAuditService = InvoiceAdjustmentService;
        }


        [HttpPost]
        public async Task<HttpResponseMessage> GetAllottedInvoiceAdjustmentRequest([FromBody]PTXboInvoiceAdjustmentAllotedSearch objSearch)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.GetAllottedInvoiceAdjustmentRequest(objSearch));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceNextAdjustmentAllottedDocument([FromBody]PTXboInvoiceAdjustmentSearch objSearch)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.GetInvoiceNextAdjustmentAllottedDocument(objSearch.UserID, objSearch.UserRoleID, objSearch.QueID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetClientAndInvoiceDetails([FromBody]int invoiceID)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.GetClientAndInvoiceDetails(invoiceID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceAdjustmentRequestDetails([FromBody]PTXboInvoiceAdjustmentRequestInput invoiceAdjustmentRequest)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.GetInvoiceAdjustmentRequestDetails(invoiceAdjustmentRequest.InvoiceAdjusmentRequestID, invoiceAdjustmentRequest.RequestType));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> GetPendingAdjustmentRequestDetails([FromBody]int invoiceID)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.GetPendingAdjustmentRequestDetails(invoiceID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetAutoAdjustmentInvoiceDetails([FromBody]PTXboAutoAdjustmentInvoiceDetailsInput autoAdjustmentInvoiceDetailsInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.GetAutoAdjustmentInvoiceDetails(autoAdjustmentInvoiceDetailsInput.InvoiceID,autoAdjustmentInvoiceDetailsInput.InvoiceAdjustmentRequestID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetCSInvoiceComments([FromBody]int invoiceID)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.GetCSInvoiceComments(invoiceID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }



        [HttpPost]
        public async Task<HttpResponseMessage> GetExemptionRemovedJurisdictions([FromBody]PTXboExemptionRemovedJurisdictionsInput exemptionRemovedJurisdictionsInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.GetExemptionRemovedJurisdictions(exemptionRemovedJurisdictionsInput.InvoiceID, exemptionRemovedJurisdictionsInput.TaxYear, exemptionRemovedJurisdictionsInput.InvoiceAdjustmentRequestID, exemptionRemovedJurisdictionsInput.PropertyDetails));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceAdjustmentLineItem([FromBody] PTXboInvoiceAdjustmentLineItem invoiceAdjustmentLineItemInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.GetInvoiceAdjustmentLineItem(Convert.ToInt32(invoiceAdjustmentLineItemInput.InvoiceAdjustmentId),Convert.ToInt32(invoiceAdjustmentLineItemInput.AccountID)));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> SaveOrUpdateInvoiceAdjustmentLineItem([FromBody] PTXboInvoiceAdjustmentLineItem invoiceAdjustmentLineItem)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.SaveOrUpdateInvoiceAdjustmentLineItem(invoiceAdjustmentLineItem));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SaveOrUpdateInvoiceAdjustmentManualLineItem([FromBody] PTXboInvoiceAdjustmentManualLineItem invoiceAdjustmentManualLineItem)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.SaveOrUpdateInvoiceAdjustmentManualLineItem(invoiceAdjustmentManualLineItem));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceAdjustmentManualLineItem([FromBody] int invoiceAdjustmentManualLineItemID)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.GetInvoiceAdjustmentManualLineItem(invoiceAdjustmentManualLineItemID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceExemptionJurisdiction([FromBody] PTXboInvoiceExemptionJurisdiction invoiceExemptionJurisdiction)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.GetInvoiceExemptionJurisdiction(invoiceExemptionJurisdiction.InvoiceAdjustmentId, invoiceExemptionJurisdiction.JurisdictionID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SaveOrUpdateInvoiceExemptionJurisdiction([FromBody] PTXboInvoiceExemptionJurisdiction ObjAdjLineItemFromDB)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.SaveOrUpdateInvoiceExemptionJurisdiction(ObjAdjLineItemFromDB));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> SaveOrUpdateInvoiceExemptionJurisdictionhistory([FromBody] PTXboInvoiceExemptionJurisdictionHistory ObjAdjLineItemFromDB)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.SaveOrUpdateInvoiceExemptionJurisdictionhistory(ObjAdjLineItemFromDB));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> SaveOrUpdateInvoiceAdjustment([FromBody] PTXboInvoiceAdjustment invoiceadjustment)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.SaveOrUpdateInvoiceAdjustment(invoiceadjustment));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SaveOrUpdateInvoiceAdjustmentAttachments([FromBody] PTXboInvoiceAdjustmentAttachments objInvoiceAttachments)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.SaveOrUpdateInvoiceAdjustmentAttachments(objInvoiceAttachments));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SaveOrUpdateInvoiceAdjustmentRequest([FromBody] PTXboInvoiceAdjustmentRequest objInvoiceAdjustmentRequest)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.SaveOrUpdateInvoiceAdjustmentRequest(objInvoiceAdjustmentRequest));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> GetExemptionAccountlist([FromBody]PTXboAutoAdjustmentInvoiceDetailsInput autoAdjustmentInvoiceDetailsInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.GetExemptionAccountlist(autoAdjustmentInvoiceDetailsInput.InvoiceID, autoAdjustmentInvoiceDetailsInput.InvoiceAdjustmentRequestID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceAdjustmentHistory([FromBody]int invoiceID)
        {
            try
            {
                var result = await Task.FromResult(_invoiceadjustmentAuditService.GetInvoiceAdjustmentHistory(invoiceID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

    }
}
