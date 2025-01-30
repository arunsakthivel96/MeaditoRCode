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
    public class InvoiceDefectsController : ApiController
    {
        private readonly IInvoiceDefectsService _invoiceDefectService;
        public InvoiceDefectsController(IInvoiceDefectsService invoiceDefectsService)
        {
            _invoiceDefectService = invoiceDefectsService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateInvoiceStatusinMainTable([FromBody]PTXboUpdateInvoiceMaintableInput maintableInput)
        {
            try
            {
                string errorMessage = string.Empty;
                var result = await Task.FromResult(_invoiceDefectService.UpdateInvoiceStatusinMainTable(maintableInput));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SubmitInvoiceGenerationDefects([FromBody]List<PTXboInvoiceDetails> lstInvoiceDetails)
        {
            try
            {
                string errorMessage = string.Empty;
                var result = await Task.FromResult(_invoiceDefectService.SubmitInvoiceGenerationDefects(lstInvoiceDetails));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceGenerationDefects([FromBody]PTXboInvoiceSearchCriteria objSearchCriteria)
        {
            try
            {
                string errorMessage = string.Empty;
                var result = await Task.FromResult(_invoiceDefectService.GetInvoiceGenerationDefects(objSearchCriteria));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceDefectsAccountDetails([FromBody]int invoiceId)
        {
            try
            {
                string errorMessage = string.Empty;
                var result = await Task.FromResult(_invoiceDefectService.GetInvoiceDefectsAccountDetails(invoiceId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceDefectsAccountJurisdictionDetails([FromBody]PTXboInvoiceDefectJurisdictionInput invoiceDefectJurisdiction)
        {
            try
            {
                string errorMessage = string.Empty;
                var result = await Task.FromResult(_invoiceDefectService.GetInvoiceDefectsAccountJurisdictionDetails(invoiceDefectJurisdiction));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceDefectsTermDetails([FromBody]int invoiceId)
        {
            try
            {
                string errorMessage = string.Empty;
                var result = await Task.FromResult(_invoiceDefectService.GetInvoiceDefectsTermDetails(invoiceId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateInvoiceDefectsTermsDetails([FromBody]PTXboInvoiceDefectDetails invoiceDefectDetails)
        {
            try
            {
                string errorMessage = string.Empty;
                var result = await Task.FromResult(_invoiceDefectService.UpdateInvoiceDefectsTermsDetails(invoiceDefectDetails));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}
