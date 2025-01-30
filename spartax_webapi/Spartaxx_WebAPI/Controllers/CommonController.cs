using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Spartaxx.BusinessService;
using System.Threading.Tasks;
using Spartaxx.BusinessObjects;
using Spartaxx.DataObjects;
using Newtonsoft.Json;

namespace Spartaxx_WebAPI.Controllers
{
    public class CommonController : ApiController
    {
        private readonly ICommonService _ICommonService;

        public CommonController(ICommonService ICommonService)
        {
            _ICommonService = ICommonService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getPWDocumentTypeListByFileCabinetID([FromBody]PTXboCommon common)
        {
            try
            {
                var result = _ICommonService.getPWDocumentTypeListByFileCabinetID(Convert.ToInt32(common.FileCabinetID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getIndexedNoticeDocumentRouteFrom([FromBody]PTXboCommon common)
        {
            try
            {
                var result = _ICommonService.getIndexedNoticeDocumentRouteFrom(Convert.ToInt32(common.IndexedNoticeDocumentId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getCountyURL([FromBody]PTXboCommon common)
        {
            try
            {
                var result = _ICommonService.getCountyURL(Convert.ToInt32(common.CountyID));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        //[HttpPost]
        //public async Task<HttpResponseMessage> getHearingResultNextEntryRecordList([FromBody]PTXdoIndexedNoticeDocument indexedNoticeList)
        //{
        //    dynamic result;
        //    try
        //    {
        //        result = _IHearingResultService.getHearingResultNextEntryRecordList(indexedNoticeList, (int)indexedNoticeList.Accountid, (int)indexedNoticeList.taxYear, (int)indexedNoticeList.HearingTypeid, (int)Enumerators.PTXenumNoticeQueue.EntryQueue);
        //        string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
        //        return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
        //        //throw ex;
        //    }
        //}

        //[HttpPost]
        //public async Task<HttpResponseMessage> getNextAuditRecordList([FromBody]PTXdoIndexedNoticeDocument indexedNoticeList)
        //{
        //    dynamic result;
        //    try
        //    {
        //        result = _IHearingNoticeService.getNextRecordList(indexedNoticeList, (int)Enumerators.PTXenumNoticeQueue.AuditQueue);
        //        string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
        //        return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
        //        //throw ex;
        //    }
        //}

        //[HttpPost]
        //public async Task<HttpResponseMessage> getNextDefectRecordList([FromBody]PTXdoIndexedNoticeDocument indexedNoticeList)
        //{
        //    dynamic result;
        //    try
        //    {
        //        result = _IHearingNoticeService.getNextRecordList(indexedNoticeList, (int)Enumerators.PTXenumNoticeQueue.DefectQueue);
        //        string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
        //        return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
        //        //throw ex;
        //    }
        //}

        //[HttpPost]
        //public async Task<HttpResponseMessage> getNextClarificationRequestRecordList([FromBody]PTXdoIndexedNoticeDocument indexedNoticeList)
        //{
        //    dynamic result;
        //    try
        //    {
        //        result = _IHearingNoticeService.getNextRecordList(indexedNoticeList, (int)Enumerators.PTXenumNoticeQueue.ClarificationQueue);
        //        string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
        //        return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
        //        //throw ex;
        //    }
        //}

        //[HttpPost]
        //public async Task<HttpResponseMessage> getNextQCRecordList([FromBody]PTXdoIndexedNoticeDocument indexedNoticeList)
        //{
        //    dynamic result;
        //    try
        //    {
        //        result = _IHearingNoticeService.getNextRecordList(indexedNoticeList, (int)Enumerators.PTXenumNoticeQueue.QCQueue);
        //        string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
        //        return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
        //        //throw ex;
        //    }
        //}

        //[HttpPost]
        //public async Task<HttpResponseMessage> getNextMultipleEntryRecordList([FromBody]PTXdoIndexedNoticeDocument indexedNoticeList)
        //{
        //    dynamic result;
        //    try
        //    {
        //        result = _IHearingNoticeService.getNextRecordList(indexedNoticeList, (int)Enumerators.PTXenumNoticeQueue.MultipleEntryQueue);
        //        string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
        //        return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
        //        //throw ex;
        //    }
        //}
    }
}
