using Spartaxx.BusinessObjects;
using Spartaxx.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.BusinessService
{
    public class TaskAllocationService : ITaskAllocationService
    {
        private readonly UnitOfWork _UnitOfWork = null;

        public TaskAllocationService()
        {
            _UnitOfWork = new UnitOfWork();
        }

        public List<PTXboTaskAllocationsearchResult> getTaskAllocationDetailsForHearingNotice(PTXboTaskAllocationSearchCriteria SearchCriteria)
        {
            return _UnitOfWork.TaskAllocationRepocitory.getTaskAllocationDetailsForHearingNotice(SearchCriteria);
        }

        public bool UpdateHearingNoticeTaskAllocation(List<PTXboTaskAllocationsearchResult> lstTaskAllocation)
        {
            return _UnitOfWork.TaskAllocationRepocitory.UpdateHearingNoticeTaskAllocation(lstTaskAllocation);
        }

        public List<PTXboTaskAllocationsearchResult> getTaskAllocationDetailsForClientSetup(PTXboTaskAllocationSearchCriteria SearchCriteria)
        {
            return _UnitOfWork.TaskAllocationRepocitory.getTaskAllocationDetailsForClientSetup(SearchCriteria);
        }
        public bool UpdateClientSetupAuditingTaskAllocation(List<PTXboTaskAllocationsearchResult> lstTaskAllocation)
        {
            return _UnitOfWork.TaskAllocationRepocitory.UpdateClientSetupAuditingTaskAllocation(lstTaskAllocation);
        }

        public List<PTXboTaskAllocationsearchResult> getTaskAllocationDetailsForPropertySurvey(PTXboTaskAllocationSearchCriteria SearchCriteria)
        {
            return _UnitOfWork.TaskAllocationRepocitory.getTaskAllocationDetailsForPropertySurvey(SearchCriteria);
        }

        public List<PTXboTaskAllocationsearchResult> getTaskAllocationDetailsForAffidavit(PTXboTaskAllocationSearchCriteria SearchCriteria)
        {
            return _UnitOfWork.TaskAllocationRepocitory.getTaskAllocationDetailsForAffidavit(SearchCriteria);
        }
    }
}
