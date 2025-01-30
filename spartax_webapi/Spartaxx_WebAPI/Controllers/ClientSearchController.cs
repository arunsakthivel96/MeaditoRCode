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
    public class ClientSearchController : ApiController
    {
        private readonly IClientSearchService _IClientSearchService;

        public ClientSearchController(IClientSearchService IClientSearchService)
        {
            _IClientSearchService = IClientSearchService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getSearchedAccountDetails([FromBody]PTXboClientSearch clientSearch)
        {
            try
            {
                var result = _IClientSearchService.getSearchedAccountDetails(clientSearch);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getSearchedClientDetails([FromBody]PTXboClientSearch clientSearch)
        {
            try
            {
                var result = _IClientSearchService.getSearchedClientDetails(clientSearch);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getCAFSearchedClientDetails([FromBody]PTXboClientSearch clientSearch)
        {
            try
            {
                var result = _IClientSearchService.getCAFSearchedClientDetails(clientSearch);
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