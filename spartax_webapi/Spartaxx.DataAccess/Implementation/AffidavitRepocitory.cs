using Spartaxx.BusinessObjects;
using Spartaxx.Common;
using Spartaxx.Common.BusinessObjects;
using Spartaxx.DataObjects;
using Spartaxx.Utilities.Extenders;
using Spartaxx.Utilities.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spartaxx.DataAccess
{
 public  class AffidavitRepocitory : IAffidavitRepocitory
    {
        private readonly DapperConnection _Connection;
        public AffidavitRepocitory(DapperConnection Connection)
        {
            _Connection = Connection;
        }
        public List<PTXboAffidavitAllottedDocument> getAffidavitAllottedDocuments(PTXboAffidavitAllottedDocBasedOnSearchFilter objHearingResultAllotmentSearchRequest)
        {
            string SPName = string.Empty;
            Hashtable _hashtable = new Hashtable();

            try
            {
                if (objHearingResultAllotmentSearchRequest.AssignedQueue == Enumerators.PTXenumNoticeQueue.AuditQueue)
                    SPName = StoredProcedureNames.usp_getAffidavitAuditAllottedDocuments; 

                _hashtable.Add("@UserID", objHearingResultAllotmentSearchRequest.UserID);
                _hashtable.Add("@UserRoleID", objHearingResultAllotmentSearchRequest.UserRoleID);
                _hashtable.Add("@ClientNumber", objHearingResultAllotmentSearchRequest.ClientNumber);
                _hashtable.Add("@AccountNumber", objHearingResultAllotmentSearchRequest.AccountNumber);
                _hashtable.Add("@FirstName", objHearingResultAllotmentSearchRequest.FirstName);
                _hashtable.Add("@LastName", objHearingResultAllotmentSearchRequest.LastName);
                _hashtable.Add("@BatchCode", objHearingResultAllotmentSearchRequest.BatchCode);
                _hashtable.Add("@CADLegalName", objHearingResultAllotmentSearchRequest.CADLegalName);
                _hashtable.Add("@PropertyAddress", objHearingResultAllotmentSearchRequest.PropertyAddress);

                var result = _Connection.Select<PTXboAffidavitAllottedDocument>(SPName, _hashtable, Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                return result;
            }
            catch (Exception ex)
            { throw ex;
            }
        }
        public ExpandoObject getNextRecordList(PTXdoIndexedNoticeDocument indexedNoticeList, int Accountid, int taxYear, int HearingTypeid, int AssignedQueue)
        {
            string SPName = string.Empty;
            Hashtable _hashtable = new Hashtable();
            int Userid = 0;
            int UserRoleid = 0;
            try
            {
                if (AssignedQueue == (int)Enumerators.PTXenumNoticeQueue.AuditQueue)
                {
                    SPName = StoredProcedureNames.usp_getNextAffidavitDocument_Audit;
                    Userid = indexedNoticeList.auditedUser.Userid;
                    UserRoleid = indexedNoticeList.auditedUserRole.UserRoleid;
                } 

                _hashtable.Add("@UserID", Userid);
                _hashtable.Add("@UserRoleID", UserRoleid);
                _hashtable.Add("@AssignedQueueID", indexedNoticeList.assignedQueue.NoticeQueueId);
                _hashtable.Add("@IndexedNoticeDocumentId", indexedNoticeList.IndexedNoticeDocumentID);
                _hashtable.Add("@AccountId", indexedNoticeList.Accountid);
                _hashtable.Add("@Val_TaxYear", indexedNoticeList.taxYear);
                _hashtable.Add("@HearingTypeid", indexedNoticeList.HearingTypeid);

                var SP_Result = _Connection.SelectMultiple(SPName, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx,
                    gr => gr.Read<PTXdtHearingResults>(),
                    gr => gr.Read<PTXdtHearingResultsClarification>(),
                    gr => gr.Read<PTXdtHearingDetailsRemarks>(),
                    gr => gr.Read<PTXdtHearingTypeForResult>()
                );

                var result = convertdtResultToNextRecordObject(SP_Result, indexedNoticeList, AssignedQueue); 
                return result;
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }

        private ExpandoObject convertdtResultToNextRecordObject(dynamic SP_Result, PTXdoIndexedNoticeDocument indexedNoticeList, int AssignedQueue)
        {
            var lstHearingDetails = new List<PTXdoHearingDetails>();
            dynamic result = new ExpandoObject();
            PTXdoHearingDetails hearingDetail = new PTXdoHearingDetails();

            try
            {
                Logger.For(this).Transaction("convertdtResultToNextRecordObject-API  Reached " + ((object)indexedNoticeList).ToJson(false));
                if (SP_Result != null)
                {
                    #region IndexedNoticeDocument Object
                    var dtEntryRecordList = (List<PTXdtHearingResults>)SP_Result.Item1;
                    indexedNoticeList = dtEntryRecordList.Select(b => new PTXdoIndexedNoticeDocument
                    {
                        IndexedNoticeDocumentID = b.IndexedNoticeDocumentId,
                        batchCode = b.BatchCode,
                        PWImageID = b.PWImageID,
                        ScannedDateAndTime = b.ScannedDateAndTime,
                        HearingDetailsData  = new PTXdoHearingDetails()
                        {
                            AffidavitFiledDate = b.AffidavitFiledDate,
                            AffidavitStatusID = b.AffidavitStatusID,
                            AffidavitHearingAgentID = b.AffidavitHearingAgentID,
                            AffidavitDeliveryMethodUSmail = b.AffidavitDeliveryMethodUSmail,
                            AffidavitDeliveryMethodInPerson = b.AffidavitDeliveryMethodInPerson,
                            AffidavitDeliveryMethodEmail = b.AffidavitDeliveryMethodEmail,
                            OwnerOpinionValue =b.OwnerOpinionValue
                        },
                        //FileCabinetID = b.PWFileCabinetID, /* FileCabinetID added in NoticeDocumentType */
                        documentType = new PTXdoNoticeDocumentType()
                        {
                            NoticeDocumentTypeId = b.NoticeDocumentTypeId,
                            NoticeDocumentTypeValue = b.NoticedDocumentType,
                            FileCabinetID = b.PWFileCabinetID
                        },
                        Remarks = b.Remarks,
                        taxYear = b.Taxyear,
                        countyCode = b.CountyName,
                        accountNumber = b.AccountNumber,
                        account = new PTXdoAccount()
                        {
                            AccountId = b.AccountId,
                            EndDate = b.EndDate,
                            //propertyDetails = new PTXdoPropertyDetails()
                            //{
                            //    PropertyAddress = new PTXdoAddress()
                            //    {
                            //        addressLine1 = b.PropertyAddress
                            //    }
                            //},
                            /*Modified By :Madha.V for property details Change Request  modified on : 20 Apr 2015 Start*/

                            YearlyHearingDetails = (AssignedQueue != (int)Enumerators.PTXenumNoticeQueue.QCQueue) ? (new List<PTXdoYearlyHearingDetails>
                                {
                                    new PTXdoYearlyHearingDetails()
                                    {
                                        propertyDetails = new PTXdoPropertyDetails(){PropertyAddress = new PTXdoAddress(){addressLine1 = b.PropertyAddress}}
                                    }
                                }
                            ) : (getYearlyHearingDetailsById(b.YearlyHearingDetails)), //By Prakash

                            /*Modified By :Madha.V for property details Change Request  modified on : 20 Apr 2015 End*/
                            County = new PTXdoCounty()
                            {
                                Countyid = b.Countyid,
                                CountyName = b.CountyName,
                                CountyCode = b.CountyCode
                                //Website = b.Website
                            }
                        },
                        accountStatus = new PTXdoAccountStatus()
                        {
                            Accountstatusid = b.AccountStatusId,
                            Accountstatus = b.Accountstatus
                        },
                        accountProcessingStatus = new PTXdoAccountProcessStatus()
                        {
                            AccProcessstatusid = b.AccountProcessStatusId,
                            AccProcessstatus = b.AccountProcessStatus
                        },
                        protestStatus = new PTXdoProtestCodeValues()
                        {
                            ProtestCodeValuesId = b.ProtestCodeId,
                            ProtestCodeValues = b.ProtestCodeValues
                        },
                        userEnteredHearingResult = new PTXdoUserEnteredHearingResults() /* User Entered values */
                        {
                            UserEnteredHearingResutsID = b.UserEnteredHearingResultsId,
                            TaxYear = b.Taxyear,
                            NoticedLandValue = b.NoticedLandValue,
                            NoticedImprovedValue = b.NoticedImprovedValue,
                            NoticedMarketValue = b.NoticedMarketValue,
                            NoticedTotalValue = b.NoticedTotalvalue,
                            //Added by Balaji Task:38259
                            RenoticedLandValue = b.RenoticedLandValue,
                            RenoticedImprovedValue = b.RenoticedImprovedValue,
                            RenoticedMarketValue = b.RenoticedMarketValue,
                            RenoticeTotalValue = b.RenoticeTotalvalue,
                            PostHearingLandValue = b.PostHearingLandValue,
                            PostHearingImprovedValue = b.PostHearingImprovedValue,
                            PostHearingMarketValue = b.PostHearingMarketValue,
                            PostHearingTotalValue = b.PostHearingTotalValue,
                            HearingAgent = new PTXdoAgent()
                            {
                                AgentId = b.HearingAgentId
                            },
                            ConfirmationLetterDate = b.ConfirmationLetterDate,
                            confirmationLetterReceivedDate = b.confirmationLetterReceivedDate,
                            CompletionDate = b.CompletionDate,
                            CompletionTime = b.CompletionTime,
                            HearingResolution = new PTXdoHearingResolution() { HearingResolutionId = b.HearingResolutionId },
                            ReasonCodes = new PTXdoHearingResultsReasonCode() { HearingResultsReasonCodeId = b.ReasonCodeId },
                            DismissalAuthStatus = new PTXdoHearingDismissalAuthStatus() { HearingDismissalAuthStatusId = b.DismissalAuthStatusId },
                            DefectReason = new PTXdoDocumentDefectCodes() { DocumentDefectCodeId = b.DefectReasonId },
                            HearingResultsReport = new PTXdoHearingResultReport() { HearingResultReportId = b.HearingResultReportId },
                            HearingFinalized = b.HearingFinalized,
                            HearingResultsSentOn = b.HearingResultsSentOn,
                            ResultedGenerated = b.ResultGenerated,
                            InvoiceGenerated = b.InvoiceGenerated,
                            InvoiceID = b.InvoiceId,
                            HearingStatus = new PTXdoHearingStatus() { HearingStatusId = b.HearingStatusId },
                            ExemptInvoice = b.ExemptInvoice,
                            HRInvoiceStatus = new PTXdoHRInvoiceStatus() { HRInvoiceStatusid = b.HRInvoiceStatusid },
                            HearingTrackingCode = new PTXdoHearingTrackingCodes() { HearingTrackingId = b.HearingTrackingCodeId },
                            HearingLevel = new PTXdoHearingLevel() { HearingLevelID = b.HearingLevelId },
                            ByPassInformalHearing = b.ByPassInformalHearing,
                            ExemptionId = b.ExemptionId
                        },
                        assignedQueue = new PTXdoNoticeQueue()
                        {
                            NoticeQueueId = b.AssignedQueueId,
                            NoticeQueue = b.NoticeQueue
                        },
                        processingStatus = new PTXdoNoticeProcessingStatus()
                        {
                            NoticeProcessingStatusId = b.NoticeProcessingStatusId,
                            NoticeProcessingStatus = b.NoticeProcessingStatus
                        },

                        entryUser = new PTXdoUser()
                        {
                            Userid = b.EntryUserId
                        },
                        EntryAssignedBy = new PTXdoUser()
                        {
                            Username = b.EntryAssignBy
                        },
                        entryDataAndTime = b.EntryAssignOn,
                        AuditAssignedBy = new PTXdoUser()
                        {
                            Username = b.AuditAssignBy
                        },
                        auditedDateAndTime = b.AuditAssignOn,
                        DefectAssignedBy = new PTXdoUser()
                        {
                            Username = b.DefectAssignBy
                        },
                        defectAssignedDateAndTime = b.DefectAssignOn,
                        QCAssignedBy = new PTXdoUser()
                        {
                            Username = b.QCAssignBy
                        },
                        qcAssignedDateAndTime = b.QCAssignOn,

                        defectAssignedUser = new PTXdoUser()
                        {
                            Userid = b.DefectAssignedUserId
                        },
                        auditedUser = new PTXdoUser()
                        {
                            Userid = b.AuditedUserId
                        },
                        defectNotice = b.DefectNotice,
                        defectResolved = b.DefectResolved,
                        defectReasonCode = new PTXdoDocumentDefectCodes()
                        {
                            DocumentDefectCodeId = b.DocumentDefectCodeId,
                            DocumentDefectCodes = b.DocumentDefectCodes
                        },
                        defectRectifiedDateAndTime = b.DefectRectifiedDateAndTime,
                        client = new PTXdoClient() { clientId = b.ClientId, ClientNumber = b.ClientNumber },
                        DocumentName = b.DocumentName,
                        qcAssignedUser = new PTXdoUser() { Userid = b.QCAssignedUserId },
                        Disaster = new PTXdoDisaster()
                        {
                            DisasterId = b.DisasterId
                        },
                        IsDisasterProtest = b.IsDisasterProtest,
                        

                    }).FirstOrDefault(); 

                    var DNDCodes = dtEntryRecordList.Select(x => x.DNDcodes).FirstOrDefault(); 
                    List<PTXdoYearlyHearingDetails> objYealyHearingList = new List<PTXdoYearlyHearingDetails>();
                    objYealyHearingList = dtEntryRecordList.Select(b => new PTXdoYearlyHearingDetails
                    {
                        YearlyHearingDetailsId = b.YearlyHearingDetailsId,
                        DNDCodeId = new PTXdoDNDCodes() { DNDcodes = DNDCodes },
                        TaxYear = b.YearlyHearingDetailsId == 0 ? 0 : b.TaxYear
                    }).ToList();
                    if (indexedNoticeList.account != null)
                    {
                        indexedNoticeList.account.YearlyHearingDetails = objYealyHearingList;
                    }

                    #endregion 
                     
                    result.indexedNoticeList = indexedNoticeList; 
                    Logger.For(this).Transaction("convertdtResultToNextRecordObject-API  Ends successfully ");

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("convertdtResultToNextRecordObject-API  Error " + ex);
                throw ex;
            }

            return null;
        }

        private List<PTXdoYearlyHearingDetails> getYearlyHearingDetailsById(int YearlyHearingDetailsById)
        {
            Hashtable _hashtable = new Hashtable();
            try
            {
                Logger.For(this).Transaction("getYearlyHearingDetailsById-API  Reached " + ((object)YearlyHearingDetailsById).ToJson(false));
                _hashtable.Add("@YearlyHearingDetailsById", YearlyHearingDetailsById);

                var SP_Result = _Connection.Select<PTXdoYearlyHearingDetails>(StoredProcedureNames.usp_getYearlyHearingDetailsById, _hashtable,
                    Enumerator.Enum_CommandType.StoredProcedure, Enumerator.Enum_ConnectionString.Spartaxx);
                Logger.For(this).Transaction("getYearlyHearingDetailsById-API  Ends successfully ");
                return SP_Result;
            }
            catch (Exception ex)
            {
                Logger.For(this).Transaction("getYearlyHearingDetailsById-API  Error " + ex);
                throw ex;
            }
        }

    }
}
