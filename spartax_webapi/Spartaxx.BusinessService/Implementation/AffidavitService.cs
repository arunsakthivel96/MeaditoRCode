using Spartaxx.BusinessObjects;
using Spartaxx.DataAccess;
using Spartaxx.DataObjects;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
   public class AffidavitService :IAffidavitService
    {
        private readonly UnitOfWork _UnitOfWork = null;

        public AffidavitService()
        {
            _UnitOfWork = new UnitOfWork();
        }

        public List<PTXboAffidavitAllottedDocument> getAffidavitAllottedDocuments(PTXboAffidavitAllottedDocBasedOnSearchFilter objAffidavitAllotmentSearchRequest)
        {
            return _UnitOfWork.AffidavitRepository.getAffidavitAllottedDocuments(objAffidavitAllotmentSearchRequest);
        }
        public ExpandoObject getNextRecordList(PTXdoIndexedNoticeDocument indexedNoticeList, int Accountid, int taxYear, int HearingTypeid, int AssignedQueue)
        {
            return _UnitOfWork.AffidavitRepository.getNextRecordList(indexedNoticeList, Accountid, taxYear, HearingTypeid, AssignedQueue);
        }
    }
}
