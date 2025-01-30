using Newtonsoft.Json;
using Spartaxx.BusinessObjects;
using Spartaxx.BusinessService;
using Spartaxx.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Spartaxx_WebAPI.Controllers
{
    public class AffidavitController : ApiController
    {
        private readonly IAffidavitService _IAffidavitService;

        public AffidavitController(IAffidavitService IAffidavitService)
        {
            _IAffidavitService = IAffidavitService;
        }
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> getAffidavitAllottedDocuments([FromBody]PTXboAffidavitAllottedDocBasedOnSearchFilter objHearingNoticeAllotmentSearchRequest)
        {
            try
            {
                var result = _IAffidavitService.getAffidavitAllottedDocuments(objHearingNoticeAllotmentSearchRequest);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> getAffidavitNextRecordList([FromBody]PTXdoIndexedNoticeDocument indexedNoticeList)
        {
            dynamic result;
            try
            {
                result = _IAffidavitService.getNextRecordList(indexedNoticeList, (int)indexedNoticeList.Accountid, (int)indexedNoticeList.taxYear, (int)indexedNoticeList.HearingTypeid, (int)indexedNoticeList.assignedQueue.NoticeQueueId);
                string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

    }
}