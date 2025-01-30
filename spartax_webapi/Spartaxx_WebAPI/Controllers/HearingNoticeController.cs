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
    public class HearingNoticeController : ApiController
    {
        private readonly IHearingNoticeService _IHearingNoticeService;        

        public HearingNoticeController(IHearingNoticeService IHearingNoticeService)
        {
            _IHearingNoticeService = IHearingNoticeService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getHearingNoticeEntryAllottedDocuments([FromBody]PTXboHearingNoticeAllotmentSearchRequest objHearingNoticeAllotmentSearchRequest)
        {
            try
            {
                var result = _IHearingNoticeService.getHearingNoticeEntryAllottedDocuments(objHearingNoticeAllotmentSearchRequest);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getNextEntryRecordList([FromBody]PTXdoIndexedNoticeDocument indexedNoticeList)
        {
            dynamic result;
            try
            {
                result = _IHearingNoticeService.getNextRecordList(indexedNoticeList, (int)Enumerators.PTXenumNoticeQueue.EntryQueue);
                string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getNextAuditRecordList([FromBody]PTXdoIndexedNoticeDocument indexedNoticeList)
        {
            dynamic result;
            try
            {
                result = _IHearingNoticeService.getNextRecordList(indexedNoticeList, (int)Enumerators.PTXenumNoticeQueue.AuditQueue);
                string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getNextDefectRecordList([FromBody]PTXdoIndexedNoticeDocument indexedNoticeList)
        {
            dynamic result;
            try
            {
                result = _IHearingNoticeService.getNextRecordList(indexedNoticeList, (int)Enumerators.PTXenumNoticeQueue.DefectQueue);
                string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getNextClarificationRequestRecordList([FromBody]PTXdoIndexedNoticeDocument indexedNoticeList)
        {
            dynamic result;
            try
            {
                result = _IHearingNoticeService.getNextRecordList(indexedNoticeList, (int)Enumerators.PTXenumNoticeQueue.ClarificationQueue);
                string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getNextQCRecordList([FromBody]PTXdoIndexedNoticeDocument indexedNoticeList)
        {
            dynamic result;
            try
            {
                result = _IHearingNoticeService.getNextRecordList(indexedNoticeList, (int)Enumerators.PTXenumNoticeQueue.QCQueue);
                string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getNextMultipleEntryRecordList([FromBody]PTXdoIndexedNoticeDocument indexedNoticeList)
        {
            dynamic result;
            try
            {
                result = _IHearingNoticeService.getNextRecordList(indexedNoticeList, (int)Enumerators.PTXenumNoticeQueue.MultipleEntryQueue);
                string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getYearlyHearingDetails([FromBody]PTXboCommon common)
        {
            var result = _IHearingNoticeService.getYearlyHearingDetails(common.YearlyHearingDetailsId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> saveOrUpdateHearingNoticeIndexedNoticeDocument([FromBody]PTXboIndexedNoticeDocumentUpdate indexedNoticeDocumentUpdate)
        {
            dynamic result;
            try
            {
                result = _IHearingNoticeService.saveOrUpdateHearingNoticeIndexedNoticeDocument(indexedNoticeDocumentUpdate);
                string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        
        [HttpPost]
        public async Task<HttpResponseMessage> saveOrUpdateUserEnteredHearingNoticeDataUpdate([FromBody]PTXboUserEnteredHearingNoticeDataUpdate userEnteredHearingNoticeDataUpdate)
        {
            dynamic result;
            try
            {
                result = _IHearingNoticeService.saveOrUpdateUserEnteredHearingNoticeDataUpdate(userEnteredHearingNoticeDataUpdate);
                string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        //
        [HttpPost]
        public async Task<HttpResponseMessage> saveOrUpdateYearlyHearingDetails([FromBody]PTXboYearlyHearingDetailsList yearlyHearingDetailsList)
        {
            dynamic result;
            try
            {
                result = _IHearingNoticeService.saveOrUpdateYearlyHearingDetails(yearlyHearingDetailsList);
                string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> saveOrUpdateHearingDetails([FromBody]PTXboHearingDetailsList hearingDetailsUpdate)
        {
            dynamic result;
            try
            {
                result = _IHearingNoticeService.saveOrUpdateHearingDetails(hearingDetailsUpdate);
                string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> saveOrUpdateHearingNoticeValueNotice([FromBody]PTXboPriorValueNotice valueNotice)
        {
            dynamic result;
            try
            {
                result = _IHearingNoticeService.saveOrUpdateValueNotice(valueNotice);
                string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> saveOrUpdateUserEnteredHearingNoticeRemarks([FromBody]PTXboUserEnteredHearingNoticeRemarksUpdate userEnteredHearingNoticeRemarksUpdate)
        {
            dynamic result;
            try
            {
                result = _IHearingNoticeService.saveOrUpdateUserEnteredHearingNoticeRemarks(userEnteredHearingNoticeRemarksUpdate);
                string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }        
        [HttpPost]
        public async Task<HttpResponseMessage> saveOrUpdateUserEnteredHearingTypeForNotice([FromBody]PTXboUserEnteredHearingTypeForNoticeUpdate userEnteredHearingTypeForNoticeUpdate)
        {
            dynamic result;
            try
            {
                result = _IHearingNoticeService.saveOrUpdateUserEnteredHearingTypeForNotice(userEnteredHearingTypeForNoticeUpdate);
                string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> saveOrUpdateNoticeClarification([FromBody]PTXboNoticeClarificationUpdate noticeClarificationUpdate)
        {
            dynamic result;
            try
            {
                result = _IHearingNoticeService.saveOrUpdateNoticeClarification(noticeClarificationUpdate);
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
