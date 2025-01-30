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
    public interface IAffidavitRepocitory
    {
        List<PTXboAffidavitAllottedDocument> getAffidavitAllottedDocuments(PTXboAffidavitAllottedDocBasedOnSearchFilter objHearingResultAllotmentSearchRequest);
        ExpandoObject getNextRecordList(PTXdoIndexedNoticeDocument indexedNoticeList, int Accountid, int taxYear, int HearingTypeid, int AssignedQueue);
    }
}
