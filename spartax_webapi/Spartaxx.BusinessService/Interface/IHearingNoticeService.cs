using Spartaxx.BusinessObjects;
using Spartaxx.DataObjects;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
    public interface IHearingNoticeService
    {
        List<PTXboHearingNoticeAllocatedDocuments> getHearingNoticeEntryAllottedDocuments(PTXboHearingNoticeAllotmentSearchRequest objHearingNoticeAllotmentSearchRequest);
        ExpandoObject getNextRecordList(PTXdoIndexedNoticeDocument indexedNoticeList, int AssignedQueue);
        PTXboYearlyHearingDetailsList getYearlyHearingDetails(int YearlyHearingDetailsId);

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
