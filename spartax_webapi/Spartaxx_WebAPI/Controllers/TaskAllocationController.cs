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
    public class TaskAllocationController : ApiController
    {
        private readonly ITaskAllocationService _ITaskAllocationService;

        public TaskAllocationController(ITaskAllocationService ITaskAllocationService)
        {
            _ITaskAllocationService = ITaskAllocationService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getTaskAllocationDetailsForHearingNotice([FromBody]PTXboTaskAllocationSearchCriteria SearchCriteria)
        {
            try
            {
                var result = _ITaskAllocationService.getTaskAllocationDetailsForHearingNotice(SearchCriteria);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateHearingNoticeTaskAllocation([FromBody]List<PTXboTaskAllocationsearchResult> lstTaskAllocation)
        {
            try
            {
                var result = _ITaskAllocationService.UpdateHearingNoticeTaskAllocation(lstTaskAllocation);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getTaskAllocationDetailsForClientSetup([FromBody]PTXboTaskAllocationSearchCriteria SearchCriteria)
        {
            try
            {
                var result = _ITaskAllocationService.getTaskAllocationDetailsForClientSetup(SearchCriteria);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateClientSetupAuditingTaskAllocation([FromBody]List<PTXboTaskAllocationsearchResult> lstTaskAllocation)
        {
            try
            {
                var result = _ITaskAllocationService.UpdateClientSetupAuditingTaskAllocation(lstTaskAllocation);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getTaskAllocationDetailsForPropertySurvey([FromBody]PTXboTaskAllocationSearchCriteria SearchCriteria)
        {
            try
            {
                var result = _ITaskAllocationService.getTaskAllocationDetailsForPropertySurvey(SearchCriteria);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getTaskAllocationDetailsForAffidavit([FromBody]PTXboTaskAllocationSearchCriteria SearchCriteria)
        {
            try
            {
                var result = _ITaskAllocationService.getTaskAllocationDetailsForAffidavit(SearchCriteria);
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
