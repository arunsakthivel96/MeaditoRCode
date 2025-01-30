using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spartaxx.DataAccess;
using Spartaxx.BusinessObjects;
using Spartaxx.DataObjects;
using System.Dynamic;

namespace Spartaxx.BusinessService
{
    public class HearingResultService : IHearingResultService
    {
        private readonly UnitOfWork _UnitOfWork = null;

        public HearingResultService()
        {
            _UnitOfWork = new UnitOfWork();
        }

        public List<PTXboHearingResultAllottedDocument> getHearingResultEntryAllottedDocuments(PTXboHearingResultAllottedDocBasedOnSearchFilter objHearingResultAllotmentSearchRequest)
        {
            return _UnitOfWork.HearingResultRepocitory.getHearingResultEntryAllottedDocuments(objHearingResultAllotmentSearchRequest);
        }

        public ExpandoObject getHearingResultNextEntryRecordList(PTXdoIndexedNoticeDocument indexedNoticeList, int Accountid, int taxYear, int HearingTypeid,int AssignedQueue)
        {
            return _UnitOfWork.HearingResultRepocitory.getHearingResultNextEntryRecordList(indexedNoticeList,Accountid,taxYear,HearingTypeid,AssignedQueue);
            //if (AssignedQueue != (int)Enumerators.PTXenumNoticeQueue.QCQueue)
            //    return _UnitOfWork.HearingNoticeRepocitory.getNextRecordList(indexedNoticeList, AssignedQueue);
            //else
            //    return _UnitOfWork.HearingNoticeRepocitory.getNext_QC_RecordList(indexedNoticeList, AssignedQueue);
        }
        public List<PTXdoHearingStatus> getSelectedHearingStatusByHearingType(string hearingTypeId)
        {
            return _UnitOfWork.HearingResultRepocitory.getSelectedHearingStatusByHearingType(hearingTypeId);
        }
        public PTXboYearlyHearingDetailsList getYearlyHearingDetails(int AccountId, int TaxYear)
        {
            return _UnitOfWork.HearingResultRepocitory.getYearlyHearingDetails(AccountId, TaxYear);
        }
        public PTXboHearingDetailsList getHearingDetailsYearly(int Mode,int Id, int hearingTypeId)
        {
            return _UnitOfWork.HearingResultRepocitory.getHearingDetailsYearly(Mode, Id, hearingTypeId);
        }
        public List<PTXboExistingHearingDetailsForHearingResult> getExistingHearingDetailsForResult(PTXboExistingHearingDetailsForHearingResult objExistingHearingDetailsForHearingResult)
        {
            return _UnitOfWork.HearingResultRepocitory.getExistingHearingDetailsForResult(objExistingHearingDetailsForHearingResult);
        }
        public PTXboHearingResult getHearingResultByHearingDetailsId(int Mode,int Id)
        {
            return _UnitOfWork.HearingResultRepocitory.getHearingResultByHearingDetailsId(Mode, Id);
        }
        public PTXdoInvoiceAndHearingResultMap getInvoiceAndHearingResultMap(int HearingDetailsId)
        {
            return _UnitOfWork.HearingResultRepocitory.getInvoiceAndHearingResultMap(HearingDetailsId);
        }
        public PTXboValueNotice getValueHearingNotice(int YearlyHearingDetailsId)
        {
            return _UnitOfWork.HearingResultRepocitory.getValueHearingNotice(YearlyHearingDetailsId);
        }
        public PTXdoInvoice getInvoiceWithInvoiceAndHearingResultMapInvoice(int Mode, int HearingResultsId)
        {
            return _UnitOfWork.HearingResultRepocitory.getInvoiceWithInvoiceAndHearingResultMapInvoice(Mode, HearingResultsId);
        }
        public PTXdoInvoiceAndHearingResultMap getInvoiceWithInvoiceAndHearingResultMapPHRMAP(int Mode, int HearingResultsId)
        {
            return _UnitOfWork.HearingResultRepocitory.getInvoiceWithInvoiceAndHearingResultMapPHRMAP(Mode, HearingResultsId);
        }
        public List<PTXdoInvoiceAndHearingResultMap> getInvoiceAndHearingResultMapList(int Mode, int InvoiceID)
        {
            return _UnitOfWork.HearingResultRepocitory.getInvoiceAndHearingResultMapList(Mode, InvoiceID);
        }
        public ExpandoObject getNextRecordList(PTXdoIndexedNoticeDocument indexedNoticeList, int Accountid, int taxYear, int HearingTypeid, int AssignedQueue)
        {
            return _UnitOfWork.HearingResultRepocitory.getNextRecordList(indexedNoticeList, Accountid, taxYear, HearingTypeid, AssignedQueue);
            //if (AssignedQueue != (int)Enumerators.PTXenumNoticeQueue.QCQueue)
            //    return _UnitOfWork.HearingNoticeRepocitory.getNextRecordList(indexedNoticeList, AssignedQueue);
            //else
            //    return _UnitOfWork.HearingNoticeRepocitory.getNext_QC_RecordList(indexedNoticeList, AssignedQueue);
        }
        public List<PTXboHearingResultsPWDocumentSearch> getDocumentDetailsFromPaperwise(PTXboHearingResultsPWDocumentSearch hearingResultsPWDocumentSearch)
        {
            return _UnitOfWork.HearingResultRepocitory.getDocumentDetailsFromPaperwise(hearingResultsPWDocumentSearch);            
        }
        public bool updateHearingResultRecordList(PTXdoIndexedNoticeDocument objPTXdoIndexedNoticeDocument)
        {
            return _UnitOfWork.HearingResultRepocitory.updateHearingResultRecordList(objPTXdoIndexedNoticeDocument);
        }
        public bool saveOrUpdateUserEnteredHearingResults(PTXboUserEnteredHearingResultsInsert userEnteredHearingResults)
        {
            return _UnitOfWork.HearingResultRepocitory.saveOrUpdateUserEnteredHearingResults(userEnteredHearingResults);
        }
        public bool saveOrUpdateIndexedNoticeDocument(PTXboIndexedNoticeDocumentUpdate indexedNoticeDocumentUpdate)
        {
            return _UnitOfWork.HearingResultRepocitory.saveOrUpdateIndexedNoticeDocument(indexedNoticeDocumentUpdate);
        }
        public bool saveOrUpdateHearingDetails(PTXboHearingDetailsList hearingDetailsUpdate)
        {
            return _UnitOfWork.HearingResultRepocitory.saveOrUpdateHearingDetails(hearingDetailsUpdate);
        }
        public bool saveOrUpdateHearingDetailsRemarks(PTXboHearingDetailsRemarks hearingDetailsRemarks)
        {
            return _UnitOfWork.HearingResultRepocitory.saveOrUpdateHearingDetailsRemarks(hearingDetailsRemarks);
        }
        public bool saveOrUpdateHearingResult(PTXboHearingResults hearingResults)
        {
            return _UnitOfWork.HearingResultRepocitory.saveOrUpdateHearingResult(hearingResults);
        }
        public bool saveOrUpdateValueNotice(PTXboPriorValueNotice valueNotice)
        {
            return _UnitOfWork.HearingResultRepocitory.saveOrUpdateValueNotice(valueNotice);
        }
        public bool saveOrUpdateInvoice(PTXboInvoice invoice)
        {
            return _UnitOfWork.HearingResultRepocitory.saveOrUpdateInvoice(invoice);
        }
    }
}
