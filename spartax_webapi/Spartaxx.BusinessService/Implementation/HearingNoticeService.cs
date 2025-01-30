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
    public class HearingNoticeService : IHearingNoticeService
    {
        private readonly UnitOfWork _UnitOfWork = null;

        public HearingNoticeService()
        {
            _UnitOfWork = new UnitOfWork();
        }

        public List<PTXboHearingNoticeAllocatedDocuments> getHearingNoticeEntryAllottedDocuments(PTXboHearingNoticeAllotmentSearchRequest objHearingNoticeAllotmentSearchRequest)
        {
            return _UnitOfWork.HearingNoticeRepocitory.getHearingNoticeEntryAllottedDocuments(objHearingNoticeAllotmentSearchRequest);
        }
        public PTXboYearlyHearingDetailsList getYearlyHearingDetails(int YearlyHearingDetailsId)
        {
            return _UnitOfWork.HearingNoticeRepocitory.getYearlyHearingDetails(YearlyHearingDetailsId);
        }

        public ExpandoObject getNextRecordList(PTXdoIndexedNoticeDocument indexedNoticeList, int AssignedQueue)
        {
            if (AssignedQueue != (int)Enumerators.PTXenumNoticeQueue.QCQueue)
                return _UnitOfWork.HearingNoticeRepocitory.getNextRecordList(indexedNoticeList, AssignedQueue);
            else
                return _UnitOfWork.HearingNoticeRepocitory.getNext_QC_RecordList(indexedNoticeList, AssignedQueue);
        }
        public bool saveOrUpdateHearingNoticeIndexedNoticeDocument(PTXboIndexedNoticeDocumentUpdate indexedNoticeDocumentUpdate)
        {
            return _UnitOfWork.HearingNoticeRepocitory.saveOrUpdateHearingNoticeIndexedNoticeDocument(indexedNoticeDocumentUpdate);
        }
        public int saveOrUpdateUserEnteredHearingNoticeDataUpdate(PTXboUserEnteredHearingNoticeDataUpdate userEnteredHearingNoticeDataUpdate)
        {
            return _UnitOfWork.HearingNoticeRepocitory.saveOrUpdateUserEnteredHearingNoticeDataUpdate(userEnteredHearingNoticeDataUpdate);
        }
        public bool saveOrUpdateYearlyHearingDetails(PTXboYearlyHearingDetailsList yearlyHearingDetailsList)
        {
            return _UnitOfWork.HearingNoticeRepocitory.saveOrUpdateYearlyHearingDetails(yearlyHearingDetailsList);
        }
        public bool saveOrUpdateUserEnteredHearingNoticeRemarks(PTXboUserEnteredHearingNoticeRemarksUpdate userEnteredHearingNoticeRemarksUpdate)
        {
            return _UnitOfWork.HearingNoticeRepocitory.saveOrUpdateUserEnteredHearingNoticeRemarks(userEnteredHearingNoticeRemarksUpdate);
        }
        public bool saveOrUpdateUserEnteredHearingTypeForNotice(PTXboUserEnteredHearingTypeForNoticeUpdate userEnteredHearingTypeForNoticeUpdate)
        {
            return _UnitOfWork.HearingNoticeRepocitory.saveOrUpdateUserEnteredHearingTypeForNotice(userEnteredHearingTypeForNoticeUpdate);
        }
        public bool saveOrUpdateNoticeClarification(PTXboNoticeClarificationUpdate noticeClarificationUpdate)
        {
            return _UnitOfWork.HearingNoticeRepocitory.saveOrUpdateNoticeClarification(noticeClarificationUpdate);
        }
        public bool saveOrUpdateHearingDetails(PTXboHearingDetailsList hearingDetailsUpdate)
        {
            return _UnitOfWork.HearingNoticeRepocitory.saveOrUpdateHearingDetails(hearingDetailsUpdate);
        }
        public bool saveOrUpdateValueNotice(PTXboPriorValueNotice valueNotice)
        {
            return _UnitOfWork.HearingNoticeRepocitory.saveOrUpdateValueNotice(valueNotice);
        }
    }
}
