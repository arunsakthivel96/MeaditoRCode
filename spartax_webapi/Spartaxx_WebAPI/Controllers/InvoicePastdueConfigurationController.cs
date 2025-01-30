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
    public class InvoicePastdueConfigurationController : ApiController
    {
        private readonly IInvoicePastdueConfigurationService _invoicePastdueConfigurationService;
        public InvoicePastdueConfigurationController(IInvoicePastdueConfigurationService invoicePastdueConfigurationService)
        {
            _invoicePastdueConfigurationService = invoicePastdueConfigurationService;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetPastdueRunningStatus()
        {
            try
            {
                var result = await Task.FromResult(_invoicePastdueConfigurationService.GetPastdueRunningStatus());
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }


        
       [HttpGet]
        public async Task<HttpResponseMessage> GetPastDueHistory()
        {
            try
            {
                var result = await Task.FromResult(_invoicePastdueConfigurationService.GetPastDueHistory());
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);

            }
        }

    }
}
