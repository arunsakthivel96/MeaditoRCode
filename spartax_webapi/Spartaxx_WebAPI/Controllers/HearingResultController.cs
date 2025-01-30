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
    public class HearingResultController : ApiController
    {
        private readonly IHearingResultService _IHearingResultService;

        public HearingResultController(IHearingResultService IHearingResultService)
        {
            _IHearingResultService = IHearingResultService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getHearingResultEntryAllottedDocuments([FromBody]PTXboHearingResultAllottedDocBasedOnSearchFilter objHearingNoticeAllotmentSearchRequest)
        {
            try
            {
                var result = _IHearingResultService.getHearingResultEntryAllottedDocuments(objHearingNoticeAllotmentSearchRequest);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getHearingResultNextEntryRecordList([FromBody]PTXdoIndexedNoticeDocument indexedNoticeList)
        {
            dynamic result;
            try
            {
                result = _IHearingResultService.getHearingResultNextEntryRecordList(indexedNoticeList, (int)indexedNoticeList.Accountid, (int)indexedNoticeList.taxYear, (int)indexedNoticeList.HearingTypeid, (int)indexedNoticeList.assignedQueue.NoticeQueueId);
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
        public async Task<HttpResponseMessage> getSelectedHearingStatusByHearingType([FromBody]PTXboCommon common)
        {
            try
            {
                var result = _IHearingResultService.getSelectedHearingStatusByHearingType(common.hearingTypeId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getExistingHearingDetailsForResult([FromBody]PTXboExistingHearingDetailsForHearingResult objExistingHearingDetailsForHearingResult)
        {
            try
            {
                var result = _IHearingResultService.getExistingHearingDetailsForResult(objExistingHearingDetailsForHearingResult);
                return Request.CreateResponse(HttpStatusCode.OK, result);
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
                result = _IHearingResultService.getNextRecordList(indexedNoticeList, (int)indexedNoticeList.Accountid, (int)indexedNoticeList.taxYear, (int)indexedNoticeList.HearingTypeid, (int)indexedNoticeList.assignedQueue.NoticeQueueId);
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
        public async Task<HttpResponseMessage> getDocumentDetailsFromPaperwise([FromBody]PTXboHearingResultsPWDocumentSearch hearingResultsPWDocumentSearch)
        {
            dynamic result;
            try
            {
                result = _IHearingResultService.getDocumentDetailsFromPaperwise(hearingResultsPWDocumentSearch);
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
        public async Task<HttpResponseMessage> updateHearingResultRecordList([FromBody]PTXdoIndexedNoticeDocument objIndexedNoticeDocument)
        {
            dynamic result;
            try
            {
                result = _IHearingResultService.updateHearingResultRecordList(objIndexedNoticeDocument);
                string jsonvalue = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Request.CreateResponse(HttpStatusCode.OK, jsonvalue);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getYearlyHearingDetails([FromBody]PTXboCommon common)
        {
            try
            {
                var result = _IHearingResultService.getYearlyHearingDetails(common.AccountId, common.TaxYear);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getHearingDetailsYearly([FromBody]PTXboCommon common)
        {
            try
            {
                var result = _IHearingResultService.getHearingDetailsYearly(common.Mode, common.Id, Convert.ToInt32(common.hearingTypeId));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getHearingResultByHearingDetailsId([FromBody]PTXboCommon common)
        {
            try
            {
                var result = _IHearingResultService.getHearingResultByHearingDetailsId(common.Mode, common.Id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getInvoiceAndHearingResultMap([FromBody]PTXboCommon common)
        {
            try
            {
                var result = _IHearingResultService.getInvoiceAndHearingResultMap(common.HearingResultsId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getValueHearingNotice([FromBody]PTXboCommon common)
        {
            try
            {
                var result = _IHearingResultService.getValueHearingNotice(common.YearlyHearingDetailsId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getInvoiceWithInvoiceAndHearingResultMapInvoice([FromBody]PTXboCommon common)
        {
            try
            {
                var result = _IHearingResultService.getInvoiceWithInvoiceAndHearingResultMapInvoice(common.Mode, common.HearingResultsId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }
        [HttpPost]
        public async Task<HttpResponseMessage> getInvoiceWithInvoiceAndHearingResultMapPHRMAP([FromBody]PTXboCommon common)
        {
            try
            {
                var result = _IHearingResultService.getInvoiceWithInvoiceAndHearingResultMapPHRMAP(common.Mode, common.HearingResultsId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                //throw ex;
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> getInvoiceAndHearingResultMapList([FromBody]PTXboCommon common)
        {
            dynamic result;
            try
            {
                result = _IHearingResultService.getInvoiceAndHearingResultMapList(common.Mode, common.InvoiceID);
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
        public async Task<HttpResponseMessage> saveOrUpdateUserEnteredHearingResults([FromBody]PTXboUserEnteredHearingResultsInsert userEnteredHearingResults)
        {
            dynamic result;
            try
            {
                result = _IHearingResultService.saveOrUpdateUserEnteredHearingResults(userEnteredHearingResults);
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
        public async Task<HttpResponseMessage> saveOrUpdateIndexedNoticeDocument([FromBody]PTXboIndexedNoticeDocumentUpdate indexedNoticeDocumentUpdate)
        {
            dynamic result;
            try
            {
                result = _IHearingResultService.saveOrUpdateIndexedNoticeDocument(indexedNoticeDocumentUpdate);
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
                result = _IHearingResultService.saveOrUpdateHearingDetails(hearingDetailsUpdate);
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
        public async Task<HttpResponseMessage> saveOrUpdateHearingDetailsRemarks([FromBody]PTXboHearingDetailsRemarks hearingDetailsRemarks)
        {
            dynamic result;
            try
            {
                result = _IHearingResultService.saveOrUpdateHearingDetailsRemarks(hearingDetailsRemarks);
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
        public async Task<HttpResponseMessage> saveOrUpdateHearingResult([FromBody]PTXboHearingResults hearingResults)
        {
            dynamic result;
            try
            {
                result = _IHearingResultService.saveOrUpdateHearingResult(hearingResults);
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
        public async Task<HttpResponseMessage> saveOrUpdateValueNotice([FromBody]PTXboPriorValueNotice valueNotice)
        {
            dynamic result;
            try
            {
                result = _IHearingResultService.saveOrUpdateValueNotice(valueNotice);
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
        public async Task<HttpResponseMessage> saveOrUpdateInvoice([FromBody]PTXboInvoice invoice)
        {
            dynamic result;
            try
            {
                result = _IHearingResultService.saveOrUpdateInvoice(invoice);
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
