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
    public class AccountController : ApiController
    {
        private readonly IAccountService _IAccountService;

        public AccountController(IAccountService IAccountService)
        {
            _IAccountService = IAccountService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CreateQuickNewClientValues([FromBody]PTXboCreateQuickNewClientValues CreateQuickNewClientValues)
        {
            try
            {
                var result = _IAccountService.CreateQuickNewClientValues(CreateQuickNewClientValues.objNewClientQuickSetup, CreateQuickNewClientValues.lstPropertiesFromTaxroll);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> ReactivationHistory([FromBody]int AccountId)
        {
            try
            {
                var result = _IAccountService.ReactivationHistory(AccountId);
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