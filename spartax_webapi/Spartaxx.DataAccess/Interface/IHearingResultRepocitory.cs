using Spartaxx.BusinessObjects;
using Spartaxx.DataObjects;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    public interface IHearingResultRepocitory
    {
        List<PTXboHearingResultAllottedDocument> getHearingResultEntryAllottedDocuments(PTXboHearingResultAllottedDocBasedOnSearchFilter objHearingResultAllotmentSearchRequest);
        ExpandoObject getHearingResultNextEntryRecordList(PTXdoIndexedNoticeDocument indexedNoticeList, int Accountid, int taxYear, int HearingTypeid, int AssignedQueue);
        List<PTXdoHearingStatus> getSelectedHearingStatusByHearingType(string hearingTypeId);
        List<PTXboExistingHearingDetailsForHearingResult> getExistingHearingDetailsForResult(PTXboExistingHearingDetailsForHearingResult objExistingHearingDetailsForHearingResult);
        ExpandoObject getNextRecordList(PTXdoIndexedNoticeDocument indexedNoticeList,int Accountid,int taxYear,int HearingTypeid,int AssignedQueue);
        List<PTXboHearingResultsPWDocumentSearch> getDocumentDetailsFromPaperwise(PTXboHearingResultsPWDocumentSearch indexedNoticeList);
        bool updateHearingResultRecordList(PTXdoIndexedNoticeDocument objPTXdoIndexedNoticeDocument);
        PTXboYearlyHearingDetailsList getYearlyHearingDetails(int AccountId, int TaxYear);
        PTXboHearingDetailsList getHearingDetailsYearly(int Mode, int Id, int HearingTypeId);
        PTXboHearingResult getHearingResultByHearingDetailsId(int Mode,int Id);
        PTXdoInvoiceAndHearingResultMap getInvoiceAndHearingResultMap(int HearingDetailsId);
        PTXboValueNotice getValueHearingNotice(int YearlyHearingDetailsId);
        PTXdoInvoice getInvoiceWithInvoiceAndHearingResultMapInvoice(int Mode, int HearingResultsId);
        PTXdoInvoiceAndHearingResultMap getInvoiceWithInvoiceAndHearingResultMapPHRMAP(int Mode, int HearingResultsId);
        List<PTXdoInvoiceAndHearingResultMap> getInvoiceAndHearingResultMapList(int Mode, int InvoiceID);
        bool saveOrUpdateUserEnteredHearingResults(PTXboUserEnteredHearingResultsInsert userEnteredHearingResults);
        bool saveOrUpdateIndexedNoticeDocument(PTXboIndexedNoticeDocumentUpdate indexedNoticeDocumentUpdate);
        bool saveOrUpdateHearingDetails(PTXboHearingDetailsList hearingDetailsUpdate);
        bool saveOrUpdateHearingDetailsRemarks(PTXboHearingDetailsRemarks hearingDetailsRemarks);
        bool saveOrUpdateHearingResult(PTXboHearingResults hearingResults);
        bool saveOrUpdateValueNotice(PTXboPriorValueNotice valueNotice);
        bool saveOrUpdateInvoice(PTXboInvoice invoice);
    }
}
