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
    public class InvoicePendingController : ApiController
    {
        private readonly IInvoicePendingService _invoicePendingService;
        public InvoicePendingController(IInvoicePendingService invoicePendingService)
        {
            _invoicePendingService = invoicePendingService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetPendingInvoiceDetails([FromBody]PTXboPendingInvoiceSearchCriteria searchCriteria)
        {
            try
            {
                var result = await Task.FromResult(_invoicePendingService.GetPendingInvoiceDetails(searchCriteria));
               return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
              return  Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoicePendingAccountDetails([FromBody]PTXboPendingInvoiceInput pendingInvoiceInput)
        {
            try
            {
                var result = await Task.FromResult(_invoicePendingService.GetInvoicePendingAccountDetails(pendingInvoiceInput));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetHearingResult([FromBody]int hearingResultID)
        {
            try
            {
                var result = await Task.FromResult(_invoicePendingService.GetHearingResult(hearingResultID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> InsertInvoiceDataAccountLevel([FromBody]PTXboInvoice objInvoiceFromHearingResult)
        {
            try
            {
                var result = await Task.FromResult(_invoicePendingService.InsertInvoiceDataAccountLevel(objInvoiceFromHearingResult));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


    }
}
