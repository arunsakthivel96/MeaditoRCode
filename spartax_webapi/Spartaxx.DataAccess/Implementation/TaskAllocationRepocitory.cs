using Spartaxx.BusinessObjects;
using Spartaxx.Common;
using Spartaxx.DataObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
    public class TaskAllocationRepocitory : ITaskAllocationRepocitory
    {
        private readonly DapperConnection _Connection;
        public TaskAllocationRepocitory(DapperConnection Connection)
        {
            _Connection = Connection;
        }

        public List<PTXboTaskAllocationsearchResult> getTaskAllocationDetailsForHearingNotice(PTXboTaskAllocationSearchCriteria SearchCriteria)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@DocumentQueueTypeID", SearchCriteria.documenttypeID);
                _hashtable.Add("@AssignTypeID", SearchCriteria.NoticedProcessStatusID);
                _hashtable.Add("@BatchCode", SearchCriteria.BatchCode);
                _hashtable.Add("@ClientNumber", SearchCriteria.ClientNumber);
                _hashtable.Add("@ScanDateFrom", SearchCriteria.ScanDateFrom);
                _hashtable.Add("@ScanDateTo", SearchCriteria.ScanDateTo);
                _hashtable.Add("@NoofRecordsFrom", SearchCriteria.NoofRecordsFrom);
                _hashtable.Add("@NoofRecordsTo", SearchCriteria.NoofRecordsTo);
                _hashtable.Add("@AccountNumber", SearchCriteria.AccountNumber);
                var result = _Connection.Select<PTXboTaskAllocationsearchResult>(StoredProcedureNames.usp_GetDataEntryHearingNoticeForManualAllocationAPI,
                    _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateHearingNoticeTaskAllocation(List<PTXboTaskAllocationsearchResult> lstTaskAllocation)
        {
            Hashtable _hashtable = null;
            try
            {
                foreach (var data in lstTaskAllocation)
                {
                    _hashtable = new Hashtable();
                    _hashtable.Add("@UserID", data.AssignedTo);
                    _hashtable.Add("@UserRole", Enumerators.PTXenumTaskAllocationProcessType.DataEntry.GetDescription());
                    _hashtable.Add("@DateAndTime", DateTime.Now);
                    _hashtable.Add("@AssignedBy", data.Assigned);
                    _hashtable.Add("@BatchCode", data.BatchCode);
                    _hashtable.Add("@NoticeDocumentTypeId", Enumerators.PTXenumNoticeDocumentType.HearingNotice.GetId());
                    _hashtable.Add("@DocumentQueueTypeID", data.QueueType);
                    _hashtable.Add("@NoticeProcessingStatusId", GetNoticeProcessingStatusId(data.ProcessingStatusID));
                    _hashtable.Add("@Donotassignauto", data.isAutoAssign);
                    _hashtable.Add("@IndexedNoticeDocumentId", data.IndexedNoticeDocumentId);
                    var result = _Connection.Execute(StoredProcedureNames.USP_UpdateHearingNoticeTaskAllocation,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx, true);
                    if (result <= 0)
                        throw new Exception("No records updated");
                }
                // _Connection.CommitTransaction(); //commented by saravanans-commit is done automatically inside the _Connection.Execute
                return true;
            }
            catch (Exception ex)
            {
                // _Connection.RollbackTransaction();
                throw ex;
            }
        }

        public List<PTXboTaskAllocationsearchResult> getTaskAllocationDetailsForClientSetup(PTXboTaskAllocationSearchCriteria SearchCriteria)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                string SPName = string.Empty;
                if (SearchCriteria.documenttypeID == 450495)
                {
                    SPName = "usp_GetJVDetailsForTaskAllocation";
                    _hashtable.Add("@AllocationTypeID", SearchCriteria.AllocationTypeID);
                    _hashtable.Add("@Accountnumber", SearchCriteria.AccountNumber);
                    _hashtable.Add("@ClientNumber", SearchCriteria.ClientNumber);
                    _hashtable.Add("@County", SearchCriteria.County);
                    _hashtable.Add("@Taxyear", SearchCriteria.TaxYear);
                    _hashtable.Add("@InformalDateFrom", SearchCriteria.InformalDateFrom);
                    _hashtable.Add("@InformalDateTo", SearchCriteria.InformalDateTo);
                    _hashtable.Add("@formalDateFrom", SearchCriteria.FormalDateFrom);
                    _hashtable.Add("@formalDateTo", SearchCriteria.FormalDateTo);
                }
                else
                {
                    SPName = StoredProcedureNames.usp_GetClientSetupDetailsForManualAllocation;
                    _hashtable.Add("@AssignedQueueID", SearchCriteria.documenttypeID);
                    _hashtable.Add("@ProcessingStatusId", SearchCriteria.NoticedProcessStatusID);
                    _hashtable.Add("@Accountnumber", SearchCriteria.AccountNumber);
                    _hashtable.Add("@ScanDateFrom", SearchCriteria.ScanDateFrom);
                    _hashtable.Add("@ScanDateTo", SearchCriteria.ScanDateTo);
                    _hashtable.Add("@ClientNumber", SearchCriteria.ClientNumber);
                    _hashtable.Add("@ClientName", SearchCriteria.ClientName);
                    _hashtable.Add("@Classification", SearchCriteria.Classification);
                    _hashtable.Add("@Address", SearchCriteria.Address);
                    _hashtable.Add("@PropertyType", SearchCriteria.PropertyType);
                    _hashtable.Add("@CreatedBy", SearchCriteria.CreatedBy);
                    _hashtable.Add("@AllocationTypeID", SearchCriteria.AllocationTypeID);
                    _hashtable.Add("@TypeofSetup", SearchCriteria.TypeofSetup);
                    _hashtable.Add("@QueueType", SearchCriteria.QueueType);
                    //Added by Mohanapriya s for TFS ID : 65689 starts here
                    _hashtable.Add("@StateId", SearchCriteria.StateId);
                    _hashtable.Add("@AssignedToId", SearchCriteria.AssignedAgentId);
                    //Added by Mohanapriya s for TFS ID : 65689 ends here
                }

                var result = _Connection.Select<PTXboTaskAllocationsearchResult>(SPName,
                    _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateClientSetupAuditingTaskAllocation(List<PTXboTaskAllocationsearchResult> lstTaskAllocation)
        {
            Hashtable _hashtable = null;
            try
            {
                foreach (var data in lstTaskAllocation)
                {
                    _hashtable = new Hashtable();
                    _hashtable.Add("@ClientId", data.ClientID);
                    _hashtable.Add("@AuditUser", data.AssignedTo);
                    _hashtable.Add("@ProcessingStatus", Enumerators.PTXenumClientProcessingStatus.AssigntoAuditUser.GetId());
                    _hashtable.Add("@StateId", data.StateId); // Added by Mohanapriya s for TFS ID : 65689
                    _hashtable.Add("@AssignedToAgentId", data.AssignedToAgentId );// Added by Mohanapriya s for TFS ID : 65689
                    _hashtable.Add("@AllocationTypeId", data.AllocationTypeID);// Added by Mohanapriya s for TFS ID : 65689
                    var result = _Connection.Execute(StoredProcedureNames.USP_UpdateClientSetupTaskAllocation,
                        _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx, true);
                }
                // _Connection.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                // _Connection.RollbackTransaction();
                throw ex;
            }
        }

        private string GetNoticeProcessingStatusId(int? StatusID)
        {
            string ProcessingStatusId = string.Empty;
            if (StatusID == 0) //  UnAssigned Records
            {
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.EntryQueueUnassigned.GetId() + ",";
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.ClarificationQueueUnassigned.GetId() + ",";
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.DefectQueueUnassigned.GetId() + ",";
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.QCUnAssigned.GetId() + ",";
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.AuditQueueUnassigned.GetId();
            }
            else if (StatusID == 1)  // Assigned or inprogress Records
            {
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.EntryQueueUnassigned.GetId() + ",";
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.EntryInprogress.GetId() + ",";
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.ClarificationQueueAssigned.GetId() + ",";
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.ClarificationQueueInprogress.GetId() + ",";
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.DefectQueueAssigned.GetId() + ",";
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.DefectQueueInprogress.GetId() + ",";
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.QCInProgress.GetId() + ",";
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.QCAssigned.GetId() + ",";
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.AuditQueueAssigned.GetId() + ",";
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.AuditQueueInprogress.GetId();
            }
            else if (StatusID == 2)  // Onhold Records
            {
                ProcessingStatusId += Enumerators.PTXenumHearingNoticeProcessingStatus.OnHold.GetId();
            }
            return ProcessingStatusId;
        }

        public List<PTXboTaskAllocationsearchResult> getTaskAllocationDetailsForPropertySurvey(PTXboTaskAllocationSearchCriteria SearchCriteria)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@DocumentQueueTypeID", SearchCriteria.documenttypeID);
                _hashtable.Add("@AssignTypeID", SearchCriteria.NoticedProcessStatusID);
                _hashtable.Add("@BatchCode", SearchCriteria.BatchCode);
                _hashtable.Add("@ClientNumber", SearchCriteria.ClientNumber);
                _hashtable.Add("@ScanDateFrom", SearchCriteria.ScanDateFrom);
                _hashtable.Add("@ScanDateTo", SearchCriteria.ScanDateTo);
                _hashtable.Add("@NoofRecordsFrom", SearchCriteria.NoofRecordsFrom);
                _hashtable.Add("@NoofRecordsTo", SearchCriteria.NoofRecordsTo);
                _hashtable.Add("@AccountNumber", SearchCriteria.AccountNumber);
                var result = _Connection.Select<PTXboTaskAllocationsearchResult>(StoredProcedureNames.usp_GetDataEntryPropertySurveyForManualAllocation,
                    _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<PTXboTaskAllocationsearchResult> getTaskAllocationDetailsForAffidavit(PTXboTaskAllocationSearchCriteria SearchCriteria)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                _hashtable.Add("@DocumentQueueTypeID", SearchCriteria.documenttypeID);
                _hashtable.Add("@AssignTypeID", SearchCriteria.NoticedProcessStatusID);
                _hashtable.Add("@BatchCode", SearchCriteria.BatchCode);
                _hashtable.Add("@ClientNumber", SearchCriteria.ClientNumber);
                _hashtable.Add("@ScanDateFrom", SearchCriteria.ScanDateFrom);
                _hashtable.Add("@ScanDateTo", SearchCriteria.ScanDateTo);
                _hashtable.Add("@NoofRecordsFrom", SearchCriteria.NoofRecordsFrom);
                _hashtable.Add("@NoofRecordsTo", SearchCriteria.NoofRecordsTo);
                _hashtable.Add("@AccountNumber", SearchCriteria.AccountNumber);
                var result = _Connection.Select<PTXboTaskAllocationsearchResult>(StoredProcedureNames.usp_GetDataEntryAffidavitForManualAllocation,
                    _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
