using Spartaxx.BusinessObjects;
using Spartaxx.BusinessObjects.Invoice;
using Spartaxx.BusinessService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Spartaxx_WebAPI.Controllers
{
    //added by saravanans-tfs:47247
    public class InvoiceSpecialTermsController : ApiController
    {
        private readonly IInvoiceSpecialTermsService _invoiceSpecialTermsService;
        public InvoiceSpecialTermsController(IInvoiceSpecialTermsService invoiceSpecialTermsService)
        {
            _invoiceSpecialTermsService = invoiceSpecialTermsService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> LoadSpecialTermClients([FromBody]PTXboSpecialTermClientsFilter specialTermClientFilter)
        {
            try
            {
                var result = await Task.FromResult(_invoiceSpecialTermsService.LoadSpecialTermClients(specialTermClientFilter));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> LoadSpecialTermOutofTexasClients([FromBody]PTXboSpecialTermClientsFilter specialTermClientFilter)
        {
            try

            {
                var result = await Task.FromResult(_invoiceSpecialTermsService.LoadSpecialTermOutofTexasClients(specialTermClientFilter));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> LoadSpecialTermsDonotGenerateInvoices([FromBody]PTXboSpecialTermClientsFilter objFilter)
        {
            try
            {
                var result = await Task.FromResult(_invoiceSpecialTermsService.LoadSpecialTermsDonotGenerateInvoices(objFilter));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Added by saravanans.tfs id:55312
        /// </summary>
        /// <param name="objFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> LoadSpecialTermsReGenerateInvoices([FromBody]PTXboSpecialTermClientsFilter objFilter)
        {
            try
            {
                var result = await Task.FromResult(_invoiceSpecialTermsService.LoadSpecialTermsReGenerateInvoices(objFilter));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateInvoiceDetailsReport([FromBody]PTXboInvoiceReport objInvoice)
        {
            try
            {
                var result = await Task.FromResult(_invoiceSpecialTermsService.UpdateInvoiceDetailsReport(objInvoice));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateInvoiceProcessStatus([FromBody]PTXboUpdateInvoiceProcessingStatusInput invoiceProcessingStatus)
        {
            try
            {
                var result = await Task.FromResult(_invoiceSpecialTermsService.UpdateInvoiceProcessStatus(invoiceProcessingStatus));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SpecialTermInvoicesGeneration([FromBody]PTXboSpecialTermInput specialInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceSpecialTermsService.SpecialTermInvoicesGeneration(specialInput));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> SpecialTermoutofTexasInvoicesGeneration([FromBody]PTXboSpecialTermInput specialInput)
        {
            try
            {
                var result = await Task.FromResult(_invoiceSpecialTermsService.SpecialTermOutofTexasInvoicesGeneration(specialInput));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> SubmitForInvoiceGeneration([FromBody]PTXboSubmitForInvoiceGenerationInput invoiceGenerationInput)
        {
            try
            {
                string errorMessage = string.Empty;
                var result = await Task.FromResult(_invoiceSpecialTermsService.SubmitForInvoiceGeneration(invoiceGenerationInput.CurrentUserId, invoiceGenerationInput.CurrentUserRoleId, invoiceGenerationInput.InvoiceDetails,out errorMessage));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        /// <summary>
        /// Created by SaravananS. tfs id:61899
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GenerateInvoice([FromBody]PTXboGenerateInvoice_Request request)
        {
            try
            {
                string errorMessage = string.Empty;
                var result = await Task.FromResult(_invoiceSpecialTermsService.GenerateInvoice(request, out errorMessage));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        /// <summary>
        /// Created by SaravananS. tfs id:63613
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> GetTaxrateForTheGivenTaxyear([FromBody]PTXboSpecialTermInvoiceTaxrate request)
        {
            try
            {
                string errorMessage = string.Empty;
                var result = await Task.FromResult(_invoiceSpecialTermsService.GetTaxrateForTheGivenTaxyear(request, out errorMessage));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }



    }
}
