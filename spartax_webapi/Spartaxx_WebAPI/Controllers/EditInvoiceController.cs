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
    public class EditInvoiceController : ApiController
    {
        private readonly IEditInvoiceService _editInvoiceService;
        public EditInvoiceController(IEditInvoiceService editInvoiceService)
        {
            _editInvoiceService = editInvoiceService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetEditInvoiceDetails([FromBody]PTXboEditInvoiceSearchCriteria SearchCriteria)
        {
            try
            {
                var result = await Task.FromResult(_editInvoiceService.GetEditInvoiceDetails(SearchCriteria));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetEditInvoiceClientDetails([FromBody]int invoiceID)
        {
            try
            {
                var result = await Task.FromResult(_editInvoiceService.GetEditInvoiceClientDetails(invoiceID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetEditInvoicePayments([FromBody] PTXboInvoiceReportDetailsInput invoiceReportDetailsInput)
        {
            try
            {
                var result = await Task.FromResult(_editInvoiceService.GetEditInvoicePayments(invoiceReportDetailsInput.InvoiceId, invoiceReportDetailsInput.InvoiceTypeId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetEditInvoiceAccountDetails([FromBody]int invoiceID)
        {
            try
            {
                var result = await Task.FromResult(_editInvoiceService.GetEditInvoiceAccountDetails(invoiceID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetEditInvoiceDetailsSave([FromBody]PTXboEditInvoiceDetailsInput invoiceDetailsInput)
        {
            try
            {
                var result = await Task.FromResult(_editInvoiceService.GetEditInvoiceDetailsSave(invoiceDetailsInput.InvoiceId, invoiceDetailsInput.InvoiceAdjustmentRequestID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}
