using Spartaxx.BusinessObjects;
using Spartaxx.BusinessService;
using Spartaxx.Common.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Spartaxx_WebAPI.Controllers
{
    public class ExistingClientController : ApiController
    {
        private readonly IExistingClientService _IExistingClientService;

        public ExistingClientController(IExistingClientService IExistingClientService)
        {
            _IExistingClientService = IExistingClientService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getClientCodesDetails([FromBody]int ClientID)
        {
            try
            {
                var result = _IExistingClientService.getClientCodesDetails(ClientID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SaveAddressContactAndPhone([FromBody]PTXboSaveAddressContactAndPhone saveAddressContactAndPhone)
        {
            try
            {
                var result = _IExistingClientService.SaveAddressContactAndPhone(saveAddressContactAndPhone.AddressId, saveAddressContactAndPhone.AddressTypeId, saveAddressContactAndPhone.GroupID);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GetAccountSummary([FromBody]PTXboGetAccountSummary getAccountSummary)
        {
            try
            {
                var result = _IExistingClientService.getAccountSummary(getAccountSummary.ClientID, getAccountSummary.UserID);
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