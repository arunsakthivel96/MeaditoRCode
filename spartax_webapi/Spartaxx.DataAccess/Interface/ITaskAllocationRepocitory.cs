using Spartaxx.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    public interface ITaskAllocationRepocitory
    {
        List<PTXboTaskAllocationsearchResult> getTaskAllocationDetailsForHearingNotice(PTXboTaskAllocationSearchCriteria SearchCriteria);
        bool UpdateHearingNoticeTaskAllocation(List<PTXboTaskAllocationsearchResult> lstTaskAllocation);

        List<PTXboTaskAllocationsearchResult> getTaskAllocationDetailsForClientSetup(PTXboTaskAllocationSearchCriteria SearchCriteria);
        bool UpdateClientSetupAuditingTaskAllocation(List<PTXboTaskAllocationsearchResult> lstTaskAllocation);
        List<PTXboTaskAllocationsearchResult> getTaskAllocationDetailsForPropertySurvey(PTXboTaskAllocationSearchCriteria SearchCriteria);

        List<PTXboTaskAllocationsearchResult> getTaskAllocationDetailsForAffidavit(PTXboTaskAllocationSearchCriteria SearchCriteria);
    }
}
