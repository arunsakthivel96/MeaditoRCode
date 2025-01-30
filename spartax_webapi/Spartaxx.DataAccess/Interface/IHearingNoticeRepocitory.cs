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
    public interface IHearingNoticeRepocitory
    {
        List<PTXboHearingNoticeAllocatedDocuments> getHearingNoticeEntryAllottedDocuments(PTXboHearingNoticeAllotmentSearchRequest objHearingNoticeAllotmentSearchRequest);
        PTXboYearlyHearingDetailsList getYearlyHearingDetails(int YearlyHearingDetailsId);
        ExpandoObject getNextRecordList(PTXdoIndexedNoticeDocument indexedNoticeList, int AssignedQueue);
        ExpandoObject getNext_QC_RecordList(PTXdoIndexedNoticeDocument indexedNoticeList, int AssignedQueue);
        bool saveOrUpdateHearingNoticeIndexedNoticeDocument(PTXboIndexedNoticeDocumentUpdate indexedNoticeDocumentUpdate);
        int saveOrUpdateUserEnteredHearingNoticeDataUpdate(PTXboUserEnteredHearingNoticeDataUpdate userEnteredHearingNoticeDataUpdate);
        bool saveOrUpdateYearlyHearingDetails(PTXboYearlyHearingDetailsList yearlyHearingDetailsList);        
        bool saveOrUpdateUserEnteredHearingNoticeRemarks(PTXboUserEnteredHearingNoticeRemarksUpdate userEnteredHearingNoticeRemarksUpdate);
        bool saveOrUpdateUserEnteredHearingTypeForNotice(PTXboUserEnteredHearingTypeForNoticeUpdate userEnteredHearingTypeForNoticeUpdate);
        bool saveOrUpdateNoticeClarification(PTXboNoticeClarificationUpdate noticeClarificationUpdate);
        bool saveOrUpdateHearingDetails(PTXboHearingDetailsList hearingDetailsUpdate);
        bool saveOrUpdateValueNotice(PTXboPriorValueNotice valueNotice);
    }
}
