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
    /// <summary>
    /// added by saravanans-tfs:47247
    /// </summary>
    public class InvoiceFlatFeePreController : ApiController
    {
        private readonly IInvoiceFlatFeePreService _invoiceFlatFeeService;
        public InvoiceFlatFeePreController(IInvoiceFlatFeePreService invoiceFlatFeeService)
        {
            _invoiceFlatFeeService = invoiceFlatFeeService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceFlatFeePreAccountDetail([FromBody]PTXboInvoiceFlatFeeInput invoiceFlatFeeInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceFlatFeeService.GetInvoiceFlatFeePreAccountDetail(invoiceFlatFeeInput));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> GetInvoiceFlatFeePreClientDetail([FromBody]PTXboFlatFeeClientDetail flatFeeClientDetail)
        {
            try
            {
                var result = await Task.FromResult(_invoiceFlatFeeService.GetInvoiceFlatFeePreClientDetail(flatFeeClientDetail));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        
       [HttpPost]
        public async Task<HttpResponseMessage> FlatFeeInvoiceGeneration([FromBody]PTXboFlatFeeInvoiceGenerationInput flatFeeInvoice)
        {
            try
            {
                string errorMessage = string.Empty;
                var result = await Task.FromResult(_invoiceFlatFeeService.FlatFeeInvoiceGeneration(flatFeeInvoice, out errorMessage));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}
